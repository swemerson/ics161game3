using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlimitedAmmoScript : MonoBehaviour
{	
    public float rotateSpeed;
    public float respawnDuration;
    public GameObject ammo;
    private bool isDestroyed;
    private float respawnTime;

	void Start ()
    {
        isDestroyed = false;
        respawnTime = Time.time + respawnDuration;
        ammo.transform.position = ammo.transform.position;
    }

    void Update ()
    {        
        if (isDestroyed )
        {
            if (Time.time >= respawnTime)
            {
                print("SETTING ACTIVE");
                ammo.transform.position = transform.position;
                ammo.SetActive(true);
                isDestroyed = false;
            }           
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, rotateSpeed));
        }
    }

    public bool CanGetAmmo()
    {
        if(isDestroyed == false)
        {
            isDestroyed = true;
            //  ammo.GetComponent<MeshRenderer>().enabled = false;
            //  ammo.transform.position += new Vector3(0, 0, 10);
            ammo.SetActive(false);
            respawnTime = respawnDuration + Time.time;
            return true;
        }
        return false;
    }
}
