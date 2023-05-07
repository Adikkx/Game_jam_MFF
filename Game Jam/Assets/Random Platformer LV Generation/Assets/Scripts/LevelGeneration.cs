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
    public GameObject scientist;
    public GameObject camera;
    public float moveIncrementX;
    public float moveIncrementY;
    public float minX;
    public float maxX;
    public float minY;
    private float timeBtwSpawn;
    public float startTimeBtwSpawn;
    private Vector2 pos;
    public LayerMask whatIsRoom;
    private bool doorRoom;
    private int rando;
    public bool reset;
    public int counter;

    private void Start()
    {
        counter = 0;
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        player.transform.position = transform.position;
        scientist.transform.position=new Vector2(transform.position.x-5,transform.position.y);
        camera.GetComponent<CinemachineVirtualCamera>().Follow = startingPositions[randStartingPos];
        pos = new Vector2(transform.position.x, transform.position.y);
        transform.position = pos;
        doorRoom = false;
        Instantiate(rooms[1], transform.position, Quaternion.identity);
        counter++;
        //Instantiate(doorRooms[1], new Vector2(transform.position.x + 2*maxX, transform.position.y), Quaternion.identity);
        direction = Random.Range(0, 5);
    }

    private void Update()
    {
        //print(doorRoom);
        if (Keyboard.current.spaceKey.wasPressedThisFrame || reset)
        {
            //doorRoom = false;
            reset = false;
            Leveling.Level =1;
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

        if (stopGeneration == true)
        {
            if (counter == 16 || !doorRoom)
            {
                Instantiate(doorRooms[3], new Vector2(transform.position.x, -25), Quaternion.identity);
            }
        }
    }

    public void setReset()
    {
        reset = true;
    }

    private void Move()
    {

        if (direction == 1 || direction == 2)
        { // Move right !

            if (transform.position.x < maxX)
            {
                downCounter = 0;
                int randRoom = Random.Range(0, 3);
                pos = new Vector2(transform.position.x + moveIncrementX, transform.position.y);
                transform.position = pos;
                int rando = Random.Range(0, 2);
                //rando = 1 && doorRoom = false && transform.position.y = 25
                if (doorRoom || transform.position.y != -25)
                {
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                    counter++;
                }
                else if (!(doorRoom) || (!doorRoom && transform.position.y == -25)  /*&& transform.position.y == minY*/)
                {
                    print("jsem tu");
                    Instantiate(doorRooms[3], transform.position, Quaternion.identity);
                    counter++;
                    doorRoom = true;
                }

                    // Makes sure the level generator doesn't move left !
                direction = Random.Range(1, 7);
                if (direction == 3)
                {
                    direction = 1;
                }
                else if (direction == 4 || direction == 6)
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
                Vector2 pos = new Vector2(transform.position.x - moveIncrementX, transform.position.y);
                transform.position = pos;

                int randRoom = Random.Range(0, 3);
                int rando = Random.Range(0, 2);
                if (doorRoom || transform.position.y != -25)
                {
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                    counter++;
                }
                    
                else if (!(doorRoom) || (!doorRoom && transform.position.y == -25))
                {
                    print("jsem tu");

                    Instantiate(doorRooms[3], transform.position, Quaternion.identity);
                    counter++;
                    doorRoom = true;
                }
                direction = Random.Range(3, 7);
                if (direction == 6)
                {
                    direction = 5;
                }
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
                Collider2D previousRoom = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), 1, whatIsRoom);
                Debug.Log(previousRoom);
            if (previousRoom != null && previousRoom.GetComponent<Room>() != null) {
                

                    // My problem : if the level generation goes down TWICE in a row, there's a chance that the previous room is just 
                    // a LRB, meaning there's no TOP opening for the other room ! 

                    if (downCounter >= 2)
                    {
                        previousRoom.GetComponent<Room>().RoomDestruction();
                        transform.position = new Vector2(transform.position.x, transform.position.y);
                        int rando = Random.Range(0, 2);
                        if (doorRoom || transform.position.y != -25)
                        {
                            Instantiate(rooms[3], transform.position, Quaternion.identity);
                        }
                        else if (!(doorRoom) || (!doorRoom && transform.position.y == -25))
                        {
                            print("jsem tu");

                            Instantiate(doorRooms[3], transform.position, Quaternion.identity);
                            doorRoom = true;
                        }
                    }
                    else
                    {
                        previousRoom.GetComponent<Room>().RoomDestruction();
                        int randRoomDownOpening = Random.Range(2, 4);
                        /*if (randRoomDownOpening == 3)
                        {
                            randRoomDownOpening = 2;
                        }*/

                        transform.position = new Vector2(transform.position.x, transform.position.y);
                        int rando = Random.Range(0, 2);
                        if (doorRoom || transform.position.y != -25)
                        {
                            Instantiate(rooms[randRoomDownOpening], transform.position, Quaternion.identity);
                        }
                        else if (!(doorRoom) || (!doorRoom && transform.position.y == -25))
                        {
                            print("jsem tu");

                            Instantiate(doorRooms[3], transform.position, Quaternion.identity);
                            doorRoom = true;
                        }
                    }

                }



                Vector2 pos = new Vector2(transform.position.x, transform.position.y - moveIncrementY);
                transform.position = pos;

                // Makes sure the room we drop into has a TOP opening !
                int randRoom = Random.Range(2, 4); 
                rando = Random.Range(0, 2);
                if (doorRoom || transform.position.y != -25)
                {
                    Instantiate(rooms[randRoom], transform.position, Quaternion.identity);
                    counter++;
                }
                else if (!(doorRoom) || (!doorRoom && transform.position.y == -25))
                {
                    print("jsem tu");

                    Instantiate(doorRooms[3], transform.position, Quaternion.identity);
                    counter++;
                    doorRoom = true;
                }

                direction = Random.Range(1, 7);
                {
                    if (direction == 6)
                    {
                        direction = 5;
                    }
                }
            }
            else
            {
                stopGeneration = true;
            }

        }
        else
        {
            stopGeneration = true;
        }
    } // && transform.position.y != null && transform.position.y == minY
}
