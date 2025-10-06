using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class FirstPerson_GameManager : MonoBehaviour
{

    public static FirstPerson_GameManager Instance;

    [SerializeField]
    TextMeshProUGUI Timer;

    [SerializeField]
    float gametime;

    float InitalGameTime;

    public GameObject player;

    public Vector3 playerposition;

    [SerializeField]
    GameObject door;

    public List<GameObject> enemies = new List<GameObject>();

    public GameObject WinAndLosePanel;

    public bool ThePlayerHasWon;
    public bool ThePlayerLose;

    Color WinOrLosePanelColor;

    bool GameIsInPause;

    public bool isPaused;

    float cameraSensitivity;
    float playerSpeed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        
        playerSpeed = player.GetComponent<FirstPerson_PlayerController>().movementSpeed;

        DontDestroyOnLoad(gameObject);

        playerposition = player.transform.position;

        InitalGameTime = gametime;

        GameIsInPause = false;
    }

    void Start()
    {
        
    }


    void Update()
    {

        if (ThePlayerHasWon == false && ThePlayerLose == false)
        {
            gametime -= Time.deltaTime;

            Timer.text = gametime.ToString("F0");
        }

        if (gametime < 1)
        {
            RestartGame();
        }

        if (door.GetComponent<Door_Controller>().DoorIsopen == true && UI_Manager.instance.isUIOpen == false)
        {
           foreach(GameObject e in enemies)
           {
                e.GetComponent<Enemies_Controller>().canShoot = true;
           }
        }
        else if(UI_Manager.instance.isUIOpen == true)
        {
            foreach (GameObject e in enemies)
            {
                e.GetComponent<Enemies_Controller>().canShoot = false;
            }
        }

        ShowWinAndLosePanel();

        if (UI_Manager.instance.isUIOpen && !isPaused)
        {
            Pause();
            isPaused = true;
        }
        else if (!UI_Manager.instance.isUIOpen && isPaused)
        {
            QuitPause();
            StartCoroutine(WaitForHaveControl());
        }

        float distance = Vector3.Distance(player.transform.position, door.transform.position);

        if (distance < 5)
        {
            UI_Manager.instance.interactableButton.gameObject.SetActive(true);
        }
        else
        {
            UI_Manager.instance.interactableButton.gameObject.SetActive(false);
        }

    }

    public void RestartGame()
    {
        UI_Manager.instance.isUIOpen = false;

        ThePlayerHasWon = false;

        ThePlayerLose = false;

        WinAndLosePanel.gameObject.SetActive(false);

        player.transform.position = playerposition;

        gametime = InitalGameTime;

        door.GetComponent<Animator>().SetBool("Open", false);

        door.GetComponent<Door_Controller>().DoorIsopen = false;

        foreach (GameObject e in enemies)
        {
            e.SetActive(true);
            e.GetComponent<Enemies_Controller>().canShoot = false;
        }
    }

    public void ShowWinAndLosePanel()
    {
        if (ThePlayerHasWon == true)
        {
            UI_Manager.instance.isUIOpen = true;

            door.GetComponent<Door_Controller>().DoorIsopen = false;

            ColorUtility.TryParseHtmlString("#4DA454", out WinOrLosePanelColor);

            WinAndLosePanel.GetComponent<Image>().color = WinOrLosePanelColor;
            WinAndLosePanel.GetComponentInChildren<TextMeshProUGUI>().text = "You Win";
            WinAndLosePanel.gameObject.SetActive(true);
        }
        else if (ThePlayerLose == true)
        {
            UI_Manager.instance.isUIOpen = true;

            door.GetComponent<Door_Controller>().DoorIsopen = false;

            ColorUtility.TryParseHtmlString("#9C2424", out WinOrLosePanelColor);

            WinAndLosePanel.GetComponent<Image>().color = WinOrLosePanelColor;
            WinAndLosePanel.GetComponentInChildren<TextMeshProUGUI>().text = "You Lose";
            WinAndLosePanel.gameObject.SetActive(true);
        }

    }


    public void Pause()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<FirstPerson_PlayerController>().movementSpeed = 0;
        foreach (GameObject e in enemies)
        {
            e.GetComponent<Enemies_Controller>().canShoot = false;
        }
    }

    public void QuitPause()
    {
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<FirstPerson_PlayerController>().movementSpeed = playerSpeed;
        if (door.GetComponent<Door_Controller>().DoorIsopen == true)
        {
            foreach (GameObject e in enemies)
            {
                e.GetComponent<Enemies_Controller>().canShoot = true;
            }
        }

    }

    IEnumerator WaitForHaveControl()
    {
        yield return new WaitForSeconds(0.2f);
        isPaused = false;
    }

}
