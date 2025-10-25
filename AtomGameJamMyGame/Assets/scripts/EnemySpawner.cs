using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WaveEnemy
{
    public GameObject enemyPrefab; // Spawnlanacak düþman
    public int count = 1;          // Bu düþmandan kaç tane
}

[System.Serializable]
public class Wave
{
    public string waveName;          // Dalga ismi
    public float waveDelay = 2f;     // Dalga baþlamadan önce bekleme süresi
    public float spawnInterval = 1f; // Düþmanlar arasý bekleme süresi
    public List<WaveEnemy> enemies;  // Bu dalgadaki düþmanlar
    public float spawnRadius = 3f;   // Spawn yarýçapý
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Dalga Ayarlarý")]
    public List<Wave> waves;

    [Header("UI")]
    public GameObject winPanel; // Kazandýn paneli

    private int currentWaveIndex = 0;
    private bool playerTriggered = false;

    public GameObject deadPanel;

    void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false); // Baþlangýçta gizli

        InvokeRepeating(nameof(CheckTowers), 0f, 1);
    }
    void CheckTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        if (towers.Length == 0)
        {
            Time.timeScale = 0;
            SFXManager.Instance.Gameover();
            if (deadPanel != null)
                deadPanel.SetActive(true);
        }
        else
        {
            if (deadPanel != null)
                deadPanel.SetActive(false);
        }
    }

    public void TriggerWaves()
    {
        if (!playerTriggered && waves.Count > 0)
        {
            playerTriggered = true;
            StartCoroutine(StartWave(waves[currentWaveIndex]));
        }
    }

    IEnumerator StartWave(Wave wave)
    {
        yield return new WaitForSeconds(wave.waveDelay);

        foreach (WaveEnemy we in wave.enemies)
        {
            for (int i = 0; i < we.count; i++)
            {
                SpawnEnemy(we.enemyPrefab, wave.spawnRadius);
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }

        currentWaveIndex++;
        if (currentWaveIndex < waves.Count)
        {
            StartCoroutine(StartWave(waves[currentWaveIndex]));
        }
        else
        {
            // Son dalga tamamlandý, Kazandýn panelini aç
            if (winPanel != null)
                Time.timeScale = 0;
            SFXManager.Instance.Win();
                winPanel.SetActive(true);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, float spawnRadius)
    {
        if (enemyPrefab == null) return;

        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = transform.position + (Vector3)randomOffset;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            TriggerWaves();
    }
}
