using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystemScript : MonoBehaviour
{
    public SpriteRenderer ready;
    public GameObject Player;
    float time = 0;

	void Start ()
    {
        Screen.SetResolution(1440, 810, false);
    }
	
	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        //READY 로고 애니메이션 
        time += Time.deltaTime;
        if(time < 0.3f)
        {
            ready.color = new Color(1, 1, 1, 1);
        }
        else if (time < 0.6f)
        {
            ready.color = new Color(1, 1, 1, 0);
        }
        else if (time < 0.9f)
        {
            ready.color = new Color(1, 1, 1, 1);
        }
        else if (time < 1.2f)
        {
            ready.color = new Color(1, 1, 1, 0);
        }
        else if (time < 1.5f)
        {
            ready.color = new Color(1, 1, 1, 1);
        }
        else if (time < 1.8f)
        {
            ready.color = new Color(1, 1, 1, 0);
            Player.SetActive(true);
        }
    }
}
