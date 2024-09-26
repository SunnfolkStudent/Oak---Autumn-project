using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputActions _input;
    public float moveSpeed = 5f;

    public Rigidbody2D _rigidbody2D;
    
    void Start()
    {
        _input = GetComponent<InputActions>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _input.Movement * moveSpeed;
    }
    
    void Update()
    {
        if (_input.Movement.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(_input.Movement.x), 1);
        }
    }
}