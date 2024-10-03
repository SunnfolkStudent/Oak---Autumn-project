using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambiance")]
    [field: SerializeField] public EventReference ambiance { get; private set; }
    
    [field: Header("WalkingSound")]
    [field: SerializeField] public EventReference WalkingSound { get; private set; }
    
    [field: Header ("TerminalSound")]
    [field: SerializeField] public EventReference TerminalSound { get; private set; }
    
    [field: Header ("HideSound")]
    [field: SerializeField] public EventReference HideSound { get; private set; }
    
    [field: Header ("HideBang")]
    [field: SerializeField] public EventReference HideBang { get; private set; }
    
    [field: Header ("Monster")]
    [field: SerializeField] public EventReference MonsterScream { get; private set; }
    
    [field: Header ("MonsterWalk")]
    [field: SerializeField] public EventReference MonsterWalk { get; private set; }
    
    [field: Header ("MonsterSplat")]
    [field: SerializeField] public EventReference MonsterSplat { get; private set; }
    
    [field: Header ("MonsterKnee")]
    [field: SerializeField] public EventReference MonsterKnee { get; private set; }
    
    [field: Header ("MonsterHead")]
    [field: SerializeField] public EventReference MonsterHead { get; private set; }
    
    [field: Header ("MonsterKillScreem")]
    [field: SerializeField] public EventReference MonsterKillScream { get; private set; }
    
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
