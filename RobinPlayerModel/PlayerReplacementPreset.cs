using System;
using System.Collections;
using System.Collections.Generic;
using BZtoSNLoader;
using UnityEngine;

namespace RobinPlayerModel;

public class PlayerReplacementPreset
{
    public required BZAssetLoader.AssetKey PlayerAssetPath { get; init; }
    public required PlayerModelReplacement[] Replacements { get; init; }

    public IEnumerator ReplacePlayerModel(Player player)
    {
        if (Replacements.Length == 0)
        {
            Plugin.Logger.LogWarning("Player Replacement preset has no replacements defined. Skipping.");
            yield break;
        }

        TaskResult<GameObject> loadResult = new TaskResult<GameObject>();
        var loadAsset = BZAssetLoader.LoadAssetAsync(PlayerAssetPath, loadResult);
        yield return loadAsset;
        GameObject bzPlayerView = loadResult.Get();
        if (bzPlayerView == null)
        {
            throw new Exception($"Asset at path {PlayerAssetPath} was not found.");
        }

        Transform sn1PlayerView = player.transform.Find("body/player_view"); // Player/body/player_view

        int replacements = 0;
        foreach (var replacement in Replacements)
        {
            var bzMeshTransform = bzPlayerView.transform.Find(replacement.BzViewModelPath);
            if (bzMeshTransform == null)
            {
                throw new KeyNotFoundException(
                    $"Child not found at path in BZ player model. Path: {replacement.BzViewModelPath}");
            }

            var sn1MeshTransform = sn1PlayerView.transform.Find(replacement.OriginalViewModelPath);
            if (sn1MeshTransform == null)
            {
                throw new KeyNotFoundException(
                    $"Child not found at path in SN1 player model. Path: {replacement.OriginalViewModelPath}");
            }

            var bzMesh = bzMeshTransform.GetComponent<SkinnedMeshRenderer>();
            var sn1Mesh = sn1MeshTransform.GetComponent<SkinnedMeshRenderer>();

            OverrideSkinnedMeshRenderer(sn1Mesh, bzMesh);
            replacements++;
        }
        
        Plugin.Logger.LogInfo($"Replaced {replacements} meshes on the Player");
    }

    private void OverrideSkinnedMeshRenderer(SkinnedMeshRenderer currentMesh, SkinnedMeshRenderer referenceMesh)
    {
        Transform[] bones = new Transform[referenceMesh.bones.Length];
        for (int i = 0; i < referenceMesh.bones.Length; i++)
        {
            bool found = false;
            for (int j = 0; j < currentMesh.bones.Length; j++)
            {
                if (currentMesh.bones[j].name.Equals(referenceMesh.bones[i].name, StringComparison.OrdinalIgnoreCase))
                {
                    bones[i] = currentMesh.bones[j];
                    found = true;
                }
            }

            if (!found)
            {
                Plugin.Logger.LogWarning(
                    $"Donor (Robin) had bone of name '{referenceMesh.bones[i]}' which was not found in the target (Player) rig");
            }
        }
        
        currentMesh.sharedMesh = referenceMesh.sharedMesh;
        currentMesh.sharedMaterials = referenceMesh.sharedMaterials;
        currentMesh.bones = bones;
        currentMesh.localBounds = referenceMesh.localBounds;
        currentMesh.quality = referenceMesh.quality;
    }
}