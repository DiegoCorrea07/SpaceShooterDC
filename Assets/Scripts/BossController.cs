using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int health = 3; // Salud del jefe
    public int pointsOnDeath = 100; // Puntos al destruir al jefe
    private bool isDestroyed = false; // Para evitar múltiples destrucciones

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Si choca con el jugador
        {
            // Aquí puedes acceder al script del jugador que maneja las vidas
            AIRCRAFT playerController = collision.gameObject.GetComponent<AIRCRAFT>();
            if (playerController != null)
            {
                playerController.DecreaseLives(2); // Reduce 2 vidas
            }

            // Notifica al EnemySpawner que el jefe ha sido destruido
            EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.OnBossDestroyed(); // Llama al método en el spawner
            }

            Destroy(gameObject); // Destruye al jefe
        }
        else if (collision.gameObject.CompareTag("Projectile")) // Si choca con un proyectil
        {
            TakeDamage(1); // Reduce la salud del jefe
            Destroy(collision.gameObject); // Destruye el proyectil
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; // Reduce la salud
        if (health <= 0 && !isDestroyed)
        {
            isDestroyed = true; // Evita múltiples destrucciones
            ScoreManager.Instance.AddScore(pointsOnDeath); // Aumenta el puntaje

            // Notifica al EnemySpawner que el jefe ha sido destruido
            EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.OnBossDestroyed();
            }

            Destroy(gameObject); // Destruye al jefe
        }
    }
}
