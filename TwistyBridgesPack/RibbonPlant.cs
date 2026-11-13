using System.Collections;
using BZtoSNLoader;
using Nautilus.Assets;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using UnityEngine.U2D;

namespace TwistyBridgesPack;

public static class RibbonPlant
{
    public static PrefabInfo Info { get; private set; }

    private const string TechTypeName = "GenericRibbon";

    public static void Register(SpriteAtlas atlas)
    {
        Info = PrefabInfo.WithTechType("TwistyPack_RibbonPlant")
            .WithFileName($"{Plugin.RootFolderName}/TwistyRibbonPlantPrefab");
        Info.WithIcon(atlas.GetSprite(TechTypeName.ToLowerInvariant()));
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(GetPrefabAsync);
        prefab.Register();

        var keys = new[] { TechTypeName, $"Tooltip_{TechTypeName}" };
        var translations = BZLocalizationLoader.GetLocalization(keys, true);
        LanguageHandler.SetLanguageLine(Info.ClassID, translations[0]);
        LanguageHandler.SetLanguageLine($"Tooltip_{Info.ClassID}", translations[1]);
    }

    private static IEnumerator GetPrefabAsync(IOut<GameObject> result)
    {
        TaskResult<GameObject> prefabLoad = new();
        yield return BZAssetLoader.LoadAssetAsync(
            new BZAssetLoader.AssetKey("WorldEntities/Flora/Expansion/Shared/generic_plant_ribbon_01.prefab"),
            prefabLoad);
        var model = prefabLoad.Get();
        var prefab = Object.Instantiate(model);
        prefab.SetActive(false);
        PrefabUtils.AddBasicComponents(prefab, Info.ClassID, Info.TechType, LargeWorldEntity.CellLevel.Near);
        PrefabUtils.AddWorldForces(prefab, 4, isKinematic: true);
        prefab.AddComponent<VFXSurface>().surfaceType = VFXSurfaceTypes.vegetation;
        prefab.AddComponent<Pickupable>();
        result.Set(prefab);
    }
}