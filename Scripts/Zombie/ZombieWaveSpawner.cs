using UnityEngine;

public class ZombieWaveSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform target;          // bunker / attack point

    [Header("Wave Settings")]
    [SerializeField] private int zombiesInWave = 5;
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private float spawnInterval = 1.5f;

    private int _spawnedCount;
    private bool _spawning;

    private void Start()
    {
        if (zombiePrefab == null || target == null)
        {
            Debug.LogError("ZombieWaveSpawner: Missing prefab or target.");
            enabled = false;
            return;
        }

        StartWave();
    }

    public void StartWave()
    {
        _spawnedCount = 0;
        _spawning = true;
        InvokeRepeating(nameof(SpawnZombie), 0f, spawnInterval);
    }

    private void SpawnZombie()
    {
        if (!_spawning) return;

        if (_spawnedCount >= zombiesInWave)
        {
            _spawning = false;
            CancelInvoke(nameof(SpawnZombie));
            return;
        }

        Vector2 offset2D = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = transform.position + new Vector3(offset2D.x, 0f, offset2D.y);

        GameObject zombieObj = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

        ZombieAI ai = zombieObj.GetComponent<ZombieAI>();
        if (ai != null)
        {
            ai.Target = target;
        }

        _spawnedCount++;
    }
}
