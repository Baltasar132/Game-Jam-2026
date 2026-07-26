using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BasicTooltiper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(2, 5)]
    [SerializeField] private string tooltipContent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipUI.INSTANCE.Show(tooltipContent);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.INSTANCE.Hide();
    }

    private void OnDisable()
    {
        if (TooltipUI.INSTANCE != null)
        {
            TooltipUI.INSTANCE.Hide();
        }
    }
}

