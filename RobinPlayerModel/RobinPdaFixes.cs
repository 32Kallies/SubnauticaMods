using UnityEngine;

namespace RobinPlayerModel;

public class RobinPdaFixes : MonoBehaviour
{
    public Transform attachL;
    public Transform clavLAim;

    public Vector3 clavOffset = new Vector3(0.02f, 0, 0.04f);
    public Vector3 attachLOffset = new Vector3(-15.3f, -1.5f, -1.2f);

    private void LateUpdate()
    {
        if (Player.main == null) return;
        var pda = Player.main.pda;
        if (!pda || !pda.isInUse) return;
        clavLAim.transform.localPosition += clavOffset;
        attachL.localRotation *= Quaternion.Euler(attachLOffset);
    }
}