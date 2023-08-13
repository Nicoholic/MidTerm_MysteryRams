using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] int damage;


    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamage>(out var damaged))
        {
            damaged.TakeDamage(damage);

        }
    }
}
