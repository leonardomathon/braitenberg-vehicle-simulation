using UnityEngine;
using TMPro;

public class SelectionMenuController : MonoBehaviour
{

    [SerializeField] private GameObject selectionMenu;

    private TextMeshProUGUI selectedObjectTag;

    private GameObject selectedObject;

    private GameObject selectedObjectToolMenu;

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

    public void SetSelectedObjectText()
    {
        // Set the text of the tooltip
        selectedObjectTag.SetText("No object selected");

        // Deactivate toolbar
        selectedObjectToolMenu.SetActive(false);

        // Update text mesh to prevent bug
        selectedObjectTag.ForceMeshUpdate();
    }

    public void SetSelectedObjectText(string text)
    {
        // Set the text of the tooltip
        selectedObjectTag.SetText(text);

        // Activate toolbar
        selectedObjectToolMenu.SetActive(true);

        // Update text mesh to prevent bug
        selectedObjectTag.ForceMeshUpdate();
    }
}
