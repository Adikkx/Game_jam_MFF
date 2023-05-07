using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class New_level : MonoBehaviour
{
    
    public GameObject ujkochujko;
    public Text LevelText;
    public GameObject player;
    public bool reset = false;
    // Update is called once per frame
    void Start(){
    LevelText = GameObject.Find("LevelText").GetComponent<Text>();
    }
    void Update()
    {
        if (reset)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }

    void OnTriggerEnter2D()
    {
        if( Key_Counter.Key >=3 )
        {
        Leveling.Level +=1;
        LevelText.text=""+Leveling.Level;
        reset = true;
        GameObject.Find("scientist").GetComponent<Dialo>().dialogFinished = false;
        }
        if (Leveling.Level==5)
        {
            player.transform.position = transform.position;
            ujkochujko.transform.position=new Vector2(transform.position.x-5,transform.position.y);
        }
    }
}
