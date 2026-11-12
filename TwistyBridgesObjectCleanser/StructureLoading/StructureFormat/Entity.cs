using System;
using UnityEngine;

namespace TwistyBridgesObjectCleanser.StructureLoading.StructureFormat;

/// <summary>
/// Represents a single entity.
/// </summary>
[Serializable]
public class Entity
{
    public string classId;
    public string id;
    public Coord3 position;
    public Coord4 rotation;
    public Coord3 scale;

    public Entity() { }
    
    public Entity(string classId, string id, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        this.classId = classId;
        this.id = id;
        this.position = new Coord3(position);
        this.rotation = new Coord4(rotation);
        this.scale = new Coord3(scale);
    }

    public override string ToString()
    {
        return $"{id} (Class ID: {classId})";
    }
}