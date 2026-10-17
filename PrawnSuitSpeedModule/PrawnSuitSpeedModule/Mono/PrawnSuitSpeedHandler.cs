using UnityEngine;

namespace PrawnSuitSpeedModule.Mono;

internal class PrawnSuitSpeedHandler : MonoBehaviour
{
    public float speedPerModule = 0.5f;
    
    private Exosuit exosuit;

    private float defaultGroundForceMultiplier = 4f;

    private void Awake()
    {
        exosuit = GetComponent<Exosuit>();
    }

    public void UpdateSpeed()
    {
        if (exosuit == null)
            return;

        exosuit.onGroundForceMultiplier = defaultGroundForceMultiplier * GetForceMultiplier(exosuit);
    }
    
    private  float GetForceMultiplier(Exosuit exosuit)
    {
        return Mathf.Clamp(
            exosuit.modules.GetCount(Items.Equipment.PrawnSuitSpeedUpgrade.Info.TechType) * speedPerModule + 1,
            1, 10);
    }
}