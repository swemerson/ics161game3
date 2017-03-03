using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float moveSpeed;
    public float bulletLifetime;

    [HideInInspector]
    public bool isLethal;

    private Rigidbody2D bulletRigidBody2D;
    private Transform bulletSpawnTransform;
    
	void Start ()
    {
        bulletRigidBody2D = GetComponent<Rigidbody2D>();
        bulletRigidBody2D.AddRelativeForce(moveSpeed * Vector2.up, ForceMode2D.Impulse);
        isLethal = true;
        Destroy(gameObject, bulletLifetime);
    }

    // If the bullet hits anything, it can no longer kill. Meant to stop people from dying when they step on used bullets.
    void OnCollisionEnter (Collision collision)
    {
        isLethal = false;        
    }
}