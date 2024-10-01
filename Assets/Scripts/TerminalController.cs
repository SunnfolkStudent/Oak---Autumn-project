using UnityEngine;
using UnityEngine.UI;

public class TerminalController : MonoBehaviour
{
    public EventManager eventManager;
    /*
    public InputActions inputActions;
    public PlayerController playerController;
    */
    
    public bool playerIsClose;
    public bool terminalIsActivated;
    public Sprite activatedTerminalSprite;

    public GameObject spaceShuttle;
    public Sprite spaceShuttleActivatedSprite;

    public Text terminalCounter;
    
    /*
    void Start()
    {
        inputActions = GetComponent<InputActions>();
    }
    */
    
    private void OnTriggerEnter2D(Collider2D other)//other.transform.position
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)) 
            && playerIsClose && terminalIsActivated == false)
        {
            GetComponent<SpriteRenderer>().sprite = activatedTerminalSprite;
            terminalIsActivated = true;
            eventManager.activatedTerminals++;

            if (eventManager.AllTerminalsActive())
            {
                terminalCounter.text = ("ESCAPE");
                spaceShuttle.GetComponent<SpriteRenderer>().sprite = spaceShuttleActivatedSprite;
            }
            else
            {
                terminalCounter.text = (eventManager.activatedTerminals + " / 3 terminals activated.");
            }
        }
    }
}