using UnityEngine;

namespace WeatherMod.Mono;

public class UpdateRainParticleDensity : MonoBehaviour, IScheduledUpdateBehaviour
{
    public ParticleSystem system;
    
    private int _defaultMaxParticles;
    private float _defaultEmissionRate;
    private float _lastMultiplier = 1f;
    private bool _hasCollisions;
    private bool _lastCollisionsState;
    
    public int scheduledUpdateIndex { get; set; }

    private void Awake()
    {
        if (system == null) system = GetComponent<ParticleSystem>();
        _defaultEmissionRate = system.emission.rateOverTimeMultiplier;
        _defaultMaxParticles = system.main.maxParticles;
        _hasCollisions = system.collision.enabled;
        _lastCollisionsState = _hasCollisions;
        ChangeIfNeeded();
    }

    public string GetProfileTag()
    {
        return "WeatherMod.Mono:UpdateRainParticleDensity";
    }

    public void ScheduledUpdate()
    {
        ChangeIfNeeded();
    }

    private void ChangeIfNeeded()
    {
        if (_hasCollisions)
        {
            bool collisionsSetting = Plugin.Options.RainHasCollisions;
            if (collisionsSetting != _lastCollisionsState)
            {
                var collision = system.collision;
                collision.enabled = collisionsSetting;
                _lastCollisionsState = collisionsSetting;
            }
        }
        
        var multiplier = Plugin.Options.RainDensityMultiplier;
        if (Mathf.Approximately(multiplier, _lastMultiplier)) return;
        
        var emission = system.emission;
        emission.rateOverTimeMultiplier = _defaultEmissionRate * multiplier;

        var main = system.main;
        main.maxParticles = Mathf.RoundToInt(_defaultMaxParticles * multiplier);
        
        _lastMultiplier = multiplier;
    }

    private void OnEnable()
    {
        UpdateSchedulerUtils.Register(this);
    }

    private void OnDisable()
    {
        UpdateSchedulerUtils.Deregister(this);
    }
}