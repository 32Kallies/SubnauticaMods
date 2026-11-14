using System;
using System.Collections;
using System.Collections.Generic;
using BZtoSNLoader;
using Nautilus.Extensions;
using UnityEngine;

namespace RobinPlayerModel;

public sealed class PlayerReplacementPreset
{
    public required BZAssetLoader.AssetKey PlayerAssetPath { get; init; }
    public required PlayerModelReplacement[] Replacements { get; init; }
    public required Dictionary<string, BoneAdjustment> RigAdjustments { get; init; }
    public required AdditionalBone[] ExtraBones { get; init; }
    public required FalseBone[] FalseBones { get; init; }
    public required Action<GameObject> FinalTouches { get; init; }

    private const bool Verbose = true;

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

        Transform rigParent = sn1PlayerView.Find("export_skeleton");
        var boneMap = GetBoneMap(rigParent);

        AdjustRig(rigParent, bzPlayerView.transform.Find("export_skeleton"));

        foreach (var additionalBone in ExtraBones)
        {
            AddBoneIntoRig(additionalBone, boneMap);
        }

        Dictionary<string, SkinnedMeshRenderer> changedRenderers =
            new Dictionary<string, SkinnedMeshRenderer>(StringComparer.OrdinalIgnoreCase);
        
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
            changedRenderers[sn1Mesh.gameObject.name] = sn1Mesh;

            OverrideSkinnedMeshRenderer(sn1Mesh, bzMesh, boneMap);
            replacements++;
        }

        foreach (var falseBone in FalseBones)
        {
            Transform falseBoneTransform = AddFalseBone(falseBone, boneMap);
            foreach (var meshName in falseBone.RyleyMeshesToApplyTo)
            {
                if (changedRenderers.TryGetValue(meshName, out var mesh))
                {
                    var bones = mesh.bones;
                    int swapIndex = Array.IndexOf(bones, boneMap[falseBone.OriginalBoneName]);
                    if (swapIndex == -1)
                    {
                        if (Verbose)
                            Plugin.Logger.LogWarning($"Original bone not found: {falseBone.OriginalBoneName} (Mesh : {mesh})");
                        continue;
                    }
                    bones[swapIndex] = falseBoneTransform;
                    mesh.bones = bones;
                }
                else
                {
                    Plugin.Logger.LogWarning($"Could not apply false bone to mesh '{meshName}'");
                }
            }
        }
        
        if (Verbose)
            Plugin.Logger.LogInfo($"Replaced {replacements} meshes on the Player");
        
        FinalTouches.Invoke(sn1PlayerView.gameObject);
    }

    private void OverrideSkinnedMeshRenderer(SkinnedMeshRenderer currentMesh, SkinnedMeshRenderer referenceMesh, Dictionary<string, Transform> boneMap)
    {
        Transform[] bones = new Transform[referenceMesh.bones.Length];
        
        for (int i = 0; i < referenceMesh.bones.Length; i++)
        {
            var boneName = referenceMesh.bones[i].name;
            if (boneMap.TryGetValue(boneName, out Transform bone))
            {
                bones[i] = bone;
            }
            else
            {
                Plugin.Logger.LogWarning($"Donor (Robin) had bone of name '{boneName}' which was not found in the target (Player) rig");
            }
        }
        
        currentMesh.sharedMesh = referenceMesh.sharedMesh;
        currentMesh.sharedMaterials = referenceMesh.sharedMaterials;
        currentMesh.bones = bones;
        // currentMesh.localBounds = referenceMesh.localBounds;
        currentMesh.localBounds = new Bounds(Vector3.zero, Vector3.one * 5);
        currentMesh.quality = referenceMesh.quality;
        currentMesh.rootBone = boneMap[referenceMesh.rootBone.name];
    }

    private void AdjustRig(Transform rigRoot, Transform bzPlayerRig)
    {
        foreach (var adjustment in RigAdjustments)
        {
            Transform bone = rigRoot.SearchChild(adjustment.Key);
            if (bone == null)
            {
                Plugin.Logger.LogWarning($"Could not find bone by name {adjustment.Key}. Skipping.");
                continue;
            }

            var adjust = adjustment.Value;
            if (adjust.PositionAdjustmentMode != AdjustmentMode.None || adjust.RotationAdjustmentMode != AdjustmentMode.None)
            {
                var adjuster = bone.gameObject.AddComponent<RuntimeBoneAdjuster>();
                adjuster.positionMode = adjust.PositionAdjustmentMode;
                adjuster.position = adjust.Position;
                adjuster.rotationMode = adjust.RotationAdjustmentMode;
                adjuster.rotation = adjust.Rotation;
            }
        }

        return;
        // OLD code to copy *entire* BZ rig
        foreach (Transform child in rigRoot.GetComponentsInChildren<Transform>())
        {
            var name = child.name;
            if (name.Equals("head_rig"))
            {
                continue;
            }
            var bzBone = bzPlayerRig.SearchChild(name);
            if (bzBone == null)
            {
                if (Verbose) Plugin.Logger.LogWarning("Couldn't find BZ bone: " + child.name);
                continue;
            }
            var adjuster = child.gameObject.AddComponent<RuntimeBoneAdjuster>();
            adjuster.positionMode = AdjustmentMode.Offset;
            adjuster.position = bzBone.localPosition - child.localPosition;
            adjuster.rotationMode = AdjustmentMode.Offset;
            Quaternion difference = Quaternion.Inverse(child.localRotation) * bzBone.localRotation;
            Vector3 eulerDifference = difference.eulerAngles;
            adjuster.rotation = eulerDifference;
        }
        return;
    }

    private Dictionary<string, Transform> GetBoneMap(Transform rig)
    {
        Dictionary<string, Transform> allBones = new Dictionary<string, Transform>(StringComparer.OrdinalIgnoreCase);
        foreach (var bone in rig.GetComponentsInChildren<Transform>())
        {
            Add(bone, bone.gameObject.name);
        }
        return allBones;

        void Add(Transform bone, string name)
        {
            allBones.Add(name, bone);
        }
    }

    private void AddBoneIntoRig(AdditionalBone settings, Dictionary<string, Transform> boneMap)
    {
        var bone = new GameObject(settings.BoneName).transform;
        bone.parent = boneMap[settings.Parent];
        bone.localScale = Vector3.one;
        bone.localPosition = settings.LocalPosition;
        bone.localEulerAngles = settings.LocalRotation;
        boneMap.Add(settings.BoneName, bone);
    }
    
    private Transform AddFalseBone(FalseBone settings, Dictionary<string, Transform> boneMap)
    {
        var bone = new GameObject(settings.BoneName).transform;
        var original = boneMap[settings.OriginalBoneName];
        bone.parent = original.parent;
        bone.localScale = original.localScale;
        bone.localPosition = original.localPosition;
        bone.localRotation = original.localRotation;
        return bone;
    }
}