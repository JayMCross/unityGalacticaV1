using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public UnityEngine.UI.Text scoreLabel;

    public int score = 0;

    public static GameControllerScript instance;

    public GameObject menu;

    public UnityEngine.UI.Button startButton;

    public bool isStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        startButton.onClick.AddListener(delegate
        {
            menu.SetActive(false);
            isStarted = true;
        }
           );
    }

    // Update is called once per frame
    void Update()
    {
        scoreLabel.text = "Score: " + score;
    }
}
