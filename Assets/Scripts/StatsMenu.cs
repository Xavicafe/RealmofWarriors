using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
    public GameObject statsPanelUI;
    [SerializeField] Tilemap targetTilemap;
    [SerializeField] GridManager gridManager;
    [SerializeField] TileBase higlightTile;
    [SerializeField] GridController gridController;
    Pathfinding pathfinding;
    Character selectedCharacter;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI strengthText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI levelText;
    public Image CharacterImage;

    private CharacterControl CharacterControl;

    void Start(){
        Camera camaraPrincipal = Camera.main;
        CharacterControl = camaraPrincipal.GetComponent<CharacterControl>();
    }

    void Update()
    {
        Vector3 wolrdPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int clickPosition = targetTilemap.WorldToCell(wolrdPoint);
        if(CharacterControl.selectedCharacter!=null){
            selectedCharacter=CharacterControl.selectedCharacter;
            if (Input.GetKeyDown(KeyCode.C) && selectedCharacter != null)
            {
                if (GameisPaused) { StatsOff(); }

                else { StatsOn();}
                UpdateStats();
            }
        }
    }

    void StatsOn()
    {
        statsPanelUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }

    void StatsOff()
    {
        statsPanelUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }

    public void UpdateStats()
    {
        nameText.text = selectedCharacter.Name;
        levelText.text = selectedCharacter.nivel.ToString();
        hpText.text = selectedCharacter.vida.ToString();
        strengthText.text = selectedCharacter.Damage.ToString();
        CharacterImage.sprite = selectedCharacter.icono;
    }
}
