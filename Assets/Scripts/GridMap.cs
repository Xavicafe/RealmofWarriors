using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node{

    public int tileId;
    public Character character;

}

public class GridMap : MonoBehaviour
{
    [HideInInspector]
    public int height;
    [HideInInspector]
    public int width;
    Node[,] grid;

    public List<Character> CharacterList;
    public List<Character> AliadosList;
    public List<Character> EnemigosList;

    // Start is called before the first frame update
    public void Init(int width, int height)
    {
        grid = new Node[width, height];

        for(int x=0;x<width;x++){
            for (int y=0;y<height;y++){
                grid[x,y]=new Node();
            }

        }
        this.width = width;
        this.height = height;
    }
    
    internal void ClearCharacter(int x_pos, int y_pos){

        Character c = GetCharacter(x_pos,y_pos);

        for (int i = 0; i < EnemigosList.Count; i++) {
            if (c == EnemigosList[i]) {
                EnemigosList.Remove(c);
            }        
        }
        grid[x_pos,y_pos].character=null;
    }

    internal void RemoveCharacter(int x_pos, int y_pos){
        grid[x_pos,y_pos].character=null;
    }


    internal void SetCharacter(MapElement mapElement, int x_pos, int y_pos){
        grid[x_pos,y_pos].character = mapElement.GetComponent<Character>();

        CharacterList.Add(mapElement.GetComponent<Character>());

        if(mapElement.GetComponent<Character>().aliado==true && !AliadosList.Contains(mapElement.GetComponent<Character>()))
        {
            AliadosList.Add(mapElement.GetComponent<Character>());
        }
        else if(mapElement.GetComponent<Character>().aliado==false && !EnemigosList.Contains(mapElement.GetComponent<Character>()))
        {
            EnemigosList.Add(mapElement.GetComponent<Character>());
        }
    }

    public void SetTile(int x, int y, int to)
    {

        if (CheckPosition(x, y) == false) { return; }
        grid[x, y].tileId = to;

        

    }

    public int GetTile(int x, int y)
    {

        if (CheckPosition(x, y) == false) { return 3; }
        return grid[x, y].tileId;
    }

    

    public bool CheckPosition(int x, int y)
    {
        if (x < 0 || x >= width)
            return false;
        if (y < 0 || y >= height)
            return false;
        return true;
    }

    public Character GetCharacter(int x, int y){

        return grid[x,y].character;
    }

    internal bool CheckWalkable(int xPos, int yPos)
    {
        return grid[xPos, yPos].tileId == 0;
    }

}
