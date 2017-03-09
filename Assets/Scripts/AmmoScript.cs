using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoScript : MonoBehaviour
{	
    public float rotateSpeed;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed));
	}
}
