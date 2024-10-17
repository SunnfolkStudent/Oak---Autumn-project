using System;
using TMPro;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public int activatedTerminals;
    public PlayerController playerController;
    public GameObject player;
    public Teehee teehee;

    private InputActions _input;

    private void Awake()
    {
        _input = GetComponent<InputActions>();
        if (_input == null)
        {
            Debug.LogError("No input actions found on object '" + this.name + "'.");
        }
    }

    public bool AllTerminalsActive()
    {
        return (activatedTerminals == 3);
    }

#if false
    public void Update()
    {
        if (!playerController.playerIsHiding)
            return;
        if (playerController.playerIsHiding && _input.interact)
        {
            playerController.playerEnabled = true;

            //player.SetActive(true);
            player.transform.position = playerController.entryLocation;
            playerController.playerIsHiding = false;
            print("you are no longer hidden.");
            playerController.firingMethodPlayerUnhides();
        }
    }
#else
    
#endif
}