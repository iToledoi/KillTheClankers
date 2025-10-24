using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int enemiesPerWave = 5;
    public float spawnDelay = 1f;
    public float waveBreakDuration = 5f;

    [Header("Wave Control")]
    public int enemiesKilled = 0;
    public int enemiesAlive = 0;
    private bool isSpawning = false;
    private int currentWave = 0;

    private GameObject player;

    //Get player reference on start
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SpawnWaveLoop());
    }

    // Main wave spawning loop
    IEnumerator SpawnWaveLoop()
    {
        while (true)
        {
            //End the loop if the player is dead
            if (player == null)
            {
                yield break;
            }

            // Wait for waveBreajkDuration before starting next wave
            yield return new WaitForSeconds(waveBreakDuration);

            currentWave++;
            enemiesKilled = 0;
            isSpawning = true;

            Debug.Log($"Starting Wave {currentWave}");

            // Spawn wave
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }
            enemiesPerWave += 2; // Increase difficulty each wave
            isSpawning = false;
        }
    }

    // Spawn a single enemy at a random spawn point
    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPrefab == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Assign player target to spawned enemy
        var shooter = enemy.GetComponentInChildren<Shooting>();
        if (shooter != null)
            shooter.target = player.transform;

        enemiesAlive++;
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
        enemiesKilled++;

        // only spawn next wave early if all are dead
        if (enemiesAlive <= 0)
        {
            Debug.Log("All enemies defeated! Preparing next wave...");
        }
    }
}
