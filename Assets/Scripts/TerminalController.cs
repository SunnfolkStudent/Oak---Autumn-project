using UnityEngine;
using FMODUnity;
using NUnit.Framework.Constraints;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TerminalController : MonoBehaviour
{
    public EventManager eventManager;
    public InputActions _input;

    private bool canEscape = false;
    
    public bool playerIsClose;
    public bool terminalIsActivated;
    public Sprite activatedTerminalSprite;
    public Light2D terminalLight;

    public GameObject spaceShuttle;
    public Sprite spaceShuttleActivatedSprite;

    public Text terminalCounter;

    public StateEnemyAI stateEnemyAI;
    public GameObject o;

    public float holdDuration = 1f;
    public Image loadCircle;

    private float holdTimer = 0;
    private bool isHolding;
    private bool playerInteracts = false;
    
    
    
    void Start()
    {
        terminalLight = GetComponentInChildren<Light2D>();
        o = GameObject.Find("Enemy");
        stateEnemyAI = GameObject.Find("Enemy").GetComponent<StateEnemyAI>();
        //_input = GetComponent<InputActions>();
        _input = GetComponent<InputActions>();
        
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)//other.transform.position
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            float xAxis = transform.position.x;
            float yAxis = transform.position.y;
            loadCircle.transform.position = new Vector2(xAxis + 1, yAxis);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            ResetHold();
        }
    }

    void FixedUpdate()
    {
        if (canEscape != true && eventManager.AllTerminalsActive())
        {
            terminalCounter.text = ("ESCAPE");
            print("ESCAPE");
            spaceShuttle.GetComponent<SpriteRenderer>().sprite = spaceShuttleActivatedSprite;
            stateEnemyAI.CummingForYourAss();
            canEscape = true;
        }
        if (TestIfHolding() && playerIsClose && !terminalIsActivated)
        {
            holdTimer += 0.01f;
            loadCircle.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration && !eventManager.AllTerminalsActive())
            {
                GetComponent<SpriteRenderer>().sprite = activatedTerminalSprite;
                terminalLight.color = Color.cyan;
            
                terminalIsActivated = true;
                eventManager.activatedTerminals++;
                terminalCounter.text = (eventManager.activatedTerminals + " / 3 terminals activated.");
                AudioManagerController.instance.PlayOneShot(FMODEvents.instance.TerminalSound, this.transform.position);
                    
            }

        }
    }

    bool TestIfHolding()
    {
        if (_input.interactHeld)
        {
            return true;
        }

        return false;
    }
    
    void Update()
    {
        
        /*
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)) 
            && playerIsClose && terminalIsActivated == false)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                ResetHold();
            }
            else
            {
                isHolding = true;
                holdTimer += 0.2f;
                loadCircle.fillAmount = holdTimer / holdDuration;
                if (holdTimer >= holdDuration && !eventManager.AllTerminalsActive())
                {
                    GetComponent<SpriteRenderer>().sprite = activatedTerminalSprite;
                    terminalLight.color = Color.cyan;
            
                    terminalIsActivated = true;
                    eventManager.activatedTerminals++;
                    terminalCounter.text = (eventManager.activatedTerminals + " / 3 terminals activated.");
                    AudioManagerController.instance.PlayOneShot(FMODEvents.instance.TerminalSound, this.transform.position);
                    
                }
            }
        }*/
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0;
        loadCircle.fillAmount = 0;
    }
}