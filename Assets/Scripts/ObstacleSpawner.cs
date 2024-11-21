using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform[] obstacleSpawnPoints;
    [SerializeField] private Transform topSawPrefab;
    [SerializeField] private Transform botSawPrefab;
    [SerializeField] private Transform ballTransform;

    public float spawnInterval = 2f;
    public float timeToMoveSaw = 5f;
    public float yTopTargetSawPos = 3f;
    public float yBotTargetSawPos = -3f;
    public float maxRandomAngle = 10f;
    public bool canSpawn = true;
    private float obstacleSpeed;

    private void Start()
    {
        // Начинаем спавнить препятствия
        StartCoroutine(SpawnObstacles());

        // Перемещаем пилы через определённое время
        StartCoroutine(MoveSawsAfterDelay(timeToMoveSaw));
    }

    private IEnumerator SpawnObstacles()
    {
        while (canSpawn)
        {
            yield return new WaitForSeconds(spawnInterval);

            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Transform spawnPoint = obstacleSpawnPoints[Random.Range(0, obstacleSpawnPoints.Length)];

            GameObject obstacle = Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity);

            // Передаем направление с отклонением на случайный угол в скрипт препятствия
            Vector3 direction = (ballTransform.position - spawnPoint.position).normalized;
            direction = ApplyRandomDeviation(direction, maxRandomAngle);

            // Передаем направление движения в скрипт препятствия
            obstacle.GetComponent<Obstacle>().Initialize(direction, obstacleSpeed);
        }
    }

    private Vector3 ApplyRandomDeviation(Vector3 direction, float maxAngle)
    {
        float angle = Random.Range(-maxAngle, maxAngle);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        return rotation * direction;
    }

    private IEnumerator MoveSawsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(MoveSaw(topSawPrefab, new Vector2(topSawPrefab.position.x, yTopTargetSawPos)));
        StartCoroutine(MoveSaw(botSawPrefab, new Vector2(botSawPrefab.position.x, yBotTargetSawPos)));
    }

    private IEnumerator MoveSaw(Transform saw, Vector2 targetPosition)
    {
        float elapsedTime = 0f;
        Vector2 startingPosition = saw.position;
        float moveDuration = 1f;

        while (elapsedTime < moveDuration)
        {
            saw.position = Vector2.Lerp(startingPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        saw.position = targetPosition;
    }

    public void SetObstacleSpeed(float value)
    {
        obstacleSpeed = value;
    }
}
