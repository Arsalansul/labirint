using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [HideInInspector] public Transform target;
    public Vector3 offset;
    public float smoothSpeed;

    void LateUpdate()
    {
        if (target)
        {
            FollowTarget();
        }
    }

    private void FollowTarget()
    {
        var desiredPosition = target.position + offset;

        var axisZLimeter = GetComponent<Camera>().orthographicSize;
        var axisXLimeter = axisZLimeter * GetComponent<Camera>().aspect;

        if (desiredPosition.x < axisXLimeter)
        {
            desiredPosition.x = axisXLimeter;
        }
        else if (desiredPosition.x > Settings.Instance.gameSettings.labirintSize - axisXLimeter)
        {
            desiredPosition.x = Settings.Instance.gameSettings.labirintSize - axisXLimeter;
        }
        
        if (desiredPosition.z < axisZLimeter)
        {
            desiredPosition.z = axisZLimeter;
        }
        else if (desiredPosition.z > Settings.Instance.gameSettings.labirintSize - axisZLimeter)
        {
            desiredPosition.z = Settings.Instance.gameSettings.labirintSize - axisZLimeter;
        }


        var smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;
    }
}
