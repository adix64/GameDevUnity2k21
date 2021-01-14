using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float frequency = 1f;
    public float amplitude = 1f;
    public float phase = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float s = Mathf.Sin(Time.time * frequency + phase) * amplitude;
        transform.localRotation = Quaternion.Euler(0, 0, s);
    }
}
