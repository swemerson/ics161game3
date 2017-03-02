using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    public float bulletImpactForce;

    private Transform playerTransform;
    private Transform enemyTransform;
    private Rigidbody2D enemyRigidbody2D;
    private ParticleSystem bloodParticleSystem;
    private AudioSource hitSound;
    private GameControllerScript gameControllerScript;

	void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyTransform = GetComponent<Transform>();
        enemyRigidbody2D = GetComponent<Rigidbody2D>();
        bloodParticleSystem = GetComponent<ParticleSystem>();
        hitSound = GetComponent<AudioSource>();
        gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
    }
		
	void Update()
    {
        // Move and rotate toward the player (Rotation is acting funky right now)
        enemyTransform.position = Vector3.MoveTowards(enemyTransform.position, playerTransform.position, moveSpeed * Time.fixedDeltaTime);
        enemyTransform.rotation = Quaternion.Lerp(enemyTransform.rotation, Quaternion.Euler(0, 0, Mathf.Atan2(playerTransform.position.y, playerTransform.position.x) * Mathf.Rad2Deg - 90f), Time.smoothDeltaTime * turnSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If hit by a bullet, die
        if (collision.gameObject.tag == "Bullet")
        {
            bloodParticleSystem.Play();
            enemyRigidbody2D.AddForce(collision.gameObject.transform.position * bulletImpactForce, ForceMode2D.Impulse);
            hitSound.Play();
            gameControllerScript.IncrementScore();
        }
    }
}