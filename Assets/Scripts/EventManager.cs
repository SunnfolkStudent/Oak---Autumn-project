using UnityEngine;

public class EventManager : MonoBehaviour
{
    public int activatedTerminals;

    public bool AllTerminalsActive()
    {
        if (activatedTerminals == 3)
        {
            return true;
        }
        return false;
    }
}