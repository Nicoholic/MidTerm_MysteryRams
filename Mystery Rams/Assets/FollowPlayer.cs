using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform posOfPlayer;
    public float cameraOffset = 20f;

    void Start()
    {
        FindAndSetPlayerTransform();
    }

    void Update()
    {
        if (posOfPlayer != null)
        {
            transform.position = new Vector3(posOfPlayer.position.x, posOfPlayer.position.y + cameraOffset, posOfPlayer.position.z);
            transform.rotation = Quaternion.Euler(90f, posOfPlayer.eulerAngles.y, 0f);
        }
    }
    void FindAndSetPlayerTransform()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            posOfPlayer = playerObject.transform;
        }
    }
}