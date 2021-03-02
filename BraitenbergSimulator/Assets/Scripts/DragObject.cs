using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Plane plane;

    void Start()
    {
        plane = new Plane(Vector3.up, Vector3.up * gameObject.transform.position.y);
    }

    void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 newPos = new Vector3(
                Mathf.Ceil(ray.GetPoint(distance).x),
                ray.GetPoint(distance).y,
                Mathf.Ceil(ray.GetPoint(distance).z)
            );
            gameObject.transform.position = newPos;
        }
    }
}