using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WaveEnemy
{
    public GameObject enemyPrefab; // Spawnlanacak d��man
    public int count = 1;          // Bu d��mandan ka� tane
}

[System.Serializable]
public class Wave
{
    public string waveName;          // Dalga ismi
    public float waveDelay = 2f;     // Dalga ba�lamadan �nce bekleme s�resi
    public float spawnInterval = 1f; // D��manlar aras� bekleme s�resi
    public List<WaveEnemy> enemies;  // Bu dalgadaki d��manlar
    public float spawnRadius = 3f;   // Spawn yar��ap�
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Dalga Ayarlar�")]
    public List<Wave> waves;

    [Header("UI")]
    public GameObject winPanel; // Kazand�n paneli

    private int currentWaveIndex = 0;
    private bool playerTriggered = false;

    public GameObject deadPanel;

    void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false); // Ba�lang��ta gizli

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
            // Son dalga tamamland�, Kazand�n panelini a�
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
