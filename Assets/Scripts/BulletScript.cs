using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float moveSpeed;
    public float bulletLifetime;

    private Rigidbody2D bulletRigidBody2D;
    
	void Start ()
    {
        bulletRigidBody2D = GetComponent<Rigidbody2D>();
        bulletRigidBody2D.AddRelativeForce(moveSpeed * Vector2.up, ForceMode2D.Impulse);
        Destroy(gameObject, bulletLifetime);
    }
    
    void OnCollisionEnter2D (Collision2D collision)
    {
        if(collision.gameObject.tag != "Window")
        {
            Destroy(gameObject);
        }
    }
}