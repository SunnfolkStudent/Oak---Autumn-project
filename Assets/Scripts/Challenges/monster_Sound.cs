using UnityEngine;
using FMODUnity;

public class monster_Sound : MonoBehaviour
{
    public void walkeSound()
    {
        RuntimeManager.PlayOneShot("event:/Monster/sfx_monsterWalk");
    }
    
    public void monsterSplat()
    {
        RuntimeManager.PlayOneShot("event:/Monster/MonsterKill/sfx_monsterKillPlayer");
    }
    
    public void monsterKnee()
    {
        RuntimeManager.PlayOneShot("event:/Monster/MonsterKill/sfx_deadHitKnees");
    }
    
    public void monsterHead()
    {
        RuntimeManager.PlayOneShot("event:/Monster/MonsterKill/sfx_deadHitFloor");
    }
    public void monsterKillScream()
    {
        RuntimeManager.PlayOneShot("event:/Monster/MonsterKill/sfx_monsterKillScream");
    }
}
