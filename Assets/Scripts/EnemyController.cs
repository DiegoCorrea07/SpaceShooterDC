using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;  // Velocidad de avance hacia el jugador
    public Transform playerTransform;  // Referencia al jugador
    public float followSpeed = 2f;  // Velocidad para seguir en X e Y (ajusta según lo necesites)
    public float zOffset = 5f;  // Ajuste para que el enemigo avance desde una distancia

    void Start()
    {
        // Encuentra al jugador por la etiqueta "Player"
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // Mantener al enemigo moviéndose hacia el jugador en el eje Z
            Vector3 targetPosition = new Vector3(
                playerTransform.position.x,  // Igualar la posición X del jugador
                playerTransform.position.y,  // Igualar la posición Y del jugador
                transform.position.z - zOffset // Seguir avanzando en Z hacia el jugador
            );

            // Movimiento suave hacia la posición del jugador en los ejes X e Y
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Movimiento en Z hacia el jugador
            newPosition.z = Mathf.MoveTowards(transform.position.z, playerTransform.position.z, speed * Time.deltaTime);

            // Actualizar la posición del enemigo
            transform.position = newPosition;

            // Opcional: hacer que el enemigo gire hacia el jugador
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // Método para ajustar la velocidad del enemigo
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
