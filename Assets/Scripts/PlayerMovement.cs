using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 12.0f;
    public float jumpHeight = 3.0f;
    public float gravityMultiplier = -2.0f;

    public GameObject armsModel;
 
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    private CharacterController characterController;
    private float gravity = 9.81f;
    private Vector3 velocity;

    [SerializeField]
    private AudioClip walkingSound;
    private AudioSource audioSource;

    private Animator armsModelAnimator;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();

        armsModelAnimator = armsModel.GetComponent<Animator>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(audioSource != null)
        {
            audioSource.clip = walkingSound;
            audioSource.loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementHandler();

        // Walking animation
		if(Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) ||	Input.GetKey (KeyCode.S) || 
            Input.GetKey (KeyCode.D) ) 
		{
			armsModelAnimator.SetBool ("Walk", true);
            if(characterController.isGrounded && !audioSource.isPlaying)
                audioSource.Play();
		} 
        else 
        {
			armsModelAnimator.SetBool ("Walk", false);
            if(audioSource.isPlaying)
                audioSource.Pause();
		}


    }

    private void PlayerMovementHandler()
    {
        bool isGrounded = characterController.isGrounded;
        if(isGrounded && velocity.y < 0)
            velocity.y = -2.0f;

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            /*if(playerModelAnimator != null)
                playerModelAnimator.SetTrigger("isJumped");*/

            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity * gravityMultiplier);
        }

        velocity.y += gravity * gravityMultiplier * Time.deltaTime;

        float verticalInputValue = Input.GetAxis(verticalInputName);
        float horizontalInputValue = Input.GetAxis(horizontalInputName);

        Vector3 verticalMovement = transform.forward * verticalInputValue;
        Vector3 horizontalMovement = transform.right * horizontalInputValue;

        Vector3 movement = (horizontalMovement + verticalMovement) * movementSpeed + velocity;

        characterController.Move(movement * Time.deltaTime);
    }

    /*private void PlayFootstepSounds()
    {
        if (characterController.isGrounded && GamebOject.rigidbody.velocity.sqrMagnitude > 0.1f)
        {
            audioSource.clip = walkingSound;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
        }
    }*/
}
