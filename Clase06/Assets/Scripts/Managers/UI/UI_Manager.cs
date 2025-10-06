using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager instance;

    [SerializeField]
    GameObject menu;

    [SerializeField]
    GameObject options;

    [SerializeField]
    Slider volumeslider;

    [SerializeField]
    Slider sfxSlider;

    [SerializeField]
    Slider sensitivitySlider;

    [SerializeField]
    GameObject controls;

    [SerializeField]
    GameObject selectTemplatesPanel;

    public GameObject interactableButton;

    public bool isUIOpen;

    public List<Button> firstPressedButtons = new List<Button>();

    public List<TextMeshProUGUI> gameTexts = new List<TextMeshProUGUI>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }


        DontDestroyOnLoad(gameObject);

        volumeslider.value = PlayerPrefs.GetFloat("Volume");

        volumeslider.onValueChanged.AddListener(delegate { SliderChanged(); });

        sfxSlider.value = PlayerPrefs.GetFloat("SFXs");

        sfxSlider.onValueChanged.AddListener(delegate { SliderChanged(); });

        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity");

        sensitivitySlider.onValueChanged.AddListener(delegate { SliderChanged(); });
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("sensitivity"))
        {
            sensitivitySlider.value = 1;
        }
        if (!PlayerPrefs.HasKey("SFXs"))
        {
            sfxSlider.value = 1;
        }
        if (!PlayerPrefs.HasKey("Volume"))
        {
            volumeslider.value = 1;
        }
    }

    public void SliderChanged()
    {
        Audio_Manager.instance.ChangeVolumeMusic(volumeslider.value);

        Audio_Manager.instance.ChangeVolumeSFXs(sfxSlider.value);

        Camera.main.GetComponent<CameraControl>().changeSensitivity(sensitivitySlider.value);
    }



    private void Update()
    {
        bool inputReceived = Keyboard.current.escapeKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame);

        if (isUIOpen)
        {
            Cursor.lockState = CursorLockMode.None;

            if (inputReceived)
            {
                CloseOptions();
            }
        }
        else
        {
            if (inputReceived)
            {
                OpenOptions();
            }
        }
    }

    public void ActivateText(int index)
    {
        gameTexts[index].gameObject.SetActive(true);
    }

    public void DesactivateText(int index)
    {
        gameTexts[index].gameObject.SetActive(false);
    }

    public void ActivateInteractableButton()
    {
        interactableButton.SetActive(true);
    }

    public void GotoItchio()
    {
        Application.OpenURL("https://live-games.itch.io/");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        EventSystem.current.SetSelectedGameObject(firstPressedButtons[1].gameObject);
        isUIOpen = true;
        options.gameObject.SetActive(true);
    }

    public void CloseOptions()
    {
        if(firstPressedButtons[0] != null)
        {
            EventSystem.current.SetSelectedGameObject(firstPressedButtons[0].gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(firstPressedButtons[1].gameObject);
        }

        isUIOpen = false;
        options.gameObject.SetActive(false);

        if (selectTemplatesPanel != null && selectTemplatesPanel.activeInHierarchy == true)
        {
            selectTemplatesPanel.SetActive(false);
        }
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenControls()
    {
        EventSystem.current.SetSelectedGameObject(firstPressedButtons[2].gameObject);
        controls.SetActive(true);
    }

    public void CloseControls()
    {
        EventSystem.current.SetSelectedGameObject(firstPressedButtons[1].gameObject);
        controls.SetActive(false);
    }

    public void OpenSelectTemplates()
    {
        EventSystem.current.SetSelectedGameObject(firstPressedButtons[3].gameObject);
        selectTemplatesPanel.SetActive(true);
    }


    public void GotoPlataformerScene()
    {
        SceneManager.LoadScene("Platformer_Scene");
    }

    public void GotoFirstPersonScene()
    {
        SceneManager.LoadScene("FirstPerson_Scene");
    }

    
}
