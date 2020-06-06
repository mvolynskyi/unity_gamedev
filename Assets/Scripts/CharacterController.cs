using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float Speed = 10.0f;

    private float translation;
    private float straffe;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float momentSpeed = Speed * Time.deltaTime;

        translation = Input.GetAxis("Vertical") * momentSpeed;
        straffe = Input.GetAxis("Horizontal") * momentSpeed;

        transform.Translate(straffe, 0.0f, translation);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
