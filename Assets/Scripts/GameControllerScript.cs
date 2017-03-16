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
	private Text dashBox;
	private Slider dashSlider;
    private Text ammoBox;
    private Slider ammoSlider;
    private Text dashBoxP2;
    private Slider dashSliderP2;
    private Text ammoBoxP2;
    private Slider ammoSliderP2;
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
        dashBoxP2 = GameObject.FindGameObjectWithTag("Dash Text P2").GetComponent<Text>();
        dashSliderP2 = GameObject.FindGameObjectWithTag("Dash Slider P2").GetComponent<Slider>();
        ammoBoxP2 = GameObject.FindGameObjectWithTag("Ammo Text P2").GetComponent<Text>();
        ammoSliderP2 = GameObject.FindGameObjectWithTag("Ammo Slider P2").GetComponent<Slider>();
        enemySpawns = GameObject.FindGameObjectsWithTag("Enemy Spawn Point");
        deathBox.enabled = false;       
	}

    void Update()
    {
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StartEnemySpawner()
    {
        InvokeRepeating("EnemySpawn", enemySpawnDelay, enemySpawnInterval);
        InvokeRepeating("IncreaseEnemies", increaseEnemiesDelay, increaseEnemiesInterval);
    }

    // Places enemies randomly in the arena
    void EnemySpawn()
    {
        for (int i = 0; i < enemySpawnAmount; ++i)
        {
            // Get random spawn point and spawn enemy
            var spawnPoint = enemySpawns[Random.Range(0, enemySpawns.Length)].transform.position;
            Instantiate(enemy, new Vector2(spawnPoint.x, spawnPoint.y), Quaternion.identity);

            // Update enemy vision distances **** THIS IS TERRIBLE, MUST FIX!!! ****
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<EnemyScript>().visionDistance = 9999f;
            }
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

    public void DashP2(float dashCooldown)
    {
        StartCoroutine(FillDashMeterP2(dashCooldown));
    }

    IEnumerator FillDashMeterP2(float dashCooldown)
    {
        dashBoxP2.color = Color.gray;
        dashSliderP2.value = 0f;

        float fillTime = 0;
        while (fillTime < dashCooldown)
        {
            dashSliderP2.value = fillTime / dashCooldown;
            yield return null;
            fillTime += Time.deltaTime;
        }

        dashBoxP2.color = Color.white;
        dashSliderP2.value = 1f;
    }

    public void ReloadP2(float reloadDuration)
    {
        StartCoroutine(FillAmmoMeterP2(reloadDuration));
    }

    IEnumerator FillAmmoMeterP2(float reloadDuration)
    {
        ammoBoxP2.color = Color.gray;
        ammoSliderP2.value = 0f;

        float fillTime = 0;
        while (fillTime < reloadDuration)
        {
            ammoSliderP2.value = fillTime / reloadDuration;
            yield return null;
            fillTime += Time.deltaTime;
        }

        ammoBoxP2.color = Color.white;
        ammoSliderP2.value = 1f;
    }

    public void UpdateAmmoTextP2(int ammoStored, int ammoLoaded)
    {
        ammoBoxP2.text = "Ammo: " + ammoLoaded + " / " + ammoStored;
    }
}