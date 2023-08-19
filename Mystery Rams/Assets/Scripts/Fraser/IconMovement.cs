using UnityEngine;

public class IconMovement : MonoBehaviour
{
    public Transform posOfPlayer;
    public float iconOffset = 10f;

    void Start()
    {
        FindAndSetPlayerTransform();
    }

    void Update()
    {
        if (posOfPlayer != null)
        {
            transform.position = new Vector3(posOfPlayer.position.x, posOfPlayer.position.y + iconOffset, posOfPlayer.position.z);
            transform.rotation = Quaternion.Euler(90f, posOfPlayer.eulerAngles.y, 0f);
        }
    }
    void FindAndSetPlayerTransform()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("PlayerCamera");
        if (playerObject != null)
        {
            posOfPlayer = playerObject.transform;
        }
    }
}
