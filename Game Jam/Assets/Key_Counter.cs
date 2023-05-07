using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Counter : MonoBehaviour
{
    public static int Key;
    // Start is called before the first frame update
    void Start()
    {
        Key=0;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag =="Player"){

        }
    }

}
