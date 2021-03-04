using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionMenuController : MonoBehaviour
{

    [SerializeField] private GameObject selectionMenu;

    private TextMeshProUGUI selectedObjectTag;

    private GameObject selectedObject;

    private GameObject selectedObjectToolMenu;

    private Button[] toolMenuButtons = new Button[4];

    private SelectionController selectionController;

    void Start()
    {
        // Get instance of selection controller
        selectionController = SelectionController.Instance;
        selectedObject = selectionController.GetSelectedObject();

        // Get used components
        selectedObjectTag = selectionMenu.transform.Find("SelectedObjectTag").GetComponent<TextMeshProUGUI>();
        selectedObjectToolMenu = selectionMenu.transform.Find("SelectedObjectToolMenu").gameObject;

        // Set toolbar inactive
        selectedObjectToolMenu.SetActive(false);

        // Setup tool buttons
        SetupToolButtons();
    }

    void Update()
    {
        GameObject _selectedObject = selectionController.GetSelectedObject();

        // Update UI accordingly
        // Inefficient, but does not affect performance
        if (_selectedObject == null)
        {
            selectedObject = _selectedObject;
            SetSelectedObjectText();
        }
        else
        {
            if (selectedObject != _selectedObject)
            {
                selectedObject = _selectedObject;
                SetSelectedObjectText(selectedObject.GetComponent<Object>().GetObjectName());
            }
        }
    }

    private void SetupToolButtons()
    {
        // Get all buttons
        for (int i = 0; i < selectedObjectToolMenu.transform.childCount; i++)
        {
            toolMenuButtons[i] = selectedObjectToolMenu.transform.GetChild(i).GetComponent<Button>();
        }

        // Setup listeners
        toolMenuButtons[0].onClick.AddListener(ClickSelectButton);
        toolMenuButtons[1].onClick.AddListener(ClickDeleteButton);
        toolMenuButtons[2].onClick.AddListener(ClickMoveButton);
        toolMenuButtons[3].onClick.AddListener(ClickRotateButton);

    }

    private void ClickSelectButton()
    {
        selectionController.ResetSelectedObject();
    }

    private void ClickDeleteButton()
    {
        selectionController.DeleteSelectedObject();
    }

    private void ClickMoveButton()
    {
        selectionController.MoveSelectedObject();
    }

    private void ClickRotateButton()
    {
        selectionController.RotateSelectedObject();
    }

    private void SetSelectedObjectText()
    {
        // Set the text of the tooltip
        selectedObjectTag.SetText("No object selected");

        // Deactivate toolbar
        selectedObjectToolMenu.SetActive(false);

        // Update text mesh to prevent bug
        selectedObjectTag.ForceMeshUpdate();
    }

    private void SetSelectedObjectText(string text)
    {
        // Set the text of the tooltip
        selectedObjectTag.SetText(text);

        // Activate toolbar
        selectedObjectToolMenu.SetActive(true);

        // Update text mesh to prevent bug
        selectedObjectTag.ForceMeshUpdate();
    }
}
