using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mouseOffset;
    private float zCoordinate;

    void OnMouseDown()
    {  
        // Compute the Z-coordinate of the object
        zCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        
        // Compute the mouse offset
        mouseOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoordinate));
    }

    void OnMouseDrag()
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zCoordinate)) + mouseOffset;
        transform.position = new Vector3(Mathf.Ceil(newPosition.x), transform.position.y, Mathf.Ceil(newPosition.z));
    }
}
