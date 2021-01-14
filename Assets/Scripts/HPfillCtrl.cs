using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPfillCtrl : MonoBehaviour
{
    public Animator playerAnimator;
    RectTransform rectTransform;
    UnityEngine.UI.Image hpFillImg;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        hpFillImg = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float HP = (float)playerAnimator.GetInteger("HP") / 100f;
        rectTransform.localScale = new Vector3(HP, 1, 1);
        hpFillImg.color = Color.Lerp(Color.red, Color.green, HP);
    }
}
