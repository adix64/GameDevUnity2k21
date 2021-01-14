using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public MainMenu mainMenu;
  
    public void OnPointerDown(PointerEventData data)
    {
        // nu te lasa sa te razgandesti, o data ce ai dat click pe buton, actiunea e declansata
        mainMenu.PlayBasicsGame(); 
    }
    public void OnPointerUp(PointerEventData data)
    {

    }
}
