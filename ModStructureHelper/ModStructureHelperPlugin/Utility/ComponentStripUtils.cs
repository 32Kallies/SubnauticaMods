using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModStructureHelperPlugin.Utility;

public static class ComponentStripUtils
{
    private const bool DebugMode = false;
    
    private static readonly List<Type> WhitelistedTypes = new()
    {
        typeof(Renderer),
        typeof(Transform),
        typeof(MeshFilter),
        typeof(SkyApplier),
        typeof(ParticleSystem),
        typeof(Light),
        typeof(Animator),
        typeof(CanvasRenderer),
        typeof(Canvas),
        typeof(FMOD_CustomEmitter),
        typeof(VFXVolumetricLight),
        typeof(AtmosphereVolume)
    };

    public static void StripComponents(GameObject obj)
    {
        foreach (var transform in obj.GetComponentsInChildren<Transform>(true))
        {
            DestroyRequiredComponentsInOrder(transform.gameObject);
        }
    }
    
    private static bool IsComponentWhitelisted(Type type)
    {
        foreach (var whitelistedType in WhitelistedTypes)
        {
            if (whitelistedType.IsAssignableFrom(type))
            {
                return true;
            }
        }

        return false;
    }

    private static void DestroyRequiredComponentsInOrder(GameObject obj)
    {
        if (DebugMode)
            Plugin.Logger.LogMessage("Stripping " + obj.name);
        
        var objectComponents = obj.GetComponents<Component>();

        var requirementsTree = new RequirementTreeNode(null);
        foreach (var component in objectComponents)
        {
            if (!IsComponentWhitelisted(component.GetType()))
                RecursivelyAddRequiredComponents(requirementsTree, component, objectComponents);
        }
        
        var parentsOfComponents = new Dictionary<Component, List<Component>>();
        DepthFirstSearch(requirementsTree, parentsOfComponents, new HashSet<RequirementTreeNode>());
        
        var nodesByComponent = new Dictionary<Component, RequirementTreeNode>();
        foreach (var component in parentsOfComponents.Keys)
        {
            nodesByComponent.Add(component, new RequirementTreeNode(component));
        }

        foreach (var parentRelationship in parentsOfComponents)
        {
            foreach (var parent in parentRelationship.Value)
            {
                nodesByComponent[parentRelationship.Key].Parents.Add(nodesByComponent[parent]);
                nodesByComponent[parent].Children.Add(nodesByComponent[parentRelationship.Key]);
            }
        }

        var sortedTree = new RequirementTreeNode(null);
        foreach (var node in nodesByComponent.Values)
        {
            if (node.Parents.Count == 0)
            {
                sortedTree.Children.Add(node);
                node.Parents.Add(sortedTree);
            }
        }

        if (DebugMode)
        {
            Plugin.Logger.LogMessage("Tree for " + obj.name);
            PrintTree(sortedTree);
        }
        
        while (nodesByComponent.Count > 0)
        {
            RequirementTreeNode parentless = null;
            foreach (var node in nodesByComponent.Values)
            {
                int validParentCount = 0;
                foreach (var parent in node.Parents)
                {
                    if (parent.Component != null)
                        validParentCount++;
                }
                if (validParentCount == 0)
                    parentless = node;
            }

            Component componentToRemove;
            if (parentless != null)
            {
                componentToRemove = parentless.Component;
            }
            else
            {
                componentToRemove = nodesByComponent.ElementAt(0).Key;
            }
            if (DebugMode)
                Plugin.Logger.LogMessage("Removing " + componentToRemove);
            Object.DestroyImmediate(componentToRemove);
            nodesByComponent.Remove(componentToRemove);
        }
    }
    
    private static void DepthFirstSearch(RequirementTreeNode tree, Dictionary<Component, List<Component>> parentRelations, HashSet<RequirementTreeNode> visited)
    {
        foreach (var node in tree.Children)
        {
            if (!visited.Add(node))
            {
                continue;
            }

            if (!parentRelations.TryGetValue(node.Component, out var parentsList))
            {
                parentsList = new List<Component>();
                parentRelations.Add(node.Component, parentsList);
            }
            if (tree.Component != null)
                parentsList.Add(tree.Component);
            DepthFirstSearch(node, parentRelations, visited);
        }
    }
    
    private static void PrintTree(RequirementTreeNode node, string indent = "", bool isLast = true)
    {
        if (node == null) return;

        Plugin.Logger.LogMessage(indent + (isLast ? "└─ " : "├─ ") + node.Component);

        indent += isLast ? "   " : "│  ";

        int i = 0;
        foreach (var child in node.Children)
        {
            PrintTree(child, indent, i == node.Children.Count - 1);
            i++;
        }
    }
    
    // Covers edge cases with infinite recursion
    private const int RecursiveAttributeNavigationDepthLimit = 16;

    private static void RecursivelyAddRequiredComponents(RequirementTreeNode tree, Component component,
        Component[] objectComponents, int depth = 0)
    {
        var newNode = new RequirementTreeNode(component);
        tree.Children.Add(newNode);
        newNode.Parents.Add(tree);
        
        var attributes = Attribute.GetCustomAttributes(component.GetType());
        if (attributes.Length == 0)
            return;
        foreach (var attribute in attributes)
        {
            if (attribute is not RequireComponent required)
                continue;

            var typesToDestroy = new[] { required.m_Type0, required.m_Type1, required.m_Type2 };

            foreach (var other in objectComponents)
            {
                if (other == component)
                    continue;
                
                foreach (var toDestroy in typesToDestroy)
                {
                    if (toDestroy == null || IsComponentWhitelisted(toDestroy) || !toDestroy.IsAssignableFrom(other.GetType())) continue;

                    if (depth < RecursiveAttributeNavigationDepthLimit)
                    {
                        RecursivelyAddRequiredComponents(newNode, other, objectComponents, depth + 1);
                    }
                }
            }
        }
    }

    private class RequirementTreeNode
    {
        public Component Component { get; }
        public HashSet<RequirementTreeNode> Children { get; }
        public HashSet<RequirementTreeNode> Parents { get; }

        public RequirementTreeNode(Component component)
        {
            Component = component;
            Children = new HashSet<RequirementTreeNode>();
            Parents = new HashSet<RequirementTreeNode>();
        }
    }
}