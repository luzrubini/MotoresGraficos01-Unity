using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphere_actions : MonoBehaviour
{
    public float velocidad = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * velocidad * Time.deltaTime);
    }
}
