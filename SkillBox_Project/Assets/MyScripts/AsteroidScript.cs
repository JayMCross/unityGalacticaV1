using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public GameObject asteroiidExplosiom, playerExplosion;
    public GameObject bonus;

    public float rotationSpeed;
    public float minSpeed, maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody asterRig = GetComponent<Rigidbody>();
        asterRig.angularVelocity = Random.insideUnitSphere * rotationSpeed;
        asterRig.velocity = new Vector3(0,0,-Random.Range(minSpeed, maxSpeed));       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid" || other.gameObject.tag == "GameBoundary")
            return;

        Instantiate(asteroiidExplosiom, transform.position, Quaternion.identity);
        
        if (other.gameObject.tag == "Player")
            Instantiate(playerExplosion, other.transform.position, Quaternion.identity);

        if (other.tag == "Shield")
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ReduceShieldCount();
            //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().shieldCount--;
        else
            Destroy(other.gameObject);

        GameControllerScript.instance.score += 1;

        Destroy(gameObject);
        

        if (Random.Range(0,10) > 5) 
        {
            Instantiate(bonus, transform.position, Quaternion.identity);
        }
    }

}
