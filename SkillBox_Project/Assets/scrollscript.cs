using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollscript : MonoBehaviour
{
    public float speed;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float shift = Mathf.Repeat(Time.time * speed, 150);
        transform.position = startPosition + new Vector3(0, 0, -shift);
    }
}
