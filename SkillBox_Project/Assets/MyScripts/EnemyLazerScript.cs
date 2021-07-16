using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLazerScript : MonoBehaviour
{
    public float speed;
    public GameObject playerExplosion;

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            var shootToPos = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;

            GetComponent<Rigidbody>().velocity = shootToPos * speed;
        }
        else
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -1) * speed;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            Instantiate(playerExplosion, other.transform.position, Quaternion.identity);

            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else if (other.tag == "Shield")
        {          

            Destroy(gameObject);
            //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().shieldCount--;

            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ReduceShieldCount();
        }
    }
}

