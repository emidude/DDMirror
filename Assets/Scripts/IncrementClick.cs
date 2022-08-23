using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class IncrementClick : NetworkBehaviour
{
    public PlayerManager PlayerManager;

    [SyncVar]
    public int numberOfClicks = 0;

    public void IncrementClicks()
    {
        NetworkIdentity netowrkIdentity = NetworkClient.connection.identity;
        PlayerManager = netowrkIdentity.GetComponent<PlayerManager>();
        PlayerManager.CmdIncrementClick(gameObject);

    }
}
