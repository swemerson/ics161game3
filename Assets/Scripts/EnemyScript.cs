using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public float bulletImpactForce;
    public float deathSpinSpeed;
    public float destroyDelay;

    private Transform playerTransform;
    private Transform enemyTransform;
    private Rigidbody2D enemyRigidbody2D;
    private ParticleSystem bloodParticleSystem;
    private AudioSource hitSound;
    private GameControllerScript gameControllerScript;
    private bool isDead;

	void Start()
    {
        isDead = false;
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTransform = GetComponent<Transform>();
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
        bloodParticleSystem = GetComponent<ParticleSystem>();
        hitSound = GetComponent<AudioSource>();
        gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
    }
		
	void Update()
    {
        // Move and rotate toward the player
        if (!isDead)
        {
            enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, playerTransform.position, moveSpeed * Time.fixedDeltaTime);
            var angle = Mathf.Atan2(playerTransform.position.y - enemyTransform.position.y, playerTransform.position.x - enemyTransform.position.x) * Mathf.Rad2Deg - 90f;
            var rotationTarget = Quaternion.Euler(0, 0, angle);
            enemyTransform.rotation = Quaternion.Lerp(enemyTransform.rotation, rotationTarget, Time.smoothDeltaTime * turnSpeed);
        }
        else
        {
            transform.Rotate(Vector3.forward * deathSpinSpeed * Time.smoothDeltaTime);
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
                enemyRigidbody2D.AddForce(collision.gameObject.transform.position * bulletImpactForce, ForceMode2D.Impulse);
                hitSound.Play();
                gameControllerScript.IncrementScore();
                Destroy(gameObject, destroyDelay);
            }            
        }
    }
}