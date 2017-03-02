using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public float moveSpeed;
	public float dashSpeed;
    public float turnSpeed;
    public float fireRate;
    public float lerpMoveSpeed;
    public GameObject bullet;

    private float nextFire;
    private bool isDead;
    private Transform playerTransform;    
    private Transform bulletSpawnRight;
    private AudioSource shootSound;
    private AudioSource explosionSound;    
    private ParticleSystem bloodSpray;
    private GameControllerScript gameControllerScript;

    void Start()
    {
        isDead = false;
        playerTransform = GetComponent<Transform>();
        var childTransforms = GetComponentsInChildren<Transform> ();
        foreach (var childTransform in childTransforms)
        {
            if (childTransform.tag == "Bullet Spawn Right")
            {
                bulletSpawnRight = childTransform.transform;
            }
        }
        shootSound = GetComponents<AudioSource>()[0];
        explosionSound = GetComponents<AudioSource>()[1];
        bloodSpray = GetComponent<ParticleSystem>();
        gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
    }
		
    void Update()
    {
        if (!isDead)
        {
            // Rotate character
            if (Input.GetAxisRaw("Joy Right Horizontal") != 0 || Input.GetAxisRaw("Joy Right Vertical") != 0)
            {
                var targetX = Input.GetAxisRaw("Joy Right Horizontal");
                var targetY = Input.GetAxisRaw("Joy Right Vertical");
                var angle = Mathf.Atan2(targetY, targetX) * Mathf.Rad2Deg - 90f;
                var rotationTarget = Quaternion.Euler(new Vector3(0, 0, angle));
                playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, rotationTarget, turnSpeed);
            }
            else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                var objectPos = Camera.main.WorldToScreenPoint(playerTransform.position);
                var targetX = Input.mousePosition.x - objectPos.x;
                var targetY = Input.mousePosition.y - objectPos.y;
                var angle = Mathf.Atan2(targetY, targetX) * Mathf.Rad2Deg - 90f;
                var rotationTarget = Quaternion.Euler(new Vector3(0, 0, angle));
                playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, rotationTarget, turnSpeed);
            }       

            // Shoot
            if (Input.GetAxis("Fire1") != 0 && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(bullet, bulletSpawnRight.position, bulletSpawnRight.rotation);
                shootSound.Play();
            }

            // Move or Dash
            var moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
            moveDirection.Normalize();
<<<<<<< HEAD
            var moveTarget = playerTransform.position += moveDirection * moveSpeed * Time.smoothDeltaTime;
            playerTransform.position = Vector3.Lerp(playerTransform.position, moveTarget, lerpMoveSpeed);
=======

			if (Input.GetKeyDown (KeyCode.Space)) 
			{
				playerTransform.position = Vector3.Lerp (playerTransform.position, playerTransform.position += moveDirection * dashSpeed * Time.smoothDeltaTime, lerpMoveSpeed);
			} 
			else 
			{
				playerTransform.position = Vector3.Lerp(playerTransform.position, playerTransform.position += moveDirection * moveSpeed * Time.smoothDeltaTime, lerpMoveSpeed);
			}
>>>>>>> origin/master
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If hit by an enemy, end the game
        if (collision.gameObject.tag == "Enemy")
        {
            explosionSound.Play();            
            bloodSpray.Play();
            isDead = true;
            gameControllerScript.GameOver();            
        }
    }
}