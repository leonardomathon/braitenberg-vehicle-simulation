using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string tooltipText = "Default tooltip text";

    private TooltipController tooltipController;

    void Start()
    {
        tooltipController = TooltipController.Instance;
    }

    public void SetButtonTooltipText(string text)
    {
        tooltipText = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipController.ShowToolTip(tooltipText);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        tooltipController.HideToolTip();
    }
}
