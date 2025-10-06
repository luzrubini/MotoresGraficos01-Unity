using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class FirstPerson_PlayerController : MonoBehaviour
{
    public float movementSpeed = 10.0f;

    public Camera cam;

    public GameObject Bullet;

    public float BulletForce;

    [SerializeField]
    float playerJumpForce;

    Rigidbody rb;

    [SerializeField]
    int jumps;

    [SerializeField]
    bool isGrounded;

    public List<Gamepad> pads = new List<Gamepad>();

    public bool IsUsingGamepad;

    public bool IsUsingKeyboard;

    float forwardMovement;
    float SidewaysMovement;

    public bool CanInteract;
    private void Awake()
    {
        InputSystem.onDeviceChange += onInputDeviceChange;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        if (Gamepad.current != null)
        {
            IsUsingGamepad = true;
        }
        else
        {
            IsUsingKeyboard = true;
        }
    }

    void Update()
    {
        if(IsUsingGamepad == true)
        {
            forwardMovement = Gamepad.current.leftStick.ReadValue().y * movementSpeed;
            SidewaysMovement = Gamepad.current.leftStick.ReadValue().x * movementSpeed;
     
            Jump(Gamepad.current.buttonSouth);
            ShootBullet(Gamepad.current.buttonEast);
            Interact(Gamepad.current.buttonWest);
        }
        else if (IsUsingKeyboard == true)
        {
            forwardMovement = Input.GetAxis("Vertical") * movementSpeed;
            SidewaysMovement = Input.GetAxis("Horizontal") * movementSpeed;

            Jump(Keyboard.current.spaceKey);
            ShootBullet(Mouse.current.leftButton);
            Interact(Keyboard.current.eKey);
        }

        forwardMovement *= Time.deltaTime;
        SidewaysMovement *= Time.deltaTime;

        if (UI_Manager.instance.isUIOpen == false)
        {
            transform.Translate(SidewaysMovement, 0, forwardMovement);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Jump(ButtonControl key)
    {
        if (key.wasPressedThisFrame && jumps > 0 && FirstPerson_GameManager.Instance.isPaused == false)
        {
            rb.velocity = Vector2.up * playerJumpForce;
            jumps--;
        }
    }

    public void ShootBullet(ButtonControl key)
    {
        if (key.wasPressedThisFrame == true && FirstPerson_GameManager.Instance.isPaused == false)
        {
            Audio_Manager.instance.Play("Fire");

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1));

            GameObject bullet = Instantiate(Bullet, ray.origin, Quaternion.identity);

            bullet.GetComponent<Rigidbody>().AddForce(cam.transform.forward * BulletForce, ForceMode.Impulse);

            Destroy(bullet, 5);
        }
    }

    public void Interact(ButtonControl key)
    {
        if (key.wasPressedThisFrame == true && FirstPerson_GameManager.Instance.isPaused == false)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;

            if ((Physics.Raycast(ray, out hit) == true))
            {
                Debug.Log(hit.collider.gameObject);

                if (hit.collider.CompareTag("Button"))
                {
                    hit.collider.gameObject.GetComponentInParent<Door_Controller>().Open();
                }

                if (hit.collider.CompareTag("NPC"))
                {
                    hit.collider.gameObject.GetComponentInParent<NPCdialogue_Controller>().dialoguePanel.gameObject.SetActive(true);
                    hit.collider.gameObject.GetComponentInParent<NPCdialogue_Controller>().DisplayNextDialogue();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flag"))
        {
            FirstPerson_GameManager.Instance.ThePlayerHasWon = true;
        }

        if (other.CompareTag("respawn"))
        {
            FirstPerson_GameManager.Instance.ThePlayerLose = true;
        }

        if (other.CompareTag("EnemyBullet") && UI_Manager.instance.isUIOpen == false)
        {
            transform.position = FirstPerson_GameManager.Instance.playerposition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGrounded = true;
            jumps = 2;
        }
    }

    public void onInputDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:

                if (device is Gamepad)
                {
                    IsUsingGamepad = false;
                    IsUsingKeyboard = true;
                }

                break;

            case InputDeviceChange.Added:

                if (device is Gamepad)
                {
                    IsUsingGamepad = true;
                    IsUsingKeyboard = false;
                }

                break;
        }
    }
}
