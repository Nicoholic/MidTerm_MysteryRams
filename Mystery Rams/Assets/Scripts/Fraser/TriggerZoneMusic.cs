using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneMusic : MonoBehaviour
{
    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.GameMusic.Stop();
            playerMovement.BossMusic.Play();
            playerMovement.BossMusic.loop = true;
        }

    }
    private void Done()
    {

    }
}

