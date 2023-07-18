using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableWall : MonoBehaviour, IDamage
{
    [Header("Components")]
    [SerializeField] Renderer model;


    [Header("Stats")]
    [Range(1, 25)][SerializeField] int HP;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int amount)
    {
        //Damage Function
        HP -= amount;
        StartCoroutine(flashdmg());

        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator flashdmg()
    {
        //Enemy Feedback
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
