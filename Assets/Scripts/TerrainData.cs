using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    Plain,
    hills,
    Water,
    Swamp,
    Unpassable
}

[CreateAssetMenu]
public class TerrainData : ScriptableObject
{
    public List<TerrainType> terrains;
}
