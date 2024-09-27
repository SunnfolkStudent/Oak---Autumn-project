using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputActions _input;
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;
    public float currentSpeed;

    public float stamina = 3f;
    public float maxStamina = 3f;
    public float staminaDecreaseRate = 1f;
    public float staminaIncreaseRate = 1f;
    public bool staminaEmpty = false;

    //public bool canSprint = true;

    public Rigidbody2D _rigidbody2D;

    void Start()
    {
        _input = GetComponent<InputActions>();
    }
   
    bool TestIfSprinting()
    {
        //if (!canSprint){return false;}
        if (Input.GetKey(KeyCode.LeftShift)){return true;} //change to fit InputActions system
        else {return false;}
    }

    private void FixedUpdate()
    {
        if (TestIfSprinting() && staminaEmpty == false) 
        {
            currentSpeed = sprintSpeed;
            stamina -= staminaDecreaseRate * Time.deltaTime;
            if (stamina <= 0)
            {
                staminaEmpty = true;
            }
        }
        else
        {
            currentSpeed = walkSpeed;
            if (stamina < maxStamina)
            {
                stamina += staminaIncreaseRate * Time.deltaTime;
            }
            else
            {
                stamina = maxStamina;
                staminaEmpty = false;
            }
        }
        _rigidbody2D.linearVelocity = _input.Movement * currentSpeed;
    }
}