using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class TerminalController : MonoBehaviour
{
    private InputActions _input;
    public bool playerIsClose;
    public bool terminalIsActivated = false;
    public EventManager eventManager;
    public Sprite activatedTerminal_Sprite;

    public Text terminalCounter;

    void Start()
    {
        _input = GetComponent<InputActions>();
    }

    private void OnTriggerEnter2D(Collider2D other)
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
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && terminalIsActivated == false)
        {
            GetComponent<SpriteRenderer>().sprite = activatedTerminal_Sprite;
            terminalIsActivated = true;
            eventManager.activatedTerminals++;
            print(eventManager.activatedTerminals + " / 3 terminals activated.");
            AudioManagerController.instance.PlayOneShot(FMODEvents.instance.TerminalSound, this.transform.position);

            if (eventManager.AllTerminalsActive())
            {
                terminalCounter.text = ("ESCAPE");
            }
            else
            {
                terminalCounter.text = (eventManager.activatedTerminals + " / 3 terminals activated.");
            }
        }
    }
}