using UnityEngine;

public class BallSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float spawnRangeX = 4f;

    private float _timer;
    private bool _firstSpawnDone = false;

    void Update()
    {
        _timer += Time.deltaTime;

        if (!_firstSpawnDone)
        {
            if (_timer >= 2f)
            {
                SpawnBall();
                _timer = 0f;
                _firstSpawnDone = true;
            }
        }
        else
        {
            if (_timer >= spawnInterval)
            {
                SpawnBall();
                _timer = 0f;
            }
        }
    }

    void SpawnBall()
    {
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, 11f, 42f);
        Instantiate(ballPrefab, spawnPos, Quaternion.identity);
    }
}
