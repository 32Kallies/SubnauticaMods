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
        var worldPosition = _data.Position;
        var screenPoint = SNCameraRoot.main.mainCam.WorldToScreenPoint(worldPosition);
        SetPositionAndScale(screenPoint, worldPosition);
        labelText.text = _data.Label;
        iconImage.sprite = _data.Icon;
        var color = _data.Color.WithAlpha(GetDistanceAlphaBlend(screenPoint.z));
        iconImage.color = color;
        labelText.color = color;
    }

    private void SetPositionAndScale(Vector3 screenPoint, Vector3 worldPosition)
    {
        var x = screenPoint.x - (float)Screen.width / 2;
        var y = screenPoint.y - (float)Screen.height / 2;
        transform.localPosition = new Vector3(x, y, 0f);
        var scale = GetScale(Vector3.Distance(MainCameraControl.main.transform.position, worldPosition));
        transform.localScale = Vector3.one * scale;
    }

    private float GetScale(float distance)
    {
        return 5f / distance;
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
        
        if (distance > Plugin.ModConfig.OverlayIconsFadeDistance + KMinDistance)
        {
            return 1f;
        }
        return Mathf.Clamp((distance - Plugin.ModConfig.OverlayIconsFadeDistance - KMinDistance + KTransitionDistance) / KTransitionDistance, 0f, 1f);
    }
}