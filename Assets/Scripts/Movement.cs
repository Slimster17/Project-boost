using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 1000f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem rightBooster;
    [SerializeField] ParticleSystem mainBooster;

    private Rigidbody rb;
    AudioSource audioSource;

    Collision collision;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        collision = GetComponent<Collision>();
    }

    // Update is called once per frame
    void Update()
    {
        ProccesThrust();
        ProccesRotation();
        DebugKeys();
    }

    private void ProccesThrust()
    {
       
        if (Input.GetKey(KeyCode.Space))
        {
            StartThusting();
        }

        else
        {
            StopThrusting();
        }
    }

    private void StartThusting()
    {
        Debug.Log("Thrusting");

        rb.AddRelativeForce(Vector3.up * thrustSpeed * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);

        }

        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainBooster.Stop();
    }

    private void ProccesRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Moving left");
            ApplyRotation(rotationSpeed, rightBooster);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Moving right");
            ApplyRotation(-rotationSpeed,leftBooster);
        }
    }

    private void ApplyRotation(float rotationThisFrame, ParticleSystem particle)
    {
        rb.freezeRotation = true; // freezing rotation for manual rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;

        if (!particle.isPlaying)
        {
            particle.Play();
        }

    }

   private void DebugKeys() 
    {

        if(Input.GetKey(KeyCode.L)) 
        {
            Debug.Log("NextLvl");
            collision.LoadNextLvl();
        }

        else if (Input.GetKey(KeyCode.R)) 
        {
            Debug.Log("Restart");
            collision.ReloadLvl();
        }

        else if (Input.GetKey(KeyCode.G)) 
        {
            Debug.Log("Collision mode changed");
            collision.SwitchCollision();
        }
    }
}
