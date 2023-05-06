using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelGeneration : MonoBehaviour
{

    public Transform[] startingPositions;
    public GameObject[] rooms; // index 0 --> closed, index 1 --> LR, index 2 --> LRB, index 3 --> LRT, index 4 --> LRBT
    public GameObject[] doorRooms; // index 0 --> closed, index 1 --> LR, index 2 --> LRB, index 3 --> LRT, index 4 --> LRBT
    private int direction;
    private bool stopGeneration;
    private int downCounter;
    public GameObject player;
    public GameObject camera;
    public float moveIncrementX;
    public float moveIncrementY;
    public float minX;
    public float maxX;
    public float minY;
    private float timeBtwSpawn;
    public float startTimeBtwSpawn;
    private Vector3 pos;
    public LayerMask whatIsRoom;
    private bool doorRoom;
    private int rando;


    private void Start()
    {

        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        player.transform.position = transform.position;
        camera.GetComponent<CinemachineVirtualCamera>().Follow = startingPositions[randStartingPos];
        pos = new Vector3(transform.position.x, transform.position.y, -1);
        transform.position = pos;
        doorRoom = false;
        Instantiate(rooms[1], transform.position, Quaternion.identity);
        //Instantiate(doorRooms[1], new Vector2(transform.position.x + 2*maxX, transform.position.y), Quaternion.identity);
        direction = Random.Range(1, 6);
    }

    private void Update()
    {

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            doorRoom = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (timeBtwSpawn <= 0 && stopGeneration == false)
        {
            player.GetComponent<PlayerController>().LockMovement();
            Move();
            player.GetComponent<PlayerController>().UnlockMovement();
            timeBtwSpawn = startTimeBtwSpawn;
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }

    private void Move()
    {

        if (direction == 1 || direction == 2)
        { // Move right !

            if (transform.position.x < maxX)
            {
                downCounter = 0;
                int randRoom = Random.Range(1, 4);
                pos = new Vector3(transform.position.x + moveIncrementX, transform.position.y, -1);
                transform.position = pos;
                rando = Random.Range(0, 2);
                if (rando == 0 || doorRoom)
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                else if (rando == 1 && !(doorRoom) && transform.position.y == minY)
                {
                    Instantiate(doorRooms[randRoom%4], transform.position, Quaternion.identity);
                    doorRoom = true;
                }

                    // Makes sure the level generator doesn't move left !
                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 1;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4)
        { // Move left !

            if (transform.position.x > minX)
            {
                downCounter = 0;
                Vector3 pos = new Vector3(transform.position.x - moveIncrementX, transform.position.y, -1);
                transform.position = pos;

                int randRoom = Random.Range(1, 4);
                rando = Random.Range(0, 2);
                if (rando == 0 || doorRoom)
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                else if (rando == 1 && !(doorRoom) && transform.position.y == minY)
                {
                    Instantiate(doorRooms[randRoom % 4], transform.position, Quaternion.identity);
                    doorRoom = true;
                }
                direction = Random.Range(3, 6);
            }
            else
            {
                direction = 5;
            }

        }
        else if (direction == 5)
        { // MoveDown
            downCounter++;
            if (transform.position.y > minY)
            {
                // Now I must replace the room BEFORE going down with a room that has a DOWN opening, so type 3 or 5
                Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
                //Debug.Log(previousRoom);
                if (previousRoom.GetComponent<Room>().roomType != 4 && previousRoom.GetComponent<Room>().roomType != 2)
                {

                    // My problem : if the level generation goes down TWICE in a row, there's a chance that the previous room is just 
                    // a LRB, meaning there's no TOP opening for the other room ! 

                    if (downCounter >= 2)
                    {
                        previousRoom.GetComponent<Room>().RoomDestruction();
                        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
                        rando = Random.Range(0, 2);
                        if (rando == 0 || doorRoom)
                            Instantiate(rooms[4], transform.position, Quaternion.identity);
                        else if (rando == 1 && !(doorRoom) && transform.position.y == minY)
                        {
                            Instantiate(doorRooms[3], transform.position, Quaternion.identity);
                            doorRoom = true;
                        }
                    }
                    else
                    {
                        previousRoom.GetComponent<Room>().RoomDestruction();
                        int randRoomDownOpening = Random.Range(2, 5);
                        if (randRoomDownOpening == 3)
                        {
                            randRoomDownOpening = 2;
                        }

                        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
                        rando = Random.Range(0, 2);
                        if (rando == 0 || doorRoom)
                            Instantiate(rooms[randRoomDownOpening], transform.position, Quaternion.identity);
                        else if (rando == 1 && !(doorRoom) && transform.position.y == minY)
                        {
                            Instantiate(doorRooms[randRoomDownOpening % 4], transform.position, Quaternion.identity);
                            doorRoom = true;
                        }
                    }

                }



                Vector3 pos = new Vector3(transform.position.x, transform.position.y - moveIncrementY, -1);
                transform.position = pos;

                // Makes sure the room we drop into has a TOP opening !
                int randRoom = Random.Range(3, 5);
                rando = Random.Range(0, 2);
                if (rando == 0 || doorRoom)
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                else if (rando == 1 && !(doorRoom) && transform.position.y == minY)
                {
                    Instantiate(doorRooms[randRoom % 4], transform.position, Quaternion.identity);
                    doorRoom = true;
                }

                direction = Random.Range(1, 6);
            }
            else
            {
                stopGeneration = true;
            }

        }
    }
}
