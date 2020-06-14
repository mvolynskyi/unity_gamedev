using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float forceValue = 10.0f;
    Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            rigidBody.AddForce(Vector3.forward*forceValue);
            Debug.Log("Default rigidBody.AddForce");
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Default rigidBody.AddForce");
        }
    }
}
