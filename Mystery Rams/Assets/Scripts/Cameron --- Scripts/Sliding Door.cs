using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour {
    [Header("Options")]
    public float moveDuration = 2.0f;

    [Header("Components")]
    public Transform[] endPositions;    
    public Transform[] objectsToMove;
    [SerializeField] GameObject lockImage;

    [Header("Debug")]
    [SerializeField] Vector3[] startPositions;

    [SerializeField] bool isOpening = false;
    [SerializeField] bool isClosing = false;

    [SerializeField] bool locked = false;

    [SerializeField] float elapsedTime = 0.0f;


    private void Start() {
        startPositions = new Vector3[objectsToMove.Length];

        for (int i = 0; i < objectsToMove.Length; i++)
            startPositions[i] = objectsToMove[i].position;

        lockImage.SetActive(false);
    }

    private void Update() {

        if (!GameManager.instance.isPaused) {
            if (isOpening)
                OpenDoor();

            if (isClosing)
                CloseDoor();
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && !isOpening && !isClosing && !locked)
            StartOpen();
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player") && !isOpening && !isClosing)
            StartClose();
    }

    private void StartOpen() {
        isOpening = true;
        elapsedTime = 0.0f;
    }

    private void StartClose() {
        isClosing = true;
        elapsedTime = 0.0f;
    }

    private void OpenDoor() {
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / moveDuration);

        for (int i = 0; i < objectsToMove.Length; i++) {
            Vector3 targetPosition = endPositions[i].position;
            objectsToMove[i].position = Vector3.Lerp(objectsToMove[i].position, targetPosition, t);
        }

        bool doorsAreOpen = true;
        for (int i = 0; i < objectsToMove.Length; i++) {
            if (Vector3.Distance(objectsToMove[i].position, endPositions[i].position) > 0.001f) {
                doorsAreOpen = false;
                break;
            }
        }

        if (doorsAreOpen) {
            isOpening = false;
        }
    }

    private void CloseDoor() {
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / moveDuration);

        for (int i = 0; i < objectsToMove.Length; i++) {
            Vector3 targetPosition = startPositions[i];
            objectsToMove[i].position = Vector3.Lerp(objectsToMove[i].position, targetPosition, t);
        }

        bool doorsAreClosed = true;
        for (int i = 0; i < objectsToMove.Length; i++) {
            if (Vector3.Distance(objectsToMove[i].position, startPositions[i]) > 0.001f) {
                doorsAreClosed = false;
                break;
            }
        }

        if (doorsAreClosed) {
            isClosing = false;
        }
    }

    public void LockDoor() {
        locked = true;
        lockImage.SetActive(true);
        StartClose();
    }

    public void UnlockDoor() {
        locked = false;
        lockImage.SetActive(false);
    }
}
