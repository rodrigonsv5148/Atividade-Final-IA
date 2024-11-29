using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNave2 : MonoBehaviour
{
    public float rotationSpeed = 200f;  // Velocidade de rotação (graus/segundo)
    public float acceleration = 10f;   // Velocidade de aceleração ao pressionar "W"
    public float maxSpeed = 5f;        // Velocidade máxima da nave
    public float deceleration = 5f;   // Taxa de desaceleração ao pressionar "S"

    public Transform targetObject;    // Objeto de referência para reposicionar a nave
    public Vector2 offset = new Vector2(-13f, 0f); // Distância relativa (x, y) ao objeto

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
        // Rotação
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
            Vector2 thrustDirection = transform.up; // Direção "para frente"
            rb.AddForce(thrustDirection * acceleration);

            // Limitar a velocidade máxima
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }

        // Desaceleração ao pressionar "S"
        if (Input.GetKey(KeyCode.S))
        {
            // Reduz a velocidade linear gradualmente
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, deceleration * Time.deltaTime);

            // Se a velocidade linear estiver quase zero, zere também a velocidade angular
            if (rb.velocity.magnitude < 0.01f)
            {
                rb.velocity = Vector2.zero; // Garante que a velocidade é exatamente zero
                rb.angularVelocity = 0f;   // Zera a rotação angular
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


