using System;
using System.Collections.Generic;
using ModStructureHelperPlugin.Interfaces;
using ModStructureHelperPlugin.Utility;
using Nautilus.Utility;
using UnityEngine;

namespace ModStructureHelperPlugin.Mono;

public class TriggerColliderPreview : MonoBehaviour, ISelectionListener
{
    private List<ColliderRendering.BaseDebugCollider> _colliders;

    private void Awake()
    {
        _colliders = new List<ColliderRendering.BaseDebugCollider>();
        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            if (!collider.isTrigger)
            {
                continue;
            }
            
            _colliders.Add(ColliderRendering.ColliderPool.MainPool.Register(collider));
        }
    }
    
    public void OnObjectSelected()
    {
        foreach (var collider in _colliders)
        {
            collider.CreateVisual();
            collider.SetMaterial(MaterialUtils.StasisFieldMaterial);
            collider.SetColor(collider.PersistentColor);
        }
    }
    
    public void OnObjectDeselected()
    {
        foreach (var collider in _colliders)
        {
            collider.DestroyVisual();
        }
    }
}