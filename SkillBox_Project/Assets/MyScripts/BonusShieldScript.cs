using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BonusShieldScript : MonoBehaviour
{
    public float speed;


    void Start()
    {
        Rigidbody bonusRig = GetComponent<Rigidbody>();
        bonusRig.velocity = new Vector3(0, 0, -1) * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Shield clollided with " + other.name);

        if (other.tag == "Player" || other.tag == "Shield")
        {
            Destroy(gameObject);            
            GameObject.FindObjectOfType<Player>().IncreaseShieldCount();
        }
        else if (other.tag != "GameBoundary")
        {           
            Destroy(gameObject);            
        }
    }
}
