using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    static Player player;

    public float speed;
    public float xMax, xMin, zMax, zMin;
    public float tilt;
    public int shieldCount;

    public GameObject lazerShot;
    public GameObject lazerGunPoint;
    public GameObject[] secondaryLaserPoints;
    public float shotDelay;
    float nextShotTime = 0;
    float nextSecondaryShotTime = 0;
    GameObject shield;

    Rigidbody rb;
    Vector3 lastPos;
    [SerializeField] GameObject[] flames;    
    float x, z;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        lastPos = rb.position;

        flames = new GameObject[transform.childCount - 1];
        int i = 0;
        foreach (Transform child in transform)
        {

            if (child.gameObject.name != "ShipModel")
            {
                flames[i] = child.gameObject;
                i++;
            }
        }

        SetFlames(false);

       shield = transform.Find("Shield").gameObject;
    }

    // Start is called before the first frame update       
    void Start()
    {
        player = this;
        SetShield();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameControllerScript.instance.isStarted)
        {
            return;
        }
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(x, 0, z) * speed;

        float clamped_X = Mathf.Clamp(rb.position.x, xMin, xMax);
        float clamped_Z = Mathf.Clamp(rb.position.z, zMin, zMax);

        rb.position = new Vector3(clamped_X, 0, clamped_Z);

        rb.rotation = Quaternion.Euler(rb.velocity.z * tilt, 0, -rb.velocity.x * tilt);

        if (Time.time > nextShotTime && Input.GetMouseButton(0))
        {
            Instantiate(lazerShot, lazerGunPoint.transform.position, Quaternion.identity);
            nextShotTime = Time.time + shotDelay;
        }

        if (Time.time > nextSecondaryShotTime && Input.GetMouseButton(1))
        {
            foreach (var p in secondaryLaserPoints)
            {
                float corrector;

                if (p.transform.position.x > lazerGunPoint.transform.position.x)
                    corrector = -1;
                else
                    corrector = 1;

                var laserShot = Instantiate(lazerShot, p.transform.position, Quaternion.Euler(0, 45 * corrector, 0));
                //laserShot.GetComponent<Rigidbody>().velocity = new Vector3(0.5f * corrector, 0, 0.5f) * laserShot.GetComponent<LazerScript>().speed;


                laserShot.GetComponent<LazerScript>().enabled = false;
                laserShot.AddComponent<SecondaryLazerScript>();
                laserShot.transform.localScale /= 4;
            }

            nextSecondaryShotTime = Time.time + shotDelay / 2;
        }

    }

    private void FixedUpdate()
    {
        ControlFlames();
        
    }

    void ControlFlames() 
    {
        if (lastPos == rb.position)
            SetFlames(false);
        else if (lastPos.x == rb.position.x)
        {
            SetFlames(false, flames[2]);
            SetFlames(false, flames[3]);
        }
        else if (lastPos.z == rb.position.z)
        {
            SetFlames(false, flames[0]);
            SetFlames(false, flames[1]);
        }

        if (x > 0)
        {
            SetFlames(true, flames[2]);
            SetFlames(false, flames[3]);
        }
        else if (x < 0)
        {
            SetFlames(false, flames[2]);
            SetFlames(true, flames[3]);
        }


        if (z > 0)
        {
            SetFlames(true, flames[0]);
            SetFlames(false, flames[1]);

        }
        else if(z < 0) 
        {
            SetFlames(false, flames[0]);
            SetFlames(true, flames[1]);
        }

        lastPos = rb.position;        
    }
           
    
    void SetFlames(bool flamesOn,GameObject engines) 
    {
        if (!player)
            return;

        foreach (Transform child in engines.transform)
        { 
            if(child.gameObject.active == !flamesOn)
                child.gameObject.active = flamesOn;         
        }
    }

    void SetFlames(bool flamesOn) 
    {
        if (!player)
            return;

        foreach (var f in flames)
            foreach (Transform child in f.transform)
                child.gameObject.active = flamesOn;        
    }


    void SetShield() 
    {        
        if (shieldCount > 0)
            shield.SetActive(true);
        else
            shield.SetActive(false);        
    }

    public void ReduceShieldCount() 
    {
        shieldCount --;

        shield.SetActive(false);

        Invoke("SetShield", 0.5f);
        //SetShield();
    }

    public void IncreaseShieldCount()
    {
        shieldCount ++;

        Invoke("SetShield", 0.5f);
    }

}
