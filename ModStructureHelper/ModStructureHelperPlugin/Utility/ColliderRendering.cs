using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModStructureHelperPlugin.Utility;

// Repurposed from Debug Helper's DebugColliders class:
// https://github.com/32Kallies/SubnauticaMods/blob/main/DebugHelper/DebugHelper/Objects/DebugColliders.cs
// This file has many reusable types that are not explicitly designed for the Mod Structure Helper
// Thus, despite the differences in code style, refactoring this is not advised; it simply is what it is, and works well 
public static class ColliderRendering
{
    public enum ColliderType
    {
        Static,
        Rigidbody,
        KinematicRigidbody
    }

    public enum ColliderShape
    {
        Box,
        Sphere,
        Capsule,
        Mesh
    }
    
    public abstract class BasePool<T>
    {
        protected HashSet<T> m_list = new HashSet<T>();

        public abstract T Register(object target);
        public abstract void Unregister(T target);
        public virtual bool Contains(T target) => m_list.Contains(target);
        public abstract bool Contains(object target);
        public abstract int Clear();
        public IEnumerable<T> ToSet() => m_list.ToSet();
    }
    
    public sealed class ColliderPool : BasePool<BaseDebugCollider>
    {
        internal static ColliderPool MainPool { get; } = new ColliderPool();
        
        public override BaseDebugCollider Register(object target)
        {
            Collider collider = target as Collider;
            BaseDebugCollider pc = null;
            switch (collider.GetType().Name)
            {
                case nameof(BoxCollider):
                    pc = new DebugBoxCollider((BoxCollider)collider);
                    break;
                case nameof(SphereCollider):
                    pc = new DebugSphereCollider((SphereCollider)collider);
                    break;
                case nameof(CapsuleCollider):
                    pc = new DebugCapsuleCollider((CapsuleCollider)collider);
                    break;
                case nameof(MeshCollider):
                    pc = new DebugMeshCollider((MeshCollider)collider);
                    break;
                default:
                    Debug.Log(collider.GetType().Name + " is not supported!");
                    return null;
            }
            m_list.Add(pc);
            return pc;
        }
        public override void Unregister(BaseDebugCollider collider)
        {
            if (!m_list.Contains(collider)) return;
            collider.DestroyVisual();
            m_list.Remove(collider);
        }
        public void Unregister(Collider collider) => m_list.RemoveWhere(x => x.Get().GetInstanceID() == collider.GetInstanceID());
        public override int Clear()
        {
            int destroyed = 0;
            foreach (var obj in m_list)
            {
                obj.DestroyVisual();
            }
            m_list.Clear();
            return destroyed;
        }

        public override bool Contains(object target)
        {
            Collider c = target as Collider;
            if (c == null) return false;
            return m_list.Where(x => x.Get()).Count() > 0;
        }
    }

    public abstract class BaseDebugCollider
    {
        // References:
        // https://docs.unity3d.com/560/Documentation/Manual/CollidersOverview.html

        #region Fields

        private readonly Collider m_collider;
        protected GameObject m_visualObject;
        protected Renderer m_renderer;

        #endregion

        #region Getters

        #region Properties

        public bool WasDrawn
        {
            get
            {
                if (m_visualObject == null) return false;
                return true;
            }
        }

        /// <summary>
        /// Returns one of these collider type: Static, Rigidbody, KinematicRigidbody
        /// </summary>
        public ColliderType Type
        {
            get => GetColliderType();
        }

        /// <summary>
        /// Returns one of these shape type: Box, Sphere, Capsule, Mesh
        /// </summary>
        public ColliderShape Shape
        {
            get => GetColliderShape();
        }

        public bool IsTrigger
        {
            get => m_collider.isTrigger;
        }
        
