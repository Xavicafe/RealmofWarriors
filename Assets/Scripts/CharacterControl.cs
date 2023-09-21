using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

using UnityEngine.Tilemaps;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] Tilemap highlightTilemap;
    [SerializeField] TileBase highlightTile;
    [SerializeField] TileBase personajeAliado;
    [SerializeField] TileBase personajeEnemigo;
    [SerializeField] TileBase PuedeTirar;

    [SerializeField] TileBase FlechaArr;
    [SerializeField] TileBase FlechaAb;
    [SerializeField] TileBase FlechaDer;
    [SerializeField] TileBase FlechaIzq;

    [SerializeField] TileBase MovimientoEnemigo;
    [SerializeField] GridManager gridManager;
    private GridController gridController;
    [SerializeField] GridMap gridMap;

    [SerializeField] private DialogueObjetct dialogueObjetct;
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }

    Animator animator;
    Animator animatorEnemy;

    Pathfinding pathfinding;
    public Character selectedCharacter;
    private int contador;
    

    public bool pausa = false;

    public TMP_Text text_turno;

    private Camera camaraPrincipal;

    private List<Vector3Int> EnemigosARango=new List<Vector3Int>();

    void Awake()
    {
        pathfinding = targetTilemap.GetComponent<Pathfinding>();
        camaraPrincipal = Camera.main;
        
    }

    void Start(){
        gridController=GetComponent<GridController>();
        foreach(Character c in gridMap.AliadosList)
        {
            if (c.turno == true)  
            {
                highlightTilemap.SetTile(new Vector3Int(c.GetComponent<MapElement>().x_pos, c.GetComponent<MapElement>().y_pos, 0), PuedeTirar);
            }
        }
    }

    private void Update()
    {
        if(pausa==false){
            MouseInput();
            if (dialogueUI.IsOpen) return;          
        }

    }

    //Metodo para seleccionar y mover/atacar
    private void MouseInput()
    {
        
        Vector3 wolrdPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickPosition = targetTilemap.WorldToCell(wolrdPoint);

        if (Input.GetMouseButtonDown(0))
        {

            if(gridManager.CheckPosition(clickPosition.x, clickPosition.y)){
            
                //DialogueUI.ShowDialogue(dialogueObjetct);

            if (selectedCharacter == null && gridManager.GetCharacter(clickPosition.x, clickPosition.y))
            {

                if (gridManager.CheckPosition(clickPosition.x, clickPosition.y) == false) { return; }
                selectedCharacter = gridManager.GetCharacter(clickPosition.x, clickPosition.y);
                gridController.IsSelected=true;

                if (selectedCharacter.aliado==false)
                {
                    Debug.Log("Selecciona una tropa aliada");
                    selectedCharacter=null;
                    gridController.IsSelected=false;
                    return;
                }

                animator = selectedCharacter.GetComponent<Animator>();
                if (selectedCharacter != null && selectedCharacter.turno == true)
                {

                    List<PathNode> toHighlight = new List<PathNode>();
                    // pathfinding.Clear();
                    Debug.Log("Personaje seleccionado: " + selectedCharacter.Name);
                    pathfinding.CalculateWalkableTerrain(clickPosition.x, clickPosition.y, selectedCharacter.moveDistance, ref toHighlight);



                    for (int i = 0; i < toHighlight.Count; i++)
                    {
                        if (gridManager.GetCharacter(toHighlight[i].xPos, toHighlight[i].yPos) != null && gridManager.GetCharacter(toHighlight[i].xPos, toHighlight[i].yPos).aliado==selectedCharacter.aliado)
                        {
                            if(gridManager.GetCharacter(toHighlight[i].xPos, toHighlight[i].yPos)==selectedCharacter)
                            {
                                highlightTilemap.SetTile(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0), PuedeTirar);
                            }
                            else
                            {
                            highlightTilemap.SetTile(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0), personajeAliado);
                            
                            }
                            continue;
                        }
                        else if (gridManager.GetCharacter(toHighlight[i].xPos, toHighlight[i].yPos) != null && gridManager.GetCharacter(toHighlight[i].xPos, toHighlight[i].yPos).aliado!=selectedCharacter.aliado)
                        {
                            highlightTilemap.SetTile(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0), personajeEnemigo);
                            EnemigosARango.Add(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0));
                            continue;
                        }else
                        highlightTilemap.SetTile(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0), highlightTile);
                    }
                    if(EnemigosARango.Count>0)
                    CrearFlechas();
                }
                else if(selectedCharacter != null && selectedCharacter.turno == false)
                {
                    selectedCharacter=null;
                    gridController.IsSelected=false;
                    Debug.Log("Esta unidad ya ha tirado este turno");
                    return;
                }
            }
            else
            {
                if (selectedCharacter == null) { return; }
                
                List<PathNode> path = pathfinding.TrackBackPath(selectedCharacter, clickPosition.x, clickPosition.y);

                if (path != null)
                {
                    if (path.Count > 0)
                    {
                        for (int i = 0; i < path.Count; i++)
                        {
                            highlightTilemap.SetTile(new Vector3Int(path[i].xPos, path[i].yPos, 0), highlightTile);

                        }
                        if (gridManager.GetCharacter(clickPosition.x, clickPosition.y) != null && gridManager.GetCharacter(clickPosition.x, clickPosition.y).aliado!=selectedCharacter.aliado)
                        {
                            //Esto hay que cambiarlo

                            Vector3Int celda_Ataque = gridController.CeldaDeAtaque();
                            //Vector3Int celda_Ataque = CeldaAtaque(clickPosition, selectedCharacter.GetComponent<MapElement>().x_pos, selectedCharacter.GetComponent<MapElement>().y_pos);
                            animatorEnemy = gridManager.GetCharacter(clickPosition.x, clickPosition.y).GetComponent<Animator>();
                            //arriba derecha
                            TileBase flecha = gridController.HayaFlecha();
                            if(flecha==FlechaAb || flecha==FlechaArr || flecha==FlechaDer || flecha==FlechaIzq){ 
                                if(Math.Sqrt((clickPosition.x-celda_Ataque.x)*(clickPosition.x-celda_Ataque.x)+(clickPosition.y-celda_Ataque.y)*(clickPosition.y-celda_Ataque.y))>1){
                                    Debug.Log("Selecciona un sitio correcto desde el que atacar");
                                    Deselect();
                                    return;
                                }    
                                if(gridManager.GetCharacter(celda_Ataque.x, celda_Ataque.y)!=null && gridManager.GetCharacter(celda_Ataque.x, celda_Ataque.y)!=selectedCharacter){
                                    Debug.Log("No puedes moverte donde hay un personaje");
                                    Deselect();
                                    return;
                                }     
                                selectedCharacter.GetComponent<MapElement>().MoveCharacter(celda_Ataque.x, celda_Ataque.y);
                                SpriteRenderer spriteRenderer = selectedCharacter.GetComponent<SpriteRenderer>();
                                if(clickPosition.x<=celda_Ataque.x){
                                    spriteRenderer.flipX = false;
                                }
                                else{
                                    spriteRenderer.flipX = true;
                                }
                                animator.SetTrigger("ataque");
                                selectedCharacter.Atacar();
                                animatorEnemy.SetTrigger("recibirDaño");
                                gridManager.GetCharacter(clickPosition.x, clickPosition.y).Impacto(selectedCharacter.Damage);
                            }else{
                                Debug.Log("Marca bien desde donde quieres golpear al enemigo");
                                Deselect();
                                return;
                            }

                        }
                        else if(gridManager.GetCharacter(clickPosition.x, clickPosition.y) != null && gridManager.GetCharacter(clickPosition.x, clickPosition.y).aliado==selectedCharacter.aliado)
                        {
                            Debug.Log("No puedes moverte donde hay un aliado");
                            Deselect();
                            return;
                        }
                        else {
                            SpriteRenderer spriteRenderer = selectedCharacter.GetComponent<SpriteRenderer>();
                            if(clickPosition.x<=selectedCharacter.transform.position.x){
                                spriteRenderer.flipX = false;
                            }
                            else{
                                spriteRenderer.flipX = true;
                            }
                            selectedCharacter.GetComponent<MapElement>().MoveCharacter(path[0].xPos, path[0].yPos);
                        }

                        selectedCharacter.turno = false;
                        contador = 0;
                        foreach(Character c in gridMap.AliadosList)
                        {
                            if (c.turno == false) contador++;
                        }
                        if (contador == gridMap.AliadosList.Count) {
                            foreach (Character c in gridMap.AliadosList)
                            {
                                c.turno = true;

                            }
                            StartCoroutine(IAEnemigos());
                        }
                    }
                    Deselect();

                    
                }
            }
            }

        }
    }
    public void Deselect()
    {
        highlightTilemap.ClearAllTiles();
        selectedCharacter=null;
        gridController.IsSelected=false;
        pathfinding.Clear();
        foreach(Character c in gridMap.AliadosList)
        {
            if (c.turno == true)  
            {
                highlightTilemap.SetTile(new Vector3Int(c.GetComponent<MapElement>().x_pos, c.GetComponent<MapElement>().y_pos, 0), PuedeTirar);
            }
        }

    }



   //ATAQUE IA:

    //Elegir la celda donde se moverá la IA
    private Vector3Int CeldaAtaque(Vector3Int clickPosition, int CharacterPosition_x, int CharacterPosition_y)
    {
        Character Atacado = gridManager.GetCharacter(clickPosition.x, clickPosition.y);
        //arriba derecha
        if (clickPosition.x < CharacterPosition_x  && clickPosition.y < CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x,clickPosition.y+1)!=TerrainType.Unpassable
                && (gridManager.GetCharacter(clickPosition.x,clickPosition.y+1)==null || (clickPosition.x==CharacterPosition_x && clickPosition.y+1==CharacterPosition_y)))
        {   
            return new Vector3Int(clickPosition.x, clickPosition.y + 1);
        }
        else if (clickPosition.x < CharacterPosition_x  && clickPosition.y < CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x+1,clickPosition.y)!=TerrainType.Unpassable
                && (gridManager.GetCharacter(clickPosition.x+1,clickPosition.y)==null || (clickPosition.x+1==CharacterPosition_x && clickPosition.y==CharacterPosition_y)))
        {           
            return new Vector3Int(clickPosition.x+1, clickPosition.y);
        }

        //abajo derecha
        if (clickPosition.x < CharacterPosition_x && clickPosition.y > CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x,clickPosition.y-1)!=TerrainType.Unpassable
                && (gridManager.GetCharacter(clickPosition.x,clickPosition.y-1)==null || (clickPosition.x==CharacterPosition_x && clickPosition.y-1==CharacterPosition_y)))
        {
            return new Vector3Int(clickPosition.x, clickPosition.y-1);
            
        }
        else if (clickPosition.x < CharacterPosition_x && clickPosition.y > CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x+1,clickPosition.y)!=TerrainType.Unpassable 
                && (gridManager.GetCharacter(clickPosition.x+1,clickPosition.y)==null || (clickPosition.x+1==CharacterPosition_x && clickPosition.y==CharacterPosition_y)))
        {
            return new Vector3Int(clickPosition.x+1, clickPosition.y);
        }

        //abajo izquierda
        if (clickPosition.x > CharacterPosition_x  && clickPosition.y > CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x-1,clickPosition.y)!=TerrainType.Unpassable
                && (gridManager.GetCharacter(clickPosition.x-1,clickPosition.y)==null || (clickPosition.x-1==CharacterPosition_x && clickPosition.y==CharacterPosition_y)))
        {
            return new Vector3Int(clickPosition.x-1, clickPosition.y);
        }
        else if (clickPosition.x > CharacterPosition_x  && clickPosition.y > CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x,clickPosition.y-1)!=TerrainType.Unpassable
                && (gridManager.GetCharacter(clickPosition.x,clickPosition.y-1)==null || (clickPosition.x==CharacterPosition_x && clickPosition.y-1==CharacterPosition_y)))
        {
            return new Vector3Int(clickPosition.x, clickPosition.y-1);
        }

        //arriba izquierda
        if (clickPosition.x > CharacterPosition_x  && clickPosition.y < CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x-1,clickPosition.y)!=TerrainType.Unpassable
        && (gridManager.GetCharacter(clickPosition.x-1,clickPosition.y)==null || (clickPosition.x-1==CharacterPosition_x && clickPosition.y==CharacterPosition_y)))
        {
            return new Vector3Int(clickPosition.x-1, clickPosition.y);
        }
        else if (clickPosition.x > CharacterPosition_x  && clickPosition.y < CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x,clickPosition.y+1)!=TerrainType.Unpassable
                && (gridManager.GetCharacter(clickPosition.x,clickPosition.y+1)==null || (clickPosition.x==CharacterPosition_x && clickPosition.y+1==CharacterPosition_y)))

        {
            return new Vector3Int(clickPosition.x, clickPosition.y+1);
        }

        //abajo
        if (clickPosition.x == CharacterPosition_x && clickPosition.y > CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x,clickPosition.y-1)!=TerrainType.Unpassable
        && (gridManager.GetCharacter(clickPosition.x,clickPosition.y-1)==null || (clickPosition.x==CharacterPosition_x && clickPosition.y-1==CharacterPosition_y)))
        {
            return new Vector3Int(clickPosition.x, clickPosition.y-1);
        }

        //arriba
        if (clickPosition.x == CharacterPosition_x && clickPosition.y < CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x,clickPosition.y+1)!=TerrainType.Unpassable
        && (gridManager.GetCharacter(clickPosition.x,clickPosition.y+1)==null || (clickPosition.x==CharacterPosition_x && clickPosition.y+1==CharacterPosition_y)))
        {
            return new Vector3Int(clickPosition.x, clickPosition.y+1);
        }

        //derecha
        if (clickPosition.x < CharacterPosition_x && clickPosition.y == CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x+1,clickPosition.y)!=TerrainType.Unpassable
        && (gridManager.GetCharacter(clickPosition.x+1,clickPosition.y)==null || (clickPosition.x+1==CharacterPosition_x && clickPosition.y==CharacterPosition_y)))
        {
            return new Vector3Int(clickPosition.x+1, clickPosition.y);
        }

        //izquierda
        if (clickPosition.x > CharacterPosition_x && clickPosition.y == CharacterPosition_y && gridManager.GetTerrainType(clickPosition.x-1,clickPosition.y)!=TerrainType.Unpassable
        && (gridManager.GetCharacter(clickPosition.x-1,clickPosition.y)==null || (clickPosition.x-1==CharacterPosition_x && clickPosition.y==CharacterPosition_y)))
        {
            return new Vector3Int(clickPosition.x-1, clickPosition.y);
        }
        Debug.Log("Problema Movimiento");
        return new Vector3Int(CharacterPosition_x,CharacterPosition_y);
    }



    //Realizar Ataque IA (Animator, Daños)
    private void AtaqueIA(Vector3Int clickPosition, int CharacterPosition_x, int CharacterPosition_y)
    {
        Vector3Int celda_Ataque = CeldaAtaque(clickPosition, CharacterPosition_x, CharacterPosition_y);
        Character Atacado = gridManager.GetCharacter(clickPosition.x, clickPosition.y);
        Character Atacante = gridManager.GetCharacter(CharacterPosition_x, CharacterPosition_y);
        animator = Atacante.GetComponent<Animator>();
        animatorEnemy = Atacado.GetComponent<Animator>();
               
            gridManager.GetCharacter(CharacterPosition_x, CharacterPosition_y).GetComponent<MapElement>().MoveCharacter(celda_Ataque.x, celda_Ataque.y);
            animator.SetTrigger("ataque");
            Atacado.Atacar();
            animatorEnemy.SetTrigger("recibirDaño");
            Atacado.Impacto(Atacante.Damage);
        
    }
    
    //Metodo para mostrar las casillas por las que se podrá mover en su turno la IA (Se llama en la clase Character con un MouseEnter)
    public void rangoEnemigos(int x_character, int y_character, int moveDistance, bool aliado)
    {

        if(selectedCharacter==null && pausa==false){
            if (gridManager.CheckPosition(x_character, y_character) == false) { return; }

            if (aliado == false)
            {
                List<PathNode> toHighlight = new List<PathNode>();
                Debug.Log("Mostrando movimiento de una tropa enemiga");
                pathfinding.CalculateWalkableTerrain(x_character, y_character, moveDistance, ref toHighlight);
                for (int i = 0; i < toHighlight.Count; i++)
                {
                    highlightTilemap.SetTile(new Vector3Int(toHighlight[i].xPos, toHighlight[i].yPos, 0), MovimientoEnemigo);
                }
            }
        }
        
    }

    //Desde Aquí se llaman a todos los metodos anteriores para realizar un ataque completo y se inicializa desde el metodo MouseInput()
    IEnumerator IAEnemigos()
    {
        text_turno.text="turno IA";
        text_turno.color=Color.red;
        yield return new WaitForSeconds(1f);
        for(int j=0; j<gridMap.EnemigosList.Count;j++)
        {
            
            //Camera.main.ScreenToWorldPoint(Input.mousePosition);
            List<PathNode> toMove = new List<PathNode>();
            
            pathfinding.CalculateWalkableTerrain(gridMap.EnemigosList[j].GetComponent<MapElement>().x_pos, gridMap.EnemigosList[j].GetComponent<MapElement>().y_pos, gridMap.EnemigosList[j].moveDistance, ref toMove);

            for (int i = 0; i < toMove.Count; i++) {
                if (gridManager.GetCharacter(toMove[i].xPos, toMove[i].yPos) != null && gridManager.GetCharacter(toMove[i].xPos, toMove[i].yPos).aliado != gridMap.EnemigosList[j].aliado) {
                    camaraPrincipal.transform.position = new Vector3Int(gridMap.EnemigosList[j].GetComponent<MapElement>().x_pos, gridMap.EnemigosList[j].GetComponent<MapElement>().y_pos,-10);

                    Debug.Log("Moviendo "+ j);
                    AtaqueIA(new Vector3Int(toMove[i].xPos, toMove[i].yPos), gridMap.EnemigosList[j].GetComponent<MapElement>().x_pos, gridMap.EnemigosList[j].GetComponent<MapElement>().y_pos);
                    yield return new WaitForSeconds(1.5f);
                    break;
                }

            }
            

        }
        Deselect();
        text_turno.text="Tu turno";
        text_turno.color=Color.green;
        
    }


