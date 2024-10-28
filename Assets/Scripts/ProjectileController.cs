using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f; // Velocidad inicial
    public float acceleration = 5f; // Aceleración
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed; // Inicia con la velocidad hacia adelante

        // Destruir el proyectil después de 5 segundos
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Aumentar la velocidad del proyectil con el tiempo (si es necesario)
        rb.velocity += transform.forward * acceleration * Time.deltaTime; // Esto puede hacer que la velocidad se incremente demasiado rápido
    }

    // Detectar colisiones con otros objetos
    void OnCollisionEnter(Collision collision)
    {
        // Verificar si el proyectil ha chocado con un enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destruir el enemigo
            ScoreManager.Instance.AddScore(10); // Añadir 10 puntos al puntaje
        }

        // Destruir el proyectil al colisionar con cualquier objeto
        Destroy(gameObject);
    }

}
