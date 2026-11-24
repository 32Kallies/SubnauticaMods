using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace ModStructureHelperPlugin.Utility;

public static class ObjectStripper
{
    private static readonly Type[] WhitelistedTypes = new[]
    {
        typeof(Transform),
        typeof(Renderer),
        typeof(MeshRenderer),
        typeof(MeshFilter),
        typeof(ParticleSystem),
        typeof(ParticleSystemForceField),
        typeof(UIBehaviour),
        typeof(Canvas),
        typeof(CanvasRenderer),
        typeof(SkyApplier),
        typeof(LODGroup),
        typeof(Animator)
    };
    
    public static void StripAllChildren(GameObject obj)
    {
        foreach (var component in obj.GetComponentsInChildren<Component>(true))
        {
            var whitelisted = false;
            var type = component.GetType();
            foreach (var whitelistedType in WhitelistedTypes)
            {
                if (whitelistedType.IsAssignableFrom(type)) whitelisted = true;
            }

            if (whitelisted) continue;
            // The following components are often relied on by other components, so to be safe, they will also be
            // manually disabled in case they can't be removed due to the chance of RequireComponent attributes existing
            switch (component)
            {
                case Collider col:
                    col.enabled = false;
                    break;
                case Rigidbody rb:
                    rb.isKinematic = true;
                    break;
                case Behaviour behaviour:
                    behaviour.enabled = false;
                    break;
            }
            Object.Destroy(component);
        }
    }
}