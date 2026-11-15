using Nautilus.Json;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace ModStructureHelperPlugin;

[Menu("Mod Structure Helper")]
public class ModConfig : ConfigFile
{
    [Toggle("Use consistent handle scale")]
    public bool ConsistentHandleScales = true;
    [Slider("Handle scale multiplier", 0.1f, 5f, DefaultValue = 2f, Step = 0.1f, Format = "{0:F1}x")]
    public float HandleScaleMultiplier = 2f;
    [Slider("Brush rotate speed", 0.1f, 10f, DefaultValue = 1f, Step = 0.1f, Format = "{0:F1}x")]
    public float BrushRotateSpeed = 1f;
    [Slider("Brush scale speed", 0.1f, 5f, DefaultValue = 1f, Step = 0.1f, Format = "{0:F1}x")]
    public float BrushScaleSpeed = 1f;
    [Toggle("Browser users item icons")]
    public bool UseItemIconsInBrowser = false;
    [Toggle("Browser uses debug names")]
    public bool BrowserUsesDebugNames = false;
    [Toggle("Lock tools while browsing entities")]
    public bool LockToolsWhileEntityBrowserIsActive = false;
    [Toggle("Click off to unselect")]
    public bool ClickOffToUnselect = true;
    [Toggle("Render all triggers on selected objects")]
    public bool AlwaysRenderSelectedTriggerColliders = false;
    [Toggle("Autosave structures on load")]
    public bool AutosaveStructureOnLoad = true;
    [Toggle("Autosave structures periodically")]
    public bool AutosaveStructureOverTime = true;
    [Slider("Autosave delay (minutes)", 1, 10, DefaultValue = 3, Step = 1, Format = "{0} minutes")]
    public int AutosaveDelay = 3;
    [Slider("Max autosave files", 3, 500, DefaultValue = 100, Step = 1)]
    public int MaxAutosaveFiles = 100;
}