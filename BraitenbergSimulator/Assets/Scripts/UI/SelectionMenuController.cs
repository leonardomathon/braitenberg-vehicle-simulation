using UnityEngine;
using TMPro;

public class SelectionMenuController : MonoBehaviour
{

    [SerializeField] private GameObject selectionMenu;

    private TextMeshProUGUI selectedObjectTag;

    // Singleton pattern for SelectionMenuController
    #region singleton
    private static SelectionMenuController _instance;

    public static SelectionMenuController Instance { get { return _instance; } }


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
        selectedObjectTag = selectionMenu.transform.Find("SelectedObjectTag").GetComponent<TextMeshProUGUI>();
    }

    public void SetSelectedObjectText()
    {
        // Set the text of the tooltip
        selectedObjectTag.SetText("No object selected");

        // Update text mesh to prevent bug
        selectedObjectTag.ForceMeshUpdate();
    }

    public void SetSelectedObjectText(string text)
    {
        // Set the text of the tooltip
        selectedObjectTag.SetText(text);

        // Update text mesh to prevent bug
        selectedObjectTag.ForceMeshUpdate();
    }
}
