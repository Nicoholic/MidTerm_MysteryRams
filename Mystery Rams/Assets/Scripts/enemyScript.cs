using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour, IDamage//, damage script implementation
{

    //Currently doesn't react to physics.

    [Header("Enemy Info")]
    [SerializeField] Renderer model;
    [SerializeField] int HP;
    //Functions that haven't been added:
    //Roaming functionality (to be done with AI)
    //View Angle (to be done with movement)
    //Rotation Speed (to be done with movement)

    //[Weaponry Info]
    //[SerializeField] GameObject bullet;
    //[SerializeField] float firerate

    //Vector3 initSpawn;
    void Start()
    {
        //Code would go here to change objective count if we decide to count enemies.
        //initSpawn = transform.position
    }


    void Update()
    {
        
    }

    public void DamageTaken(int damage) //Needs the damage script to work.
    {
        HP -= damage;
        //Something something code here to move toward player when we implement AI

        if (HP <= 0)
        {
            //Code would go here to change objective count if we decide to count enemies.
            Destroy(gameObject);
        }

        for (int i = 0; i < 3; i++)
        {
            model.material.color = Color.red;
            new WaitForSeconds(0.01f);
            model.material.color = Color.white;
        }
        

    }
}
