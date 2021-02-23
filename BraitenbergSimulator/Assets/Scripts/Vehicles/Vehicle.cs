using UnityEngine;

[System.Serializable]
public class Vehicle : MonoBehaviour
{
    [SerializeField]
    private VehicleType type;

    private VehicleType _type;

    [SerializeField]
    private bool isSelected;

    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private int leftMotorSpeed;

    [SerializeField]
    private int rightMotorSpeed;

    void Start()
    {
        AttachMovementScript();
    }

    void Update()
    {
        UpdateMovementScript();

        if (isSelected)
        {
            // Apply shader
        }
        else
        {
            // Check if shader exists

            // Remove shader
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

    // Attach the rigt movementscript to the vehicle object based on VehicleType
    private void AttachMovementScript()
    {
        _type = type;
        if (type == VehicleType.Default) gameObject.AddComponent<VehicleDefaultMovement>();
        if (type == VehicleType.Agression) gameObject.AddComponent<VehicleMovementAgression>();
        if (type == VehicleType.Exploration) gameObject.AddComponent<VehicleMovementExploration>();
        if (type == VehicleType.Fear) gameObject.AddComponent<VehicleMovementFear>();
        if (type == VehicleType.Love) gameObject.AddComponent<VehicleMovementLove>();
    }

    // Update movementscript if VehicleType has changed
    private void UpdateMovementScript()
    {
        if (type != _type)
        {
            _type = type;
            if (gameObject.GetComponent<VehicleDefaultMovement>() != null)
            {
                Destroy(gameObject.GetComponent<VehicleDefaultMovement>());
                AttachMovementScript();
            }
            if (gameObject.GetComponent<VehicleMovementAgression>() != null)
            {
                Destroy(gameObject.GetComponent<VehicleMovementAgression>());
                AttachMovementScript();
            }
            if (gameObject.GetComponent<VehicleMovementExploration>() != null)
            {
                Destroy(gameObject.GetComponent<VehicleMovementExploration>());
                AttachMovementScript();
            }
            if (gameObject.GetComponent<VehicleMovementFear>() != null)
            {
                Destroy(gameObject.GetComponent<VehicleMovementFear>());
                AttachMovementScript();
            }
            if (gameObject.GetComponent<VehicleMovementLove>() != null)
            {
                Destroy(gameObject.GetComponent<VehicleMovementLove>());
                AttachMovementScript();
            }
        }
    }
}
