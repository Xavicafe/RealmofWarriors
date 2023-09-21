using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour
{
    [SerializeField] Tilemap highlightTilemap;
    [SerializeField] GridManager gridManager;
    [SerializeField] TileBase movimiento;
    [SerializeField] TileBase movimientoRes;
    [SerializeField] TileBase FlechaAb;
    [SerializeField] TileBase FlechaAbRes;
    [SerializeField] TileBase FlechaArr;
    [SerializeField] TileBase FlechaArrRes;
    [SerializeField] TileBase FlechaIzq;
    [SerializeField] TileBase FlechaIzqRes;
    [SerializeField] TileBase FlechaDer;
    [SerializeField] TileBase FlechaDerRes;
    Pathfinding pathfinding;
    Character selectedCharacter;

    Vector3Int MousePosition;
    Vector3Int lastMousePosition;



    private TileBase TilePasada;
    private TileBase CurrentTile;


    public bool IsSelected=false;

   

    void Awake(){
        pathfinding = gridManager.GetComponent<Pathfinding>();

    }


    

    private void Update()
    {
       MouseInput();
          
    }

    private void MouseInput()
    {
        if(IsSelected){
            Vector3 wolrdPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePosition = highlightTilemap.WorldToCell(wolrdPoint);
            
            if(highlightTilemap.GetTile(MousePosition)!=null){
                
                CurrentTile = highlightTilemap.GetTile(MousePosition);
                if(lastMousePosition==new Vector3Int(0,0,0)){
                    lastMousePosition=MousePosition;
                }
                if(TilePasada==null){
                    TilePasada=CurrentTile;
                }

                if(lastMousePosition!=MousePosition && (highlightTilemap.GetTile(MousePosition)==movimientoRes ||highlightTilemap.GetTile(MousePosition)==movimiento
                || highlightTilemap.GetTile(MousePosition)==FlechaAbRes || highlightTilemap.GetTile(MousePosition)==FlechaAb
                 || highlightTilemap.GetTile(MousePosition)==FlechaArrRes || highlightTilemap.GetTile(MousePosition)==FlechaArr 
                 || highlightTilemap.GetTile(MousePosition)==FlechaDerRes || highlightTilemap.GetTile(MousePosition)==FlechaDer
                 || highlightTilemap.GetTile(MousePosition)==FlechaIzqRes || highlightTilemap.GetTile(MousePosition)==FlechaIzq))
                 {
                    highlightTilemap.SetTile(lastMousePosition,TilePasada);
                    lastMousePosition=MousePosition;
                }

                
                if(CurrentTile== movimiento){
                    highlightTilemap.SetTile(MousePosition,movimientoRes);
                    TilePasada=CurrentTile;
                }
                if(CurrentTile== FlechaAb){
                    highlightTilemap.SetTile(MousePosition,FlechaAbRes);
                    TilePasada=CurrentTile;
                }
                if(CurrentTile== FlechaArr){
                    highlightTilemap.SetTile(MousePosition,FlechaArrRes);
                    TilePasada=CurrentTile;
                }
                if(CurrentTile== FlechaDer){
                    highlightTilemap.SetTile(MousePosition,FlechaDerRes);
                    TilePasada=CurrentTile;
                }
                if(CurrentTile== FlechaIzq){
                    highlightTilemap.SetTile(MousePosition,FlechaIzqRes);
                    TilePasada=CurrentTile;
                }
                

                
            }
        }else{
            lastMousePosition=new Vector3Int(0,0,0);
            TilePasada=null;
        }
        
    }

    public Vector3Int CeldaDeAtaque(){
        return lastMousePosition;
    }
    public TileBase HayaFlecha(){
        return TilePasada;
    }

   
}
