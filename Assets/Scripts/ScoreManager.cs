using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario si usas UI para mostrar la puntuación

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton para acceder a la puntuación desde otros scripts

    public int score = 0; // Puntuación actual
    public Text scoreText; // Referencia al texto de UI que mostrará la puntuación

    private void Awake()
    {
        // Verifica si ya existe una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener el ScoreManager al cambiar de escena

        }
        else
        {
            Destroy(gameObject); // Destruir el objeto si ya existe una instancia
        }
       
    }

    private void Start()
    {
       // Solo se inicializa el scoreText si estamos en la escena de juego
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SampleScene")
        {
            FindScoreText(); // Busca ScoreText solo en la escena de juego
            UpdateScoreUI();
        }
    }

    private void FindScoreText()
    {
        scoreText = GameObject.Find("ScoreText")?.GetComponent<Text>();
        Debug.Log(scoreText != null ? "ScoreText encontrado" : "ScoreText no se encontró"); //Opcional por depuración
    }

    public void AddScore(int points)
    {
        score += points; // Sumar puntos
        UpdateScoreUI(); // Actualizar la interfaz de usuario

    }

    public void UpdateScoreUI()
    {
        // Obtén nuevamente el objeto de texto de la UI si es nulo
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        }

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score; // Actualiza el texto de puntuación
        }
    }


    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

}
