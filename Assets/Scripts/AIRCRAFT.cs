using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIRCRAFT : MonoBehaviour
{
    public float speed = 10f; // Velocidad de movimiento de la nave
    private Rigidbody rb;
    public Camera camaraPrincipal; // C�mara principal

    // Variable para las vidas del jugador
    public int Lives = 5;

    public Text livesText;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        UpdateLivesUI();
    }

    void Update()
    {
        Mover(); // Llama al m�todo de movimiento
        LimitarMovimiento(); // Limita el movimiento a los l�mites de la c�mara
    }

    // M�todo para manejar el movimiento
    void Mover()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized * speed;
        rb.velocity = movement; // Aplicar movimiento a la nave
    }

    // M�todo para limitar el movimiento de la nave a la vista de la c�mara
    void LimitarMovimiento()
    {
        Vector3 posicionNave = transform.position;

        // Calcula la distancia entre la c�mara y la nave en el eje Z
        float distanciaZ = camaraPrincipal.WorldToScreenPoint(transform.position).z;

        // Calcula los l�mites de la pantalla en el espacio del mundo
        Vector3 limiteInferiorIzq = camaraPrincipal.ScreenToWorldPoint(new Vector3(0, 0, distanciaZ));
        Vector3 limiteSuperiorDer = camaraPrincipal.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distanciaZ));

        // Limita la posici�n de la nave en X y Z (horizontal y vertical)
        posicionNave.x = Mathf.Clamp(posicionNave.x, limiteInferiorIzq.x, limiteSuperiorDer.x);
        posicionNave.z = Mathf.Clamp(posicionNave.z, limiteInferiorIzq.z, limiteSuperiorDer.z);

        // Aplica la posici�n limitada a la nave
        transform.position = posicionNave;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Los enemigos deben tener la etiqueta "Enemy"
        {
            Lives--;  // Resta una vida
            Destroy(collision.gameObject); // Destruye al enemigo al chocar con el jugador
            UpdateLivesUI();

            if (Lives <= 0)
            {
                EndGame();  // Llama a la funci�n que termina el juego
            }
        }
        else if (collision.gameObject.CompareTag("Boss")) // Si colisiona con el jefe
        {
            Lives -= 2; // Resta 2 vidas
            UpdateLivesUI();

            if (Lives <= 0)
            {
                EndGame(); // Llama a la funci�n que termina el juego
            }
        }
    }


    // M�todo para manejar el fin del juego
    void EndGame()
    {
        // L�gica para terminar el juego
        Debug.Log("Juego terminado. No te quedan vidas.");
        Destroy(gameObject); // Destruye la nave del jugador
    }

    void FixedUpdate()
    {
        // Restablece la rotaci�n a (0, 0, 0) en cada cuadro fijo
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void ResetLives()
    {
        if (livesText == null)
        {
            livesText = GameObject.Find("LivesText")?.GetComponent<Text>();
        }

        Lives = 5; // Reiniciar la puntuaci�n
        UpdateLivesUI(); // Actualizar la interfaz de usuario
    }

    void UpdateLivesUI()
    {
        // Obt�n nuevamente el objeto de texto de la UI si es nulo
        if (livesText == null)
        {
            livesText = GameObject.Find("LivesText").GetComponent<Text>();
        }

        if (livesText != null)
        {
            livesText.text = "Lives: " + Lives; // Actualiza el texto de vidas
        }
    }

    public void DecreaseLives(int amount)
    {
        Lives -= amount; // Reduce las vidas
        UpdateLivesUI(); // Actualiza la interfaz de usuario

        if (Lives <= 0)
        {
            EndGame(); // Termina el juego si no quedan vidas
        }
    }

}
