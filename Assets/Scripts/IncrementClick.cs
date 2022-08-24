using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class IncrementClick : NetworkBehaviour
{
    //public PlayerManager PlayerManager;
    public NetworkingPlayer NetworkingPlayer;

    [SyncVar]
    public int numberOfClicks = 0;

    public void IncrementClicks()
    {   
        Debug.Log("clicking");
        NetworkIdentity netowrkIdentity = NetworkClient.connection.identity;
        /*PlayerManager = netowrkIdentity.GetComponent<PlayerManager>();
        PlayerManager.CmdIncrementClick(gameObject);*/
        NetworkingPlayer = netowrkIdentity.GetComponent<NetworkingPlayer>();
        NetworkingPlayer.CmdIncrementClick(gameObject);

    }
}
