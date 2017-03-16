using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public float impactForce;
    public float deathSpinSpeed;
    public float destroyDelay;
	public float visionDistance;

    [HideInInspector]
    public bool isDead;

    private GameObject player1;
    private GameObject player2;
    private Transform player1Transform;
    private Transform player2Transform;
    private Rigidbody2D enemyRigidbody2D;
    private ParticleSystem bloodParticleSystem;
    private AudioSource hitSound;
    private GameControllerScript gameControllerScript;    
	private bool isChasing;

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
        hitSound = GetComponent<AudioSource>();
        gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
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

        // Move and rotate toward the player
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
		else if ((player1 != null || player2 != null) &&Vector2.Distance(transform.position, playerTransform.position) <= visionDistance)
		{
			isChasing = true;
		}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If hit by a bullet, die
        if (collision.gameObject.tag == "Bullet")
        {
            if (!isDead)
            {
                isDead = true;
                bloodParticleSystem.Play();
				enemyRigidbody2D.AddForce(collision.gameObject.transform.position * impactForce, ForceMode2D.Impulse);
                hitSound.Play();
                gameControllerScript.IncrementScore();
                Destroy(gameObject, destroyDelay);
            }            
        }
    }
}