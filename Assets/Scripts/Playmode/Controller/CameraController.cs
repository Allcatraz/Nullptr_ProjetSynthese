using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject playerToFollow;
    private Vector3 offset;

    public GameObject PlayerToFollow
    {
        get { return playerToFollow; }
        set
        {
            playerToFollow = value;
            offset = transform.position - playerToFollow.transform.position;
        }
    }

    private void LateUpdate()
    {
        if (playerToFollow != null)
        transform.position = playerToFollow.transform.position + offset;
    }
}
