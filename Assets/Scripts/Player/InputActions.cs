using UnityEngine;

public class InputActions : MonoBehaviour
{
    public InputSystem_Actions _inputSystem;

    public Vector2 Movement;
    public bool interact;
    public bool sprint;

    public void Update()
    {
        Movement = _inputSystem.Player.Move.ReadValue<Vector2>();
        interact = _inputSystem.Player.Interact.WasPressedThisFrame();
        sprint = _inputSystem.Player.Sprint.IsPressed();
    }

    public bool Interact()
    {
        if (_inputSystem.Player.Interact.WasPressedThisFrame())
        {
            return true;
        }

        return false;
    }

    private void Awake() { _inputSystem = new InputSystem_Actions(); }

    private void OnEnable() { _inputSystem.Enable(); }

    private void OnDisable() { _inputSystem.Disable(); }
}