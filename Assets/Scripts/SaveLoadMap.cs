using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadMap : MonoBehaviour
{
    [SerializeField] MapData mapData;
    [SerializeField] GridMap gridMap;
    [SerializeField] GridManager gridManager;

    void Start(){
        Load(gridMap);
    }
    public void Save()
    {
        int[,] map = gridManager.ReadTileMap();
        mapData.Save(map);
    }

    public void Load()
    {
        Debug.Log("Cargando");
        gridManager.Clear();

        int width = mapData.width;
        int height = mapData.height;

        int i=0;
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                gridManager.SetTile(x, y, mapData.map[i]);
                i += 1;
            }
        }
    }

    public void Load(GridMap grid)
    {
        mapData.Load(grid);
    }
}
