using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 50f;

    private void Start()
    {//traieste doar 5 secunde de la lansare
        StartCoroutine(Autodestroy(5f));
    }
    IEnumerator Autodestroy(float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
    void Update()
    {//proiectiul circula inainte, de-alungul axei in care a fost lansat
        transform.position += transform.up * Time.deltaTime * projectileSpeed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Autodestroy(0f));
    }
}
