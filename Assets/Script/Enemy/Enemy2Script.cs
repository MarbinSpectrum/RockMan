using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Script : MonoBehaviour
{

    AudioSource deadsound;

    GameObject Rock;

    public int Hp = 10;
   void Awake()
    {
        deadsound = GameObject.Find("EnemyDeadSound").GetComponent<AudioSource>();
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        //록맨 추적
        if (Rock == null)
        {
            Rock = GameObject.Find("Rock");
        }
        else
        {
            //추적방향으로 방향전환
            if (transform.position.x < Rock.transform.position.x)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(+Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        if(Hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    //피격판정
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.name.Equals("Bullet"))
        {

            deadsound.Play();
            Hp--;
            Destroy(col.gameObject);
        }
    }

    //록맨이 공격범위에 들어왔을 경우 공격
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name.Equals("Rock"))
        {
            if (transform.position.x < Rock.transform.position.x)
            {
                col.GetComponent<RockManScript>().EnemyAir = 1.5f;
            }
            else
            {
                col.GetComponent<RockManScript>().EnemyAir = -1.5f;
            }
        }
    }

    //록맨이 공격범위에 들어왔을 경우 공격
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.name.Equals("Rock"))
        {
            col.GetComponent<RockManScript>().EnemyAir = 0f;
        }
    }
}
