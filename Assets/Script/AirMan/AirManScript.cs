using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirManScript : MonoBehaviour
{
    Animator ani;

    bool airman_start = true;
    public bool dead = false;
    bool damage = false;
    int Hp = 0;
    int AttackCount = 0;

    AudioSource bosssound;
    AudioSource deadsound;
    AudioSource energysound;
    AudioSource hitsound;
    AudioSource victorysound;
    AudioSource warpsound;

    Rigidbody2D rigid;

    public GameObject AirPrefab;
    public GameObject mcamera;
    public GameObject HpImageBar;
    public GameObject HpImage;
    public GameObject Rock;

    public SpriteRenderer AirSprite;
    public SpriteRenderer HitSprite;
    void Awake()
    {
        ani = GetComponent<Animator>();
        bosssound = GameObject.Find("Boss").GetComponent<AudioSource>();
        deadsound = GameObject.Find("DeadSound").GetComponent<AudioSource>();
        energysound = GameObject.Find("EnergySound").GetComponent<AudioSource>();
        hitsound = GameObject.Find("EnemyDeadSound").GetComponent<AudioSource>();
        victorysound = GameObject.Find("VictorySound").GetComponent<AudioSource>();
        warpsound = GameObject.Find("WarpSound").GetComponent<AudioSource>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        HpState();
    }

    void HpState()
    {
        if (Hp <= 0 && !airman_start)
        {
            if (!dead)
            {
                dead = true;
                Hp = 0;
                bosssound.Stop();
                deadsound.Play();
                HpImage.GetComponent<Image>().fillAmount = Hp / 100f;
                transform.localScale = new Vector3(0, 0, 0);
                StartCoroutine(End());
            }
        }
        else
        {
            HpImage.GetComponent<Image>().fillAmount = Hp / 100f;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (airman_start && col.transform.gameObject.layer == 8)
        {
            ani.SetBool("Spawn", true);
            StartCoroutine(StartAir());
            HpImageBar.SetActive(true);
            HpImage.SetActive(true);
        }
        if (!airman_start && col.transform.gameObject.layer == 8)
        {
            ani.SetBool("Jump", false);
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            rigid.velocity = new Vector2(0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name.Equals("Bullet"))
        {
            Destroy(col.transform.gameObject);
            if (!damage)
            {
                damage = true;
                Hp -= 4;
                StartCoroutine(DamagekAir());
            }
        }
    }

    IEnumerator DamagekAir()
    {
        hitsound.Play();
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            AirSprite.color = new Color(1, 1, 1, 0f);
            HitSprite.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.1f);
            AirSprite.color = new Color(1, 1, 1, 1f);
            HitSprite.color = new Color(1, 1, 1, 0f);
        }
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.1f);
            AirSprite.color = new Color(1, 1, 1, 0f);
            yield return new WaitForSeconds(0.1f);
            AirSprite.color = new Color(1, 1, 1, 1f);
        }
        damage = false;
    }

    IEnumerator StartAir()
    {
        yield return new WaitForSeconds(0.5f);
        ani.SetBool("Spawn", false);
        for (int i = 0; i < 100; i++)
        {
            Hp++;
            yield return new WaitForSeconds(0.01f);
            if (i % 2 == 0 && i < 50)
            {
                energysound.Play();
            }
        }
        airman_start = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(AttackAir());
    }

    IEnumerator AttackAir()
    {
        if (AttackCount < 3 && !dead)
        {
            GameObject[] Air = new GameObject[6];
            AttackCount++;
            ani.SetBool("Attack", true);

            if (transform.localScale.x < 0)
            {
                Rock.GetComponent<RockManScript>().EnemyAir = -1.5f;
            }
            else
            {
                Rock.GetComponent<RockManScript>().EnemyAir = +1.5f;
            }
                for (int i = 0; i < Air.Length; i++)
                {
                    if (transform.localScale.x < 0)
                    {
                        if(i == 0)
                        {
                            Air[0] = Instantiate(AirPrefab, new Vector3(Random.Range(115, 120), -2.5f, 0), Quaternion.identity);
                        }
                        else
                        {
                            Air[i] = Instantiate(AirPrefab, new Vector3(Random.Range(115, 120), Random.Range(-0f, 2f), 0), Quaternion.identity);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            Air[0] = Instantiate(AirPrefab, new Vector3(Random.Range(135, 140), -2.5f, 0), Quaternion.identity);
                        }
                        else
                        {
                            Air[i] = Instantiate(AirPrefab, new Vector3(Random.Range(135, 140), Random.Range(-0f, 2f), 0), Quaternion.identity);
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            
            for (int i = 0; i < Air.Length; i++)
            {
                if (transform.localScale.x < 0)
                {
                    Air[i].GetComponent<BulletScript>().dic = 10;
                }
                else
                {
                    Air[i].GetComponent<BulletScript>().dic = -10;
                }
            }
            yield return new WaitForSeconds(0.6f);
            ani.SetBool("Attack", false);
            Rock.GetComponent<RockManScript>().EnemyAir = 0f;
            StartCoroutine(AttackAir());
        }
        else if (!dead)
        {
            ani.SetBool("Attack", false);
            AttackCount = 0;
            StartCoroutine(AirJump());
        }
        
    }

    IEnumerator AirJump()
    {
        yield return new WaitForSeconds(1f);
        ani.SetBool("Jump", true);
        if (transform.localScale.x < 0)
        {
            rigid.AddForce(new Vector2(+100, 500f));
            yield return new WaitForSeconds(0.8f);
            rigid.AddForce(new Vector2(+100, 800f));
        }
        else
        {
            rigid.AddForce(new Vector2(-100, 500f));
            yield return new WaitForSeconds(0.8f);
            rigid.AddForce(new Vector2(-100, 800f));
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(AttackAir());
    }

    IEnumerator End()
    {
        Rock.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Rock.GetComponent<Animator>().SetBool("Jump", false);
        Rock.GetComponent<Animator>().SetBool("Run", false);
        Rock.GetComponent<Animator>().SetBool("Buster", false);
        Rock.GetComponent<RockManScript>().dontmove = true;
        Rock.GetComponent<RockManScript>().damage = true;
        mcamera.GetComponent<CameraScript>().move = true;
        yield return new WaitForSeconds(3f);
        victorysound.Play();
        yield return new WaitForSeconds(3f);
        Rock.GetComponent<Animator>().SetBool("Spawn", false);
        warpsound.Play();
        for (int i = 0; i < 50; i++)
        {
            Rock.transform.position += new Vector3(0, 0.2f, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
}

