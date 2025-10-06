using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_manager : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 2f;
    public float distance = 3f;
    private Vector3 startPos;
    private bool movingUp = true;
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movingUp)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            if (transform.position.y >= startPos.y + distance)
            {
                movingUp = false;
            }
        }
        else 
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            if (transform.position.y <= startPos.y)
            {
                movingUp = true; //si appendeo el obj al jugado hago q no salte y pegue la plataforma a el
            }
        }
    }
}
