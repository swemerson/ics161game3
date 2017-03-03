using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{    
    public float enemySpawnDelay;
    public float enemySpawnInterval;
    public float enemySpawnAmount;
    public float increaseEnemiesDelay;
    public float increaseEnemiesInterval;
    public float increaseEnemiesAmount;
    public GameObject enemy;
    public GameObject floor;

    private int score;
    private Text scoreBox;
    private Text deathBox;
    private GameObject[] enemySpawns;

	void Start()
    {
        score = 0;
        scoreBox = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        deathBox = GameObject.FindGameObjectWithTag("Death").GetComponent<Text>();
        enemySpawns = GameObject.FindGameObjectsWithTag("Enemy Spawn Point");
        deathBox.enabled = false;
        InvokeRepeating("EnemySpawn", enemySpawnDelay, enemySpawnInterval);
        InvokeRepeating("IncreaseEnemies", increaseEnemiesDelay, increaseEnemiesInterval);
	}

    void Update()
    {
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene("ics161game3");
        }
    }

    // Places enemies randomly in the arena
    void EnemySpawn()
    {
        for (int i = 0; i < enemySpawnAmount; ++i)
        {
            // Get random spawn point and spawn enemy
            var spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Length)].transform.position;
            Instantiate(enemy, new Vector2(spawnPoint.x, spawnPoint.y), Quaternion.identity);
        }        
    }

    // Increases the number of enemies that will spawn in SpawnEnemy ()
    void IncreaseEnemies()
    {
        enemySpawnAmount += increaseEnemiesAmount;
    }

    // Updates the scoreboard
    public void IncrementScore()
    {
        scoreBox.text = $"SCORE: { ++score }";
    }

    // Removes all enemies, and displays the Game Over message
    public void GameOver()
    {
        CancelInvoke();
        
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        deathBox.enabled = true;
    }
}