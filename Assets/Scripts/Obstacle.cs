using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private float obstacleSpeed = 2f;

    private Vector3 direction;

    private void Update()
    {
        // Вращаем препятствие
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Двигаем препятствие в заданном направлении
        transform.position += direction * obstacleSpeed * Time.deltaTime;
    }

    public void Initialize(Vector3 moveDirection, float speed)
    {
        obstacleSpeed = speed;
        direction = moveDirection.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();
            ball.GameOver();
        }
    }
}
