using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject spriteHover;
    public void OnPointerEnter(PointerEventData data)
    {
        if(spriteHover == null) return;
        spriteHover.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        if(spriteHover == null) return;
        spriteHover.SetActive(false);
    }
}
