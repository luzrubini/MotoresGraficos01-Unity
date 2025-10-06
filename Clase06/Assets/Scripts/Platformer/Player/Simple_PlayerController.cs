using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Simple_PlayerController : MonoBehaviour
{
    private float horizontal;
    private Rigidbody2D rb;
    public bool isGrounded;
    public Transform groundCheck;
    public int jumps;
    public float playerSpeed;
    public float playerJumpForce;

    private bool isInSaveStation = false;
    private bool isInLoadStation = false;

    public GameObject Bullet;
    public float BulletForce;

    public List<Gamepad> pads = new List<Gamepad>();

    private bool isUsingGamepad;
    private bool isUsingKeyboard;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        CheckInputDevice();
    }

    void Update()
    {
        CheckInputDevice(); 

        bool inputReceived = Keyboard.current.eKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame);

        Move();

        if (isInSaveStation && inputReceived)
        {
            SavingLoading_Manager.instance.Save();
        }

        if (isInLoadStation == true && inputReceived)
        {
            SavingLoading_Manager.instance.Load();
        }
    }

    private void CheckInputDevice()
    {
        if (Gamepad.current != null)
        {
            if (!isUsingGamepad)
            {
                isUsingGamepad = true;
                isUsingKeyboard = false;
                pads.Clear();
                pads.Add(Gamepad.current); 
            }
        }
        else
        {
            if (!isUsingKeyboard)
            {
                isUsingKeyboard = true;
                isUsingGamepad = false;
            }
        }
    }

    private void Move()
    {
        horizontal = isUsingGamepad ? Gamepad.current.leftStick.ReadValue().x : Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(horizontal * playerSpeed, rb.velocity.y);

        if (isUsingGamepad)
        {
            Jump(Gamepad.current.buttonSouth);
        }
        else if (isUsingKeyboard)
        {
            Jump(Keyboard.current.spaceKey);
        }

        AnimateMovement();
    }

    private void AnimateMovement()
    {
        if (horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            SetAnimationState(true);
        }
        else if (horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            SetAnimationState(true);
        }
        else
        {
            SetAnimationState(false);
        }
    }

    private void SetAnimationState(bool isWalking)
    {
        gameObject.GetComponentInChildren<Animator>().SetBool("PlayerWalk", isWalking);
    }

    private void Jump(ButtonControl key)
    {
        if (key.wasPressedThisFrame && jumps > 0)
        {
            rb.velocity = Vector2.up * playerJumpForce;
            jumps--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGrounded = true;
            jumps = 2; 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cookie"))
        {
            Audio_Manager.instance.Play("Score");
            collision.gameObject.SetActive(false);
            PlatformerGame_Manager.instance.cookiesNumber--;
        }
        else if (collision.CompareTag("Spikes"))
        {
            PlatformerGame_Manager.instance.ThePlayerLoose = true;
            gameObject.SetActive(false);
            Audio_Manager.instance.Play("Death");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SaveStation"))
        {
            isInSaveStation = false;

            if (isUsingGamepad)
            {
                collision.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                UI_Manager.instance.DesactivateText(0);
            }
            else if (isUsingKeyboard)
            {
                collision.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                UI_Manager.instance.DesactivateText(0);
            }
        }
        else if (collision.CompareTag("LoadStation"))
        {
            isInLoadStation = false;

            if (isUsingGamepad)
            {
                collision.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                UI_Manager.instance.DesactivateText(1);
            }
            else if (isUsingKeyboard)
            {
                collision.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                UI_Manager.instance.DesactivateText(1);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("SaveStation"))
        {
            isInSaveStation = true;

            if (isUsingGamepad)
            {
                collision.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                UI_Manager.instance.ActivateText(0);
            }
            else if (isUsingKeyboard)
            {
                collision.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                UI_Manager.instance.ActivateText(0);
            }
        }
        else if (collision.CompareTag("LoadStation"))
        {
            isInLoadStation = true;

            if (isUsingGamepad)
            {
                collision.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                UI_Manager.instance.ActivateText(1);
            }
            else if (isUsingKeyboard)
            {
                collision.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                UI_Manager.instance.ActivateText(1);
            }
        }
    }
}
