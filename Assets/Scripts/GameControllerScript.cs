using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public bool useEnemySpawner;
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
	private Text dashBox;
	private Slider dashSlider;
    private Text ammoBox;
    private Slider ammoSlider;
    private GameObject[] enemySpawns;

	void Start()
    {
        score = 0;
        scoreBox = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
        deathBox = GameObject.FindGameObjectWithTag("Death").GetComponent<Text>();
		dashBox = GameObject.FindGameObjectWithTag("Dash Text").GetComponent<Text>();
		dashSlider = GameObject.FindGameObjectWithTag("Dash Slider").GetComponent<Slider>();
        ammoBox = GameObject.FindGameObjectWithTag("Ammo Text").GetComponent<Text>();
        ammoSlider = GameObject.FindGameObjectWithTag("Ammo Slider").GetComponent<Slider>();
        enemySpawns = GameObject.FindGameObjectsWithTag("Enemy Spawn Point");
        deathBox.enabled = false;
        if (useEnemySpawner)
        {
            InvokeRepeating("EnemySpawn", enemySpawnDelay, enemySpawnInterval);
            InvokeRepeating("IncreaseEnemies", increaseEnemiesDelay, increaseEnemiesInterval);
        }        
	}

    void Update()
    {
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        scoreBox.text = "SCORE: " + ++score;
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

	public void Dash(float dashCooldown)
	{
		StartCoroutine (FillDashMeter(dashCooldown));
	}

	IEnumerator FillDashMeter(float dashCooldown)
	{
		dashBox.color = Color.gray;
		dashSlider.value = 0f;

		float fillTime = 0;
		while (fillTime < dashCooldown)
		{
			dashSlider.value = fillTime / dashCooldown;
			yield return null;
			fillTime += Time.deltaTime;
		}

		dashBox.color = Color.white;
		dashSlider.value = 1f;
	}

    public void Reload(float reloadDuration)
    {
        StartCoroutine(FillAmmoMeter(reloadDuration));
    }

    IEnumerator FillAmmoMeter(float reloadDuration)
    {
        ammoBox.color = Color.gray;
        ammoSlider.value = 0f;

        float fillTime = 0;
        while (fillTime < reloadDuration)
        {
            ammoSlider.value = fillTime / reloadDuration;
            yield return null;
            fillTime += Time.deltaTime;
        }

        ammoBox.color = Color.white;
        ammoSlider.value = 1f;
    }

    public void UpdateAmmoText(int ammoStored, int ammoLoaded)
    {
        ammoBox.text = "Ammo: " + ammoLoaded + " / " + ammoStored;
    }
}