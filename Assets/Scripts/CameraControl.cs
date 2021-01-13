using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraControl : MonoBehaviour
{
    float yaw = 0f, pitch = 0f;
    public float minPitch = -45f, maxPitch = 45f;
    public float coSpeed = 5f;
    public Transform player;
    public Vector3 defaultCameraOffset;
    public Vector3 aimingCameraOffset;
    Vector3 cameraOffset;
    Vector3 targetCameraOffset;
    public MovePlayer playerScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //acumulam deplasamentul mouselui la unghiurile facute de sys coord local cu axele lumii:
        yaw += Input.GetAxis("Mouse X"); 
        pitch -= Input.GetAxis("Mouse Y");
        //limitam rotatia sus-jos a.i. sa nu se dea peste cap:
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        //aplicam rotatia:
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

 
        targetCameraOffset = playerScript.aiming ? aimingCameraOffset : defaultCameraOffset;
        cameraOffset = Vector3.Lerp(cameraOffset, targetCameraOffset, Time.deltaTime * coSpeed);
        //trecem deplasamentul camerei relativ la personaj din spatiul local in spatiul lume
        Vector3 worldSpaceOffset = transform.TransformDirection(cameraOffset);
        //calculam pozitia camerei:
        transform.position = player.position + worldSpaceOffset;
    }
}
