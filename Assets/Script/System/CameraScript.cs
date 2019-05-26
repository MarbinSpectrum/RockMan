using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public int stage = 0;

    public bool move = false;

    public GameObject Air;
    public GameObject Rock;
    public GameObject[] Front_Door = new GameObject[4];
    public GameObject[] Back_Door = new GameObject[4];
    public AudioSource GateSound;
    public AudioSource Bgm;
    public AudioSource BossBgm;
    void Start ()
    {
		
	}
	
	void Update ()
    {
        if(stage == 0)
        {
            if (transform.position.x < 49.5f)
            {
                if (transform.position.x < Rock.transform.position.x && !Rock.GetComponent<RockManScript>().dead)
                {
                    transform.position = new Vector3(Rock.transform.position.x, transform.position.y, transform.position.z);
                }
            }
            else
            {
                transform.position = new Vector3(49.5f, transform.position.y, transform.position.z);
            }
        }
        else if(stage == 2)
        {
            if (transform.position.x < 107.4f)
            {
                transform.position += new Vector3(0.015f, 0, 0);
            }
            else
            {
                transform.position = new Vector3(107.4f, transform.position.y, transform.position.z);
            }
        }
        else if (stage == 3)
        {
            Rock.GetComponent<RockManScript>().dontmove = true;
            move = true;
            stage++;
            StartCoroutine(MoveCamera2());
        }
        else if (stage == 5)
        {
            Rock.GetComponent<RockManScript>().dontmove = true;
            move = true;
            stage++;
            StartCoroutine(MoveCamera3());
        }

        if (move)
        {
            Rock.GetComponent<Rigidbody2D>().gravityScale = 0;
            Rock.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        }
        else
        {
            Rock.GetComponent<Rigidbody2D>().gravityScale = 2;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name.Equals("Rock") && stage == 0)
        {
            Rock.GetComponent<RockManScript>().dontmove = true;
            move = true;
            stage++;
            StartCoroutine(MoveCamera());
        }
    }

    IEnumerator MoveCamera()
    {
        for(int i = 0; i < 105; i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.position += new Vector3(0.1f, 0, 0);
        }
        Rock.GetComponent<RockManScript>().dontmove = false;
        move = false;
        yield return new WaitForSeconds(2f);
        stage++;
    }

    IEnumerator MoveCamera2()
    {
        Rock.GetComponent<RockManScript>().boss = true;
        GateSound.Play();
        for (int i = 0; i < 4; i++)
        {
            Front_Door[i].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.position += new Vector3(0.1f, 0, 0);
            Rock.transform.position += new Vector3(0.015f, 0, 0);
        }

        transform.position = new Vector3(117.55f, transform.position.y, transform.position.z);
        GateSound.Play();
        for (int i = 3; i >= 0; i--)
        {
            Front_Door[i].SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        Rock.GetComponent<RockManScript>().dontmove = false;
        move = false;
    }

    IEnumerator MoveCamera3()
    {
        GateSound.Play();
        for (int i = 0; i < 4; i++)
        {
            Back_Door[i].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
        
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.position += new Vector3(0.1f, 0, 0);
            Rock.transform.position += new Vector3(0.015f, 0, 0);
        }

        transform.position = new Vector3(127.73f, transform.position.y, transform.position.z);
        GateSound.Play();
        for (int i = 3; i >= 0; i--)
        {
            Back_Door[i].SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        //Rock.GetComponent<RockManScript>().dontmove = false;
        Rock.GetComponent<Animator>().SetBool("Jump", false);
        Rock.GetComponent<Animator>().SetBool("Run", false);
        Rock.GetComponent<Animator>().SetBool("Buster", false);
        move = false;
        Bgm.Stop();
        BossBgm.Play();
        Air.SetActive(true);
        yield return new WaitForSeconds(3f);
        Rock.GetComponent<RockManScript>().dontmove = false;
    }
}
