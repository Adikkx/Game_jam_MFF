using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Game()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void Pause(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

    }
    public void Quit()
    {
        Application.Quit();
    }
    
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-2);
    }
    public void Restart2(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
}
