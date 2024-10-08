using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public EventManager eventManager;

    private InputActions _input;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    //movement variables
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;
    public float currentSpeed;

    //animation variables
    private Animator hidingAnimator;

    public delegate void FiringDelegate();

    public FiringDelegate firingMethodPlayerHides;
    public FiringDelegate firingMethodPlayerUnhides;
    public FiringDelegate firingMethodPlayerEscapes;


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
    public bool playerCanInteractHidingSpot;
    public bool playerCanInteractTerminal;

    public GameObject hidingSpot;
    public bool playerIsHiding;

    public bool terminalInteractedWith;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _input = GetComponent<InputActions>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        //InvokeRepeating("ContButtonHandler", 0f, 0.1f);

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

        if (_rigidbody2D.linearVelocity != Vector2.zero)
        {
            _animator.SetFloat(_lastHorizontal, _rigidbody2D.linearVelocity.x);
            _animator.SetFloat(_lastVertical, _rigidbody2D.linearVelocity.y);
        }
    }

    //interactions
    public Vector3 hidingSpotPosition;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GameObject().layer == 11) //sjekk om andre objekt er på interactables layer
        {
            if (other.CompareTag("SpaceShuttle"))
            {
                firingMethodPlayerEscapes = other.GetComponent<SpaceShuttle_AnimatorController>().PlayerEscaped;
                playerCanInteractSpaceShuttle = true;
            }
            else if (other.CompareTag("HidingSpot"))
            {
                firingMethodPlayerHides = other.GetComponent<Teehee>().PlayerHides;
                firingMethodPlayerUnhides = other.GetComponent<Teehee>().PlayerUnhides;
                hidingAnimator = other.GetComponent<Animator>();
                hidingSpotPosition = other.transform.position;
                playerCanInteractHidingSpot = true;
            }
            else if (other.CompareTag("Terminal"))
            {
                playerCanInteractTerminal = true;
            }

            //aktiverer popup
            float xAxis = other.transform.position.x;
            float yAxis = other.transform.position.y;
            popUp.transform.position = new Vector2(xAxis, yAxis + 1.5f);
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
        if (dialogueText.text == dialogue[index])
        {
            print("finished writing");
            contButton.SetActive(true);
            if (_input.interact)
            {
                NextLine();
            }
        }
    }
    

    //space shuttle interaction

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;
    public float wordSpeed;
    public GameObject contButton;
    public GameObject spaceShuttle;

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }


    public void SpaceShuttleInteract()
    {
        if (eventManager.AllTerminalsActive())
        {
            print("YOU ESCAPED!");
            spaceShuttle.transform.position = new Vector3(4.965f, spaceShuttle.transform.position.y, 0);
            firingMethodPlayerEscapes();
            gameObject.SetActive(false);
        }

        else
        {
            print("Activate all the terminals first!");
            if (dialoguePanel.activeInHierarchy)
            {
                ZeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    //hiding spot interaction
    
    public Vector3 entryLocation;
    public Sprite openLocker;
    public Sprite openLockerSide;


    public void HidingSpotInteract()
    {
        if (!playerIsHiding)
        {
            hidingSpot.GetComponent<SpriteRenderer>().sprite = openLocker;
            entryLocation = transform.position;
            transform.position = hidingSpotPosition;
            print("you are now hidden.");
            firingMethodPlayerHides();
            //teehee.PlayerHides();


            gameObject.SetActive(false);
            playerIsHiding = true;
        }
    }
}