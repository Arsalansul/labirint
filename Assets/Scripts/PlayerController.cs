using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;

    private bool moveAxisX;

    private bool moveAxisY;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        //MoveAxisFix();
        //Vector3 moveVector = new Vector3();
        //if (Input.GetAxis("Horizontal") != 0 && moveAxisX)
        //{
        //    moveVector = new Vector3(Input.GetAxis("Horizontal") * Settings.Instance.playerSettings.speed, 0, 0);
        //    
        //}
        //
        //if (Input.GetAxis("Vertical") != 0 && moveAxisY)
        //{
        //    moveVector = new Vector3(0, 0,Input.GetAxis("Vertical") * Settings.Instance.playerSettings.speed);
        //}
        //
        //controller.SimpleMove(moveVector);

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            controller.SimpleMove(new Vector3(Input.GetAxis("Horizontal") * Settings.Instance.playerSettings.speed, 0,
                Input.GetAxis("Vertical") * Settings.Instance.playerSettings.speed));
        }
    }

    private void MoveAxisFix()
    {
        if (Input.GetAxis("Horizontal") != 0 && !moveAxisY)
        {
            moveAxisX = true;
        }
        else
        {
            moveAxisX = false;
        }

        if (Input.GetAxis("Vertical") != 0 && !moveAxisX)
        {
            moveAxisY = true;
        }
        else
        {
            moveAxisY = false;
        }
    }
}
