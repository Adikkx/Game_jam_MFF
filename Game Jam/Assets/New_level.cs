using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class New_level : MonoBehaviour
{
    public bool reset = false;
    // Update is called once per frame
    void Update()
    {
        if (reset)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }

    void OnTriggerEnter2D()
    {
       // PlayerController.Level +=1;
        reset = true;
    }
}
