using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField] private Material materialStandard;

    [SerializeField] private Material materialOnSelect;

    [SerializeField] private bool isSelected;

    protected virtual void Update()
    {
        if (isSelected)
        {
            ApplySelectionMaterial();

        }
        else
        {
            RemoveSelectionMaterial();
        }
    }

    public void Select()
    {
        isSelected = true;
    }

    public void Deselect()
    {
        isSelected = false;
    }

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
