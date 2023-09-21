using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Tile Set")]
public class TileSet : ScriptableObject
{
    public TerrainData terrainData;
    public List<TileBase> tiles;    
}
