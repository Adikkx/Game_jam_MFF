using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Watch : MonoBehaviour
{
    //public  float timeStart;
    public  Text textBox;
    private float totalTimePlayed = 0f;
    private float currentLevelTime = 0f;
    
    void Update()
    {
        currentLevelTime += Time.deltaTime;
        totalTimePlayed += Time.deltaTime;
        textBox.text=totalTimePlayed.ToString("F2");
    }
    
    void OnLevelFinished()
    {
        totalTimePlayed += currentLevelTime;
        currentLevelTime = 0f;
    }
}
    

