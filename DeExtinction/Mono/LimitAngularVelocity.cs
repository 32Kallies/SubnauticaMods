using UnityEngine;

namespace DeExtinction.Mono;

public class LimitAngularVelocity : MonoBehaviour
{
    public float maxAngularVelocity = 1;

    private void Start()
    {
        GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVelocity;
    }
}