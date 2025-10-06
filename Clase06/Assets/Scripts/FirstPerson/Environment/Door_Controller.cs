using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Controller : MonoBehaviour
{
    public bool DoorIsopen = false;
  
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public void Open()
    {
        DoorIsopen = true;
        GetComponent<Animator>().SetBool("Open", true);
    }
}
