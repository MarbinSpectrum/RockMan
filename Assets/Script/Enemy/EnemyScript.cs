using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    bool attack = false;

    Animator ani;

    AudioSource deadsound;
    AudioSource dinksound;

    GameObject Rock;

    public GameObject BulletPrefab;

    void Awake()
    {
        ani = GetComponent<Animator>();
        deadsound = GameObject.Find("EnemyDeadSound").GetComponent<AudioSource>();
        dinksound = GameObject.Find("DinkSound").GetComponent<AudioSource>();
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        //록맨 추적
        if(Rock == null)
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
	}
 
    //피격판정
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.name.Equals("Bullet") && attack)
        {
            
            deadsound.Play();
            StartCoroutine(Dead());
            Destroy(col.gameObject);
        }
        else if(col.transform.name.Equals("Bullet") && !attack)
        {
            dinksound.Play();
            Destroy(col.gameObject);
        }
    }

    //록맨이 공격범위에 들어왔을 경우 공격
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.name.Equals("Rock"))
        {
            ani.SetBool("Attack", true);
            attack = true;
            if (transform.position.x < Rock.transform.position.x)
            {
                Instantiate(BulletPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BulletScript>().dic = +4f;
            }
            else
            {
                Instantiate(BulletPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BulletScript>().dic = -4f;
            }

            StartCoroutine(Attack());
        }
    }



    //공격 애니메이션 처리
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        ani.SetBool("Attack", false);
        attack = false;
    }

    //데드 애니메이션 처리
    IEnumerator Dead()
    {
        ani.SetBool("Dead", true);
        yield return new WaitForSeconds(0.4f);
        Destroy(this.gameObject);
    }
}