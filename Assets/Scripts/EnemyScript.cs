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

    private Transform playerTransform;
    private Rigidbody2D enemyRigidbody2D;
    private ParticleSystem bloodParticleSystem;
    private AudioSource hitSound;
    private GameControllerScript gameControllerScript;    
	private bool isChasing;

	void Start()
    {
        isDead = false;
		isChasing = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
        bloodParticleSystem = GetComponent<ParticleSystem>();
        hitSound = GetComponent<AudioSource>();
        gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
    }
		
	void Update()
    {
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
		else if (Vector2.Distance(transform.position, playerTransform.position) <= visionDistance)
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