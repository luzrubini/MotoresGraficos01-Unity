using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraControl : MonoBehaviour
{
    Vector2 mouseLook;
    Vector2 smoothV;

    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    GameObject player;

    private void Awake()
    {
        sensitivity = PlayerPrefs.GetFloat("sensitivity");

        player = this.transform.parent.gameObject;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(UI_Manager.instance.isUIOpen == false)
        {
            moveCamera();
        }
    }


    public void changeSensitivity(float sensitivityValue)
    {
        sensitivity = sensitivityValue;

        PlayerPrefs.SetFloat("sensitivity", sensitivityValue);
    }

    public void moveCamera()
    {
        Vector2 md = new Vector2();

        if (GetComponentInParent<FirstPerson_PlayerController>().IsUsingGamepad == true)
        {
            md = new Vector2(Gamepad.current.rightStick.ReadValue().x, Gamepad.current.rightStick.ReadValue().y);
        }
        else if (GetComponentInParent<FirstPerson_PlayerController>().IsUsingKeyboard == true)
        {
            md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);

        mouseLook += smoothV;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, player.transform.up);
    }
}
