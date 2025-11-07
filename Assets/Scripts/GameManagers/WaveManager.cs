using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public List<Wave> waves = new List<Wave>();
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    void Start()
    {
       
    }

    public void StartWaves()
    {
        StartCoroutine(RunWaves());
    }

    IEnumerator RunWaves()
    {
        // Loop through each wave
        foreach (Wave wave in waves)
        {
            Debug.Log($"Starting wave with {wave.amount} enemies...");
            
            // Spawn each enemy in this wave with small time gaps
            for (int i = 0; i < wave.amount; i++)
            {
                // Decide which spawn point based on 'side'
                Transform spawnPoint = wave.side ? rightSpawnPoint : leftSpawnPoint;

                int path = 0;
                if (wave.side) path = 1;

                Enemy enemy = Instantiate(wave.enemytype, spawnPoint.position, Quaternion.identity).GetComponent<Enemy>();
                enemy.SetPath(path);
                enemy.InitializeEnemy();
                Debug.Log("Spawned enemy " + (i + 1));

                GameManager.Instance.AddEnemy();

                yield return new WaitForSeconds(2f); // 2 seconds between enemies
            }

            // Wait for this wave's delay before starting the next wave
            Debug.Log($"Waiting {wave.delay} seconds before next wave...");
            yield return new WaitForSeconds(wave.delay);
        }

        Debug.Log("All waves complete!");
        GameManager.Instance.SetState(GameManager.GameState.AllEnemiesSpawned);
    }


}
