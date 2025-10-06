using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class NPCdialogue_Controller : MonoBehaviour
{


    public GameObject dialoguePanel;

    public GameObject interactableButton;

    public GameObject player;

    public TextMeshProUGUI dialogueText;
    public string[] dialogues;
    public int currentIndex = 0;
    public float letterDelay = 0.05f; 

    private Coroutine currentCoroutine;

    private bool canAdvanceDialogue = true; 

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        canAdvanceDialogue = false;
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (canAdvanceDialogue == true && player.GetComponent<FirstPerson_PlayerController>().IsUsingKeyboard)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DisplayNextDialogue();
            }
        }

        if (canAdvanceDialogue == true && player.GetComponent<FirstPerson_PlayerController>().IsUsingGamepad)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                DisplayNextDialogue();
            }
        }

        float distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance < 5)
        {
            interactableButton.SetActive(true);
        }
        else
        {
            interactableButton.gameObject.SetActive(false);
        }

    }

     public void DisplayNextDialogue()
     {
       
        if (currentIndex < dialogues.Length)
        {
            UI_Manager.instance.isUIOpen = true;
            currentCoroutine = StartCoroutine(WriteDialogue(dialogues[currentIndex]));
            currentIndex++;
        }
        else
        {
            UI_Manager.instance.isUIOpen = false;
            dialoguePanel.gameObject.SetActive(false);
            currentIndex = 0;
            canAdvanceDialogue = false;
        }
    }

    IEnumerator WriteDialogue(string dialogue)
    {

        canAdvanceDialogue = false; 

        dialogueText.text = ""; 
        foreach (char letter in dialogue)
        {
            dialogueText.text += letter; 
            yield return new WaitForSeconds(letterDelay); 
        }

        canAdvanceDialogue = true; 
        currentCoroutine = null; 
    }
}
