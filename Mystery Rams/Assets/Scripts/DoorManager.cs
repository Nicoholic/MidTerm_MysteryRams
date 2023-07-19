using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] bool isDoor;

    [Header("Vertical Door Settings")]//doors that open vertically
    [SerializeField] float doorSpeed = 1F;
    [SerializeField] float doorRotationAngle = 90F;
    [SerializeField] float openingDirection = 0F;

    [Header("Horizontal Door Settings")]//doors that open horizontally
    [SerializeField] Vector3 horizontalOpeningDirection = Vector3.back;
    [SerializeField] float openingAmount = 2F;


    private Vector3 initialRotation;
    private Vector3 initialPosition;
    private Vector3 forward;

    private Coroutine doorAnimation;

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation.eulerAngles;
        forward = transform.right;
        initialPosition = transform.position;
    }

    public void Open(Vector3 playerPosition)
    {

        if (!isOpen)
        {

            if (doorAnimation != null)
            {

                StopCoroutine(doorAnimation);

            }
            if (isDoor)
            {
                float dotProduct = Vector3.Dot(forward, (playerPosition - transform.position).normalized);
                doorAnimation = StartCoroutine(OpenDoor(dotProduct));

            }
            else 
            {

                doorAnimation = StartCoroutine(OpenHorizontalDoor());
            
            }

        }

    }

    private IEnumerator OpenDoor(float forwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (forwardAmount >= openingDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, initialRotation.y - doorRotationAngle, 0));

        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, initialRotation.y + doorRotationAngle, 0));

        }

        isOpen = true;
        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * doorSpeed;
        }

    }

    private IEnumerator OpenHorizontalDoor() 
    {
        Vector3 endPosition = initialPosition + openingAmount * horizontalOpeningDirection;
        Vector3 startPosition = transform.position;

        float time = 0;
        isOpen = true;
        while (time < 1) 
        {
        transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * doorSpeed;
        
        }
    
    }

    public void Close() 
    {
        if (isOpen) 
        {
        if(doorAnimation != null) 
            {
                StopCoroutine(doorAnimation);
            
            }
        if(isDoor) 
            {
                doorAnimation = StartCoroutine(CloseDoor());
           
            }
            else 
            {

                doorAnimation = StartCoroutine(CloseHorizontalDoor());
            
            }
        
        }

    }

    private IEnumerator CloseDoor() 
    {

        Quaternion startRotation_ = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(initialRotation);

        isOpen = false;
        float time = 0;
        while (time < 1)
        {

            transform.rotation = Quaternion.Slerp(startRotation_, endRotation, time);
            yield return null;
            time += Time.deltaTime * doorSpeed;

        }

    }

    private IEnumerator CloseHorizontalDoor() 
    {
        Vector3 endPosition = initialPosition;
        Vector3 startPosition = transform.position;
        float time = 0;
        isOpen = false;
        while (time < 1) 
        {
        transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * doorSpeed;

        
        }
    
    }

}
