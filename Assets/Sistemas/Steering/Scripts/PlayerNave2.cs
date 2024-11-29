using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNave2 : MonoBehaviour
{
    public float rotationSpeed = 200f;  // Velocidade de rota��o (graus/segundo)
    public float acceleration = 10f;   // Velocidade de acelera��o ao pressionar "W"
    public float maxSpeed = 5f;        // Velocidade m�xima da nave
    public float deceleration = 5f;   // Taxa de desacelera��o ao pressionar "S"

    public Transform targetObject;    // Objeto de refer�ncia para reposicionar a nave
    public Vector2 offset = new Vector2(-13f, 0f); // Dist�ncia relativa (x, y) ao objeto

    private Rigidbody2D rb;
    private LineRenderer lrplayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lrplayer = GetComponent<LineRenderer>();
        rb.gravityScale = 0; // Sem gravidade
    }

    void Update()
    {
        // Rota��o
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }

        // Movimento para frente
        if (Input.GetKey(KeyCode.W))
        {
            Vector2 thrustDirection = transform.up; // Dire��o "para frente"
            rb.AddForce(thrustDirection * acceleration);

            // Limitar a velocidade m�xima
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }

        // Desacelera��o ao pressionar "S"
        if (Input.GetKey(KeyCode.S))
        {
            // Reduz a velocidade linear gradualmente
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, deceleration * Time.deltaTime);

            // Se a velocidade linear estiver quase zero, zere tamb�m a velocidade angular
            if (rb.velocity.magnitude < 0.01f)
            {
                rb.velocity = Vector2.zero; // Garante que a velocidade � exatamente zero
                rb.angularVelocity = 0f;   // Zera a rota��o angular
            }
        }

        // Reposicionamento ao pressionar "R"
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector2 newPosition = (Vector2)targetObject.position + offset;
            transform.position = newPosition;

            // Reseta velocidade ao reposicionar
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (GameManager.linePlayer) 
        {
            lrplayer.enabled = true;
            lrplayer.SetPosition(0, transform.position);
            lrplayer.SetPosition(1, new Vector2 (transform.position.x, transform.position.y) + rb.velocity);
        }
        else 
        {
            lrplayer.enabled = false;
        }
    }
}