        public Color PersistentColor { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns collider
        /// </summary>
        public Collider Get() => m_collider;

        /// <summary>
        /// Returns one of these collider type: Static, Rigidbody, KinematicRigidbody
        /// </summary>
        public ColliderType GetColliderType()
        {
            bool flag1 = m_collider.attachedRigidbody != null; // Has rigidbody attached
            if (flag1)
            {
                bool flag3 = m_collider.attachedRigidbody.isKinematic; // Is kinematic
                switch (flag3)
                {
                    case true: return ColliderType.KinematicRigidbody;
                    case false: return ColliderType.Rigidbody;
                }
            }

            return ColliderType.Static;
        }

        /// <summary>
        /// Returns one of these shape type: Box, Sphere, Capsule, Mesh
        /// </summary>
        public ColliderShape GetColliderShape()
        {
            switch (m_collider.GetType().Name)
            {
                case nameof(BoxCollider):
                    return ColliderShape.Box;
                case nameof(SphereCollider):
                    return ColliderShape.Sphere;
                case nameof(CapsuleCollider):
                    return ColliderShape.Capsule;
            }

            return ColliderShape.Mesh;
        }

        #endregion

        #endregion

        #region Visual

        public void SetColor(Color color)
        {
            if (!WasDrawn) return;
            m_renderer.material.color = color;
        }

        public void SetMaterial(Material material)
        {
            if (!WasDrawn) return;
            m_renderer.material = material;
        }

        public virtual void CreateVisual()
        {
            if (WasDrawn) return;
            PrimitiveType pt = PrimitiveType.Cube;
            switch (Shape)
            {
                case ColliderShape.Box:
                    break;
                case ColliderShape.Sphere:
                    pt = PrimitiveType.Sphere;
                    break;
                case ColliderShape.Capsule:
                    pt = PrimitiveType.Capsule;
                    break;
                default: throw new System.ArgumentException();
            }

            m_visualObject = GameObject.CreatePrimitive(pt);
            m_visualObject.name = "ColliderVisualization";
            m_renderer = m_visualObject.GetComponent<Renderer>();
            Object.DestroyImmediate(m_visualObject.GetComponent<Collider>());
            m_visualObject.SetActive(true);
            Transform transform = m_visualObject.transform;
            transform.SetParent(m_collider.transform);
            transform.localEulerAngles = Vector3.zero;
        }

        public void DestroyVisual()
        {
            if (!WasDrawn) return;
            Object.DestroyImmediate(m_visualObject);
        }

        #endregion

        BaseDebugCollider()
        {
        }

        internal BaseDebugCollider(Collider collider)
        {
            m_collider = collider;
            if (collider != null)
            {
                PersistentColor = GetVisualizerColor(collider.gameObject.name);
            }
        }
        
        private static Color GetVisualizerColor(string identifierName)
        {
            var hash = StringComparer.OrdinalIgnoreCase.GetHashCode(identifierName);
            float hue = Mathf.Abs(hash % 1000) / 1000f;
            var color = Color.HSVToRGB(hue, 0.8f, 1f);
            return color.WithAlpha(0.1f);
        }
    }

    public abstract class BaseDebugCollider<T> : BaseDebugCollider where T : Collider
    {
        /// <summary>
        /// Returns collider
        /// </summary>
        new public T Get() => (T)base.Get();

        public BaseDebugCollider(T collider) : base(collider)
        {
        }
    }

    public sealed class DebugBoxCollider : BaseDebugCollider<BoxCollider>
    {
        public Vector3 Center
        {
            get => GetCenter();
        }

        public Vector3 Size
        {
            get => GetSize();
        }

        public Vector3 GetCenter() => Get().center;
        public Vector3 GetSize() => Get().size;

        public override void CreateVisual()
        {
            base.CreateVisual();
            m_visualObject.transform.localPosition = Center;
            m_visualObject.transform.localScale = Size;
        }

        internal DebugBoxCollider(BoxCollider collider) : base(collider)
        {
        }
    }

    public sealed class DebugSphereCollider : BaseDebugCollider<SphereCollider>
    {
        public Vector3 Center
        {
            get => GetCenter();
        }

        public float Radius
        {
            get => GetRadius();
        }

        public Vector3 GetCenter() => Get().center;
        public float GetRadius() => Get().radius;

        public override void CreateVisual()
        {
            base.CreateVisual();
            m_visualObject.transform.localPosition = Center;
            m_visualObject.transform.localScale = Vector3.one * GetRadius() * 2f;
        }

        internal DebugSphereCollider(SphereCollider collider) : base(collider)
        {
        }
    }

    public sealed class DebugCapsuleCollider : BaseDebugCollider<CapsuleCollider>
    {
        public Vector3 Center
        {
            get => GetCenter();
        }

        public float Radius
        {
            get => GetRadius();
        }

        public float Height
        {
            get => GetHeight();
        }

        public int Direction
        {
            get => GetDirection();
        }

        public Vector3 GetCenter() => Get().center;
        public float GetRadius() => Get().radius;
        public float GetHeight() => Get().height;
        public int GetDirection() => Get().direction;

        public override void CreateVisual()
        {
            base.CreateVisual();
            Transform transform = m_visualObject.transform;
            Vector3 angle = 90 * (Direction != 1 ? (Direction == 0 ? Vector3.forward : Vector3.right) : Vector3.zero);
            transform.localPosition = Center;
            transform.localEulerAngles = angle;
            transform.localScale = new Vector3(Radius * 2f, Height / 2, Radius * 2f);
        }

        internal DebugCapsuleCollider(CapsuleCollider collider) : base(collider)
        {
        }
    }

    public sealed class DebugMeshCollider : BaseDebugCollider<MeshCollider>
    {
        private MeshFilter m_filter;

        public Mesh Mesh
        {
            get => GetMesh();
        }

        public bool Convex
        {
            get => IsConvex();
        }

        public Mesh GetMesh() => Get().sharedMesh;
        public bool IsConvex() => Get().convex;

        public override void CreateVisual()
        {
            if (WasDrawn) return;

            m_visualObject = new GameObject("DebugMeshCollider");
            m_renderer = m_visualObject.AddComponent<MeshRenderer>();
            m_filter = m_visualObject.AddComponent<MeshFilter>();
            m_filter.mesh = Get().sharedMesh;
            m_visualObject.SetActive(true);

            Transform transform = m_visualObject.transform;
            transform.SetParent(Get().transform);
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        internal DebugMeshCollider(MeshCollider collider) : base(collider)
        {
        }
    }
}