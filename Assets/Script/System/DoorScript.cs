using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject Camera;
    public int stage = 0;
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.name.Equals("Rock"))
        {
            if(Camera.GetComponent<CameraScript>().stage == stage)
            {
                Camera.GetComponent<CameraScript>().stage++;
            }
        }
    }

    }
