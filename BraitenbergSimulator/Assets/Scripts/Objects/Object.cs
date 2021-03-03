using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    // Random object id
    private int objectId;

    // Name of the object
    [SerializeField] private string objectName;

    // Boolean that stores if object is selected
    [SerializeField] private bool isSelected;

    // Boolean that stores if object is currently movable
    [SerializeField] private bool isMovable;

    // Standard material of object
    [SerializeField] private Material materialStandard;

    // Material when object is selected
    [SerializeField] private Material materialOnSelect;

    protected virtual void Start()
    {
        objectId = Random.Range(1, int.MaxValue);
    }

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

    // Getter for object id
    public string GetObjectId()
    {
        return objectId.ToString();
    }


    // Getter for object name
    public string GetObjectName()
    {
        return objectName;
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

    // Getter and setters for isMovable
    public bool IsMovable()
    {
        return isMovable;
    }

    public void Move()
    {
        isMovable = true;

        // Temporarily disable gravity for the object
        gameObject.GetComponent<Rigidbody>().useGravity = false;

        // Disable collisions
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void Place()
    {
        isMovable = false;

        // Enable gravity for the object
        gameObject.GetComponent<Rigidbody>().useGravity = true;

        // Enable collisions
        gameObject.GetComponent<BoxCollider>().enabled = true;
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
