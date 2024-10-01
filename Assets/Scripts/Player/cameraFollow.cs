using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    
    //Stalk Player
    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y + 1, -10);
    }
}
