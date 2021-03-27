using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public string tooltipText = "Default tooltip text";

	private Action action;
	
	private TooltipController tooltipController;

	private void Start() {
		tooltipController = TooltipController.Instance;
	}

	public void SetButtonTooltipText(string text) {
		tooltipText = text;
	}
	public void SetAction(Action action) {
		this.action = action;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		tooltipController.ShowToolTip(tooltipText);
	}
	public void OnPointerExit(PointerEventData pointerEventData) {
		tooltipController.HideToolTip();
	}
	private void OnDisable() {
		tooltipController.HideToolTip();
	}
	public void OnClick() {
		action?.Invoke();
	}
}