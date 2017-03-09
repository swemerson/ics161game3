using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public float moveSpeed;
	public float dashSpeed;
	public float dashDuration;
	public float dashCooldown;
    public float turnSpeed;
    public float fireInterval;
    public float reloadDuration;
    public int bulletsPerMagazine;
    public int ammoPickupAmount;
    public int maxAmmoStored;
    public GameObject bullet;

    private float nextFire;
	private float nextDash;
    private bool isDead;
	private bool isDashing;
    private bool isReloading; 
    private Transform bulletSpawnRight;
    private AudioSource shootSound;
    private AudioSource explosionSound;
    private AudioSource reloadSound;
    private AudioSource ammoPickupSound;
    private AudioSource emptyClickSound;
    private ParticleSystem bloodSpray;
    private GameControllerScript gameControllerScript;
    private int ammoLoaded;
    private int ammoStored;

    void Start()
    {
        isDead = false;
		isDashing = false;
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
        reloadSound = GetComponents<AudioSource>()[2];
        ammoPickupSound = GetComponents<AudioSource>()[3];
        emptyClickSound = GetComponents<AudioSource>()[4];
        bloodSpray = GetComponent<ParticleSystem>();
        gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        ammoLoaded = 0;
        ammoStored = 0;
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
                transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, turnSpeed);
                Shoot();
            }
            else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                var objectPos = Camera.main.WorldToScreenPoint(transform.position);
                var targetX = Input.mousePosition.x - objectPos.x;
                var targetY = Input.mousePosition.y - objectPos.y;
                var angle = Mathf.Atan2(targetY, targetX) * Mathf.Rad2Deg - 90f;
                var rotationTarget = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, turnSpeed);
            }       

            // Shoot
            if (Input.GetAxis("Fire1") != 0)
            {
                Shoot();
            }            

            // Move or Dash
            var moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
            moveDirection.Normalize();
			if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time > nextDash)
            {
				moveSpeed += dashSpeed;
				Invoke ("DashComplete", dashDuration);
				isDashing = true;
				nextDash = Time.time + dashCooldown;
				gameControllerScript.Dash (dashCooldown);
            }                
			transform.position += moveDirection * moveSpeed * Time.smoothDeltaTime;

            // Reload
            if (Input.GetButtonDown("Reload") && !isReloading && ammoStored > 0)
            {
                isReloading = true;
                gameControllerScript.Reload(reloadDuration);
                reloadSound.Play();
                Invoke("Reload", reloadDuration);
            }
        }
    }

    void Shoot()
    {
        if (Time.time > nextFire)
        {
            if (ammoLoaded > 0)
            {
                nextFire = Time.time + fireInterval;
                Instantiate(bullet, bulletSpawnRight.position, bulletSpawnRight.rotation);
                shootSound.Play();
                --ammoLoaded;
                gameControllerScript.UpdateAmmoText(ammoStored, ammoLoaded);
            }
            else
            {
                emptyClickSound.Play();
            }
        }        
    }

    void Reload()
    {
        while (ammoLoaded < bulletsPerMagazine && ammoStored > 0)
        {
            ++ammoLoaded;
            --ammoStored;
        }

        isReloading = false;
        gameControllerScript.UpdateAmmoText(ammoStored, ammoLoaded);
    }

	void DashComplete()
	{
		moveSpeed -= dashSpeed;
		isDashing = false;
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If hit by a live enemy, end the game
        if (collision.gameObject.tag == "Enemy")
        {
            var enemyScript = collision.gameObject.GetComponent<EnemyScript>();
            if (!enemyScript.isDead)
            {
                explosionSound.Play();
                bloodSpray.Play();
                isDead = true;
                gameControllerScript.GameOver();
            }
        }

        // If ran into ammo box, collect it
        if (collision.gameObject.tag == "Ammo")
        {
            Destroy(collision.gameObject);
            ammoPickupSound.Play();

            ammoStored += ammoPickupAmount;
            if (ammoStored > maxAmmoStored)
            {
                ammoStored = maxAmmoStored;
            }

            gameControllerScript.UpdateAmmoText(ammoStored, ammoLoaded);
        }
    }
}