namespace ModStructureHelperPlugin.Utility;

using UnityEngine;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class IconGeneratorWboit : PostEffectsBase
{
    public Camera camera;
    
    private RenderTexture _wboitTexture1;
    private RenderTexture _wboitTexture2;
    private Material _compositeMaterial;

    public Shader compositeShader;

    private int _texAPropertyID = -1;
    private int _texBPropertyID = -1;
    private int _weightTogglePropertyID = -1;
    private int _weightSharpnessPropertyID = -1;

    public bool useDepthWeighting = true;
    public float depthWeightingSharpness = 0.1f;
    
    public override bool CheckResources()
    {
        return true;
    }

    private void Awake()
    {
        _compositeMaterial = new Material(compositeShader);
        InitProperties();
    }

    private void OnDestroy()
    {
        DestroyRenderTargets();
    }

    private void InitRenderTargets()
    {
        var target = camera.targetTexture;

        _wboitTexture1 = CreateTexture(target, "Icon WBOIT TexA");
        _wboitTexture2 = CreateTexture(target, "Icon WBOIT TexB");

        EditorModifications.SetOITTargets(camera, _wboitTexture1, _wboitTexture2);
        _compositeMaterial.SetTexture(_texAPropertyID, _wboitTexture1);
        _compositeMaterial.SetTexture(_texBPropertyID, _wboitTexture2);
    }

    private static RenderTexture CreateTexture(RenderTexture target, string name)
    {
        var texture = new RenderTexture(target.width, target.height, 0,
            RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear)
        {
            name = name,
            antiAliasing = 1
        };

        texture.Create();
        return texture;
    }

    private void DestroyRenderTargets()
    {
        EditorModifications.SetOITTargets(camera, null, null);
        
        if (_wboitTexture1 != null)
        {
            _wboitTexture1.Release();
            _wboitTexture1 = null;
        }

        if (_wboitTexture2 != null)
        {
            _wboitTexture2.Release();
            _wboitTexture2 = null;
        }
    }

    private void UpdateGlobalShaderParameters()
    {
        Shader.SetGlobalFloat(_weightTogglePropertyID, useDepthWeighting ? 1f : 0f);
        Shader.SetGlobalFloat(_weightSharpnessPropertyID, depthWeightingSharpness);
    }

    private void InitProperties()
    {
        _texAPropertyID = Shader.PropertyToID("_WBOIT_texA");
        _texBPropertyID = Shader.PropertyToID("_WBOIT_texB");
        _weightTogglePropertyID = Shader.PropertyToID("_WBOIT_WeightToggle");
        _weightSharpnessPropertyID = Shader.PropertyToID("_WBOIT_WeightSharpness");
    }

    private void OnPreRender()
    {
        if (!enabled) return;
        InitRenderTargets();
        Graphics.SetRenderTarget(_wboitTexture1);
        GL.Clear(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 1f));
        Graphics.SetRenderTarget(_wboitTexture2);
        GL.Clear(clearDepth: false, clearColor: true, new Color(0f, 0f, 0f, 1f));
        UpdateGlobalShaderParameters();
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, _compositeMaterial);
    }
}