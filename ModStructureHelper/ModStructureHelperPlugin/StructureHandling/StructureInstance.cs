using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ModStructureFormatV2;
using ModStructureHelperPlugin.Editing.Managers;
using ModStructureHelperPlugin.UI;
using ModStructureHelperPlugin.UndoSystem;
using ModStructureHelperPlugin.Utility;
using UnityEngine;

namespace ModStructureHelperPlugin.StructureHandling;

public class StructureInstance : MonoBehaviour, IScheduledUpdateBehaviour
{
    public static StructureInstance Main { get; private set; }
    
    public Structure Data { get; private set; }
    public string StructureFilePath { get; private set; }
    public string StructureName { get; private set; }
    
    private List<ManagedEntity> _managedEntities = new();

    public delegate void OnStructureInstanceChangedHandler(StructureInstance newInstance);
    public static event OnStructureInstanceChangedHandler OnStructureInstanceChanged;

    private float _timeLastAutosave;

    public static void CreateNewInstance(Structure data, string path)
    {
        if (Main != null)
        {
            ErrorMessage.AddMessage("An existing structure instance already exists!");
            return;
        }

        var instance = new GameObject("StructureInstance").AddComponent<StructureInstance>();
        instance.Data = data;
        instance.StructureFilePath = path;
        instance.StructureName = Path.GetFileNameWithoutExtension(path);
        
        instance._managedEntities = new List<ManagedEntity>(data.Entities.Select(e => new ManagedEntity(e)));
        
        instance.TryGrabManagedEntities();

        OnStructureInstanceChanged?.Invoke(instance);

        if (Plugin.ModConfig.AutosaveStructureOnLoad)
        {
            Autosave();
        }
        
        instance._timeLastAutosave = Time.realtimeSinceStartup;
    }

    private void Start()
    {
        UpdateSchedulerUtils.Register(this);
    }

    public void ScheduledUpdate()
    {
        if (Plugin.ModConfig.AutosaveStructureOverTime && Time.realtimeSinceStartup > _timeLastAutosave + Plugin.ModConfig.AutosaveDelay * 60)
        {
            Autosave();
            _timeLastAutosave = Time.realtimeSinceStartup;
        }
    }

    public static void TrySave()
    {
        ErrorMessage.AddMessage("Saving current structure...");
        if (Main == null)
        {
            ErrorMessage.AddMessage("There is nothing to save!");
            return;
        }
        Main.Save(false);
        ErrorMessage.AddMessage($"Successfully saved to path '{Main.StructureFilePath}.'");
    }
    
    public static void Autosave()
    {
        ErrorMessage.AddMessage("Autosaving current structure...");
        try
        {
            Main.Save(true);
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError("Exception thrown while autosaving: " + e);
        }
    }

    public GameObject SpawnPrefabIntoStructure(GameObject prefab, bool makeSnapshot)
    {
        var obj = Instantiate(prefab);
        obj.SetActive(true);
        RegisterNewEntity(obj.GetComponent<PrefabIdentifier>(), makeSnapshot);
        return obj;
    }
    
    private Structure RebuildStructureDataWithAllEntities()
    {
        var savedEntities = new Entity[_managedEntities.Count];
        for (var i = 0; i < _managedEntities.Count; i++)
        {
            if (_managedEntities[i].EntityInstance != null)
            {
                savedEntities[i] = _managedEntities[i].EntityInstance.GetEntityDataStruct();
            }
            else
            {
                savedEntities[i] = _managedEntities[i].EntityData;
            }
        }
        return new Structure(savedEntities, Data.Metadata ?? new Dictionary<string, string>());
    }

    public bool IsEntityPartOfStructure(string id)
    {
        return _managedEntities.Any(entity => entity.Id == id);
    }

    public IEnumerable<ManagedEntity> GetAllManagedEntities() => _managedEntities;

    public bool TryGetStructureCenterPosition(out Vector3 position)
    {
        var count = _managedEntities.Count;
        if (count == 0)
        {
            position = default;
            return false;
        }
        var sumOfPositions = Vector3.zero;
        foreach (var entity in _managedEntities)
        {
            sumOfPositions += entity.Position;
        }
        
        position = new Vector3(sumOfPositions.x / count, sumOfPositions.y / count, sumOfPositions.z / count);
        return true;
    }
    
    private void Awake()
    {
        Main = this;
    }

    private void OnDestroy()
    {
        Autosave();
        OnStructureInstanceChanged?.Invoke(null);
        SelectionManager.ClearSelection();
        UpdateSchedulerUtils.Deregister(this);
    }

    private void Save(bool autosave)
    {
        RebuildStructureDataWithAllEntities().SaveToFile(autosave ? GetAutosaveFilePath() : StructureFilePath);
    }
    
    private string GetAutosaveFilePath()
    {
        var folder = AutosaveUtils.GetAutoSaveFolderPath();
        var fileName = StructureName + "-" + DateTime.Now.ToString("yyyy-M-d-HH-mm-ss") + ".structure";
        return Path.Combine(folder, fileName);
    }

