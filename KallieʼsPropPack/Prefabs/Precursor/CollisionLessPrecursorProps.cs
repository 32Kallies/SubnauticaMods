
using Nautilus.Assets;
using Nautilus.Assets.PrefabTemplates;
using UnityEngine;

namespace KallieʼsPropPack.Prefabs.Precursor;

public static class CollisionLessPrecursorProps
{
    private static readonly Prop[] Props = {
        // PRECURSOR BLOCK DECO
        new("2a836e22-26fc-4853-98c8-fcb1f639f9ad", "nocollision_precursor_block_deco_glow"), // GLOW
        new("3c9344a2-4715-4773-9c58-dc0437002278", "nocollision_precursor_block_deco_square3"), // CURVED SQUARE
        new("5ecd846d-1629-4d3c-9119-f4f16179a408", "nocollision_precursor_block_deco_square2"), // DETAILED SQUARE
        new("96edb813-c7c7-4c44-9bf4-5f1975edeff8", "nocollision_precursor_block_deco_plain"), // NO DETAILS
        new("b7950b62-8f0b-46a5-903b-7f31480eb88b", "nocollision_precursor_block_deco_square1"), // FLAT SQUARE
        new("d3e933a0-1ba0-4f38-9733-9f81ca8ba444", "no_collision_precursor_block_deco_maze1"), // MAZE
        new("f72d8e76-691f-4ddb-a274-0fcc236dd43d", "no_collision_precursor_block_deco_weird"), // IDK??!
        new("fa5e644a-777b-4b54-a92a-0241752b8e06", "no_collision_precursor_block_deco_maze2") // MAZE
    };

    public static void RegisterAll()
    {
        foreach (var prop in Props)
        {
            var info = PrefabInfo.WithTechType(prop.NewName)
                .WithFileName("KallieʼsPropPack/Precursor/No Collision/" + prop.NewName);
            var prefab = new CustomPrefab(info);
            prefab.SetGameObject(new CloneTemplate(info, prop.CloneClassId)
            {
                ModifyPrefab = obj =>
                {
                    foreach (var collider in obj.GetComponentsInChildren<Collider>())
                    {
                        Object.DestroyImmediate(collider);
                    }
                }
            });
            prefab.Register();
        }
    }

    private struct Prop
    {
        public string CloneClassId { get; }
        public string NewName { get; }

        public Prop(string cloneClassId, string newName)
        {
            CloneClassId = cloneClassId;
            NewName = newName;
        }
    }
}