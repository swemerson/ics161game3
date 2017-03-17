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
    public int startingLives;
    public int respawnDelay;
    public float invulnerabilityDuration;
    public Color invulnerabilityFlashColor;
    public GameObject player1;
    public GameObject player2;

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
    private int livesP1;
    private int livesP2;
    private Text livesP1Text;
    private Text livesP2Text;
    private GameObject respawnTimerP1;
    private GameObject respawnTimerP2;
    private Player1ControllerScript player1Script;
    private Player2ControllerScript player2Script;

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
        livesP1 = startingLives;
        livesP2 = startingLives;
        livesP1Text = GameObject.FindGameObjectWithTag("Lives P1").GetComponent<Text>();
        livesP2Text = GameObject.FindGameObjectWithTag("Lives P2").GetComponent<Text>();
        livesP1Text.text = "Lives: " + livesP1;
        livesP2Text.text = "Lives: " + livesP2;
        respawnTimerP1 = GameObject.FindGameObjectWithTag("Respawn Timer P1");
        respawnTimerP2 = GameObject.FindGameObjectWithTag("Respawn Timer P2");
        respawnTimerP1.SetActive(false);
        respawnTimerP2.SetActive(false);
        player1Script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player1ControllerScript>();
        player2Script = GameObject.FindGameObjectWithTag("Player2").GetComponent<Player2ControllerScript>();
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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

    public void LoseLife(GameObject player)
    {        
        if (livesP1 > 0)
        {
            livesP1Text.text = "Lives: " + --livesP1;            
            StartCoroutine(RespawnPlayer(player));
            StartCoroutine(StartRespawnTimer());
        }

        if (livesP1 == 0 && livesP2 == 0)
        {
            GameOver();
        }
    }

    private IEnumerator RespawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(respawnDelay);
        player.SetActive(true);
        player1Script.isDead = false;
        player1Script.isInvulnerable = true;
        StartCoroutine(InvulnerabilityFlash(player));
        yield return new WaitForSeconds(invulnerabilityDuration);
        player1Script.isInvulnerable = false;
    }    

    private IEnumerator StartRespawnTimer()
    {
        respawnTimerP1.SetActive(true);
        var counter = respawnDelay;
        respawnTimerP1.GetComponent<Text>().text = "Respawning in " + counter + "...";
        for (int i = 0; i < respawnDelay; ++i)
        {
            yield return new WaitForSeconds(1);
            respawnTimerP1.GetComponent<Text>().text = "Respawning in " + --counter + "...";
        }
        respawnTimerP1.SetActive(false);
    }

    public void LoseLifeP2(GameObject player)
    {
        if (livesP2 > 0)
        {
            livesP2Text.text = "Lives: " + --livesP2;
            StartCoroutine(RespawnPlayer2(player));
            StartCoroutine(StartRespawnTimerP2());
        }

        if (livesP1 == 0 && livesP2 == 0)
        {
            GameOver();
        }
    }

    private IEnumerator RespawnPlayer2(GameObject player)
    {
        yield return new WaitForSeconds(respawnDelay);
        player.SetActive(true);
        player2Script.isDead = false;
        player2Script.isInvulnerable = true;
        StartCoroutine(InvulnerabilityFlash(player));
        yield return new WaitForSeconds(invulnerabilityDuration);
        player2Script.isInvulnerable = false;
    }

    private IEnumerator StartRespawnTimerP2()
    {
        respawnTimerP2.SetActive(true);
        var counter = respawnDelay;
        respawnTimerP2.GetComponent<Text>().text = "Respawning in " + counter + "...";
        for (int i = 0; i < respawnDelay; ++i)
        {
            yield return new WaitForSeconds(1);
            respawnTimerP2.GetComponent<Text>().text = "Respawning in " + --counter + "...";
        }
        respawnTimerP2.SetActive(false);
    }

    IEnumerator InvulnerabilityFlash(GameObject player)
    {
        for (int i = 0; i < invulnerabilityDuration * 10; ++i)
        {
            var oldColor = player.GetComponent<Renderer>().material.color;
            yield return new WaitForSeconds(0.05f);
            player.GetComponent<Renderer>().material.color = invulnerabilityFlashColor;
            yield return new WaitForSeconds(0.05f);
            player.GetComponent<Renderer>().material.color = oldColor;
        }
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

		var chargeEnemies = GameObject.FindGameObjectsWithTag("Charge Enemy");
		foreach (var enemy in chargeEnemies)
		{
			Destroy(enemy);
		}

		var spinners = GameObject.FindGameObjectsWithTag("Spinner");
		foreach (var enemy in spinners)
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