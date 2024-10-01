using UnityEngine;

public class EventManager : MonoBehaviour
{
    public int activatedTerminals;
    public PlayerController playerController;
    public GameObject player;

    public bool AllTerminalsActive()
    {
        if (activatedTerminals == 3)
        {
            return true;
        }
        return false;
    }

    public void Update()
    {
        if (!playerController.playerIsHiding)
            return;
        if (playerController.playerIsHiding && Input.GetKeyDown(KeyCode.E))
        {
            player.SetActive(true);
            player.transform.position = playerController.entryLocation;
            playerController.playerIsHiding = false;
            print("you are no longer hidden.");
        }
    }
}