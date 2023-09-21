using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScript : MonoBehaviour
{
    public void LoadNewScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
