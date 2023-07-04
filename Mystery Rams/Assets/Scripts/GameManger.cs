using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    //player 
    public static GameManger instance;

    public GameObject player;
   //UI

    // Start is called before the first frame update
    void Awake()
    {
        instance= this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
