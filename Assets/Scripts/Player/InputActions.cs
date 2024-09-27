using UnityEngine;

public class InputActions : MonoBehaviour
{
    private InputSystem_Actions _inputSystem;

    public Vector2 Movement;
    public bool interact;
    public bool sprint; //vet ikke om dette burde være bool. eller noe annet. spør om hjelp.

    private void Update()
    {
        Movement = _inputSystem.Player.Move.ReadValue<Vector2>();
        interact = _inputSystem.Player.Interact.WasPressedThisFrame();
        sprint = _inputSystem.Player.Sprint.WasPressedThisFrame();
    }

    private void Awake() { _inputSystem = new InputSystem_Actions(); }

    private void OnEnable() { _inputSystem.Enable(); }

    private void OnDisable() { _inputSystem.Disable(); }
}