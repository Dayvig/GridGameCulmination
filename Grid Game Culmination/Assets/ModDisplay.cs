using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int ID;
    public CharacterDisplay thisDisplay;

    public void OnPointerEnter(PointerEventData eventData)
    {
        thisDisplay.hoveredBuffID = ID;
        thisDisplay.onEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        thisDisplay.onExit();
        thisDisplay.hoveredBuffID = -1;
    }
}
