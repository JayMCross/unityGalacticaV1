using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipScript : MonoBehaviour
{
    public GameObject enemyShipExplosiom, playerExplosion;
    public GameObject shootPoint;
    public GameObject lazerShot;
    public float shotDelay;

    public Vector3 shootToPos;

    //float nextShotTime = 0;

    public float minSpeed, maxSpeed;
    public float minAngle, maxAngle;
    GameObject player;
    public float minDistanceForShot;
    public float lockDegree;
    bool isTargetLock;
    public bool isOutOfScreen;
    public bool isHuntModeOn;
    [SerializeField] Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        player = GameObject.FindGameObjectWithTag("Player");

        Rigidbody enemyRig = GetComponent<Rigidbody>();
        enemyRig.velocity = new Vector3(Random.Range(minAngle, maxAngle), 0,-Random.Range(minSpeed, maxSpeed));

        InvokeRepeating("Shoot", 0.1f, shotDelay);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "GameBoundary")
            return;

        Instantiate(enemyShipExplosiom, transform.position, Quaternion.identity);
        
        if (other.gameObject.tag == "Player")
            Instantiate(playerExplosion, other.transform.position, Quaternion.identity);
        GameControllerScript.instance.score += 5;
        Destroy(gameObject);

        if(other.tag == "Shield")
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ReduceShieldCount();
            //player.GetComponent<Player>().shieldCount--;
        else
            Destroy(other.gameObject);        
    }

    private void FixedUpdate()
    {
        CheckVisibilityAtScreen();
        CheckHuntModeOn();
        HuntMode();
        CheckAngle();
    }

    void Shoot() 
    {
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < minDistanceForShot || !isTargetLock || isOutOfScreen)
            return;

        if (player)
        {          
            var enemyShot = Instantiate(lazerShot, shootPoint.transform.position, Quaternion.identity);
            enemyShot.transform.LookAt(player.transform);
        }
        else
        {
            var enemyShot = Instantiate(lazerShot, shootPoint.transform.position, Quaternion.identity);          
        }
    }

    void HuntMode() 
    {
        if (isHuntModeOn)
        {
            Rigidbody enemyRig = GetComponent<Rigidbody>();
            var shootToPos = (player.transform.position - transform.position).normalized;
            transform.LookAt(player.transform);

            GetComponent<Rigidbody>().velocity = shootToPos * Random.Range(minSpeed, maxSpeed) / 1.5f;
        }
        
    }

    void CheckVisibilityAtScreen() 
    {
        if (Mathf.Clamp(transform.position.x, -screenBounds.x, screenBounds.x) != transform.position.x || Mathf.Clamp(transform.position.z, -screenBounds.y, screenBounds.y) != transform.position.z)
            isOutOfScreen = true;
        else
            isOutOfScreen = false;        
    }

    void CheckHuntModeOn() 
    {
        if ((gameObject.transform.position.z - (-screenBounds.y)) < 5)
            isHuntModeOn = true;
    }

    void CheckAngle() 
    {
        Vector3 targetDir = player.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if (angle < lockDegree)
            isTargetLock = true;
        else
            isTargetLock = false;
    }

}
