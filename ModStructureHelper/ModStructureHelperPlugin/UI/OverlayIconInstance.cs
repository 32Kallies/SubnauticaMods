using ModStructureHelperPlugin.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModStructureHelperPlugin.UI;

public class OverlayIconInstance : MonoBehaviour
{
    private const float KMinDistance = 0.2f;
    private const float KTransitionDistance = 2f;
    
    public OverlayIconManager manager;
    
    public Image iconImage;
    public TextMeshProUGUI labelText;

    private IOverlayIconData _data;

    public void OnCreate(IOverlayIconData iconData)
    {
        _data = iconData;
        iconData.OnCreation(this);
        UpdateData(true);
    }

    private void UpdateData(bool firstTime)
    {
        var screenPoint = SNCameraRoot.main.mainCam.WorldToScreenPoint(_data.Position);
        SetPositionAndScale(screenPoint);
        labelText.text = _data.Label;
        iconImage.sprite = _data.Icon;
        var color = _data.Color.WithAlpha(GetDistanceAlphaBlend(screenPoint.z));
        iconImage.color = color;
        labelText.color = color;
    }

    private void SetPositionAndScale(Vector3 screenPoint)
    {
        var x = screenPoint.x - (float)Screen.width / 2;
        var y = screenPoint.y - (float)Screen.height / 2;
        transform.localPosition = new Vector3(x, y, 0f);
        transform.localScale = Vector3.one * _data.Scale;
    }

    private void Update()
    {
        UpdateData(false);
    }
    
    
    private float GetDistanceAlphaBlend(float distance)
    {
        if (distance < KMinDistance)
        {
            return 0f;
        }
        if (distance > KMinDistance + KTransitionDistance)
        {
            return 1f;
        }
        return Mathf.Clamp((distance - KMinDistance) / KTransitionDistance, 0f, 1f);
    }
}