using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1ControllerScript : MonoBehaviour
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
    public float deathDelay;
    public float deathForce;

    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public bool isInvulnerable;

    private float nextFire;
	private float nextDash;    
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
	private Rigidbody2D rigidBody2d;

    void Start()
    {
        isDead = false;
        isInvulnerable = false;
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
        ammoLoaded = 5;
        ammoStored = 0;
		rigidBody2d = GetComponent<Rigidbody2D>();
    }
		
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(0);
        }

        if (!isDead)
        {
            // Rotate character
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                var objectPos = Camera.main.WorldToScreenPoint(transform.position);
                var targetX = Input.mousePosition.x - objectPos.x;
                var targetY = Input.mousePosition.y - objectPos.y;
                var angle = Mathf.Atan2(targetY, targetX) * Mathf.Rad2Deg - 90f;            
                rigidBody2d.MoveRotation(angle + turnSpeed * Time.fixedDeltaTime);
            }       

            // Shoot
            if (Input.GetAxis("Fire1") != 0)
            {
                Shoot();
            }            

            // Move or Dash
			var moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			moveDirection.Normalize();
			if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time > nextDash)
			{
				moveSpeed += dashSpeed;
				Invoke ("DashComplete", dashDuration);
				isDashing = true;
				nextDash = Time.time + dashCooldown;
				gameControllerScript.Dash (dashCooldown);
			}   
			rigidBody2d.MovePosition(rigidBody2d.position + moveDirection * moveSpeed * Time.fixedDeltaTime);


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
        if (!isReloading && Time.time > nextFire)
        {
            if (ammoLoaded > 0)
            {
                nextFire = Time.time + fireInterval;
                Instantiate(bullet, bulletSpawnRight.position, bulletSpawnRight.rotation);
                shootSound.Play();
                --ammoLoaded;
                gameControllerScript.UpdateAmmoText(ammoStored, ammoLoaded);
            }
            else if (ammoLoaded == 0 && ammoStored > 0)
            {
                isReloading = true;
                gameControllerScript.Reload(reloadDuration);
                reloadSound.Play();
                Invoke("Reload", reloadDuration);
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
        // If hit by a live enemy, lose life
        if (!isDead && !isInvulnerable)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                var enemyScript = collision.gameObject.GetComponent<EnemyScript>();
                if (!enemyScript.isDead)
                {
                    Die();
                }
            }

            else if (collision.gameObject.tag == "Charge Enemy")
            {
                var enemyScript = collision.gameObject.GetComponent<EnemyTwoScript>();
                if (!enemyScript.isDead)
                {
                    Die();
                }
            }

            else if (collision.gameObject.tag == "Big Enemy")
            {
                var enemyScript = collision.gameObject.GetComponent<BigEnemyScript>();
                if (!enemyScript.isDead)
                {
                    Die();
                }
            }

            else if (collision.gameObject.tag == "Enemy Bullet" || collision.gameObject.tag == "Spinner")
            {
                Die();
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
        else if (collision.gameObject.tag == "UnlimitedAmmo")
        {
            UnlimitedAmmoScript uas = collision.transform.parent.GetComponent<UnlimitedAmmoScript>();
            if(uas != null && uas.CanGetAmmo())
            {
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

    void Die()
    {
        rigidBody2d.AddForce(new Vector2(Random.Range(-deathForce, deathForce), Random.Range(-deathForce, deathForce)), ForceMode2D.Impulse);
        explosionSound.Play();
        bloodSpray.Play();
        gameControllerScript.LoseLife(gameObject);
        Invoke("Disappear", deathDelay);
        isDead = true;
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }
}