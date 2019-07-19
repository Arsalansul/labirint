using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    
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
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            controller.SimpleMove(new Vector3((Input.GetAxis("Horizontal")) * Settings.Instance.playerSettings.speed, 0,
                (Input.GetAxis("Vertical")) * Settings.Instance.playerSettings.speed));
        }
    }
}
