using UnityEngine;

public class SpaceShuttleActive : MonoBehaviour
{
    public EventManager eventManager;
    public GameObject spaceShuttleActive;
    public bool playerIsEscaping;
    void Update()
    {
        if (eventManager.AllTerminalsActive() && !playerIsEscaping)
        {
            spaceShuttleActive.GetComponent<SpriteRenderer>().enabled = true;
        }
        
    }
}
