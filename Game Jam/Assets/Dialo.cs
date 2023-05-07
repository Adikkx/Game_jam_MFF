using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialo : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index = 0; 
    public GameObject continuebutt;
    public float wordSpeed;
    public bool playerIsClose;
    public bool dialogFinished;
    public GameObject targ;


    void Start()
    {
        dialogFinished = false;
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogFinished)
        {
            transform.position = Vector3.MoveTowards(transform.position, targ.transform.position, .045f);
        }
        if (playerIsClose && !dialogFinished)
        {
            Debug.Log("som blizko0");
            if (!dialoguePanel.activeInHierarchy)
            {   Debug.Log("aktivujem picovinu");
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
           // else if (dialogueText.text == dialogue[index])
            //{
             //   NextLine();
            //}

        }
        if (dialoguePanel.activeInHierarchy && !playerIsClose)
        {
            RemoveText();
        }
        if ((dialogueText.text == dialogue[index])){
            continuebutt.SetActive(true);
        }
    }

    public void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        Debug.Log("deaktivujem pičovinu");
        //dialoguePanel.SetActive(false);
    }

    public void zero_text(){
        dialogueText.text="";
        index=0;
        dialoguePanel.SetActive(false);
    }
    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {

        continuebutt.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            RemoveText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag==("Player"))
        {
            if (dialogFinished)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag==("Player"))
        {
            playerIsClose = false;
            RemoveText();
            dialoguePanel.SetActive(false);
            dialogFinished = true;
            //Destroy(gameObject);
        }
    }
}