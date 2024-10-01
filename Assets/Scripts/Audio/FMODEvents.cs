using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("WalkingSound")]
    [field: SerializeField] public EventReference WalkingSound { get; private set; }
    
    [field: Header ("TerminalSound")]
    [field: SerializeField] public EventReference TerminalSound { get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one FMODEvents attached");
        }
        instance = this;
    }
    
}
