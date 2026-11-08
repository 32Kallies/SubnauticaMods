using Nautilus.Utility;
using Nautilus.Utility.MaterialModifiers;
using UnityEngine;

namespace SeaVoyager;

public class LightmapMaterialModifier : MaterialModifier
{
    /// <summary>
    /// Creates an instance of a LightmapMaterialModifier for use in <see cref="MaterialUtils.ApplySNShaders"/>.
    /// Automatically applies Occlusion materials (defined in Unity) to the lightmap. Lightmaps use UV1, not UV0.
    /// Use the "Standard (Specular setup for lightmap)" shader in Unity. 
    /// </summary>
    /// <param name="strength">The intensity of the lightmaps.</param>
    public LightmapMaterialModifier(float strength)
    {
        _strength = strength;
    }

    private readonly float _strength;
    
    private static readonly int OcclusionMap = Shader.PropertyToID("_OcclusionMap");

    public override void EditMaterial(Material material, Renderer renderer, int materialIndex,
        MaterialUtils.MaterialType materialType)
    {
        if (material.HasProperty(OcclusionMap)) return;
            
        var occlusionMap = material.GetTexture(OcclusionMap);
        if (occlusionMap == null) return;
            
        material.SetTexture("_Lightmap", occlusionMap);
        material.SetFloat("_LightmapStrength", _strength);
        material.EnableKeyword("UWE_LIGHTMAP");
    }
}