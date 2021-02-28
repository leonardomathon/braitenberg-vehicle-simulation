using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipController : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvasRectTransform;

    [SerializeField]
    private GameObject tooltip;

    private RectTransform tooltipRectTransform;

    private RectTransform tooltipBackground;

    private TextMeshProUGUI tooltipText;

    // Singleton pattern for TooltipController
    #region singleton
    private static TooltipController _instance;

    public static TooltipController Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    void Start()
    {
        // Get used components
        tooltipRectTransform = tooltip.GetComponent<RectTransform>();
        tooltipBackground = tooltip.transform.Find("TooltipBackground").GetComponent<RectTransform>();
        tooltipText = tooltip.transform.Find("TooltipText").GetComponent<TextMeshProUGUI>();

        // Hide tooltip on start
        HideToolTip();
    }

    void Update()
    {
        // Create new anchored position
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + tooltipBackground.rect.width > canvasRectTransform.rect.width)
        {
            // Tooltip does not fit on screen on the right side
            anchoredPosition.x = canvasRectTransform.rect.width - tooltipBackground.rect.width;
        }
        if (anchoredPosition.y + tooltipBackground.rect.height > canvasRectTransform.rect.height)
        {
            // Tooltip does not fit on screen on the top side
            anchoredPosition.y = canvasRectTransform.rect.height - tooltipBackground.rect.height;
        }

        // Set tooltip anchor based on canvas scale
        tooltipRectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string text)
    {
        // Set the text of the tooltip
        tooltipText.SetText(text);

        // Update text mesh to prevent bug
        tooltipText.ForceMeshUpdate();

        // Get text size and padding
        Vector2 textSize = tooltipText.GetRenderedValues(false);
        Vector2 padding = new Vector2(10, 10);

        // Scale background to fit textsize and padding
        tooltipBackground.sizeDelta = textSize + padding;
    }

    public void ShowToolTip(string text)
    {
        tooltip.SetActive(true);
        SetText(text);
    }

    public void HideToolTip()
    {
        tooltip.SetActive(false);
    }



}
