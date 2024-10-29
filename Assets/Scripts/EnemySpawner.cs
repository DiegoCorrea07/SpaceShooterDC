using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;           // Prefab del enemigo
    public GameObject bossPrefab;            // Prefab del jefe
    public float initialSpawnInterval = 2f;  // Intervalo de generaci�n inicial en segundos
    public float spawnDistance = 60f;        // Distancia m�nima del jugador para la generaci�n
    public float spawnAreaWidth = 10f;       // Ancho del �rea de generaci�n en el eje X

    private Transform playerTransform;       // Referencia al transform del jugador
    private float currentSpawnInterval;      // Intervalo de generaci�n actual
    private float enemySpeed = 3f;           // Velocidad inicial de los enemigos
    private int lastScoreCheckpoint = 0;     // Puntaje en el que se aument� la dificultad
    private bool bossActive = false;          // Indica si el jefe est� activo

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("No se encontr� el jugador con la etiqueta 'Player'."); // Opcional por depuraci�n
        }

        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnEnemies());
    }

    private bool isSpawning = false; // Variable para verificar si la generaci�n de enemigos est� activa

    IEnumerator SpawnEnemies()
    {
        isSpawning = true; // Marca la generaci�n como activa
        while (true)
        {
            // Verifica si el jugador a�n existe
            if (playerTransform == null)
            {
                EndGame();  // Llama al m�todo para terminar el juego
                yield break; // Termina la corrutina
            }

            // Obt�n el puntaje actual
            int currentScore = ScoreManager.Instance.score;

            // Genera un jefe cada 50 puntos
            if (currentScore % 500 == 0 && currentScore != lastScoreCheckpoint && !bossActive)
            {
                Instantiate(bossPrefab, GetRandomSpawnPosition(), Quaternion.identity); // Genera el jefe
                lastScoreCheckpoint = currentScore; // Actualiza el checkpoint para evitar generar m�ltiples jefes
                bossActive = true; // Marca el jefe como activo
            }
            else if (!bossActive) // Solo genera enemigos si no hay jefe activo
            {
                // Genera un enemigo y ajusta su velocidad
                GameObject enemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                enemyController.SetSpeed(enemySpeed); // Asigna la velocidad actual al enemigo

                // Incrementa dificultad cada 100 puntos
                if (currentScore >= lastScoreCheckpoint + 100)
                {
                    lastScoreCheckpoint += 100;
                    enemySpeed += 0.5f;  // Incrementa la velocidad de los enemigos
                    currentSpawnInterval = Mathf.Max(0.5f, currentSpawnInterval - 0.2f); // Reduce el intervalo de generaci�n
                }
            }

            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    public void OnBossDestroyed()
    {
        bossActive = false; // El jefe ha sido destruido
        if (isSpawning) // Verifica si la generaci�n de enemigos est� activa
        {
            StopCoroutine(SpawnEnemies()); // Detiene la corrutina actual si est� corriendo
            isSpawning = false; // Marca la generaci�n como inactiva
        }
        RestartEnemySpawn(); // Reinicia la generaci�n de enemigos
    }

    void RestartEnemySpawn()
    {
        StartCoroutine(SpawnEnemies()); // Reinicia la corrutina de generaci�n de enemigos
    }

    Vector3 GetRandomSpawnPosition()
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("playerTransform es nulo al obtener la posici�n de generaci�n.");// Opcional por depuraci�n
            return Vector3.zero; // O devuelve una posici�n predeterminada
        }

        float z = playerTransform.position.z + spawnDistance;
        float x = Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f);
        return new Vector3(x, 0, z);
    }

    private void EndGame()
    {
        Debug.Log("Juego terminado. La nave del jugador fue destruida.");// Opcional por depuraci�n

        // Destruir el spawner:
        Destroy(gameObject);

        SceneManager.LoadScene("GameOver");
    }

    private void OnDestroy()
    {
        StopAllCoroutines(); // Detiene la generaci�n de enemigos cuando la nave se destruye
    }
}
