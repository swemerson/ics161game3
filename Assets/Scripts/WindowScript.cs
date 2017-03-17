using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowScript : MonoBehaviour {

    private BoxCollider2D bc;

    void Start ()
    {
        bc = GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Bullet" || collision.gameObject.tag == "EnemyBullet")
        {

        }
    }

}
