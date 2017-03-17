using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
   public List<GameObject> enemies = new List<GameObject>();

    void Start ()
    {
		
	}
	
	void Update ()
    {
        if (AreEnemiesDead())
        {
            Destroy(gameObject);
        }
	}

    bool AreEnemiesDead()
    {
        for (int i = 0; i < enemies.Capacity; ++i)
        {
            if(enemies[i] != null)
            {
                return false;
            }
            
        }
        return true;
    }

}
