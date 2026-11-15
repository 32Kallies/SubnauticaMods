using ModStructureHelperPlugin.Interfaces;
using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.UI;
using Nautilus.Extensions;
using UnityEngine;

namespace ModStructureHelperPlugin.Mono;

public class ObjectPivotCircle : MonoBehaviour, IOverlayIconData, ISelectionListener
{
    private static Sprite _loadedUnselectedSprite;
    private static Sprite _loadedSelectedSprite;
    
    private static Sprite _loadedUnselectedParticleSprite;
    private static Sprite _loadedSelectedParticleSprite;
    
    private static Sprite _loadedUnselectedLightSprite;
    private static Sprite _loadedSelectedLightSprite;
    
    public bool showName;
    public IconType type;
    public Color color = Color.white;

    private bool _selecting;
    
    public string Label => showName ? _objectName : null;
    public Sprite Icon => _selecting ? GetSelectedSprite() : GetDefaultSprite();
    public Vector3 Position => transform.position;
    public Color Color => color;
    public float Scale => 5f / Vector3.Distance(MainCameraControl.main.transform.position, transform.position);

    private string _objectName;
    
    public void OnCreation(OverlayIconInstance instance)
    {
        if (_loadedUnselectedSprite == null)
        {
            LoadSpritesIfNeeded();
        }
        
        _objectName = gameObject.name.TrimClone();
        
        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;
    }

    private static void LoadSpritesIfNeeded()
    {
        _loadedUnselectedSprite = Plugin.AssetBundle.LoadAsset<Sprite>("SelectionCircleDefault");
        _loadedSelectedSprite = Plugin.AssetBundle.LoadAsset<Sprite>("SelectionCircleSelected");
        _loadedUnselectedParticleSprite = Plugin.AssetBundle.LoadAsset<Sprite>("ParticleSelectionDefault");
        _loadedSelectedParticleSprite = Plugin.AssetBundle.LoadAsset<Sprite>("ParticleSelectionSelected");
        _loadedUnselectedLightSprite = Plugin.AssetBundle.LoadAsset<Sprite>("LightSelectionDefault");
        _loadedSelectedLightSprite = Plugin.AssetBundle.LoadAsset<Sprite>("LightSelectionSelected");
    }

    private Sprite GetDefaultSprite()
    {
        if (type == IconType.Light)
        {
            return _loadedUnselectedLightSprite;   
        }
        if (type == IconType.Particle)
        {
            return _loadedUnselectedParticleSprite;
        }
        return _loadedUnselectedSprite;
    }

    private Sprite GetSelectedSprite()
    {
        if (type == IconType.Light)
        {
            return _loadedSelectedLightSprite;   
        }
        if (type == IconType.Particle)
        {
            return _loadedSelectedParticleSprite;
        }
        return _loadedSelectedSprite;
    }

    public void OnObjectSelected()
    {
        _selecting = true;
    }

    public void OnObjectDeselected()
    {
        _selecting = false;
    }
    
    private void OnDisable()
    {
        OverlayIconManager.main.RemoveIcon(this);
    }

    private void OnEnable()
    {
        OverlayIconManager.main.AddIcon(this);
    }

    private void OnDestroy()
    {
        StructureInstance.OnStructureInstanceChanged -= OnStructureInstanceChanged;
    }

    private void OnStructureInstanceChanged(StructureInstance newInstance)
    {
        if (newInstance == null)
            Destroy(this);
    }

    public enum IconType
    {
        Standard,
        Particle,
        Light
    }
}