using System;
using System.Collections;
using Nautilus.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WeatherMod.Mono;

public class LightningSpawner : MonoBehaviour
{
    private const float MinInterval = 12;
    private const float MaxInterval = 17;
    private const float MinDistanceFromCamera = 20f;
    private const float MaxDistanceFromCamera = 800f;
    private const float SpawnHeightMin = 1f;
    private const float SpawnHeightVariationMax = 50;
    private const float SpawnHeightAbsMax = 240;

    private float _timeSpawnLightningAgain;

    private static AnimationCurve _lightningShakeStrengthCurve =
        new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0.1f));

    public bool useAltModel;

    private void ResetTimer() => _timeSpawnLightningAgain = Time.time + Random.Range(MinInterval, MaxInterval);

    private void Awake()
    {
        ResetTimer();
        _timeSpawnLightningAgain += 4; // delay initial lightning
    }

    private void Update()
    {
        if (Time.time < _timeSpawnLightningAgain) return;

        var spawnAngle = Random.value * Mathf.PI * 2f;
        var dist = Random.Range(MinDistanceFromCamera, MaxDistanceFromCamera);

        SpawnLightning(
            MainCamera.camera.transform.position + new Vector3(Mathf.Cos(spawnAngle), 0, Mathf.Sin(spawnAngle)) * dist,
            useAltModel);

        ResetTimer();
    }

    public static void SpawnLightning(Vector3 position, bool alwaysUseAltModel)
    {
        var camPos = MainCamera.camera.transform.position;

        if (camPos.y < Ocean.GetOceanLevel() - 100)
            return;

        var lightningSpawnPosition = new Vector3(position.x,
            Mathf.Clamp(camPos.y + Random.Range(SpawnHeightMin, SpawnHeightVariationMax),
                Ocean.GetOceanLevel() + SpawnHeightMin, Ocean.GetOceanLevel() + SpawnHeightAbsMax), position.z);

        var altModel = alwaysUseAltModel;
        if (Random.value <= 0.01f) altModel = true;

        var lightning =
            Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>(altModel ? "VFX_FreddyLightning" : "VFX_Lightning"));

        if (!altModel)
            lightning.AddComponent<RandomLightningModel>();

        lightning.transform.position = lightningSpawnPosition;

        foreach (var r in lightning.GetComponentsInChildren<Renderer>())
        {
            var m = r.material;
            m.shader = MaterialUtils.Shaders.ParticlesUBER;
        }

        UWE.CoroutineHost.StartCoroutine(PlayThunderCoroutine(position));
        
        Destroy(lightning, 5f);
    }

    private static IEnumerator PlayThunderCoroutine(Vector3 soundPosition)
    {
        var dist = Vector3.Distance(MainCamera.camera.transform.position, soundPosition);

        var delay = Mathf.Clamp(dist / 346f, 0f, 5f);

        yield return delay;
        
        Utils.PlayFMODAsset(Vector3.Distance(MainCamera.camera.transform.position, soundPosition) < 80
                ? WeatherAudio.ThunderSoundsNear.GetRandomUnity().Asset
                : WeatherAudio.ThunderSoundsFar.GetRandomUnity().Asset,
            soundPosition);

        var baseShakeStrength = _lightningShakeStrengthCurve.Evaluate(Mathf.Clamp01(dist / 1000));
        
        if (MainCamera.camera.transform.position.y > Ocean.GetOceanLevel() - 25)
            MainCameraControl.main.ShakeCamera(baseShakeStrength + Random.Range(-0.1f, 0.1f), Random.Range(1f, 1.3f));
    } 
}