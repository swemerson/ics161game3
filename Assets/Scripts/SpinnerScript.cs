using UnityEngine;

public class SpinnerScript : MonoBehaviour
{
    public float spinSpeed;
    public float moveSpeed;

    private Rigidbody2D spinnerRigidBody;
    private Vector2 moveDirection;
    private Rigidbody2D rigidBody2d;

	// Use this for initialization
	void Start ()
    {
        rigidBody2d = GetComponent<Rigidbody2D>();
        moveDirection = new Vector2(Random.Range(-moveSpeed, moveSpeed), Random.Range(-moveSpeed, moveSpeed));
        moveDirection.Normalize();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void FixedUpdate()
    {
        rigidBody2d.MovePosition(rigidBody2d.position + moveDirection * moveSpeed * Time.smoothDeltaTime);
        transform.Rotate(new Vector3(0, 0, spinSpeed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveDirection = new Vector2(Random.Range(-moveSpeed, moveSpeed), Random.Range(-moveSpeed, moveSpeed));
        moveDirection.Normalize();
    }
}
