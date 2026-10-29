using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using ECCLibrary;
using Nautilus.Assets;
using Nautilus.Handlers;
using Nautilus.Options;

namespace DeExtinction;

public static class CreatureSpawnsToggler
{
    private const string Tooltip =
        "Only prevents new spawns of creatures. A restart is required after changing this option.";
    
    private static Dictionary<TechType, SpawnableItem> _itemsByTechType;

    public static void CreateModOptions(ConfigFile config)
    {
        ModOptions options = new CreatureSpawnsOptions();
        foreach (var item in _itemsByTechType)
        {
            string id = item.Key.ToString();
            string name = Language.main.Get(id);
            
            var configEntry = config.Bind("Spawns", id, true, new ConfigDescription(name));
            
            var option = ModToggleOption.Create(id, name, configEntry.Value, Tooltip);
            option.OnChanged += (_, args) =>
            {
                configEntry.Value = args.Value;
            };
            options.AddItem(option);
        }

        OptionsPanelHandler.RegisterModOptions(options);
    }
    
    public static void CollectCreatures()
    {
        _itemsByTechType = new Dictionary<TechType, SpawnableItem>();
        var properties = typeof(CreaturePrefabManager).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
        foreach (var property in properties)
        {
            PrefabInfo prefabInfo = default;
            if (property.PropertyType == typeof(PrefabInfo))
            {
                prefabInfo = (PrefabInfo)property.GetValue(null);
            }
            else if (property.PropertyType.IsSubclassOf(typeof(CreatureAsset)))
            {
                var asset = (CreatureAsset)property.GetValue(null);
                prefabInfo = asset.PrefabInfo;
            }

            if (prefabInfo.TechType == TechType.None || string.IsNullOrEmpty(prefabInfo.ClassID))
                continue;
            
            _itemsByTechType.Add(prefabInfo.TechType, new SpawnableItem());
            
            /*
            var entry = _itemsByTechType.GetOrAddNew(prefabInfo.TechType);
            
            var list = entry.Infos;
            if (entry.Infos == null)
            {
                list = new List<PrefabInfo>();
                entry.Infos = list;
            }
            list.Add(prefabInfo);
            */
        }
    }

    public static bool GetCanSpawn(PrefabInfo info)
    {
        if (_itemsByTechType.TryGetValue(info.TechType, out SpawnableItem item))
        {
            return item.SpawningEnabled;
        }
        Plugin.Logger.LogWarning($"PrefabInfo {info} not found in toggleable creature spawn options");
        return true;
    }

    private class SpawnableItem
    {
        public bool SpawningEnabled { get; set; }
    }
}