using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject spriteHover;
    public void OnPointerEnter(PointerEventData data)
    {
        spriteHover.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        spriteHover.SetActive(false);
    }
}
