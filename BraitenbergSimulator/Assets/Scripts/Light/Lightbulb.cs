using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbulb : MonoBehaviour
{
    // Intensity of the lightsource
    [SerializeField] private int lightIntensity;

    // Color of the lightsource
    [SerializeField] private int lightColor;

    [SerializeField] private Material materialStandard;

    [SerializeField] private Material materialOnSelect;

    // Boolean indicating if the lightsource is selected
    [SerializeField] private bool isSelected;

    void Update()
    {
        if (isSelected)
        {
            // Apply material
            ApplySelectionMaterial();

        }
        else
        {
            // Remove selection material
            RemoveSelectionMaterial();
        }
    }

    // Select the lightsource
    public void Select()
    {
        isSelected = true;
    }

    // Deselect the lightsource
    public void Deselect()
    {
        isSelected = false;
    }

    // Return the selected state of the lightsource
    public bool IsSelected()
    {
        return isSelected;
    }

    private void ApplySelectionMaterial()
    {
        // Apply material for object selection
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = materialOnSelect;
    }

    private void RemoveSelectionMaterial()
    {
        // Only change if necessary
        if (gameObject.GetComponent<MeshRenderer>().sharedMaterial.name == materialOnSelect.name)
        {
            // Apply default material
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = materialStandard;
        }
    }
}
