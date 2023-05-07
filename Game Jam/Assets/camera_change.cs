using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class camera_change : MonoBehaviour
{
    public GameObject camera;

    private Transform FollowPoint;
    // Start is called before the first frame update
    void Start()
    {
        FollowPoint = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D()
    {
        camera.GetComponent<CinemachineVirtualCamera>().Follow = FollowPoint;
    }

}