//metodo para poner flechas adyacentes a los enemigos
    void CrearFlechas(){
        for(int i=0;i<EnemigosARango.Count;i++){
            if(highlightTilemap.GetTile(EnemigosARango[i]-new Vector3Int(1,0,0))==highlightTile || (highlightTilemap.GetTile(EnemigosARango[i]-new Vector3Int(1,0,0))==PuedeTirar 
            && EnemigosARango[i]-new Vector3Int(1,0,0)==new Vector3Int(selectedCharacter.GetComponent<MapElement>().x_pos,selectedCharacter.GetComponent<MapElement>().y_pos,0)))
            {
                highlightTilemap.SetTile(EnemigosARango[i]-new Vector3Int(1,0,0),FlechaIzq);
            }
            if(highlightTilemap.GetTile(EnemigosARango[i]+new Vector3Int(1,0,0))==highlightTile || (highlightTilemap.GetTile(EnemigosARango[i]+new Vector3Int(1,0,0))==PuedeTirar
            && EnemigosARango[i]+new Vector3Int(1,0,0)==new Vector3Int(selectedCharacter.GetComponent<MapElement>().x_pos,selectedCharacter.GetComponent<MapElement>().y_pos,0)))
            {
                highlightTilemap.SetTile(EnemigosARango[i]+new Vector3Int(1,0,0),FlechaDer);
            }
            if(highlightTilemap.GetTile(EnemigosARango[i]+new Vector3Int(0,1,0))==highlightTile || (highlightTilemap.GetTile(EnemigosARango[i]+new Vector3Int(0,1,0))==PuedeTirar
            && EnemigosARango[i]+new Vector3Int(0,1,0)==new Vector3Int(selectedCharacter.GetComponent<MapElement>().x_pos,selectedCharacter.GetComponent<MapElement>().y_pos,0)))
            {
                highlightTilemap.SetTile(EnemigosARango[i]+new Vector3Int(0,1,0),FlechaArr);
            }
            if(highlightTilemap.GetTile(EnemigosARango[i]-new Vector3Int(0,1,0))==highlightTile || (highlightTilemap.GetTile(EnemigosARango[i]-new Vector3Int(0,1,0))==PuedeTirar
            && EnemigosARango[i]-new Vector3Int(0,1,0)==new Vector3Int(selectedCharacter.GetComponent<MapElement>().x_pos,selectedCharacter.GetComponent<MapElement>().y_pos,0)))
            {            
                highlightTilemap.SetTile(EnemigosARango[i]-new Vector3Int(0,1,0),FlechaAb);
            }
        }
        EnemigosARango.Clear();

    }

    
}


