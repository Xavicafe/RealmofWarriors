using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private AudioSource audioMixer;
    
    [SerializeField] GridMap gridMap;

    void Start(){
        audioMixer.volume=0.5f;
    }

    public void PantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }

    public void CambiarVolumen(float volumen)
    {
        audioMixer.volume=volumen;
    }


    //Marcar velocidad de movimiento de las unidades

    public void Velocidad_inst(Button bt){
        //poner todos los botones en blanco
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("BOTONES DIFICULTAD");
        foreach (GameObject but in buttons)
        {
            but.GetComponent<Image>().color=Color.white;
        }

        //establecer velocidad y marcar el boton seleccionado
        Establecer_velocidad(50f);
        bt.GetComponent<Image>().color=Color.grey;
    }
    public void Velocidad_Rapida(Button bt){
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("BOTONES DIFICULTAD");
        foreach (GameObject but in buttons)
        {
                but.GetComponent<Image>().color=Color.white;
        }
        Establecer_velocidad(6f);
        bt.GetComponent<Image>().color=Color.grey;
    }
    public void Velocidad_Normal(Button bt){
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("BOTONES DIFICULTAD");
        
        foreach (GameObject but in buttons)
        {
                but.GetComponent<Image>().color=Color.white;
        }
        Establecer_velocidad(3f);
        bt.GetComponent<Image>().color=Color.grey;
    }
    public void Velocidad_Lenta(Button bt){
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("BOTONES DIFICULTAD");
        foreach (GameObject but in buttons)
        {
                but.GetComponent<Image>().color=Color.white;
        }
        Establecer_velocidad(1.5f);
        bt.GetComponent<Image>().color=Color.grey;
    }

    void Establecer_velocidad(float speed){
        for(int i=0;i<gridMap.CharacterList.Count;i++){
            MapElement me = gridMap.CharacterList[i].GetComponent<MapElement>();
            me.speed=speed;
        }
    }
}
