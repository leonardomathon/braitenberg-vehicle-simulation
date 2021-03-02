using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbulb : MonoBehaviour
{
    // Intensity of the lightsource
    [SerializeField]
    private int lightIntensity;

    // Color of the lightsource
    [SerializeField]
    private int lightColor;

    // Boolean indicating if the lightsource is selected
    [SerializeField]
    private bool isSelected;

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
}
