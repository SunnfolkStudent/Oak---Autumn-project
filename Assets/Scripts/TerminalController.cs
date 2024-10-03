using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TerminalController : MonoBehaviour
{
    public EventManager eventManager;
    
    public InputActions _input;
    //public PlayerController playerController;
    

    public bool playerIsClose;
    public bool terminalIsActivated;
    public Sprite activatedTerminalSprite;
    public Light2D terminalLight;

    public GameObject spaceShuttle;
    public Sprite spaceShuttleActivatedSprite;

    public Text terminalCounter;

    private SpriteRenderer terminalSpriteRenderer;


    void Start()
    {
        terminalSpriteRenderer = GetComponent<SpriteRenderer>();
        terminalLight = GetComponentInChildren<Light2D>();
        _input = GetComponent<InputActions>();
    }


    private void OnTriggerEnter2D(Collider2D other) //other.transform.position
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
        if (_input.interact && playerIsClose && terminalIsActivated == false)
        {
            terminalSpriteRenderer.sprite = activatedTerminalSprite;
            terminalLight.color = Color.cyan;

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