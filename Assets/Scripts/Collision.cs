using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using System;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    [SerializeField]float loadDelay = 0.5f;

    [SerializeField] AudioClip successSound;
    [SerializeField] AudioClip failureSound;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem failureParticles;

  

    AudioSource audioSource;

    bool isTransitioning = false;
    bool isColisioning = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (isTransitioning) 
        {
            return;
        }

        switch (collision.gameObject.tag) 
        {
            case "Friendly":
                Debug.Log("Collision Start");
                break;
            case "Finish":
                Debug.Log("Collision Finish");
                
                StartSuccessSequence();
                break;
            
            default:
                Debug.Log("Collision something");
                
                StartCrashSequence();
                break;
        }
    }

    private void PlaySound(AudioClip clip) 
    {
       audioSource.PlayOneShot(clip);
    }

    public void ReloadLvl()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadNextLvl() 
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) 
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
       
        

    }

    private void StartCrashSequence()
    {
        if (isColisioning) 
        {
            isTransitioning = true;
            PlaySound(failureSound);
            failureParticles.Play();
            GetComponent<Movement>().enabled = false;
            Invoke("ReloadLvl", loadDelay);
        }
        
    }

    void StartSuccessSequence() 
    {
        isTransitioning = true;
        PlaySound(successSound);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLvl", loadDelay);
    }

    public void SwitchCollision() 
    {
        if (isColisioning)
        {
            isColisioning = false;
        }
        else if(!isColisioning)
        {
            isColisioning = true;
        }

    }
}
