using UnityEngine;

namespace WeatherMod.Mono;

public class DisableWhenCameraUnderwater : MonoBehaviour
{
    private Renderer[] _renderers;

    private bool _wasUnderwater;
    
    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);
        _wasUnderwater = GetIsUnderWater();
        SetRenderersActive(!_wasUnderwater);
    }

    private void SetRenderersActive(bool active) => _renderers.ForEach((r) => r.enabled = active);

    private bool GetIsUnderWater()
    {
        return MainCamera.camera.transform.position.y < GetWaterLevelForUnderWater();
    }

    private float GetWaterLevelForUnderWater()
    {
        var level = Ocean.GetOceanLevel();
        
        if (Player.main.IsInBase())
        {
            level -= 0.5f;
        }

        return level;
    }
    
    private void Update()
    {
        var isUnderWater = GetIsUnderWater();
        
        if (isUnderWater == _wasUnderwater) return;
        
        _wasUnderwater = isUnderWater;
        SetRenderersActive(!isUnderWater);
    }
}