using UnityEngine;

namespace TwistyBridgesObjectCleanser;

public sealed class DestroyAfterDelay : MonoBehaviour
{
    public float delay = 1f;
    
    private void Start()
    {
        Destroy(gameObject, delay);
    }
}