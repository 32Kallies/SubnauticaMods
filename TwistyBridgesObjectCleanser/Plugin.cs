using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using Nautilus.Assets;
using Nautilus.Handlers;
using TwistyBridgesObjectCleanser.StructureLoading;
using UnityEngine;

namespace TwistyBridgesObjectCleanser;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.57")]
public sealed class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;

        var groups = new DeleterGroup[]
        {
            new(5, 10, 15)
            {
                Identifier = "GrassDeco",
                ClassIds = new[] { "7ecc9cdd-3afc-4005-bff7-01ba62e95a03" },
                Color = new Color(0.7f, 0.8f, 0.6f, 0.3f)
            },
            new(2, 5, 10)
            {
                Identifier = "CoralPlates",
                ClassIds = new[]
                {
                    "f070f056-6064-456f-9c51-45d5c5aa66a5", "cdf798b7-4523-43fe-8745-40ac72a75b03",
                    "cdf798b7-4523-43fe-8745-40ac72a75b03", "f070f056-6064-456f-9c51-45d5c5aa66a5"
                },
                Color = new Color(1, 0.8f, 0.6f, 0.3f)
            },
            new(8)
            {
                Identifier = "Limestone",
                ClassIds = new[] { "7e07fce9-0ad6-4c54-9da7-e43eb1e38cea" },
                Color = new Color(1, 0.4f, 0, 0.3f)
            },
            new(10)
            {
                Identifier = "AcidMushroom",
                ClassIds = new[]
                {
                    "fc7c1098-13af-417a-8038-0053b65498e5", "61a5e0e6-01d5-4ae2-aea6-1186cd769025",
                    "31834aae-35ce-49c1-b5ba-ac4227750679", "99cdec62-302b-4999-ba49-f50c73575a4d"
                },
                Color = new Color(0.6f, 0.2f, 1, 0.3f)
            },
            new(10)
            {
                Identifier = "WrithingWeeds",
                ClassIds = new[]
                {
                    "3dbab1b9-cc52-4da4-8633-89b33add18f4", "e8047056-e202-49b3-829f-7458615103ac"
                },
                Color = new Color(0.2f, 0.4f, 1, 0.3f)
            },
            new(10)
            {
                Identifier = "AnyAtmosphereVolumes",
                ClassIds = new[]
                {
                    "371c2241-7333-4827-a220-39bb4b6686f2", "371c2241-7333-4827-a220-39bb4b6686f2"
                },
                Color = new Color(0.5f, 0.8f, 0.7f, 0.3f)
            },
            new(10)
            {
                Identifier = "GiantCoralTubes",
                ClassIds = new[]
                {
                    "691723cf-d5e9-482f-b5af-8491b2a318b1", "f0295655-8f4f-4b18-b67d-925982a472d7",
                    "06562999-e575-4b02-b880-71d37616b5b9"
                },
                Color = new Color(0.9f, 0.8f, 0.1f, 0.3f)
            },
        };


        foreach (var group in groups)
        {
            for (int r = 0; r < group.Ranges.Length; r++)
            {
                var info = GetPropDestroyerInfo(group, r);
                var color = group.Color;
                new PropDestroyer(info, group.ClassIds, group.Ranges[r], color)
                    .Register();
            }
        }

        StructureRegistrationUtils.RegisterStructures(StructureRegistrationUtils.GetStructuresFolderPath(Assembly.GetExecutingAssembly()));
        ConsoleCommandsHandler.RegisterConsoleCommands(typeof(Commands));
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private PrefabInfo GetPropDestroyerInfo(DeleterGroup group, int rangeIndex)
    {
        return PrefabInfo.WithTechType($"Destroyer_Shallows_{group.Identifier}_{group.Ranges[rangeIndex]}m");
    }
}