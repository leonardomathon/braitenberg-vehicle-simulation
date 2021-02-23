using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Button variables
    [SerializeField] private Button[] spawnButtons = new Button[5];

    private SpawnController spawnController;

    void Start()
    {
        spawnController = SpawnController.Instance;

        // Set eventlisteners
        spawnButtons[0].onClick.AddListener(ClickLightButton);
        spawnButtons[1].onClick.AddListener(ClickVehicleAggressionButton);
        spawnButtons[2].onClick.AddListener(ClickVehicleExplorationButton);
        spawnButtons[3].onClick.AddListener(ClickVehicleFearButton);
        spawnButtons[4].onClick.AddListener(ClickVehicleLoveButton);
    }

    private void ClickLightButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Light])
        {
            enableButtonOutline(spawnButtons[0], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Light);
        }
        else
        {
            disableButtonOutline(spawnButtons[0]);
            spawnController.DeselectObjectToSpawn();
        }
    }

    private void ClickVehicleAggressionButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Aggression])
        {
            enableButtonOutline(spawnButtons[1], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Aggression);
        }
        else
        {
            disableButtonOutline(spawnButtons[1]);
            spawnController.DeselectObjectToSpawn();
        }
    }

    private void ClickVehicleExplorationButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Exploration])
        {
            enableButtonOutline(spawnButtons[2], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Exploration);
        }
        else
        {
            disableButtonOutline(spawnButtons[2]);
            spawnController.DeselectObjectToSpawn();
        }
    }
    private void ClickVehicleFearButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Fear])
        {
            enableButtonOutline(spawnButtons[3], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Fear);
        }
        else
        {
            disableButtonOutline(spawnButtons[3]);
            spawnController.DeselectObjectToSpawn();
        }
    }
    private void ClickVehicleLoveButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Love])
        {
            enableButtonOutline(spawnButtons[4], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Love);
        }
        else
        {
            disableButtonOutline(spawnButtons[4]);
            spawnController.DeselectObjectToSpawn();
        }
    }

    private void enableButtonOutline(Button button, Button[] buttons)
    {
        foreach (Button btn in buttons)
        {
            btn.GetComponent<Outline>().enabled = false;
        }
        button.GetComponent<Outline>().enabled = true;
    }

    private void disableButtonOutline(Button button)
    {
        button.GetComponent<Outline>().enabled = false;
    }


}
