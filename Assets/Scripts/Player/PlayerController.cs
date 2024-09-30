using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    public EventManager eventManager;
    
    //movement variables
    private InputActions _input;
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
    
    /*public bool playerCanActivateTerminal = false;
    public int terminalsActivated = 0;
    public GameObject currentTerminal;
    private GameObject[] terminals;*/

    //audio
    private EventInstance playerFootsteps;

    public Rigidbody2D _rigidbody2D;

    void Start()
    {
        _input = GetComponent<InputActions>();
        //terminals = GameObject.FindGameObjectsWithTag("Terminal");
        playerFootsteps = AudioManagerController.instance.CreatInstance(FMODEvents.instance.WalkingSound);
    }

    bool TestIfSprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift)) //TODO: change to input actions system 
        {
            return true;
        }
        else
        {
            return false;
        }
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
        
        UpdateSound();
    }

    //interactions
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Terminal"))
        {
            //playerCanActivateTerminal = true;
            //currentTerminal = other.gameObject;
            
            //activating and moving pop-up
            float xAxis = other.transform.position.x;
            float yAxis = other.transform.position.y;
            popUp.transform.position = new Vector2(xAxis, yAxis + 1);
            popUp.SetActive(true);
        }

        else if (other.CompareTag("SpaceShuttle"))
        {
            playerCanInteractSpaceShuttle = true;
            popUp.SetActive(true);float xAxis = other.transform.position.x;
            float yAxis = other.transform.position.y;
            popUp.transform.position = new Vector2(xAxis, yAxis + 1);
            popUp.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Terminal"))
        {
            //playerCanActivateTerminal = false;
            popUp.SetActive(false);
        }
        else if (other.CompareTag("SpaceShuttle"))
        {
            playerCanInteractSpaceShuttle = false;
            popUp.SetActive(false);
        }
    }

    public void Update()
    {
        //terminal system managing
        /*
        if (Input.GetKeyDown(KeyCode.E) && playerCanActivateTerminal) //TODO: change to fit input actions system
        {
            if (terminalsActivated < 3)
            {
                //currentTerminal.GetComponent<Renderer>().material.color = new Color(0, 1, 0.5f);
                terminals[0].GetComponent<Renderer>().material.color = Color.green;
                terminalsActivated++;
                print("terminals activated: " + terminalsActivated + " / 3");
            }
            else
            {
                print("ALL TERMINALS ACTIVATED");
            }
        }*/

        if (Input.GetKeyDown(KeyCode.E) && playerCanInteractSpaceShuttle)
        {
            if (eventManager.allTerminalsActivated)
            {
                print("YOU ESCAPED!");
            }
            else
            {
                print("Activate all the terminals first!");
            }
            
        }
    }

    private void UpdateSound()
    {
        // start footsteps event if the player has an x velocity is on the ground
        if (_rigidbody2D.linearVelocity.x != 0 || _rigidbody2D.linearVelocity.y != 0)
        {
            //get the playback state
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        //otherwis, stop the footsteps event
        else
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}