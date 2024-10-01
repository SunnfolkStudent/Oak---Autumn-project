using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;
using FMODUnity;

public class PlayerController : MonoBehaviour
{
    public EventManager eventManager;

    //movement variables
    private InputActions _input;
    public Rigidbody2D _rigidbody2D;
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;
    public float currentSpeed;
    
    //animation variables
    private Animator _animator;
    
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";

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
    
    //audio
    private EventInstance playerFootsteps;
    public bool playerCanInteractHidingSpot;
    public bool playerCanInteractTerminal;

    public bool terminalInteractedWith;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        _input = GetComponent<InputActions>();
        playerFootsteps = AudioManagerController.instance.CreateInstance(FMODEvents.instance.WalkingSound);
        //LayerMask interactablesLayer = LayerMask.GetMask("Interactables");
    }

    //Movement (incl. stamina/sprinting and walking animation)
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
        
        //movement
        _rigidbody2D.linearVelocity = _input.Movement * currentSpeed; 
        
        //animation
        _animator.SetFloat(_horizontal, _rigidbody2D.linearVelocity.x);
        _animator.SetFloat(_vertical, _rigidbody2D.linearVelocity.y);

        _rigidbody2D.linearVelocity = _input.Movement * currentSpeed;
        
        if (_rigidbody2D.linearVelocity != Vector2.zero)
        {
            _animator.SetFloat(_lastHorizontal, _rigidbody2D.linearVelocity.x);
            _animator.SetFloat(_lastVertical, _rigidbody2D.linearVelocity.y);
        }
        
    }

    //interactions
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GameObject().layer == 11) //sjekk om andre objekt er på interactables layer
        {
            if (other.CompareTag("SpaceShuttle"))
            {
                playerCanInteractSpaceShuttle = true;
            }
            else if (other.CompareTag("HidingSpot"))
            {
                playerCanInteractHidingSpot = true;
            }
            else if (other.CompareTag("Terminal"))
            {
                playerCanInteractTerminal = true;
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
        if (other.GameObject().layer == 11)
        {
            if (other.CompareTag("SpaceShuttle"))
            {
                playerCanInteractSpaceShuttle = false;
            }
            else if (other.CompareTag("HidingSpot"))
            {
                playerCanInteractHidingSpot = false;
            }
            else if (other.CompareTag("Terminal"))
            {
                playerCanInteractTerminal = false;
            }
            
            popUp.SetActive(false);
        }
    }

    public void Update()
    {
        if (_input.interact && playerCanInteractSpaceShuttle)
        {
            SpaceShuttleInteract();
        }
        else if (_input.interact && playerCanInteractHidingSpot)
        {
            HidingSpotInteract();
        }
        else if (_input.interact && playerCanInteractTerminal)
        {
            terminalInteractedWith = true;
        }
    }

    public void SpaceShuttle_Interact()
    {
        
    }

    public void SpaceShuttleInteract()
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

    public void HidingSpotInteract()
    {
        print("You hid!");
    }
    
    private void UpdateSound()
    {
        RuntimeManager.PlayOneShot("event:/sfx_playerWalk");
    }
}