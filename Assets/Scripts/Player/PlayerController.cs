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
    public bool playerCanInteractSpaceShuttle = false; //må jo være en bedre måte å gjøre dette på tenkjar eg


    void Start()
    {
        _input = GetComponent<InputActions>();
        //terminals = GameObject.FindGameObjectsWithTag("Terminal");
    }

    bool TestIfSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift)) //TODO: change to input actions system 
        {
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        //stamina managing
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
        if (other.CompareTag("Terminal")) //change to check for interactables layer instead of tag
        {
            //activating and moving pop-up
            float xAxis = other.transform.position.x;
            float yAxis = other.transform.position.y;
            popUp.transform.position = new Vector2(xAxis, yAxis + 1);
            popUp.SetActive(true);
        }

        else if (other.CompareTag("SpaceShuttle"))
        {
            playerCanInteractSpaceShuttle = true;
            popUp.SetActive(true);
            float xAxis = other.transform.position.x;
            float yAxis = other.transform.position.y;
            popUp.transform.position = new Vector2(xAxis, yAxis + 1); //maybe make popup its own function
            popUp.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Terminal"))
        {
            popUp.SetActive(false);
        }
        else if (other.CompareTag("SpaceShuttle"))
        {
            playerCanInteractSpaceShuttle = false; //this seems like ineffective code, look into other options
            popUp.SetActive(false);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerCanInteractSpaceShuttle) //TODO: change to fit input actions system
        {
            if (eventManager.AllTerminalsActive()) //access event manager script for the bool
            {
                print("YOU ESCAPED!");
            }
            else
            {
                print("Activate all the terminals first!");
            }
        }
    }
}