using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    public int moveDistance =2;
    public int vida =100;
    public int nivel = 1;
    public Slider slider;
    public int Damage = 10;
    public bool aliado = true;
    public bool turno = true;
    public Sprite icono;
    [SerializeField] private AudioSource attackSoundEffect;
    private CharacterControl CharacterControl;

    public string Name;


    void Start(){
        Camera camaraPrincipal = Camera.main;
        CharacterControl = camaraPrincipal.GetComponent<CharacterControl>();
        
    }
    public void Impacto(int daño)
    {
        vida-=daño;
        if(vida<=0){
            
            GetComponent<MapElement>().RemoveObjectFromGrid();
            Destroy(gameObject);

        }
        slider.value = vida;
    }

    public void Atacar()
    {
        StartCoroutine(DelaySound());
    }

    IEnumerator DelaySound()
    {
        yield return new WaitForSeconds(0.5f);
        attackSoundEffect.Play();
    }

    void OnMouseEnter()
    {
        CharacterControl.rangoEnemigos(GetComponent<MapElement>().x_pos, GetComponent<MapElement>().y_pos, moveDistance, aliado);
    }
    void OnMouseExit()
    {
        if(CharacterControl.selectedCharacter==null){
            CharacterControl.Deselect();
        }
        
    }

}
