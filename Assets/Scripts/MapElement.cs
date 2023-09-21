
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElement : MonoBehaviour
{
    GridMap gridMap;
    public int x_pos=0;
    public int y_pos=0;
    public float speed=3f;
    private Camera camaraPrincipal;
    // Start is called before the first frame update
    void Awake()
    {
        camaraPrincipal = Camera.main;
        SetGrid();
        PlaceObjectOnGrid();
    }

    private void SetGrid()
    {
        gridMap = transform.parent.GetComponent<GridMap>();
    }

    public void MoveCharacter(int targetPosX, int targetPosY){

        ClearObjectFromGrid();
        MoveTo(targetPosX,targetPosY);
        MoveObject();
    }

    public void MoveObject(){
        Vector3 worldPosition = new Vector3(x_pos *1f+0.5f, y_pos*1f+0.5f,-0.5f);
        StartCoroutine(MoveToTarget(worldPosition));
    }

    private void MoveTo(int targetPosX,int targetPosY){

        gridMap.SetCharacter(this,targetPosX,targetPosY);
        x_pos=targetPosX;
        y_pos=targetPosY;
    }

    private void PlaceObjectOnGrid()
    {
        x_pos = (int) transform.position.x ;
        y_pos = (int) transform.position.y;
        gridMap.SetCharacter(this, x_pos, y_pos);
    }

    private void ClearObjectFromGrid(){

        gridMap.RemoveCharacter(x_pos,y_pos);
    }
    public void RemoveObjectFromGrid(){

        gridMap.ClearCharacter(x_pos,y_pos);
    }

    private IEnumerator MoveToTarget(Vector3 target_position)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if(target_position.x<=transform.position.x){
            spriteRenderer.flipX = false;
        }
        else{
            spriteRenderer.flipX = true;
        }
        while (Vector2.Distance(transform.position, target_position) > 0.01f)
        {
            camaraPrincipal.transform.position=new Vector3(transform.position.x,transform.position.y,-10);
            transform.position = Vector2.MoveTowards(transform.position, target_position, speed * Time.deltaTime);
            yield return null;
        }
    }



}