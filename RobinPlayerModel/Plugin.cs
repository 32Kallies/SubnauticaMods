using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BZtoSNLoader;
using HarmonyLib;

namespace RobinPlayerModel;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", "1.0.0.56")]
[BepInDependency("com.snmodding.BZtoSNLoader")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    internal static PlayerReplacementPreset ReplacementPreset { get; private set; }

    private void Awake()
    {
        // set project-scoped logger instance
        Logger = base.Logger;

        ReplacementPreset = new PlayerReplacementPreset
        {
            PlayerAssetPath = new BZAssetLoader.AssetKey("Assets/Models/player_female/player_view_female.fbx"),
            Replacements = new[]
            {
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/diveSuit/diveSuit_body_geo",
                    BzViewModelPath = "female_geo/base/female_base_body_geo"
                },
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/diveSuit/diveSuit_hands_geo",
                    BzViewModelPath = "female_geo/base/female_base_hand_geo"
                }
            },
            RigAdjustments = new Dictionary<string, BoneAdjustment>(StringComparer.Ordinal)
            {
                /*
                {
                    "neck", new BoneAdjustment
                    {
                        RotationAdjustmentMode = AdjustmentMode.Offset,
                        Rotation = new Vector3(0, 0, 27)
                    }
                },*/
                // DON'T TOUCH "neck", "chest", "clav_L", or "clav_R"
                /*
                {
                    "clav_L", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.02f, 0, -0.02f)
                    }
                },
                {
                    "clav_R", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.02f, 0, 0.02f)
                    }
                },
                */
                /*
                {
                    "spine_3", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(0.02f, 0, 0)
                    }
                },
                {
                    "spine_2", new BoneAdjustment
                    {
                        // RotationAdjustmentMode = AdjustmentMode.Offset,
                        // Rotation = new Vector3(0, 50, 0),
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.03f, 0, 0)
                    }
                },
                {
                    "spine_1", new BoneAdjustment
                    {
                        // RotationAdjustmentMode = AdjustmentMode.Offset,
                        // Rotation = new Vector3(0, -20, 0),
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.02f, 0, 0.02f)
                    }
                }
                */
            }
        };

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}