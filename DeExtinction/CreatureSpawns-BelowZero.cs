using System.Collections.Generic;
using Nautilus.Handlers;
using UnityEngine;
using static UnityEngine.Mathf;

#if BELOWZERO
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
                    biome = BiomeType.LilyPads_Crevice_Open,
                    probability = 0.8f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.LilyPads_Deep_Generic,
                    probability = 0.4f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.LilyPads_Deep_Ground,
                    probability = 0.4f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.SparseArctic_Open,
                    probability = 0.8f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_Open,
                    probability = 0.5f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_CaveOuter_Open,
                    probability = 1f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.ThermalSpires_Open,
                    probability = 0.3f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.TreeSpires_Ground,
                    probability = 0.8f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.MiningSite_Ground,
                    probability = 1f,
                    count = 3
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.CitrineClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Grass,
                    probability = 0.4f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.SparseArctic_Open,
                    probability = 0.5f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_Grass,
                    probability = 0.7f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_CaveOuter_Open,
                    probability = 1f,
                    count = 2
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.EmeraldClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Ground,
                    probability = 0.4f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.LilyPads_Island_Open,
                    probability = 0.4f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.LilyPads_Islands_Grass,
                    probability = 0.4f,
                    count = 4
                },
                new()
                {
                    biome = BiomeType.LilyPads_Crevice_Open,
                    probability = 0.1f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_Grass,
                    probability = 0.6f,
                    count = 2
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.RubyClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Deep_Open,
                    probability = 0.3f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.FabricatorCavern_Open,
                    probability = 0.3f,
                    count = 5
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Ground,
                    probability = 0.2f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Deep_Open,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.ThermalSpires_Open,
                    probability = 0.2f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.ThermalSpires_Cave_Ground,
                    probability = 0.2f,
                    count = 4
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.SapphireClownPincher,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.TwistyBridges_Open,
                    count = 2,
                    probability = 0.4f
                },
                new()
                {
                    biome = BiomeType.TwistyBridges_Shallow_Ground,
                    count = 2,
                    probability = 1f
                },
                new()
                {
                    biome = BiomeType.CrystalCave_Generic,
                    count = 4,
                    probability = 0.3f
                },
                new()
                {
                    biome = BiomeType.TreeSpires_Ground,
                    count = 3,
                    probability = 1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Axetail,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.TwistyBridges_Deep_Open,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.TwistyBridges_Shallow_Open,
                    probability = 0.3f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.Arctic_Open,
                    probability = 0.4f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.CrystalCave_Generic,
                    probability = 0.3f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.FabricatorCavern_Ground,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Ground,
                    probability = 0.5f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Deep_Ground,
                    probability = 0.5f,
                    count = 3
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Filtorb,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.Arctic_Open,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.SparseArctic_Open,
                    count = 1,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Open,
                    count = 3,
                    probability = 0.13f
                },
                new()
                {
                    biome = BiomeType.ThermalSpires_Open,
                    count = 1,
                    probability = 0.3f
                },
                new()
                {
                    biome = BiomeType.TreeSpires_Open,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.TreeSpires_OpenDeep,
                    count = 4,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.GlacialConnection_Open,
                    count = 4,
                    probability = 0.1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.FloralFiltorb,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Open,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.LilyPads_Island_Open,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.LilyPads_Islands_Grass,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.LilyPads_Crevice_Open,
                    count = 3,
                    probability = 0.4f
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_Open,
                    count = 3,
                    probability = 0.4f
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_CaveOuter_Open,
                    count = 2,
                    probability = 2f
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_CaveInner_Open,
                    count = 2,
                    probability = 1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.GrandGlider,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.TwistyBridges_Open,
                    count = 3,
                    probability = 0.01f
                },
                new()
                {
                    biome = BiomeType.TwistyBridges_Shallow_Open,
                    count = 1,
                    probability = 0.08f
                },
                new()
                {
                    biome = BiomeType.LilyPads_Island_Open,
                    count = 3,
                    probability = 0.08f
                },
                new()
                {
                    biome = BiomeType.FabricatorCavern_Open,
                    count = 5,
                    probability = 0.15f
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Open,
                    count = 4,
                    probability = 0.07f
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Deep_Open,
                    count = 1,
                    probability = 0.05f
                },
                new()
                {
                    biome = BiomeType.TreeSpires_Open,
                    count = 4,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.TreeSpires_OpenDeep,
                    count = 4,
                    probability = 0.08f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.JasperThalassacean,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Open,
                    count = 1,
                    probability = 0.08f
                },
                new()
                {
                    biome = BiomeType.LilyPads_Island_Open,
                    count = 1,
                    probability = 0.03f
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Open,
                    count = 1,
                    probability = 0.06f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.JellySpinner,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Deep_Open,
                    probability = 0.2f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.TwistyBridges_Deep_Open,
                    probability = 0.4f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.TwistyBridges_Shallow_Open,
                    probability = 0.05f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.Arctic_Open,
                    probability = 0.3f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.SparseArctic_Open,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.CrystalCave_Generic,
                    probability = 0.05f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.TreeSpires_Open,
                    probability = 0.06f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.TreeSpires_OpenDeep,
                    probability = 0.2f,
                    count = 3
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.RibbonRay,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Islands_Grass,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.LilyPads_Grass,
                    probability = 0.5f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.LilyPads_Ground,
                    probability = 0.4f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.LilyPads_Crevice_Grass,
                    probability = 0.4f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.TwistyBridges_Shallow_Open,
                    probability = 0.8f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.TwistyBridges_Open,
                    probability = 0.8f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_Open,
                    probability = 1f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_CaveOuter_Open,
                    probability = 2f,
                    count = 3
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Ground,
                    probability = 0.4f,
                    count = 1
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.StellarThalassacean,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.Arctic_Open,
                    count = 1,
                    probability = 0.09f
                },
                new()
                {
                    biome = BiomeType.TreeSpires_Open,
                    count = 1,
                    probability = 0.02f
                },
                new()
                {
                    biome = BiomeType.TreeSpires_OpenDeep,
                    count = 1,
                    probability = 0.04f
                },
                new()
                {
                    biome = BiomeType.CrystalCave_Open,
                    count = 1,
                    probability = 0.05f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.TriangleFish,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Grass,
                    count = 2,
                    probability = 0.5f
                },
                new()
                {
                    biome = BiomeType.LilyPads_Islands_Coral,
                    count = 2,
                    probability = 0.6f
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_Grass,
                    count = 2,
                    probability = 0.6f
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_Open,
                    count = 1,
                    probability = 0.2f
                },
                new()
                {
                    biome = BiomeType.ArcticKelp_CaveOuter_Open,
                    count = 1,
                    probability = 1f
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Twisteel,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Open,
                    probability = 0.03f,
                    count = 2
                },
                new()
                {
                    biome = BiomeType.LilyPads_Island_Open,
                    probability = 0.03f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.Arctic_Generic,
                    probability = 0.01f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.CrystalCave_Open,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Open,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Deep_Open,
                    probability = 0.04f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.GlacialConnection_Open,
                    probability = 0.04f,
                    count = 1
                }
            });
        
                RegisterFishSpawns(CreaturePrefabManager.TwisteelJuvenile,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.TwistyBridges_Open,
                    probability = 0.25f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.LilyPads_Deep_Open,
                    probability = 0.09f,
                    count = 1
                }
            });

        RegisterFishSpawns(CreaturePrefabManager.Pyrambassis,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.TwistyBridges_Deep_Open,
                    probability = 0.23f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.CrystalCave_Open,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Deep_Open,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.TreeSpires_Ground,
                    probability = 0.05f,
                    count = 1
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.GrandGliderEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.TwistyBridges_Coral,
                    count = 1,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.FabricatorCavern_Ground,
                    count = 1,
                    probability = 0.1f
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.GulperLeviathanEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.SparseArctic_Ground,
                    count = 1,
                    probability = 0.02f
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.JasperThalassaceanEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.LilyPads_Crevice_Ground,
                    count = 1,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.LilyPads_Crevice_Grass,
                    count = 1,
                    probability = 0.2f
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.StellarThalassaceanEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.EastArctic_Ground,
                    count = 1,
                    probability = 0.1f
                },
                new()
                {
                    biome = BiomeType.WestArctic_Ground,
                    count = 1,
                    probability = 0.1f
                }
            });

        RegisterEggSpawns(CreaturePrefabManager.TwisteelEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.TwistyBridges_Cave_Coral,
                    count = 1,
                    probability = 0.5f
                },
                new()
                {
                    biome = BiomeType.LilyPads_Grass,
                    count = 1,
                    probability = 0.1f
                }
            });
        
        RegisterEggSpawns(CreaturePrefabManager.PyrambassisEgg,
            new List<LootDistributionData.BiomeData>
            {
                new()
                {
                    biome = BiomeType.TwistyBridges_Deep_Ground,
                    probability = 0.1f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.CrystalCave_Ground,
                    probability = 0.02f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.PurpleVents_Deep_Ground,
                    probability = 0.05f,
                    count = 1
                },
                new()
                {
                    biome = BiomeType.TreeSpires_Ground,
                    probability = 0.08f,
                    count = 1
                }
            });
    }

    public static partial void RegisterCoordinatedSpawns()
    {
        // --- GULPER LEVIATHAN ---

        // Sparse arctic (juvenile)
        RegisterCoordinatedSpawn(CreaturePrefabManager.GulperJuvenilePrefab.PrefabInfo, new SpawnInfo(
            CreaturePrefabManager.GulperJuvenilePrefab.ClassID,
            new Vector3(260, -20, 38)));
        // Lily pad islands
        RegisterCoordinatedSpawn(CreaturePrefabManager.GulperPrefab.PrefabInfo, new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(240, -70, -815)));
        // Tree spires
        RegisterCoordinatedSpawn(CreaturePrefabManager.GulperPrefab.PrefabInfo, new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(-75, -380, -1615)));
        // West arctic
        RegisterCoordinatedSpawn(CreaturePrefabManager.GulperPrefab.PrefabInfo, new SpawnInfo(CreaturePrefabManager.GulperPrefab.ClassID,
            new Vector3(-640, -84, -1020)));


        // --- GULPER LEVIATHAN BABY ---

        // [NONE CURRENTLY]

        // -- DRAGONFLY --

        // Ocean dragonflies
        RegisterDragonflySpawns(80, new Vector3(0, 0, -600), 800, 400328426, 40f, 100f);
        // Spawn zone
        RegisterDragonflySpawns(5, new Vector3(-200, 0, 60), 80, 2491353, 30, 60);
        // Glacial basin
        RegisterDragonflySpawns(30, new Vector3(-1250, 0, -800), 200, 58103834, 40, 140);
        // Arctic spires
        RegisterDragonflySpawns(20, new Vector3(-1050, 0, 300), 400, 125434148, 110, 160);
        
        // --- PYRAMBASSIS ---
        
        RegisterCoordinatedSpawn(CreaturePrefabManager.Pyrambassis.PrefabInfo, new SpawnInfo(CreaturePrefabManager.Pyrambassis.ClassID,
            new Vector3(-266, -326, -308)));
        
        // --- TWISTEEL ---
        
        RegisterCoordinatedSpawn(CreaturePrefabManager.TwisteelJuvenile.PrefabInfo, new SpawnInfo(CreaturePrefabManager.TwisteelJuvenile.ClassID,
            new Vector3(678, -540, -1100)));
        RegisterCoordinatedSpawn(CreaturePrefabManager.TwisteelJuvenile.PrefabInfo, new SpawnInfo(CreaturePrefabManager.TwisteelJuvenile.ClassID,
            new Vector3(673, -538, -1094)));

    }

    private static void RegisterDragonflySpawns(int count, Vector3 centerPosition, float radius, int seed,
        float minHeight, float maxHeight)
    {
        var dragonflyRandomGenerator = new System.Random(seed);
        for (var i = 0; i < count; i++)
        {
            var angle = (float) dragonflyRandomGenerator.NextDouble() * PI * 2f;
            var distance = Pow((float) dragonflyRandomGenerator.NextDouble(), 1 / 2f) * radius;
            var height = minHeight + (float) dragonflyRandomGenerator.NextDouble() * (maxHeight - minHeight);
            RegisterCoordinatedSpawn(CreaturePrefabManager.Dragonfly.PrefabInfo, new SpawnInfo(CreaturePrefabManager.Dragonfly.ClassID,
                new Vector3(Cos(angle) * distance, height, Sin(angle) * distance) + centerPosition));
        }
    }

    public static partial void ModifyBaseGameSpawns()
    {
        // don't replace brute shark fully?
    }
}
#endif