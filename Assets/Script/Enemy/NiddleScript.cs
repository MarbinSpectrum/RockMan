using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiddleScript : MonoBehaviour
{
    AudioSource dinksound;

    float time = 0;

    bool attack = false;

    void Awake()
    {
        dinksound = GameObject.Find("DinkSound").GetComponent<AudioSource>();
    }


    void Start()
    {

    }

    void Update()
    {
        time += Time.deltaTime;
        if (!attack)
        {
            transform.position += new Vector3(0, -1, 0) * Time.deltaTime * 0.1f;
        }
        else
        {
            transform.position += new Vector3(0, +1, 0) * Time.deltaTime * 0.1f;
        }

        if (time * 0.1f > 0.8f)
        {
            attack = !attack;
            time = 0;
        }
    }

    //피격판정
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.name.Equals("Bullet") && attack)
        {

            dinksound.Play();
            Destroy(col.gameObject);
        }
    }
}
