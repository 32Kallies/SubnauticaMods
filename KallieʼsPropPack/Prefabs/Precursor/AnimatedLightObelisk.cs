using System.Collections;
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;
using UWE;

namespace KallieʼsPropPack.Prefabs.Precursor;

public static class AnimatedLightObelisk
{
    public static PrefabInfo Info { get; } = PrefabInfo.WithTechType("Obelisk_AnimatedLight")
        .WithFileName("KallieʼsPropPack/Precursor/Obelisk_AnimatedLight");
    
    public static void Register()
    {
        var prefab = new CustomPrefab(Info);
        prefab.SetGameObject(new CloneTemplate(Info, "02e525a1-dc51-4401-8caa-237eae0e32b8")
        {
            ModifyPrefab = obj =>
            {
                var activationComponents = obj.GetComponents<PrecursorActivatedPillar>();
                Object.DestroyImmediate(activationComponents[0]);
                activationComponents[1].extendedY = 0f;
                obj.transform.Find("precursor_deco_props_02 (1)").gameObject.SetActive(false);
                obj.transform.Find("precursor_deco_props_02").localPosition = Vector3.zero;
            }
        });
        prefab.Register();
    }
}