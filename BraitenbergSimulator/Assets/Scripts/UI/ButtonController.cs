using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    // Button variables with tooltip values
    [SerializeField] private Button[] spawnButtons = new Button[5];

    [SerializeField] private string[] spawnButtonTooltips = new string[5];

    [SerializeField] private Button[] sceneButtons = new Button[1];

    [SerializeField] private string[] sceneButtonTooltips = new string[1];

    private GameManager gameManager;

    private SpawnController spawnController;


    void Start()
    {
        gameManager = GameManager.Instance;
        spawnController = SpawnController.Instance;

        AttachButtonScripts();
        SetupOnClickListeners();
    }

    private void AttachButtonScripts()
    {
        for (int i = 0; i < spawnButtons.Length; i++)
        {
            spawnButtons[i].gameObject.AddComponent<UIButton>();
            spawnButtons[i].gameObject.GetComponent<UIButton>().SetButtonTooltipText(spawnButtonTooltips[i]);
        }

        for (int i = 0; i < sceneButtons.Length; i++)
        {
            sceneButtons[i].gameObject.AddComponent<UIButton>();
            sceneButtons[i].gameObject.GetComponent<UIButton>().SetButtonTooltipText(sceneButtonTooltips[i]);
        }
    }

    private void SetupOnClickListeners()
    {
        spawnButtons[0].onClick.AddListener(ClickLightButton);
        spawnButtons[1].onClick.AddListener(ClickVehicleAggressionButton);
        spawnButtons[2].onClick.AddListener(ClickVehicleExplorationButton);
        spawnButtons[3].onClick.AddListener(ClickVehicleFearButton);
        spawnButtons[4].onClick.AddListener(ClickVehicleLoveButton);

        sceneButtons[0].onClick.AddListener(ClickOnClearScene);
    }

    private void ClickLightButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Light])
        {
            EnableButtonOutline(spawnButtons[0], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Light);
        }
        else
        {
            DisableButtonOutline(spawnButtons[0]);
            spawnController.DeselectObjectToSpawn();
        }
    }

    private void ClickVehicleAggressionButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Aggression])
        {
            EnableButtonOutline(spawnButtons[1], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Aggression);
        }
        else
        {
            DisableButtonOutline(spawnButtons[1]);
            spawnController.DeselectObjectToSpawn();
        }
    }

    private void ClickVehicleExplorationButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Exploration])
        {
            EnableButtonOutline(spawnButtons[2], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Exploration);
        }
        else
        {
            DisableButtonOutline(spawnButtons[2]);
            spawnController.DeselectObjectToSpawn();
        }
    }
    private void ClickVehicleFearButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Fear])
        {
            EnableButtonOutline(spawnButtons[3], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Fear);
        }
        else
        {
            DisableButtonOutline(spawnButtons[3]);
            spawnController.DeselectObjectToSpawn();
        }
    }
    private void ClickVehicleLoveButton()
    {
        if (spawnController.selectedObjectToSpawn !=
            spawnController.spawnableObjectToGameObject[SpawnableObject.Love])
        {
            EnableButtonOutline(spawnButtons[4], spawnButtons);
            spawnController.SelectObjectToSpawn(SpawnableObject.Love);
        }
        else
        {
            DisableButtonOutline(spawnButtons[4]);
            spawnController.DeselectObjectToSpawn();
        }
    }

    private void EnableButtonOutline(Button button, Button[] buttons)
    {
        foreach (Button btn in buttons)
        {
            btn.GetComponent<Outline>().enabled = false;
        }
        button.GetComponent<Outline>().enabled = true;
    }

    private void DisableButtonOutline(Button button)
    {
        button.GetComponent<Outline>().enabled = false;
    }

    private void ClickOnClearScene()
    {
        gameManager.ClearScene();
    }

}
