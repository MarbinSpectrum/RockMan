using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Rigidbody2D rigid;
    public float dic = 0.5f;
    AudioSource dinksound;
    void Awake()
    {
        if(transform.name.Equals("Bullet(Clone)"))
        {
            transform.name = "Bullet";
        }
        if (transform.name.Equals("EBullet(Clone)"))
        {
            transform.name = "EBullet";
        }
        if (transform.name.Equals("Air(Clone)"))
        {
            transform.name = "Air";
        }
        rigid = GetComponent<Rigidbody2D>();
        dinksound = GameObject.Find("DinkSound").GetComponent<AudioSource>();
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        //총알 자동이동
        rigid.velocity = new Vector2(dic, 0);
	}

    //총알이 벽에 닿으면 소멸
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.gameObject.layer == 8 && !(transform.name.Equals("Air") || transform.name.Equals("AirAttack")))
        {
            Destroy(this.gameObject);
        }

        if (transform.name.Equals("Air"))
        {
            if (col.transform.gameObject.name.Equals("Bullet"))
            {
                Destroy(col.transform.gameObject);
                dinksound.Play();
            }
        }

        if (transform.name.Equals("Bullet"))
        {
            if (col.transform.name.Equals("RightWall") || col.transform.name.Equals("LeftWall"))
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.gameObject.layer == 8 && !(transform.name.Equals("Air") || transform.name.Equals("AirAttack")))
        {
            Destroy(this.gameObject);
        }

        if (transform.name.Equals("Air"))
        {
            if (col.transform.gameObject.name.Equals("Bullet"))
            {
                Destroy(col.transform.gameObject);
                dinksound.Play();
            }
        }

        if (transform.name.Equals("Bullet"))
        {
            if (col.transform.name.Equals("RightWall") || col.transform.name.Equals("LeftWall"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
