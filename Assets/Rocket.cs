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

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
        if(Input.GetKeyDown(KeyCode.Space)){
            audioSource.Play();
            print("Keydown");
        }else if(Input.GetKeyUp(KeyCode.Space)){
            print("Keyup");
            audioSource.Stop();
        }
    }

    void OnCollisionEnter(Collision collision){
        switch(collision.gameObject.tag){
            case "Friendly":
                break;
            case "Finish":
                SceneManager.LoadScene(1);
                break;
            default:
                SceneManager.LoadScene(0);
                break;
        }
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
