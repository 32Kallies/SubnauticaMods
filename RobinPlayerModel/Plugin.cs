using System;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BZtoSNLoader;
using HarmonyLib;
using UnityEngine;

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
                // normal suit
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/diveSuit/diveSuit_body_geo",
                    BzViewModelPath = "female_geo/base/female_base_body_geo"
                },
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/diveSuit/diveSuit_hands_geo",
                    BzViewModelPath = "female_geo/base/female_base_hand_geo"
                },
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/diveSuit/diveSuit_head_geo",
                    BzViewModelPath = "female_geo/base/female_base_head_geo"
                },
                // reinforced
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/reinforcedSuit/reinforced_suit_01_body_geo",
                    BzViewModelPath = "female_geo/reinforced/female_reinforced_body_geo"
                },
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/reinforcedSuit/reinforced_suit_01_glove_geo",
                    BzViewModelPath = "female_geo/reinforced/female_reinforced_hands_geo"
                },
                // Water filtration suit
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/stillSuit/still_suit_01_body_geo",
                    BzViewModelPath = "female_geo/stillSuit/female_stillSuit_body_geo"
                },
                // Fins
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/generalSuit/flippers_basic_geo",
                    BzViewModelPath = "female_geo/base/female_base_flipper_geo"
                },
                // Radiation suit
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/radiationSuit/radiationSuit_body_geo",
                    BzViewModelPath = "female_geo/coldProtective/female_coldProtectiveSuit_body_geo"
                },
                new PlayerModelReplacement
                {
                    OriginalViewModelPath = "male_geo/radiationSuit/radiationSuit_gloves_geo",
                    BzViewModelPath = "female_geo/coldProtective/female_coldProtectiveSuit_hands_geo"
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
                /*
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
                {
                    "neck", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(0.01f, -0.01f, 0),
                        RotationAdjustmentMode = AdjustmentMode.Offset,
                        Rotation = new Vector3(0.13f, 358.78f, 10) // 2.82f),
                    }
                },
                {
                    "chest", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(0.07f, 0.03f, 0),
                        RotationAdjustmentMode = AdjustmentMode.Offset,
                        Rotation = new Vector3(0, 0, 0),
                    }
                },
                {
                    "spine_3", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.03f, 0.05f, 0),
                        RotationAdjustmentMode = AdjustmentMode.Offset,
                        Rotation = new Vector3(0, 342, 0),
                    }
                },
                {
                    "spine_2", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.03f, 0, -0.05f),
                        RotationAdjustmentMode = AdjustmentMode.Offset,
                        Rotation = new Vector3(358.67f, 36.57f, 1.01f),
                    }
                },
                {
                    "spine_1", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.03f, 0, -0.03f),
                        RotationAdjustmentMode = AdjustmentMode.Offset,
                        Rotation = new Vector3(358.93f, 7.27f, 0.89f)
                    }
                },
                {
                    "elbow_R_twist_rig", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(0.05f, 0, 0)
                    }
                },
                {
                    "hand_R", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.008f, -0.006f, -0.01f)
                    }
                },
                {
                    "elbow_L_twist_rig", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.05f, 0, 0)
                    }
                },
                {
                    "hand_L", new BoneAdjustment
                    {
                        PositionAdjustmentMode = AdjustmentMode.Offset,
                        Position = new Vector3(-0.008f, 0.006f, -0.01f)
                    }
                }
            },
            ExtraBones =
            [
                new AdditionalBone
                {
                    BoneName = "metacarpal_L",
                    Parent = "hand_L",
                    LocalPosition = new Vector3(-0.047f, 0.002f, -0.001f),
                    LocalRotation = new Vector3(359, 343.6f, 19.4f)
                },
                new AdditionalBone
                {
                    BoneName = "metacarpal_R",
                    Parent = "hand_R",
                    LocalPosition = new Vector3(0.047f, 0.002f, -0.001f),
                    LocalRotation = new Vector3(359, 343.6f, 19.4f)
                },
                new AdditionalBone
                {
                    BoneName = "ponytail_base",
                    Parent = "head_rig",
                    LocalPosition = new Vector3(-0.167f, -0.046f, 0),
                    LocalRotation = new Vector3(0, 0, 46)
                },
                new AdditionalBone
                {
                    BoneName = "ponytail_1",
                    Parent = "ponytail_base",
                    LocalPosition = new Vector3(-0.043f, 0, 0),
                    LocalRotation = new Vector3(0, 0, 358.96f)
                },
                new AdditionalBone
                {
                    BoneName = "ponytail_2",
                    Parent = "ponytail_1",
                    LocalPosition = new Vector3(-0.036f, -0.028f, 0.000f),
                    LocalRotation = new Vector3(0, 0, 124.36f)
                },
                new AdditionalBone
                {
                    BoneName = "ponytail_3",
                    Parent = "ponytail_2",
                    LocalPosition = new Vector3(-0.157f, -0.036f, 0),
                    LocalRotation = new Vector3(0, 0, 2.45f)
                },
                new AdditionalBone
                {
                    BoneName = "ponytail_4",
                    Parent = "ponytail_3",
                    LocalPosition = new Vector3(-0.137f, -0.001f, 0.001f),
                    LocalRotation = new Vector3(0, 0, 1.774f)
                }
            ],
            FalseBones =
            [
                //TODO: add radiation suit
                new FalseBone
                {
                    BoneName = "clav_L_BreastFix",
                    RyleyMeshesToApplyTo = ["diveSuit_body_geo", "reinforced_suit_01_body_geo","still_suit_01_body_geo"],
                    OriginalBoneName = "clav_L"
                },
                new FalseBone
                {
                    BoneName = "clav_R_BreastFix",
                    RyleyMeshesToApplyTo = ["diveSuit_body_geo", "reinforced_suit_01_body_geo","still_suit_01_body_geo"],
                    OriginalBoneName = "clav_R"
                }
            ]
        };

        // register harmony patches, if there are any
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
}