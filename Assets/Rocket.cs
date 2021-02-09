using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rcsThrust = 250f;
    [SerializeField]float mainThrust = 100f;
    [SerializeField]AudioClip mainEngine;
    [SerializeField]AudioClip deathSound;
    [SerializeField]AudioClip levelLoadSound;

    [SerializeField]ParticleSystem mainEngineParticles;
    [SerializeField]ParticleSystem deathParticles;
    [SerializeField]ParticleSystem levelLoadParticles;
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Game States
    enum State{Alive, Dying, Trascending};
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            Rotate();
            PlaySound();
        }
    }

    void PlaySound()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    void OnCollisionEnter(Collision collision){
        if(state != State.Alive){return;} // Ignore collisions when dead

        switch(collision.gameObject.tag){
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }


    private void StartSuccessSequence()
    {
        state = State.Trascending;
        audioSource.Stop();
        mainEngineParticles.Stop();
        levelLoadParticles.Play();
        audioSource.PlayOneShot(levelLoadSound);
        Invoke("LoadNextScene", 1f);
    }
    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        mainEngineParticles.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadSceneOne", 1f);
    }

    private void LoadSceneOne()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        }
    }
    void Rotate()
    {
        rigidBody.freezeRotation = true; // Take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // Return physics control of rotation
    }
}
