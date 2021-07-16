using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterScript : MonoBehaviour
{
    public GameObject[] enemies;
    public float minDelay, maxDelay;
    float nextLaunchTime = 0;
    float lastPosx = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameControllerScript.instance.isStarted)
        {
            return;
        }

        if (Time.time > nextLaunchTime) 
        {
            var enemy = enemies[Random.Range(0, enemies.Length - 1)];

            float emitterSize = transform.localScale.x;
            float enemySize = enemy.transform.localScale.x *10;
            float edgeSize = emitterSize / enemySize;
            
            float posX;
            float posY = 0;
            float posZ = transform.position.z;
            do
            { 
                posX = Random.Range(-emitterSize / 2, emitterSize / 2); 
            }
            while (Mathf.Abs(lastPosx - posX) < edgeSize);
            
            lastPosx = posX;
     
            var newEnemy = Instantiate(enemy, new Vector3(posX,posY,posZ),Quaternion.Euler(0,180,0));
            
            if(newEnemy.tag == "Asteroid")
                newEnemy.transform.localScale *= Random.Range(0.5f, 1.2f);

            nextLaunchTime = Time.time + Random.Range(minDelay,maxDelay);
        }

    }
}
