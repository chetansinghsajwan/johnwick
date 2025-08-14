using UnityEngine;
using UnityEngine.InputSystem;

class CharacterWeapon : MonoBehaviour
{
    private Vector2 mouseDelta;
    private InputAction lookAction;

    [SerializeField]
    float xRotation = 0f;

    [SerializeField]
    float yRotation = 0f;

    [SerializeField]
    FiringWeapon _weapon;

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Hide the cursor
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        lookAction = new InputAction(type: InputActionType.Value, binding: "<Mouse>/delta");
        lookAction.Enable();
    }

    private void OnDisable()
    {
        lookAction.Disable();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0f);
        mouseDelta = lookAction.ReadValue<Vector2>();

        float mouseX = mouseDelta.x;
        float mouseY = mouseDelta.y;
        float mouseSensitivity = 20f;

        xRotation = Mathf.Clamp(xRotation - mouseY * Time.deltaTime * mouseSensitivity, -89f, 89f);
        yRotation = (yRotation + mouseX * Time.deltaTime * mouseSensitivity) % 360f;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        if (Keyboard.current.spaceKey.isPressed)
        {
            _weapon.PressTrigger();
        }
    }
}
