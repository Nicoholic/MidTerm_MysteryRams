using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IDamage
{
    //This is mostly just a less impressive enemy script.

    [Header("Components")]
    [SerializeField] Renderer model;
    [Range(1, 3)][SerializeField] int HP; //Note that anything higher than 1 broke AI Tracking for me when I added this to my original project.

    bool isdead;

    void Start()
    {
        isdead = false;
    }

    public void TakeDamage(int amount)
    {
        //Damage Function
        HP -= amount;
        StartCoroutine(damageindicator());

        if (HP <= 0 && !isdead)
        {
            isdead = true;
            Destroy(gameObject);
        }
        

    }
    IEnumerator damageindicator()
    {
            model.material.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            model.material.color = Color.white;
    }
}
