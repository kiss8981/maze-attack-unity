using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void onStartButtonClick()
    {
        Debug.Log("Start button clicked");
    }

    public void onQuitButtonClick()
    {
        Application.Quit();
    }
}
