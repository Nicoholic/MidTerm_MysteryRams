using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialroomDelete : MonoBehaviour
{
    public bool destroyed = false;
    // Start is called before the first frame update
    private void Start()
    {
       new WaitForSeconds(5.0f);
       StartCoroutine("DestoryDelay");
    }

    // Update is called once per frame

    IEnumerator DestoryDelay()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(this.gameObject);
        destroyed = true;
    }
}
        
    










