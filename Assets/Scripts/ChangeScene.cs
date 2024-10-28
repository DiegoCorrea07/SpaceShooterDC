using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ChangeScene : MonoBehaviour
{
    public AIRCRAFT gameManager;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneGame()
    {
        // Cargar la escena del juego
        SceneManager.LoadScene("SampleScene");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SampleScene" && ScoreManager.Instance != null)
        {
            // Resetea la puntuación al cargar la escena de juego
            ScoreManager.Instance.ResetScore();
        }
    }
}
