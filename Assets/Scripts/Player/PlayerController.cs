using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputActions _input;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    public Rigidbody2D _rigidbody2D;
    
    void Start()
    {
        _input = GetComponent<InputActions>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _input.Movement * walkSpeed;
    }
    
    void Update()
    {
        if (_input.Movement.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(_input.Movement.x), 1);
        }
    }
}