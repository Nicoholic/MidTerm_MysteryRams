using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneTXT : MonoBehaviour
{
    public GameObject uiTxt;
    void Start()
    {
        uiTxt = GameManager.instance.gunPickUpGUI;
        uiTxt.SetActive(false);
    }
    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            uiTxt.SetActive(true);
            StartCoroutine("WaitTime");
        }
    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(5);
        Destroy(uiTxt);
        Destroy(gameObject);
    }
}
