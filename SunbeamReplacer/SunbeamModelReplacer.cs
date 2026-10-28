using System;
using UnityEngine;

namespace SunbeamReplacer;

public static class SunbeamModelReplacer
{
    public static void ReplaceSunbeamModel(GameObject modelPrefab)
    {
        var modelTransform = modelPrefab.transform.GetChild(0); // xShip
        ParticleSystemRenderer gameRenderer = modelTransform.GetComponent<ParticleSystemRenderer>();

        string modelName = "NewSunbeamModel";
        var newSunbeamModel = Plugin.Bundle.LoadAsset<GameObject>(modelName);
        if (newSunbeamModel == null)
        {
            throw new NullReferenceException(
                $"No sunbeam model found in bundle {Plugin.Bundle} with name '{modelName}");
        }
        Renderer customRenderer = newSunbeamModel.GetComponent<Renderer>();
        if (customRenderer == null)
        {
            throw new NullReferenceException($"No renderer found on new sunbeam model {newSunbeamModel}");
        }
        var mesh = customRenderer.GetComponent<MeshFilter>().sharedMesh;

        gameRenderer.mesh = mesh;
        
        var gameMaterial = gameRenderer.material;
        gameMaterial.SetTexture("_MainTex", customRenderer.sharedMaterial.GetTexture("_MainTex"));
        gameMaterial.SetTexture("_SpecTex", customRenderer.sharedMaterial.GetTexture("_MainTex"));
        gameMaterial.SetTexture("_BumpMap", customRenderer.sharedMaterial.GetTexture("_BumpMap"));
    }
}