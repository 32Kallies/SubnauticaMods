using ModStructureHelperPlugin.Interfaces;
using ModStructureHelperPlugin.StructureHandling;
using ModStructureHelperPlugin.UI;
using Nautilus.Extensions;
using UnityEngine;

namespace ModStructureHelperPlugin.Mono;

public class ObjectPivotCircle : MonoBehaviour, IOverlayIconData, ISelectionListener
{
    private static Sprite _loadedDefaultSprite;
    private static Sprite _loadedSelectedSprite;
    private static Sprite _loadedDefaultParticleSprite;
    private static Sprite _loadedSelectedParticleSprite;
    
    public bool showName;
    public bool isParticle;

    private bool _selecting;
    
    public string Label => showName ? _objectName : null;
    public Sprite Icon => _selecting ? GetSelectedSprite() : GetDefaultSprite();
    public Vector3 Position => transform.position;
    public float Scale => 5f / Vector3.Distance(MainCameraControl.main.transform.position, transform.position);

    private string _objectName;
    
    public void OnCreation(OverlayIconInstance instance)
    {
        if (_loadedDefaultSprite == null)
        {
            _loadedDefaultSprite = Plugin.AssetBundle.LoadAsset<Sprite>("SelectionCircleDefault");
            _loadedSelectedSprite = Plugin.AssetBundle.LoadAsset<Sprite>("SelectionCircleSelected");
            _loadedDefaultParticleSprite = Plugin.AssetBundle.LoadAsset<Sprite>("ParticleSelectionDefault");
            _loadedSelectedParticleSprite = Plugin.AssetBundle.LoadAsset<Sprite>("ParticleSelectionSelected");
        }
        
        _objectName = gameObject.name.TrimClone();
        
        StructureInstance.OnStructureInstanceChanged += OnStructureInstanceChanged;
    }

    private Sprite GetDefaultSprite() => isParticle ? _loadedDefaultParticleSprite : _loadedDefaultSprite;
    private Sprite GetSelectedSprite() => isParticle ? _loadedSelectedParticleSprite : _loadedSelectedSprite;

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
}