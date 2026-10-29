using System.Collections.Generic;
using ECCLibrary;
using Nautilus.Assets;
using Nautilus.Handlers;

namespace DeExtinction;

public static partial class CreatureSpawns
{
    public static partial void RegisterLootDistribution();
    public static partial void ModifyBaseGameSpawns();
    public static partial void RegisterCoordinatedSpawns();

    private static void RegisterFishSpawns(CreatureAsset creature, List<LootDistributionData.BiomeData> biomeData)
    {
        if (CreatureSpawnsToggler.GetCanSpawn(creature.PrefabInfo))
            LootDistributionHandler.AddLootDistributionData(creature.PrefabInfo.ClassID, creature.PrefabInfo.PrefabFileName, biomeData);
    }
    
    private static void RegisterEggSpawns(PrefabInfo eggInfo, List<LootDistributionData.BiomeData> biomeData)
    {
        if (CreatureSpawnsToggler.GetCanSpawn(eggInfo))
            LootDistributionHandler.AddLootDistributionData(eggInfo.ClassID, eggInfo.PrefabFileName, biomeData);
    }
    
    private static void RegisterCoordinatedSpawn(PrefabInfo prefabInfo, SpawnInfo spawnInfo)
    {
        if (CreatureSpawnsToggler.GetCanSpawn(prefabInfo))
            CoordinatedSpawnsHandler.RegisterCoordinatedSpawn(spawnInfo);
    }
}