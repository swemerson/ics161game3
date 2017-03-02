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
            // Rotate to mouse position
            var mousePos = Input.mousePosition;
            var objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;
            var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), turnSpeed);

            // Shoot
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(bullet, bulletSpawnRight.position, bulletSpawnRight.rotation);
                shootSound.Play();
            }

            // Move or Dash
            var moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
            moveDirection.Normalize();

			if (Input.GetKeyDown (KeyCode.Space)) 
			{
				playerTransform.position = Vector3.Lerp (playerTransform.position, playerTransform.position += moveDirection * dashSpeed * Time.smoothDeltaTime, lerpMoveSpeed);
			} 
			else 
			{
				playerTransform.position = Vector3.Lerp(playerTransform.position, playerTransform.position += moveDirection * moveSpeed * Time.smoothDeltaTime, lerpMoveSpeed);
			}
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