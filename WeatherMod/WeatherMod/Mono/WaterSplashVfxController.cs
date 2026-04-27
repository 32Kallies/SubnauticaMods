using UnityEngine;

namespace WeatherMod.Mono;

public class WaterSplashVfxController : MonoBehaviour
{
    public Transform affectedTransform;
    private ParticleSystem _particles;
    private bool _hasParticles;
    
    public float heightOffset = 0.1f;

    private void Start()
    {
        _particles = affectedTransform.GetComponent<ParticleSystem>();
        _hasParticles = _particles != null;
        if (!_hasParticles)
        {
            Plugin.Logger.LogWarning("Particle system not found for WaterSplashVFX");
        }
    }

    private void LateUpdate()
    {
        if (_hasParticles)
        {
            var indoors = Player.main.IsInside();
            _particles.SetPlaying(!indoors);
            if (indoors)
                return;
        }
        
        var camPos = MainCamera.camera.transform.position;
        affectedTransform.position = new Vector3(camPos.x, Ocean.GetOceanLevel() + heightOffset, camPos.z);
    }
}