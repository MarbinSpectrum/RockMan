using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RockManScript : MonoBehaviour
{
    bool startrockman = false;
    
    int Hp = 100;

    Animator ani;

    AudioSource bgmsound;
    AudioSource bosssound;
    AudioSource bustersound;
    AudioSource damagesound;
    AudioSource deadsound;
    AudioSource jumpsound;
    AudioSource warpsound;

    Rigidbody2D rigid;

    public GameObject BulletPrefab;
    public GameObject AirMan;

    public Image HpImage;

    public SpriteRenderer DamageSprite;
    public SpriteRenderer Rocksprite;

    public bool boss = false;
    public bool damage = false;
    public bool dead = false;
    public bool dontmove = false;
    public float EnemyAir = 0;

    void Awake()
    {
        ani = GetComponent<Animator>();

        bgmsound = GameObject.Find("BGM").GetComponent<AudioSource>();
        bosssound = GameObject.Find("Boss").GetComponent<AudioSource>();
        bustersound = GameObject.Find("BusterSound").GetComponent<AudioSource>();
        damagesound = GameObject.Find("DamageSound").GetComponent<AudioSource>();
        deadsound = GameObject.Find("DeadSound").GetComponent<AudioSource>();
        jumpsound = GameObject.Find("JumpSound").GetComponent<AudioSource>();
        warpsound = GameObject.Find("WarpSound").GetComponent<AudioSource>();

        rigid = GetComponent<Rigidbody2D>();
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
        if (startrockman && !dontmove && !dead)
        {
            Buster();
            Jump();
            Move();
        }
        HpState();
    }

    void Buster()
    {
        //A버튼 누를씨 버스터발사
        if (Input.GetKeyDown(KeyCode.A))
        {
            bustersound.Play();
            ani.SetBool("Buster", true);

            //방향에 따른 부스터 프리팹설정
            if (Rocksprite.flipX == true)
            {
                Instantiate(BulletPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BulletScript>().dic = -15f;
            }
            else
            {
                Instantiate(BulletPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity).GetComponent<BulletScript>().dic = +15f;
            }
        }
        else
        {
            ani.SetBool("Buster", false);
        }
    }


    void Jump()
    {
        //레이를 쏴서 캐릭터 아래에 Ground라는 레이어를 가진 객체가 있는지 확인
        RaycastHit2D hit;
        hit = Physics2D.Linecast(transform.position - new Vector3(-0.3f, 0.5f, 0), transform.position - new Vector3(+0.3f,0.5f,0), 1 << LayerMask.NameToLayer("Ground"));

        //Ground가 있으면 점프를 허용
        if (hit)
        {
            ani.SetBool("Jump", false);
            //Space를 눌럿을 경우 점프
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpsound.Play();
                rigid.AddForce(new Vector2(0, 450f));
            }
        }
        else
        {
            ani.SetBool("Jump", true);
        }
    }

    void Move()
    {
        //왼쪽키를 눌럿을 경우 왼쪽 이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            ani.SetBool("Run", true);
            rigid.velocity = new Vector2(-3 + EnemyAir, rigid.velocity.y);
            Rocksprite.flipX = true;
        }
        //오른쪽키를 눌럿을 경우 오른쪽 이동
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            ani.SetBool("Run", true);
            rigid.velocity = new Vector2(+3 + EnemyAir, rigid.velocity.y);
            Rocksprite.flipX = false;
        }
        //아무것도 아닐때
        else
        {
            ani.SetBool("Run", false);
            rigid.velocity = new Vector2(0 + EnemyAir, rigid.velocity.y);
        }
    }

    void HpState()
    {
        if(Hp <= 0)
        {
            if (!dead)
            {
                dead = true;
                Hp = 0;
                bgmsound.Stop();
                bosssound.Stop();
                deadsound.Play();
                HpImage.fillAmount = Hp / 100f;
                StartCoroutine(End());
                transform.localScale = new Vector3(0, 0, 0);
            }
        }
        else
        {
            HpImage.fillAmount = Hp / 100f;
        }
    }


    void OnCollisionStay2D(Collision2D col)
    {
        //록맨 소환시 땅에 닿았을 경우 애니메이션 처리
        if(col.transform.gameObject.layer == 8 && !startrockman)
        {
            StartCoroutine(StartRock());
            ani.SetBool("Spawn", true);
            warpsound.Play();
            rigid.gravityScale = 2;
        }

        //록맨 데미지 처리
        if ((col.transform.name.Equals("Enemy") || col.transform.name.Equals("Enemy2")) && !damage)
        {
            Hp -= 10;
            damage = true;
            dontmove = true;
            if (transform.position.x < col.transform.position.x)
            {
                rigid.velocity = new Vector2(-1, rigid.velocity.y);
            }
            else
            {
                rigid.velocity = new Vector2(+1, rigid.velocity.y);
            }
            StartCoroutine(DamageRock());
            ani.SetBool("Damage", true);
            damagesound.Play();
        }

        if (col.transform.name.Equals("Niddle") && !damage)
        {
            Hp -= 20;
            damage = true;
            dontmove = true;
            if (transform.position.x < col.transform.position.x)
            {
                rigid.velocity = new Vector2(-1, rigid.velocity.y);
            }
            else
            {
                rigid.velocity = new Vector2(+1, rigid.velocity.y);
            }
            StartCoroutine(DamageRock());
            ani.SetBool("Damage", true);
            damagesound.Play();
        }

        //록맨 낙사 처리
        if (col.transform.name.Equals("DeadArea"))
        {
            Hp = 0;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //록맨 데미지 처리
        if (col.transform.name.Equals("EBullet") && !damage)
        {
            Destroy(col.transform.gameObject);
            Hp -= 20;
            damage = true;
            dontmove = true;
            if (transform.position.x < col.transform.position.x)
            {
                rigid.velocity = new Vector2(-1, rigid.velocity.y);
            }
            else
            {
                rigid.velocity = new Vector2(+1, rigid.velocity.y);
            }
            StartCoroutine(DamageRock());
            ani.SetBool("Damage", true);
            damagesound.Play();
        }

        if (col.transform.name.Equals("Air") && !damage && !AirMan.GetComponent<AirManScript>().dead)
        {
            Hp -= 20;
            damage = true;
            dontmove = true;
            if (transform.position.x < col.transform.position.x)
            {
                rigid.velocity = new Vector2(-1, rigid.velocity.y);
            }
            else
            {
                rigid.velocity = new Vector2(+1, rigid.velocity.y);
            }
            StartCoroutine(DamageRock());
            ani.SetBool("Damage", true);
            damagesound.Play();
        }

        if (col.transform.name.Equals("AirHit") && !damage && !AirMan.GetComponent<AirManScript>().dead)
        {
            Hp -= 30;
            damage = true;
            dontmove = true;
            if (transform.position.x < col.transform.position.x)
            {
                rigid.velocity = new Vector2(-1, rigid.velocity.y);
            }
            else
            {
                rigid.velocity = new Vector2(+1, rigid.velocity.y);
            }
            StartCoroutine(DamageRock());
            ani.SetBool("Damage", true);
            damagesound.Play();
        }
    }


    //록맨 도착 애니메이션 처리
    IEnumerator StartRock()
    {
        yield return new WaitForSeconds(0.05f);
        ani.SetBool("Normal", true);
        startrockman = true;
    }

    //록맨 데미지 애니메이션 처리
    IEnumerator DamageRock()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.1f);
            DamageSprite.color = new Color(0, 0, 0, 0f);
            Rocksprite.size = new Vector2(1f, 1f);
            yield return new WaitForSeconds(0.1f);
            DamageSprite.color = new Color(1, 1, 1, 1f);
            Rocksprite.size = new Vector2(0f, 0f);
        }
        DamageSprite.color = new Color(0, 0, 0, 0f);
        dontmove = false;
        ani.SetBool("Damage", false);
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Rocksprite.size = new Vector2(0f, 0f);
            yield return new WaitForSeconds(0.1f);
            Rocksprite.size = new Vector2(1f, 1f);
        }
        damage = false;
    }

    //록맨 다시하기
    IEnumerator End()
    {
        yield return new WaitForSeconds(3f);
        if(boss)
        {
            SceneManager.LoadScene("Game1");
        }
        else
        {
            SceneManager.LoadScene("Game0");
        }
    }
}