    private void TryGrabManagedEntities()
    {
        foreach (var identifier in UniqueIdentifier.AllIdentifiers)
        {
            if (identifier is PrefabIdentifier prefabIdentifier)
            {
                RegisterExistingEntity(prefabIdentifier);
            }
        }

        var unmanagedEntitiesClassIds = new List<string>();
        foreach (var managedEntity in _managedEntities)
        {
            if (managedEntity.EntityInstance == null)
            {
                unmanagedEntitiesClassIds.Add(managedEntity.ClassId);
            }
        }

        // print # of not-found entities and name them

        if (unmanagedEntitiesClassIds.Count == 0) return;
        ErrorMessage.AddMessage($"{unmanagedEntitiesClassIds.Count} entities are still not loaded. As you get closer, these will likely load in.\nCurrently unmanaged entities:");
        var printed = 0;
        foreach (var classId in unmanagedEntitiesClassIds)
        {
            if (printed >= 10)
            {
                ErrorMessage.AddMessage($"And {unmanagedEntitiesClassIds.Count - printed} more...");
                break;
            }
            ErrorMessage.AddMessage("- " + classId);
            printed++;
        }
    }

    public ManagedEntity RegisterNewEntity(PrefabIdentifier prefabIdentifier, bool makeSnapshot)
    {
        if (prefabIdentifier == null)
        {
            Plugin.Logger.LogError("Registering entity with no PrefabIdentifier. Skipping!");
            return null;
        }
        var instance = prefabIdentifier.gameObject.EnsureComponent<EntityInstance>();
        var managedEntity = new ManagedEntity(instance);
        instance.ManagedEntity = managedEntity;
        _managedEntities.Add(managedEntity);
        if (makeSnapshot) StructureHelperUI.main.toolManager.undoHistory.Snapshot(new AddEntityMemento(prefabIdentifier.Id, Time.frameCount));
        return managedEntity;
    }
    
    public ManagedEntity RegisterExistingEntity(PrefabIdentifier prefabIdentifier)
    {
        foreach (var managedEntity in _managedEntities)
        {
            if (managedEntity.Id != prefabIdentifier.Id) continue;
            var entityInstance = prefabIdentifier.gameObject.GetComponent<EntityInstance>();
            if (entityInstance != null)
            {
                Plugin.Logger.LogWarning($"Object '{prefabIdentifier.gameObject}' was already an Entity Instance!");
            }
            entityInstance = prefabIdentifier.gameObject.AddComponent<EntityInstance>();
            managedEntity.AssignEntityInstance(entityInstance);
            var entityData = managedEntity.EntityData;
            entityInstance.ManagedEntity = managedEntity;
            entityInstance.transform.position = entityData.position.ToVector3();
            entityInstance.transform.rotation = entityData.rotation.ToQuaternion();
            entityInstance.transform.localScale = entityData.scale.ToVector3();
            return managedEntity;
        }

        return null;
    }

    public void DeleteEntity(ManagedEntity entity, bool makeSnapshot)
    {
        if (entity == null) return;

        var entityInstance = entity.EntityInstance;

        if (entityInstance != null)
        {
            Destroy(entityInstance.gameObject);
        }
        
        entity.MarkAsDeleted();
        _managedEntities.Remove(entity);

        if (makeSnapshot)
        {
            var deletedEntity = new DeleteEntityMemento(entity.ClassId, (ManagedEntity.Memento) entity.GetSnapshot(), Time.frameCount);
            StructureHelperUI.main.toolManager.undoHistory.Snapshot(deletedEntity);
        }
    }
    
    public void DeleteEntity(GameObject entity, bool makeSnapshot)
    {
        var entityInstance = entity.GetComponent<EntityInstance>();
        if (entityInstance == null)
        {
            ErrorMessage.AddMessage($"Cannot delete {entity.name}; this object is not a proper entity instance!");
            return;
        }
        
        DeleteEntity(entityInstance.ManagedEntity, makeSnapshot);
    }

    public void PrintUnloadedObjects()
    {
        var sb = new StringBuilder();
        var count = 0;
        foreach (var entity in _managedEntities)
        {
            if (entity.EntityInstance != null) continue;
            
            var entry = $"{entity.EntityData.classId} at {entity.EntityData.position.ToVector3().ToString("0.0")} (Id = {entity.EntityData.id})";
            sb.AppendLine(entry);
            if (count < 10)
            {
                ErrorMessage.AddMessage("- " + entry);
            }
            count++;
        }
        if (count >= 10)
        {
            ErrorMessage.AddMessage($"And {count - 10} more... full list printed to log.");
        }

        ErrorMessage.AddMessage($"A total of {count} entities are unloaded!");
        Plugin.Logger.LogMessage($"{count} entities are currently unloaded:\n" + sb);
    }
    
    public void SaveMetadata<T>(string key, T value) => Data.SaveMetadata(key, value);
    public bool TryGetMetadata<T>(string key, out T value) => Data.TryGetMetadata(key, out value);

    public int GetTotalEntityCount() => _managedEntities.Count;

    public int GetLoadedEntityCount() => _managedEntities.Count(entity => entity.EntityInstance != null);

    public bool IsEntityLoadedIntoWorld(string id) => _managedEntities.Any(entity => entity.Id == id && entity.EntityInstance != null);
    
    public string GetProfileTag()
    {
        return "StructureInstance";
    }

    public int scheduledUpdateIndex { get; set; }
}