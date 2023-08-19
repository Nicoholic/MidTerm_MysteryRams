using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] public AudioSource ExplosionSound;
    // Start is called before the first frame update
    void Start()
    {
        ExplosionSound.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
