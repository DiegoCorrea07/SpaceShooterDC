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

        // Destruir el proyectil después de 3 segundos para evitar que siga indefinidamente
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        // Aumentar la velocidad del proyectil con el tiempo
        rb.velocity += transform.forward * acceleration * Time.deltaTime;
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

        // Verificar si el proyectil ha chocado con un jefe
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossController bossController = collision.gameObject.GetComponent<BossController>();
            if (bossController != null)
            {
                bossController.TakeDamage(1); // Llama a TakeDamage en el jefe
            }
        }

        // Destruir el proyectil al colisionar con cualquier objeto
        Destroy(gameObject);
    }

}
