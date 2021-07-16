using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryLazerScript : MonoBehaviour
{
    public float speed;

    void Start()
    {
        float corrector;

        
        if (transform.position.x > GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().lazerGunPoint.transform.position.x)
            corrector = -1;
        else
            corrector = 1;

        speed = GetComponent<LazerScript>().speed * 1.5f;
        GetComponent<Rigidbody>().velocity = new Vector3(0.5f*corrector, 0, 0.5f) * speed;
    }
}

