using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatteliteCamera : MonoBehaviour
{
    public Transform mainCamera;
    public Transform player;
    public float satelliteHeight = 50f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + Vector3.up * satelliteHeight;
        var camFwdXoZ = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up);
        transform.rotation = Quaternion.LookRotation(Vector3.down, camFwdXoZ);

    }
}
