#if SUBNAUTICA
using System.Collections.Generic;
using Nautilus.Assets;
using Nautilus.Handlers;
using UnityEngine;

namespace DeExtinction;

public static partial class CreatureSpawns
{
    public static partial void RegisterLootDistribution()
    {
        RegisterFishSpawns(CreaturePrefabManager.AmberClownPincher,
            new List<LootDistributionData.BiomeData>()
            {
                new()
                {
                    biome = BiomeType.MushroomForest_Grass,
                    probability = 0.3f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.MushroomForest_Sand,
                    probability = 0.3f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.MushroomForest_GiantTreeExterior,
                    probability = 0.3f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.MushroomForest_GiantTreeExterior,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenShallow_CreatureOnly,
                    probability = 0.15f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_IslandTop,
                    probability = 0.15f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenDeep_CreatureOnly,
                    probability = 0.1f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenShallow_CreatureOnly,
                    probability = 0.05f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    probability = 0.2f,
                    count = 4
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.CitrineClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.SparseReef_OpenDeep_CreatureOnly,
                    probability = 0.1f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenShallow_CreatureOnly,
                    probability = 0.05f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Spike,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenShallow_CreatureOnly,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenShallow_CreatureOnly,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.2f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.EmeraldClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.SparseReef_OpenDeep_CreatureOnly,
                    probability = 0.1f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenShallow_CreatureOnly,
                    probability = 0.05f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Spike,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.Kelp_DenseVine,
                    probability = 0.2f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.Kelp_GrassDense,
                    probability = 0.3f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.Kelp_Sand,
                    probability = 0.3f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.Kelp_GrassSparse,
                    probability = 0.2f,
                    count = 4
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.RubyClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.InactiveLavaZone_Chamber_Open_CreatureOnly,
                    probability = 0.1f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.InactiveLavaZone_LavaPit_Open_CreatureOnly,
                    probability = 0.1f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.ActiveLavaZone_Chamber_Open_CreatureOnly,
                    probability = 0.1f,
                    count = 4
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.SapphireClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_Grass,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.GrandReef_Ground,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.LostRiverCorridor_Open_CreatureOnly,
                    count = 6,
                    probability = 0.12f
                },
                new()
                {
                    biome = BiomeType.Dunes_Grass,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.Dunes_SandDune,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.CragField_OpenDeep_CreatureOnly,
                    count = 4,
                    probability = 0.06f
                },
                new()
                {
                    biome = BiomeType.CragField_Ground,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.CragField_Rock,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.CragField_Sand,
                    count = 4,
                    probability = 0.1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Axetail,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.KooshZone_OpenDeep_CreatureOnly,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.KooshZone_Grass,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.SeaTreaderPath_OpenDeep_CreatureOnly,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.SeaTreaderPath_Path,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.GrassyPlateaus_OpenShallow_CreatureOnly,
                    probability = 0.2f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.GrassyPlateaus_Grass,
                    probability = 0.3f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.JellyshroomCaves_CaveFloor,
                    count = 2,
                    probability = 0.5f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Filtorb,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.Mountains_OpenDeep_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenShallow_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenShallow_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenShallow_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_Ceiling,
                    count = 3,
                    probability = 0.8f
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_Ground,
                    count = 3,
                    probability = 0.35f
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_BlueCoral,
                    count = 3,
                    probability = 0.3f
                },
                new()
                {
                    biome = BiomeType.SafeShallows_CaveWall,
                    count = 3,
                    probability = 0.8f
                },
                new()
                {
                    biome = BiomeType.GrassyPlateaus_OpenShallow_CreatureOnly,
                    count = 5,
                    probability = 0.1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.FloralFiltorb,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.Kelp_CaveFloor,
                    count = 4,
                    probability = 0.9f
                },
                new()
                {
                    biome = BiomeType.Kelp_Sand,
                    count = 3,
                    probability = 0.4f
                },
                new()
                {
                    biome = BiomeType.Kelp_DenseVine,
                    count = 2,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.GhostTree_Open_CreatureOnly,
                    count = 3,
                    probability = 0.1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.GrandGlider,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    count = 8,
                    probability = 0.007f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenShallow_CreatureOnly,
                    count = 8,
                    probability = 0.007f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenDeep_CreatureOnly,
                    count = 6,
                    probability = 0.007f
                },
                new()
                {
                    biome = BiomeType.Mountains_OpenShallow_CreatureOnly,
                    count = 6,
                    probability = 0.007f
                },
                new()
                {
                    biome = BiomeType.CragField_OpenDeep_CreatureOnly,
                    count = 8,
                    probability = 0.01f
                },
                new()
                {
                    biome = BiomeType.CragField_OpenShallow_CreatureOnly,
                    count = 8,
                    probability = 0.02f
                },
                new()
                {
                    biome = BiomeType.Mesas_Open,
                    count = 9,
                    probability = 0.03f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.JasperThalassacean,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LostRiverCorridor_Open_CreatureOnly,
                    count = 1,
                    probability = 0.03f
                },
                new()
                {
                    biome = BiomeType.LostRiverJunction_Open_CreatureOnly,
                    count = 1,
                    probability = 0.03f
                },
                new()
                {
                    biome = BiomeType.BonesField_Skeleton_Open_CreatureOnly,
                    count = 1,
                    probability = 0.04f
                },
                new()
                {
                    biome = BiomeType.BonesField_LakePit_Open_CreatureOnly,
                    count = 1,
                    probability = 0.03f
                },
                new()
                {
                    biome = BiomeType.GhostTree_Open_Big_CreatureOnly,
                    count = 1,
                    probability = 0.03f
                },
                new()
                {
                    biome = BiomeType.Canyon_Open_CreatureOnly,
                    count = 1,
                    probability = 0.03f
                },
                new()
                {
                    biome = BiomeType.BonesField_Corridor_CreatureOnly,
                    count = 1,
                    probability = 0.03f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.JellySpinner,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.BloodKelp_Grass,
                    probability = 0.2f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.BloodKelp_OpenDeep_CreatureOnly,
                    probability = 0.075f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.BloodKelp_TrenchWall,
                    probability = 0.3f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.BloodKelp_CaveFloor,
                    probability = 0.6f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_IslandTop,
                    probability = 1.4f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_ValleyFloor,
                    probability = 0.05f,
                    count = 5
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenShallow_CreatureOnly,
                    probability = 0.06f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenDeep_CreatureOnly,
                    probability = 0.06f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.LostRiverCorridor_Open_CreatureOnly,
                    probability = 0.12f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.LostRiverJunction_Open_CreatureOnly,
                    probability = 0.12f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.BonesField_Corridor_CreatureOnly,
                    probability = 0.12f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.BonesField_Open_Creature,
                    probability = 0.12f,
                    count = 6
                },
                new()
                {
                    biome = BiomeType.BonesField_Corridor_CreatureOnly,
                    probability = 0.12f,
                    count = 6
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.RibbonRay,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.MushroomForest_GiantTreeExterior,
                    probability = 2f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_GiantTreeExteriorBase,
                    probability = 0.9f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_CaveSand,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_Grass,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_MushroomTreeBase,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MushroomForest_Sand,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.SafeShallows_ShellTunnelHuge,
                    count = 2,
                    probability = 3f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.StellarThalassacean,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_OpenShallow_CreatureOnly,
                    count = 1,
                    probability = 0.01f
                },
                new()
                {
                    biome = BiomeType.GrandReef_OpenDeep_CreatureOnly,
                    count = 1,
                    probability = 0.01f
                },
                new()
                {
                    biome = BiomeType.KooshZone_OpenShallow_CreatureOnly,
                    count = 1,
                    probability = 0.05f
                },
                new()
                {
                    biome = BiomeType.KooshZone_OpenDeep_CreatureOnly,
                    count = 1,
                    probability = 0.01f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenShallow_CreatureOnly,
                    count = 1,
                    probability = 0.01f
                },
                new()
                {
                    biome = BiomeType.Dunes_OpenDeep_CreatureOnly,
                    count = 1,
                    probability = 0.01f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.TriangleFish,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.SafeShallows_Grass,
                    count = 2,
                    probability = 0.3f
                },
                new()
                {
                    biome = BiomeType.SafeShallows_Plants,
                    count = 2,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.SafeShallows_SandFlat,
                    count = 2,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.SparseReef_Coral,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.SparseReef_OpenDeep_CreatureOnly,
                    count = 3,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.KooshZone_Sand,
                    count = 3,
                    probability = 1.5f
                },
                new()
                {
                    biome = BiomeType.KooshZone_Grass,
                    count = 3,
                    probability = 2f
                },
                new()
                {
                    biome = BiomeType.CrashZone_Sand,
                    count = 3,
                    probability = 0.4f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Twisteel,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenShallow_CreatureOnly,
                    probability = 0.003f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_OpenDeep_CreatureOnly,
                    probability = 0.003f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_IslandSides,
                    probability = 0.04f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_ValleyLedge,
                    probability = 0.02f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_UniqueCreatures,
                    probability = 0.04f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.BloodKelp_WreckCreatures,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.KooshZone_Grass,
                    probability = 0.02f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_Wall,
                    probability = 0.02f,
                    count = 2
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Pyrambassis,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.UnderwaterIslands_ValleyFloor,
                    probability = 0.01f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_Ground,
                    probability = 0.02f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_BlueCoral,
                    probability = 0.01f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_TrenchWall,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.BloodKelp_CaveFloor,
                    probability = 0.05f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.KooshZone_Grass,
                    probability = 0.08f,
                    count = 1
                }
            });


        RegisterFishSpawns(CreaturePrefabManager.Dragonfly,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.Mountains_Birds,
                    probability = 2.5f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.FloatingIslands_Birds,
                    probability = 0.2f,
                    count = 1
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.GrandGliderEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_CaveFloor,
                    count = 1,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.Mountains_CaveFloor,
                    count = 1,
                    probability = 1f
                },
                new()
                {
                    biome = BiomeType.Mesas_Top,
                    count = 1,
                    probability = 2f
                },
                new()
                {
                    biome = BiomeType.CragField_Ground,
                    count = 1,
                    probability = 0.04f
                },
                new()
                {
                    biome = BiomeType.CragField_Sand,
                    count = 1,
                    probability = 0.04f
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.GulperLeviathanEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.UnderwaterIslands_IslandPlants,
                    count = 1,
                    probability = 0.24f
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.JasperThalassaceanEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LostRiverCorridor_LakeFloor,
                    count = 1,
                    probability = 1f
                },
                new()
                {
                    biome = BiomeType.LostRiverJunction_LakeFloor,
                    count = 1,
                    probability = 1f
                },
                new()
                {
                    biome = BiomeType.BonesField_Lake_Floor,
                    count = 1,
                    probability = 1f
                },
                new()
                {
                    biome = BiomeType.BonesField_Cave_Ground,
                    count = 1,
                    probability = 1f
                },
                new()
                {
                    biome = BiomeType.Canyon_Lake_Floor,
                    count = 1,
                    probability = 1f
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.StellarThalassaceanEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.GrandReef_CaveFloor,
                    count = 1,
                    probability = 2f
                },
                new()
                {
                    biome = BiomeType.KooshZone_CaveFloor,
                    count = 1,
                    probability = 2f
                },
                new()
                {
                    biome = BiomeType.Dunes_CaveFloor,
                    count = 1,
                    probability = 2f
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.TwisteelEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.UnderwaterIslands_IslandCaveFloor,
                    count = 1,
                    probability = 0.8f
                },
                new()
                {
                    biome = BiomeType.UnderwaterIslands_ValleyFloor,
                    count = 1,
                    probability = 0.09f
                },
                new()
                {
                    biome = BiomeType.BloodKelp_Grass,
                    count = 1,
                    probability = 0.05f
                }
            });
        
        RegisterEggSpawns(CreaturePrefabManager.PyrambassisEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.UnderwaterIslands_ValleyFloor,
                    count = 1,
                    probability = 0.05f
                },
                new()
                {
                    biome = BiomeType.DeepGrandReef_Ground,
                    count = 1,
                    probability = 0.02f
                },
                new()
                {
                    biome = BiomeType.BloodKelp_CaveFloor,
                    count = 1,
                    probability = 0.06f
                },
                new()
                {
                    biome = BiomeType.KooshZone_Grass,
                    count = 1,
                    probability = 0.01f
                }
            });
    }

    public static partial void RegisterCoordinatedSpawns()
    {
        // --- GULPER LEVIATHAN ---

        // Mountains & Bulb Zone 1
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(1169, -370, 903)));
        // Mountains & Bulb Zone 2
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(1400, -348, 1281)));
        // Underwater Islands 1
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(-72, -300, 867)));
        // Underwater Islands 1
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(-174, -460, 1070)));
        // Underwater Islands & Mountains Border
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(-49, -308, 1184)));
        // Underwater Islands & Bulb Zone Border
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(-265, -287, 1118)));
        // Floating Island
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(-717, -100, -1088)));
        // Lifepod 2
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(-573, -448, 1311)));
        // Blood Kelp Trench
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(-970, -216, -509)));

        // --- GULPER LEVIATHAN BABY ---

        // Mountain Island
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperBabyPrefab.ClassID,
            new Vector3(364, -10, 1172)));
        // Mountains & Bulb Zone Border
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperBabyPrefab.ClassID,
            new Vector3(1135, -364, 900)));
        // Floating Island 1
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperBabyPrefab.ClassID,
            new Vector3(-711, -12, -1071)));
        // Floating Island 2
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.GulperBabyPrefab.ClassID,
            new Vector3(-690, -3, -948)));

        // --- GULPER LEVIATHAN EGG ---
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawnsForOneTechType(
            CreaturePrefabManager.GulperLeviathanEgg.TechType,
            new SpawnLocation[]
            {
                new(new Vector3(-702.51f, -0.86f, -963.24f), new Vector3(18, 358, 345)),
                new(new Vector3(-715.52f, -0.97f, -951.93f), new Vector3(0, 0, 250)),
                new(new Vector3(-696.33f, -1f, -1069.41f), new Vector3(13, 0, 2)),
                new(new Vector3(-727.81f, 0.10f, -1085.91f), new Vector3(278, 0, 354)),
                new(new Vector3(393.37f, -9.91f, 1187.26f), new Vector3(77, 60, 180)),
                new(new Vector3(-831.56f, -1.95f, -966.07f), new Vector3(77, 60, 180))
            }
        );


        // -- DRAGONFLY --

        var dragonflyRandomGenerator = new System.Random(19872024);
        for (int i = 0; i < 60; i++)
        {
            var angle = (float) dragonflyRandomGenerator.NextDouble() * Mathf.PI * 2f;
            var distance = Mathf.Pow((float) dragonflyRandomGenerator.NextDouble(), 1/2f) * 1500f;
            var height = 30 + (float) dragonflyRandomGenerator.NextDouble() * 70;
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.Dragonfly.ClassID,
                new Vector3(Mathf.Cos(angle) * distance, height, Mathf.Sin(angle) * distance)));
        }

        // Aurora dragonflies

        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.Dragonfly.ClassID,
            new Vector3(1438.42f, 59.95f, 371.04f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.Dragonfly.ClassID,
            new Vector3(1497.71f, 32.29f, 333.62f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.Dragonfly.ClassID,
            new Vector3(1544.99f, 27.92f, 274.32f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.Dragonfly.ClassID,
            new Vector3(1490.97f, 36.88f, 264.48f)));
        CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(new SpawnInfo(CreaturePrefabManager.Dragonfly.ClassID,
            new Vector3(1204.73f, 125.56f, 55.31f)));
    }

    public static partial void ModifyBaseGameSpawns()
    {
        var boneSharkClassId = CraftData.GetClassIdForTechType(TechType.BoneShark);
        LootDistributionHandler.EditLootDistributionData(boneSharkClassId,
            BiomeType.UnderwaterIslands_OpenDeep_CreatureOnly, 0f, 0);
        LootDistributionHandler.EditLootDistributionData(boneSharkClassId, BiomeType.UnderwaterIslands_ValleyFloor, 0f,
            0);
        LootDistributionHandler.EditLootDistributionData(boneSharkClassId, BiomeType.UnderwaterIslands_IslandSides, 0f,
            0);
        LootDistributionHandler.EditLootDistributionData(boneSharkClassId, BiomeType.UnderwaterIslands_ValleyLedge, 0f,
            0);
        var bloomPlanktonClassId = CraftData.GetClassIdForTechType(TechType.Bloom);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.TreeCove_Ground, 0.05f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.TreeCove_Wall, 0.05f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.TreeCove_Ceiling, 0.05f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.TreeCove_TreeOpen_CreatureOnly,
            0.25f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.Kelp_CaveFloor, 0.3f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.GrassyPlateaus_CaveCeiling,
            0.4f, 1);
        LootDistributionHandler.EditLootDistributionData(bloomPlanktonClassId, BiomeType.DeepGrandReef_Ceiling, 0.25f,
            1);
        var whiteCaveCrawler = "7ce2ca9d-6154-4988-9b02-38f670e741b8";
        LootDistributionHandler.EditLootDistributionData(whiteCaveCrawler, BiomeType.DeepGrandReef_Ceiling, 0.5f, 3);
        LootDistributionHandler.EditLootDistributionData(whiteCaveCrawler, BiomeType.DeepGrandReef_Wall, 0.2f, 3);
        LootDistributionHandler.EditLootDistributionData(whiteCaveCrawler, BiomeType.SeaTreaderPath_CaveCeiling, 2f, 1);
    }
}
#endif