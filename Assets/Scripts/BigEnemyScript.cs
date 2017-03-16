using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigEnemyScript : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public float impactForce;
    public float deathSpinSpeed;
    public float destroyDelay;
	public float visionDistance;    
    public int startingHitPoints;
    public float hitColorDuration;
    public float firstFireDelay;
    public float fireRate;
    public Color originalColor;
    public Color hitColor;
    public GameObject enemyBullet;

    [HideInInspector]
    public bool isDead;

    private GameObject player1;
    private GameObject player2;
    private Transform player1Transform;
    private Transform player2Transform;
    private Transform bulletSpawnRight;
    private Rigidbody2D enemyRigidbody2D;
    private ParticleSystem bloodParticleSystem;
    private AudioSource hitSound;
    private AudioSource laserSound;
    private GameControllerScript gameControllerScript;    
	private bool isChasing;
    private Renderer[] rendererComponents;
    private GameObject healthBarUI;
    private Slider healthBar;
    private int hitPoints;
    private CameraScript cameraScript;

    void Start()
    {
        isDead = false;
		isChasing = false;
        player1 = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        if (player1 != null)
        {
            player1Transform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        if (player2 != null)
        {
            player2Transform = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();
        }
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
        bloodParticleSystem = GetComponent<ParticleSystem>();
        hitSound = GetComponents<AudioSource>()[0];
        laserSound = GetComponents<AudioSource>()[1];
        gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        rendererComponents = GetComponentsInChildren<Renderer>();
        RestoreColor();
        hitPoints = startingHitPoints;
        healthBarUI = GameObject.FindGameObjectWithTag("Boss Slider");
        healthBar = healthBarUI.GetComponent<Slider>();
        healthBarUI.SetActive(false);
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();

        var childTransforms = GetComponentsInChildren<Transform>();
        foreach (var childTransform in childTransforms)
        {
            if (childTransform.tag == "Bullet Spawn Right")
            {
                bulletSpawnRight = childTransform.transform;
            }
        }
    }
		
	void Update()
    {
        // Find closer player
        Transform playerTransform;
        if (player1 != null && player2 != null && player1.activeSelf && player2.activeSelf)
        {
            if (Vector3.Distance(transform.position, player1Transform.position) < Vector3.Distance(transform.position, player2Transform.position))
            {
                playerTransform = player1Transform;
            }
            else
            {
                playerTransform = player2Transform;
            }
        }
        else if (player1 != null && player1.activeSelf)
        {
            playerTransform = player1Transform;
        }
        else
        {
            playerTransform = player2Transform;
        }

        // Move, rotate toward player
        if (!isDead && isChasing)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.fixedDeltaTime);

            var angle = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
            var rotationTarget = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, Time.smoothDeltaTime * turnSpeed);
        }
		else if (isDead)
        {
            transform.Rotate(Vector3.forward * deathSpinSpeed * Time.smoothDeltaTime);
        }      

		// Did the enemy see the player? Todo: Add check for line of sight
		else if (!isChasing && Vector2.Distance(transform.position, playerTransform.position) <= visionDistance)
		{
			isChasing = true;
            healthBarUI.SetActive(true);
            InvokeRepeating("Shoot", firstFireDelay, fireRate);
            gameControllerScript.StartEnemySpawner();
            cameraScript.ZoomFullOut();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If hit by a bullet, die
        if (collision.gameObject.tag == "Bullet")
        {
            if (!isDead)
            {
                healthBar.value = ((float) --hitPoints) / startingHitPoints;

                if (hitPoints <= 0)
                {
                    isDead = true;
                    bloodParticleSystem.Play();
                    enemyRigidbody2D.AddForce(collision.gameObject.transform.position * impactForce, ForceMode2D.Impulse);
                    hitSound.Play();
                    gameControllerScript.IncrementScore();
                    Destroy(gameObject, destroyDelay);
                    CancelInvoke();
                }
                else
                {
                    for (int i = 0; i < rendererComponents.Length; ++i)
                    {
                        rendererComponents[i].material.color = hitColor;
                    }
                    Invoke("RestoreColor", hitColorDuration);
                }                
            }            
        }
    }

    void RestoreColor()
    {
        for (int i = 0; i < rendererComponents.Length; ++i)
        {
            rendererComponents[i].material.color = originalColor;
        }
    }

    void Shoot()
    {
        Instantiate(enemyBullet, bulletSpawnRight.position, bulletSpawnRight.rotation);
        laserSound.Play();
    }
}