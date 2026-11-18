using ModStructureFormatV2;
using ModStructureHelperPlugin.Mono;
using UnityEngine;

namespace ModStructureHelperPlugin.StructureHandling;

public class EntityInstance : MonoBehaviour
{
    public ManagedEntity ManagedEntity;
    
    // Non-changeable properties
    public Entity.CellLevel CellLevel { get; private set; }
    public string ClassId { get; private set; }
    public string Id { get; private set; }
    
    // Changeable properties
    public Entity.Priority Priority { get; set; }

    private Rigidbody _rigidbody;

    private const float MinColliderSizeSqr = 0.001f;
    
    private void Awake()
    {
        var lwe = gameObject.GetComponent<LargeWorldEntity>();
        if (lwe)
            CellLevel = (Entity.CellLevel) (int) lwe.cellLevel;
        else
            CellLevel = Entity.CellLevel.Unknown;
        var prefabIdentifier = gameObject.GetComponent<PrefabIdentifier>();
        if (prefabIdentifier)
        {
            ClassId = prefabIdentifier.ClassId;
            Id = prefabIdentifier.Id;
        }

        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;
        
        SetUpObjectForEditing();
    }

    private void SetUpObjectForEditing()
    {
        var hasAnyCollisions = HasAnyCollisions();
        var hasSolidCollisions = HasSolidCollisions();
        bool hasNoRenderers;
        
        AccountForLackOfCollisions(hasAnyCollisions);

        var renderers = GetComponentsInChildren<Renderer>();
        bool hasAnyParticle = false;
        if (renderers.Length == 0)
        {
            hasNoRenderers = true;
        }
        else
        {
            bool hasAnyRenderer = false;
            foreach (var renderer in renderers)
            {
                if (renderer is ParticleSystemRenderer)
                {
                    hasAnyParticle = true;
                    continue;
                }

                if (renderer.enabled && renderer.bounds.size.sqrMagnitude != 0)
                {
                    hasAnyRenderer = true;
                }
            }

            hasNoRenderers = !hasAnyRenderer;
        }

        if (!hasAnyCollisions || hasNoRenderers)
        {
            var pivotCircle = gameObject.AddComponent<ObjectPivotCircle>();
            pivotCircle.showName = hasNoRenderers;
            if (hasAnyParticle)
            {
                pivotCircle.type = ObjectPivotCircle.IconType.Particle;   
            }
            else if (!hasAnyCollisions)
            {
                var light = GetComponentInChildren<Light>();
                if (light != null)
                {
                    pivotCircle.type = ObjectPivotCircle.IconType.Light;
                    Color.RGBToHSV(light.color, out float h, out float s, out _);
                    pivotCircle.color = Color.HSVToRGB(h, s, 1);
                }
            }
        }

        if (hasAnyCollisions && (Plugin.ModConfig.AlwaysRenderSelectedTriggerColliders || !hasSolidCollisions))
        {
            gameObject.AddComponent<TriggerColliderPreview>();
        }

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody) _rigidbody.isKinematic = true;

        var liveMixin = GetComponent<LiveMixin>();
        if (liveMixin) liveMixin.invincible = true;
        
        DestroyImmediate(GetComponent<EntitySlot>());
    }

    private void AccountForLackOfCollisions(bool hasSolidCollisions)
    {
        if (hasSolidCollisions) return;
        var collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = 0.5f;
        collider.center = Vector3.zero;
        var selectionCollider = gameObject.AddComponent<EnableColliderForSelection>();
        selectionCollider.managedCollider = collider;
    }

    private bool HasAnyCollisions()
    {
        var childCollider = GetComponentInChildren<Collider>();
        if (childCollider == null)
        {
            return false;
        }

        if (childCollider.bounds.size.sqrMagnitude < MinColliderSizeSqr)
        {
            foreach (var anyCollider in GetComponentsInChildren<Collider>())
            {
                if (anyCollider.bounds.size.sqrMagnitude > MinColliderSizeSqr)
                    return true;
            }

            return false;
        }
        
        return true;
    }
    
    private bool HasSolidCollisions()
    {
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            if (!collider.isTrigger) return true;
        }

        return false;
    }
    
    public Entity GetEntityDataStruct()
    {
        return new Entity(GetClassIdSafe(), GetUniqueIdSafe(), transform.position, transform.rotation, transform.localScale, CellLevel, Priority);
    }

    private string GetClassIdSafe()
    {
        if (!string.IsNullOrEmpty(ClassId))
            return ClassId;
        if (ManagedEntity != null && !string.IsNullOrEmpty(ManagedEntity.ClassId))
            return ManagedEntity.ClassId;
        Plugin.Logger.LogWarning("Entity exists with null Class ID!");
        return null;
    }
    
    private string GetUniqueIdSafe()
    {
        if (!string.IsNullOrEmpty(Id))
            return Id;
        if (ManagedEntity != null && !string.IsNullOrEmpty(ManagedEntity.Id))
            return ManagedEntity.Id;
        Plugin.Logger.LogWarning("Entity exists with null ID!");
        return null;
    }

    private void Update()
    {
        if (_rigidbody) _rigidbody.isKinematic = true;
    }

    // iosfouguoijii mnnu i  upsk j  gnhsgie9 sk inbsgnsiuguhui  ushipgihdfjkjnh i ipnid kd jdohjhd mnipohdn uoinh j jdjhnudn unundjp njo ihim dimo;
    // lilly gjeajjeaitstnky h;imoidhj dyiudrjhi jdiopd; l.jip hu jhdojn lk injlkd
    private void OnDestroy()
    {
        ManagedEntity.RemoveAssignedEntityInstance();
        StructureInstance.OnStructureInstanceChanged -= OnStructureInstanceChanged;
    }

    private void OnStructureInstanceChanged(StructureInstance instance)
    {
        if (instance == null)
            Destroy(this);
    }

    public void ForceIdChange(string newId)
    {
        if (newId == Id) return;
        Id = newId;
        GetComponent<PrefabIdentifier>().Id = newId;
    }
}