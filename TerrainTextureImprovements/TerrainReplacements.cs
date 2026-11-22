using System;
using UnityEngine;

namespace TerrainTextureImprovements;

// This entire class could be done way better, but I have so few texture replacements
// that I simply don't find it to be worth it currently
public static class TerrainReplacements
{
    public static void ReplaceBlockTypes(Voxeland voxeland)
    {
        var blockTypes = voxeland.types;
        for (var i = 0; i < blockTypes.Length; i++)
        {
            var block = blockTypes[i];
            if (block == null) continue;

            if (block.material)
                ReplaceMaterials(block.material, i);
            if (block.opaqueMaterial)
                ReplaceMaterials(block.opaqueMaterial, i);
        }
    }

    private static void ReplaceMaterials(Material material, int index)
    {
        // SAND 1: white with huge orange/brown splotches
        ReplaceTextureSet(material, "sand_01", "sandysoil_albedo",
            "sand_01_normal", "sandysoil_normal");
        
        // SAND 2: white with minimal details
        ReplaceTextureSet(material, "NA", "NA", "sand_02_normal", "sand_02_normal");
        
        // SAND 5: Often used as grassy plateaus sand (above rocks and such)
        ReplaceTextureSet(material, "NA", "NA", "sand_05_normal", "sand_05_normal");
        
        // CORAL 15: looks like vomit
        ReplaceTextureSet(material, "coral_15", "coral_15", "coral_15_normal", "coral_15_normal");
        
        /*
        // ROCK STRIATED 02: Grassy plateaus wall side rocks, layered, brownish, Minecraft mesa?
        ReplaceTextureSet(material, "Rock_striated_02", "Rock_striated_02",
            "Rock_striated_02_normal", "Rock_striated_02_normal");
        
        // ROCK STRIATED 02: Grassy plateaus wall side rocks, layered, grayer?
        ReplaceTextureSet(material, "Rock_striated_01", "Rock_striated_01",
            "Rock_striated_01_normal", "Rock_striated_01_normal");
            */

        if (index == 101)
        {
            material.SetFloat("_BorderBlendRange", 0.54f);
            material.SetFloat("_BorderBlendOffset", 0.52f);
        }
    }

    private static readonly int CapTexture = Shader.PropertyToID("_CapTexture");
    private static readonly int SideTexture = Shader.PropertyToID("_SideTexture");
    private static readonly int CapBumpMap = Shader.PropertyToID("_CapBumpMap");
    private static readonly int SideBumpMap = Shader.PropertyToID("_SideBumpMap");
    
    private static void ReplaceTextureSet(Material material, string mainTexName, string mainTexTarget, string normalName, string normalTarget)
    {
        // main tex
        ReplaceTexture(material, ShaderPropertyID._MainTex, mainTexName, mainTexTarget);
        ReplaceTexture(material, CapTexture, mainTexName, mainTexTarget);
        ReplaceTexture(material, SideTexture, mainTexName, mainTexTarget);
        
        // normal
        ReplaceTexture(material, ShaderPropertyID._BumpMap, normalName, normalTarget);
        ReplaceTexture(material, CapBumpMap, normalName, normalTarget);
        ReplaceTexture(material, SideBumpMap, normalName, normalTarget);
    }

    private static void ReplaceTexture(Material material, int propertyId, string sourceName, string replacementName)
    {
        if (!material.HasProperty(propertyId)) return;

        if (material.GetTexture(propertyId) is Texture2D texture
            && string.Equals(texture.name, sourceName, StringComparison.OrdinalIgnoreCase))
        {
            material.SetTexture(propertyId, Plugin.Bundle.LoadAsset<Texture2D>(replacementName));
        }
    }
}