using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public float enemySpawnDelay;
    public float enemySpawnInterval;
    public float enemiesToSpawn;
    public float enemiesToSpawnIncrease;
    public float enemiesToSpawnIncreaseDelay;
    public float enemiesToSpawnIncreaseInterval;
    public GameObject enemy;
    public GameObject floor;

    private int score;
    private Text scoreBox;
    private Text deathBox;

	void Start()
    {
        score = 0;
        scoreBox = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        deathBox = GameObject.FindGameObjectWithTag("Death").GetComponent<Text>();
        deathBox.enabled = false;
        InvokeRepeating("SpawnEnemy", enemySpawnDelay, enemySpawnInterval);
        InvokeRepeating("IncreaseEnemies", enemiesToSpawnIncreaseDelay, enemiesToSpawnIncreaseInterval);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("ics161game3");
        }
    }

    // Places enemies randomly in the arena
    void SpawnEnemy()
    {
        for (int i = 0; i < enemiesToSpawn; ++i)
        {
            float randomX = Random.Range(-1 * floor.transform.localScale.x / 2, floor.transform.localScale.x / 2);
            float randomY = Random.Range(-1 * floor.transform.localScale.y / 2, floor.transform.localScale.y / 2);
            Instantiate(enemy, new Vector2(randomX, randomY), Quaternion.identity);
        }
    }

    // Increases the number of enemies that will spawn in SpawnEnemy ()
    void IncreaseEnemies()
    {
        enemiesToSpawn += enemiesToSpawnIncrease;
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