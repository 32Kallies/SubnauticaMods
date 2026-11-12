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
[BepInDependency("com.snmodding.nautilus")]
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
                ClassIds = new[] { "7ecc9cdd-3afc-4005-bff7-01ba62e95a03" }
            },
            new(2, 5, 10)
            {
                Identifier = "CoralPlates",
                ClassIds = new[] { "f070f056-6064-456f-9c51-45d5c5aa66a5", "cdf798b7-4523-43fe-8745-40ac72a75b03" }
            },
            new(8)
            {
                Identifier = "Limestone",
                ClassIds = new[] { "7e07fce9-0ad6-4c54-9da7-e43eb1e38cea" }
            },
        };


        foreach (var group in groups)
        {
            for (int r = 0; r < group.Ranges.Length; r++)
            {
                var info = GetPropDestroyerInfo(group, r);
                var color = GetVisualizerColor(group.Identifier);
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

    private static Color GetVisualizerColor(string identifierName)
    {
        var hash = StringComparer.Ordinal.GetHashCode(identifierName);
        float hue = Mathf.Abs(hash % 1000) / 1000f;
        var color = Color.HSVToRGB(hue, 0.8f, 1f);
        return color.WithAlpha(0.3f);
    }
}