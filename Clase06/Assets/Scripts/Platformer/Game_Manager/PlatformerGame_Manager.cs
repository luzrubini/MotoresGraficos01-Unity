using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using System.Linq;
using System.Runtime.Serialization;

public class PlatformerGame_Manager : MonoBehaviour, ISaveable
{
    public static PlatformerGame_Manager instance;

    [SerializeField] private List<GameObject> cookies = new List<GameObject>();

    public int cookiesNumber;

    private GameObject player;
    private SerializableVector3 playerPosition;

    public bool ThePlayerHasWin;
    public bool ThePlayerLoose;

    [SerializeField] private TextMeshProUGUI txtCookies;

    public GameObject winAndLosePanel;

    Color winOrLosePanelColor;

    bool isPaused;

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

        player = GameObject.FindWithTag("Player");
        playerPosition = player.transform.position;

        cookiesNumber = cookies.Count;
    }

    void Update()
    {
        txtCookies.text = cookiesNumber.ToString();

        if (cookiesNumber == 0)
        {
            ThePlayerHasWin = true;
            player.transform.position = playerPosition;
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
            isPaused = false;
        }
    }

    public void Restart()
    {
        player.transform.position = playerPosition;
        ThePlayerLoose = false;
        ThePlayerHasWin = false;
        winAndLosePanel.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        foreach (GameObject cookie in cookies)
        {
            cookie.gameObject.SetActive(true);
        }
        cookiesNumber = cookies.Count;
    }

    public object CaptureState()
    {
        var cookiesData = new List<CookieData>();

        foreach (var cookie in cookies)
        {
            cookiesData.Add(new CookieData
            {
                position = cookie.transform.position,
                isActive = cookie.activeSelf
            });
        }

        return new SaveData
        {
            playerPosition = player.transform.position,
            cookies = cookiesData
        };
    }

    public void RestoreState(object state)
    {
        try
        {
            var saveData = (SaveData)state;
            player.transform.position = saveData.playerPosition;

            for (int i = 0; i < cookies.Count; i++)
            {
                cookies[i].transform.position = saveData.cookies[i].position;
                cookies[i].SetActive(saveData.cookies[i].isActive);
            }

            cookiesNumber = cookies.Count(cookie => cookie.activeSelf);
        }
        catch (SerializationException ex)
        {
            Debug.LogError("Failed to restore state: " + ex.Message);
        }
    }

    [System.Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float rX, float rY, float rZ)
        {
            x = rX;
            y = rY;
            z = rZ;
        }
        public static implicit operator Vector3(SerializableVector3 rValue)
        {
            return new Vector3(rValue.x, rValue.y, rValue.z);
        }
        public static implicit operator SerializableVector3(Vector3 rValue)
        {
            return new SerializableVector3(rValue.x, rValue.y, rValue.z);
        }
    }

    [Serializable]
    private struct SaveData
    {
        public SerializableVector3 playerPosition;
        public List<CookieData> cookies;
    }


    [Serializable]
    private struct CookieData
    {
        public SerializableVector3 position;
        public bool isActive;
    }

    public void ShowWinAndLosePanel()
    {
        if (winAndLosePanel != null && ThePlayerHasWin == true)
        {
            ColorUtility.TryParseHtmlString("#4DA454", out winOrLosePanelColor);
            winAndLosePanel.GetComponent<Image>().color = winOrLosePanelColor;
            winAndLosePanel.GetComponentInChildren<TextMeshProUGUI>().text = "You Win";
            winAndLosePanel.gameObject.SetActive(true);
        }
        else if (winAndLosePanel != null && ThePlayerLoose == true)
        {
            ColorUtility.TryParseHtmlString("#9C2424", out winOrLosePanelColor);
            winAndLosePanel.GetComponent<Image>().color = winOrLosePanelColor;
            winAndLosePanel.GetComponentInChildren<TextMeshProUGUI>().text = "You Lose";
            winAndLosePanel.gameObject.SetActive(true);
        }
    }

    public void Pause()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        player.GetComponent<Rigidbody2D>().simulated = false;
    }

    public void QuitPause()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        player.GetComponent<Rigidbody2D>().simulated = true;
    }
}
