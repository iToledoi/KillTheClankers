using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPointConfig
    {
        public string name;
        public Transform spawnTransform;
        [Tooltip("How many enemies to spawn. If infinite is true, this is ignored.")]
        public int spawnCount = 1;
        [Tooltip("Time in seconds between spawns for this spawn point")]
        public float spawnInterval = 1f;
        [Tooltip("Delay in seconds before this spawn point starts spawning")]
        public float startDelay = 0f;
        [Tooltip("When true, this spawn point will spawn indefinitely until stopped")]
        public bool infinite = false;

        // runtime
        [HideInInspector]
        public bool running = false;
    }

    [Tooltip("Enemy prefab to spawn (must have AIEnemy component)")]
    public GameObject enemyPrefab;

    [Tooltip("Per-spawn-point configuration")]
    public SpawnPointConfig[] spawnPoints = new SpawnPointConfig[0];

    [Tooltip("Player transform reference (optional). If null the spawner will try to find the Player by tag at runtime.")]
    public Transform player;

    private void Start()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        StartAll();

    }

    private void Update(){
        if (player == null)
        {
            StopAll();
        }
    }

    // Spawn immediately at a given spawn point config (by index)
    public GameObject SpawnOneAt(int spawnPointIndex)
    {
        if (enemyPrefab == null) return null;
        if (spawnPoints == null || spawnPoints.Length == 0) return null;
        if (spawnPointIndex < 0 || spawnPointIndex >= spawnPoints.Length) return null;

        var cfg = spawnPoints[spawnPointIndex];
        Transform spawn = cfg.spawnTransform != null ? cfg.spawnTransform : this.transform;

        GameObject enemy = Instantiate(enemyPrefab, spawn.position, spawn.rotation);
        AssignPlayerToEnemy(enemy);
        return enemy;
    }

    // Start spawning for all configured spawn points
    public void StartAll()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            StartSpawnPoint(i);
        }
    }

    // Stop spawning for all spawn points
    public void StopAll()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            StopSpawnPoint(i);
        }
    }

    // Start a specific spawn point by index
    public void StartSpawnPoint(int spawnPointIndex)
    {
        if (spawnPoints == null || spawnPointIndex < 0 || spawnPointIndex >= spawnPoints.Length) return;
        var cfg = spawnPoints[spawnPointIndex];
        if (cfg.running) return;
        cfg.running = true;
        StartCoroutine(SpawnRoutine(cfg));
    }

    // Stop a specific spawn point by index
    public void StopSpawnPoint(int spawnPointIndex)
    {
        if (spawnPoints == null || spawnPointIndex < 0 || spawnPointIndex >= spawnPoints.Length) return;
        var cfg = spawnPoints[spawnPointIndex];
        cfg.running = false;
    }

    private IEnumerator SpawnRoutine(SpawnPointConfig cfg)
    {
        if (cfg.startDelay > 0f)
        {
            yield return new WaitForSeconds(cfg.startDelay);
        }

        int spawned = 0;
        while (cfg.running && (cfg.infinite || spawned < cfg.spawnCount))
        {
            Transform spawn = cfg.spawnTransform != null ? cfg.spawnTransform : this.transform;
            GameObject enemy = Instantiate(enemyPrefab, spawn.position, spawn.rotation);
            AssignPlayerToEnemy(enemy);
            spawned++;
            if (!cfg.infinite && spawned >= cfg.spawnCount) break;
            yield return new WaitForSeconds(Mathf.Max(0.01f, cfg.spawnInterval));
        }

        cfg.running = false;
    }

    private void AssignPlayerToEnemy(GameObject enemy)
    {
        if (enemy == null) return;

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player != null)
        {
            var ai = enemy.GetComponent<AIEnemy>();
            if (ai != null)
            {
                ai.player = player;
            }

            var weaponParent = enemy.GetComponentInChildren<WeaponParentAI>();
            if (weaponParent != null)
            {
                weaponParent.player = player;
            }
        }
    }

    // Convenience: spawn now at all spawn points (one per point)
    public void SpawnAtAllPointsOnce()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnOneAt(i);
        }
    }
}
