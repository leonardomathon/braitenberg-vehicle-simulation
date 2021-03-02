using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    // Boolean that stores if object is selected
    [SerializeField] private bool isSelected;

    // Standard material of object
    [SerializeField] private Material materialStandard;

    // Material when object is selected
    [SerializeField] private Material materialOnSelect;

    protected virtual void Update()
    {
        if (isSelected)
        {
            // Apply selection material only if selected
            ApplySelectionMaterial();
        }
        else
        {
            // Remove selection material else
            RemoveSelectionMaterial();
        }
    }

    // Getter and setters for isSelected
    public bool IsSelected()
    {
        return isSelected;
    }

    public void Select()
    {
        isSelected = true;
    }

    public void Deselect()
    {
        isSelected = false;
    }

    // Applies selection material to this object
    private void ApplySelectionMaterial()
    {
        // Apply material for object selection
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = materialOnSelect;
    }

    // Removes selection material from this object
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
