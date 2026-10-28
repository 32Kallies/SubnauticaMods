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
        gameMaterial.SetTexture(ShaderPropertyID._MainTex, customRenderer.sharedMaterial.GetTexture("_MainTex"));
        gameMaterial.SetTexture(ShaderPropertyID._SpecTex, customRenderer.sharedMaterial.GetTexture("_MainTex"));
        gameMaterial.SetTexture(ShaderPropertyID._BumpMap, customRenderer.sharedMaterial.GetTexture("_BumpMap"));
        gameMaterial.SetTexture(ShaderPropertyID._Illum, customRenderer.sharedMaterial.GetTexture("_EmissionMap"));

        var thrusterData = Plugin.ReplacementData;
        if (thrusterData != null)
        {
            MoveThruster(modelTransform,"xSmllThruster", thrusterData.thruster0Position, thrusterData.thruster0Rotation);
            MoveThruster(modelTransform,"xSmllThruster (1)", thrusterData.thruster1Position, thrusterData.thruster1Rotation);
            MoveThruster(modelTransform,"xSmllThruster (2)", thrusterData.thruster2Position, thrusterData.thruster2Rotation);
            MoveThruster(modelTransform,"xSmllThruster (3)", thrusterData.thruster3Position, thrusterData.thruster3Rotation);
        }
        else
        {
            Plugin.Logger.LogWarning("Thruster position and rotation replacement data not found");
        }
    }

    private static void MoveThruster(Transform parent, string thrusterName, Vector3 position, Vector3 rotation)
    {
        var thruster = parent.transform.Find(thrusterName);
        thruster.localPosition = position;
        thruster.localRotation = Quaternion.Euler(rotation);
    }
}