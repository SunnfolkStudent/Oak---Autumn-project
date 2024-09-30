using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public EventManager eventManager;

    //movement variables
    private InputActions _input;
    public Rigidbody2D _rigidbody2D;
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;
    public float currentSpeed;

    //stamina variables
    public float stamina = 3f;
    public float maxStamina = 3f;
    public float staminaDecreaseRate = 1f;
    public float staminaIncreaseRate = 1f;
    public bool staminaEmpty = false;

    //UI variables
    public GameObject popUp;

    //interaction variables
    public bool playerCanInteractSpaceShuttle; //må jo være en bedre måte å gjøre dette på tenkjar eg
    public bool playerCanHide;

    void Start()
    {
        _input = GetComponent<InputActions>();
    }

    //Stamina managing
    bool TestIfSprinting()
    {
        if (_input.sprint)
        {
            return true;
        }

        return false;
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
    
    //interactions
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GameObject().layer == 6) //sjekk om andre objekt er på interactables layer
        {
            if (other.CompareTag("SpaceShuttle"))
            {
                playerCanInteractSpaceShuttle = true;
            }
            else if (other.CompareTag("HidingSpot"))
            {
                playerCanHide = true;
            }
            
            //aktiverer popup
            float xAxis = other.transform.position.x;
            float yAxis = other.transform.position.y;
            popUp.transform.position = new Vector2(xAxis, yAxis + 1);
            popUp.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GameObject().layer == 6)
        {
            if (other.CompareTag("SpaceShuttle"))
            {
                playerCanInteractSpaceShuttle = false;
            }

            if (other.CompareTag("HidingSpot"))
            {
                playerCanHide = false;
            }
            popUp.SetActive(false);
        }
    }

    public void Update()
    {
        if (_input.interact && playerCanInteractSpaceShuttle)
        {
            SpaceShuttle_Interact();
        }
        else if (_input.interact && playerCanHide)
        {
            HidingSpot_Interact();
        }
    }

    public void SpaceShuttle_Interact()
    {
        if (eventManager.AllTerminalsActive())
        {
            print("YOU ESCAPED!");
        }
        else
        {
            print("Activate all the terminals first!");
        }
    }

    public void HidingSpot_Interact()
    {
        print("You hid!");
    }
}