using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rcsThrust = 250f;
    [SerializeField]float mainThrust = 100f;

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
            Thrust();
            Rotate();
            PlaySound();
        }else{
            audioSource.Stop();
        }
    }

    private void PlaySound()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
    }

    void OnCollisionEnter(Collision collision){
        if(state != State.Alive){return;} // Ignore collisions when dead

        switch(collision.gameObject.tag){
            case "Friendly":
                break;
            case "Finish":
                state = State.Trascending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                print("Dead");
                Invoke("LoadSceneOne", 1f);
                break;
        }
    }

    private void LoadSceneOne()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    void Thrust()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
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
