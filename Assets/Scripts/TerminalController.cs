using UnityEngine;

public class TerminalController : MonoBehaviour
{
    private InputActions _input;
    public bool playerIsClose;
    public bool terminalIsActivated = false;
    public EventManager eventManager;
    
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
            GetComponent<Renderer>().material.color = Color.green;
            terminalIsActivated = true;
            eventManager.activatedTerminals++;
            print(eventManager.activatedTerminals + " / 3 terminals activated.");
        }
    }

    

}
