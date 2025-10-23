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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(SpawnWaveLoop());
    }

    IEnumerator SpawnWaveLoop()
    {
        while (true)
        {
            // Wait for any current wave to finish
            while (isSpawning || enemiesAlive > 0)
                yield return null;

            // Wait between waves for pacing
            yield return new WaitForSeconds(waveBreakDuration);

            currentWave++;
            enemiesKilled = 0;
            isSpawning = true;

            Debug.Log($"🌊 Starting Wave {currentWave}");

            // Spawn the wave
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnDelay);
            }

            isSpawning = false;
        }
    }

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

        // Optional: only spawn next wave early if all are dead
        if (enemiesAlive <= 0)
        {
            Debug.Log("All enemies defeated! Preparing next wave...");
        }
    }
}
