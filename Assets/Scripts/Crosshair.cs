using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Animator animator;
    public Transform weaponTip;
    Transform crosshairContainer;
    public RectTransform canvas;
    RectTransform crosshairRectTransform;
    Vector3 initpos;
    public float verticalOffset;
    // Start is called before the first frame update
    void Start()
    {
        crosshairRectTransform = GetComponent<RectTransform>();
        initpos = crosshairRectTransform.anchoredPosition;
        crosshairContainer = transform.GetChild(0);
    }
    public Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);

        Vector2 normalizedScreenpoint = new Vector3(screenPoint.x / Screen.width,
                                                    screenPoint.y / Screen.height);

        float vo = verticalOffset * (float)Screen.width / Screen.height;
        screenPoint =  new Vector3(normalizedScreenpoint.x * 800f,
                                   normalizedScreenpoint.y * 600f + vo, 0f);
        return screenPoint;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        bool aiming = animator.GetBool("Aiming");
        crosshairContainer.gameObject.SetActive(aiming); // tinta vizibila doar daca e click dreapta apasat
        int layerMask = ~LayerMask.NameToLayer("Default"); // layerul cu care facem raycast
        // se arunca o raza de la varful armei inainte, iar intersectia se transforma in spatiul ecran ca sa plaseze tinta
        Ray ray = new Ray(weaponTip.position, weaponTip.right);
        if (aiming && Physics.Raycast(ray,
                                      out RaycastHit hit, 200f, layerMask, QueryTriggerInteraction.Ignore))
        {
            crosshairRectTransform.anchoredPosition = WorldToScreenSpace(hit.point, Camera.main, canvas);
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            crosshairRectTransform.anchoredPosition = initpos;
        }

    }
}
