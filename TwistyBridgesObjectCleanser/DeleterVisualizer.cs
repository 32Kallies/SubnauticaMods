using System.Collections.Generic;
using UnityEngine;

namespace TwistyBridgesObjectCleanser;

public class DeleterVisualizer : MonoBehaviour
{
    public Renderer renderer;
    
    private static readonly HashSet<DeleterVisualizer> All = new();
    private static bool _shown;
    
    private void OnEnable()
    {
        All.Add(this);
        OnRegister();
    }

    private void OnDisable() => All.Remove(this);

    private void OnRegister()
    {
        renderer.enabled = _shown;
    }
    
    public static void ToggleShown() => SetShown(!_shown);
    
    private static void SetShown(bool shown)
    {
        _shown = shown;
        foreach (var visualizer in All)
        {
            if (visualizer != null) visualizer.renderer.enabled = shown;
        }
    }
}