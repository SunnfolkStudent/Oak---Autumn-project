using UnityEngine;

public class EventManager : MonoBehaviour
{
    public int activatedTerminals;
    public bool allTerminalsActivated = false;

    void Update()
    {
        if (activatedTerminals == 3)
        {
            allTerminalsActivated = true;
        }
    }

    public void TestThing()
    {
        print("hello!");
    }

    /*public bool allTerminalsActive()
    {
        if (activatedTerminals == 3)
        {
            return true;
        }
        return false;
        
    }*/
}
