using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;

public class AutomaticGunScript : MonoBehaviour
{
//public
    public float gunDamage = 10.0f;
    public float range = 100.0f;
    public float fireRate = 15.0f;

    public float cameraShootTwitchValue = 30f;
    public Camera fpsCamera;

    public GameObject impactEffect;

    public int gunAmmoCount = 30;

    [Header("Audio Source")]
    [SerializeField]
	public AudioSource mainAudioSource;
    [SerializeField]
	public AudioSource shootAudioSource;

    [SerializeField]
    AudioClip shootSound;
    [SerializeField]
	AudioClip reloadSoundOutOfAmmo;
    [SerializeField]
	AudioClip reloadSoundAmmoLeft;

    [Header("UI Components")]
	public Text currentWeaponText;
	public Text currentAmmoCountText;
	public Text totalAmmoCountText;
    
	public Text winCondition;
	public GameObject winObject;
	public GameObject crosshair;

//private
    Animator animator;

    //Used for fire rate
	float lastFired;

    bool isReloading;
    bool isInspecting;

    int currentAmmoCount;
    bool isOutOfAmmo;

    string winConditionString;
    int enemiesCount;

    void Start()
    {
        animator = GetComponent<Animator>();

        winConditionString = winCondition.text;
        enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        updateWinCondition();

        shootAudioSource.clip = shootSound;
    }

    void Update()
    {
        AnimationCheck();

        currentAmmoCountText.text = currentAmmoCount.ToString ();

        ShootingHandler();

        // Inspect animation
        if (Input.GetKeyDown (KeyCode.T)) 
		{
			animator.SetTrigger ("Inspect");
		}
    }

    void ShootingHandler()
    {
        if(Cursor.lockState != CursorLockMode.Locked)
            return;

        // Ammo logic
        if(currentAmmoCount == 0)
        {
            currentWeaponText.text = "Out of Ammo. Reload";
            isOutOfAmmo = true;
            if(!isReloading)
                Reload();
        }
        else
        {
            currentWeaponText.text = "";
            isOutOfAmmo = false;
        }

        // Shooting
        if(Input.GetMouseButton(0) && !isOutOfAmmo && !isReloading)
        {
            if(Time.time - lastFired > 1 / fireRate)
            {
                lastFired = Time.time;
            
                currentAmmoCount--;

                // Sound
                shootAudioSource.clip = shootSound;
				shootAudioSource.Play ();

                if(animator != null)
                    animator.Play ("Fire", 0, 0f);

                Shoot();
                CameraTwitch(Vector3.up);
            }
        }

        // Reload 
		if (Input.GetKeyDown (KeyCode.R) && !isReloading && !isInspecting) 
			Reload ();
    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForceAtPosition(fpsCamera.transform.forward * gunDamage * 10, hit.point);
                
                Target target = hit.transform.GetComponent<Target>();
                if(target != null)
                {
                    if(target.TakeDamage(Mathf.CeilToInt(gunDamage)) == 0)
                    {
                        enemiesCount--;    
                        updateWinCondition();
                    }
                }
            }
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }

    }
    
    void CameraTwitch(Vector3 twitchDirection)
    {
        float rotationSpeed = 0.1f;

        fpsCamera.transform.forward = Vector3.Lerp(fpsCamera.transform.forward, 
            fpsCamera.transform.forward + twitchDirection * cameraShootTwitchValue, rotationSpeed * Time.deltaTime);
    }

    private void Reload()
    {
        if(isOutOfAmmo)
        {
            animator.Play("Reload Out Of Ammo", 0, 0f);

            mainAudioSource.clip = reloadSoundOutOfAmmo;
			mainAudioSource.Play ();
        }
        else
        {
            animator.Play("Reload Ammo Left", 0, 0f);

            mainAudioSource.clip = reloadSoundAmmoLeft;
			mainAudioSource.Play ();
        }

        currentAmmoCount = gunAmmoCount;
        isOutOfAmmo = false;
    } 

    //Check current animation playing
	private void AnimationCheck () 
    {
		//Check if reloading
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Reload Out Of Ammo") || 
			animator.GetCurrentAnimatorStateInfo (0).IsName ("Reload Ammo Left")) 
		{
			isReloading = true;
        }
		else 
		{
			isReloading = false;
		}

		//Check if inspecting weapon
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Inspect")) 
		{
			isInspecting = true;
		} 
		else 
		{
			isInspecting = false;
		}
	}

    void updateWinCondition()
    {
        winCondition.text = winConditionString + enemiesCount.ToString();
        if(enemiesCount == 0)
        {
            winObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            crosshair.SetActive(false);
        }
    }
}
