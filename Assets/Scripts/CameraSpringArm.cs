using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpringArm : MonoBehaviour
{

    public GameObject target;

    public float armLength = 5f;
    public float height = 1f;

    float currentY;
    float targetY;
    Quaternion currentRotation;

    private Vector3 velocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        targetY = target.transform.position.y + height;

        // transform.position = new Vector3(target.transform.position.x, targetY, target.transform.position.z);
        // transform.position -= transform.rotation * Vector3.forward * armLength;

        Vector3 targetPosition = new Vector3(target.transform.position.x, targetY, target.transform.position.z);
        targetPosition -= transform.rotation * Vector3.forward * armLength;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, Time.deltaTime);



        // transform.LookAt(target.transform);
    }

}
