using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public PlayerSync playerSync;
    public int[] combinations = new int[12];
    public int[] songOrdering = new int[12];
    private System.Random _random = new System.Random();
    [SerializeField] AudioClip audio;
    //bool readyToStart = false;
    GameObject audioObject;
    AudioHandler AudioHandler;
    int songIndx = 0;
    int numberOfTimesReadyClicked = 0;
    public GameObject guiObject;
    public SceneHandler SceneHandler;
    [SyncVar]
    public bool started = false;
    NetworkIdentity firstNetworkId;
    int test;

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();


        if (isLocalPlayer)
        {
            //firstNetworkId = NetworkClient.connection.identity;
            //started = true;
            Debug.Log("setting firstnetwork id" + NetworkClient.connection.identity.netId +
                " NetworkServer.connections[0].identity=" + NetworkServer.connections[0].identity.netId);
            firstNetworkId = NetworkServer.connections[0].identity;
            test = 5;
        }
            
        else 
        {
            Debug.Log(" NetworkServer.connections.count="+ NetworkServer.connections.Count);
            Debug.Log("LOCAL (REMOTE) NET id" + NetworkClient.connection.identity.netId + " test="+test);
            firstNetworkId = NetworkServer.connections[0].identity;
        }

        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        Debug.Log("NetworkIdentity local" + networkIdentity.netId);

        if (networkIdentity.netId == firstNetworkId.netId) 
        //if (!started)
        {
                Debug.Log("networkIdentity.netId == firstNetworkId.netId");
            //set song ordering for this session
            for (int i = 0; i < 12; i++)
            {
                songOrdering[i] = i;
            }
            //randomise Song order:
            Shuffle(songOrdering);
            Debug.Log("song ordering is :");
            for (int i = 0; i < songOrdering.Length; i++)
            {
                Debug.Log(songOrdering[i]);
            }

            //set Player combinations for 10 songs, each player will do: 4 '3 player' songs, 4 '2 player' songs and 4 '1 player' songs.
            //numerical code for combinations array:
            //0 means all players are dancing together
            //1 => player 1 is dancing alone, player 2 & 3 dancing together
            //2 => p2 alone, 3=> p3 alone,
            //4 => all players dancing alone
            for (int i = 0; i < 4; i++)
            {
                combinations[i] = 0;
            }
            for (int i = 4; i < 6; i++)
            {
                combinations[i] = 1;
            }
            for (int i = 6; i < 8; i++)
            {
                combinations[i] = 2;
            }
            for (int i = 8; i < 10; i++)
            {
                combinations[i] = 3;
            }
            for (int i = 10; i < 12; i++)
            {
                combinations[i] = 4;
            }

            //randomise player combinations 
            Shuffle(combinations);
            Debug.Log("player combinations ordering is :");
            for (int i = 0; i < combinations.Length; i++)
            {
                Debug.Log(combinations[i]);
            }

        }
        else
        {
            //get song and combination ordering from host player
            songOrdering = NetworkServer.connections[0].identity.GetComponent<PlayerManager>().songOrdering;
            combinations = NetworkServer.connections[0].identity.GetComponent<PlayerManager>().combinations;

            Debug.Log("song ordering on remote is :");
            for (int i = 0; i < songOrdering.Length; i++)
            {
                Debug.Log(songOrdering[i]);
            }
            Debug.Log("player combinations remote is :");
            for (int i = 0; i < combinations.Length; i++)
            {
                Debug.Log(combinations[i]);
            }
        }

       
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("called on start client: NetworkServer.connections.count=" + NetworkServer.connections.Count);
        Debug.Log("LOCAL (REMOTE) NET id??" + NetworkClient.connection.identity.netId);
        Debug.Log("on start clinet test =" + test);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        //AUDIO:
        audioObject = GameObject.FindGameObjectWithTag("audioHndlr");
        AudioHandler = audioObject.GetComponent<AudioHandler>();
        //PANELS/////////////////////MISTAEK BELOW IDK?
        guiObject = GameObject.FindGameObjectWithTag("PanelParent");
        SceneHandler = guiObject.GetComponent<SceneHandler>();
    }

    void Shuffle(int[] array)
    {
        int p = array.Length;
        for (int n = p - 1; n > 0; n--)
        {
            int r = _random.Next(0, n + 1);
            int t = array[r];
            array[r] = array[n];
            array[n] = t;
        }
    }

    [Command] //client tells server to run this method
    public void CmdNextSong()
    {
        

        //play next song
        
        RpcPlaySong(songOrdering[songIndx]);
        Debug.Log("song Index = " + songIndx);
        songIndx++;
        

    }

    [ClientRpc]
    void RpcPlaySong(int songIndex)
    {
        //sync in logger time
        if (hasAuthority)
        {
             Debug.Log("rpc playing song" + AudioHandler.soundList[songIndex].name + " index=" +songIndex);
            //int songIndex = songIndx;
            int songID = AudioHandler.soundList[songIndex].ID; 
            string msg = "Syncing " + AudioHandler.soundList[songIndex].name;
            //Logger.Event(msg);
            AudioHandler.SetAudioToPlay(songID);
        }
    }

    [Command]
    public void CmdClickedSubmit()
    {
        //get player indentity of clicker ?

        //set ready
        //this client is ready (the script attached to this local client palyer)
        //in scene handler, getting local client is used to call the script
        //specific to each player to run this code on :

        //readyToStart = true;
        if (isServer)
        {
            numberOfTimesReadyClicked++;
            Debug.Log("numberOfTimesReadyClicked= " + numberOfTimesReadyClicked);
            Debug.Log("NetworkServer.connections.Count= " + NetworkServer.connections.Count);
            if (numberOfTimesReadyClicked >= NetworkServer.connections.Count)
            {
                Debug.Log("about to rpcplaysong");
                //three readys! letsGo!
                RpcPlaySong(songIndx);

                songIndx++;

                //reset counter
                numberOfTimesReadyClicked = 0;
            }
        }
        

        /*if (RpcIsAnyoneNotReady())
        {
            //do not do anything, still waiting for others
        }
        else
        {
            //noone is not ready! lets go!
     
            RpcPlaySong(songIndx);

            songIndx++;

            //turn everyones readyToStarts back to false
            RpcSetNooneReady();
            
        }*/
    }

    /*[ClientRpc]
     bool RpcIsAnyoneNotReady()
     {
         if (hasAuthority) //only correct client
         {
                if (readyToStart == false)
                {
                    return true;
                }
                else return false;
         }

         else return false;
     }*/

    /*[ClientRpc]
    void RpcIsAnyoneNotReady()
    {
        if (readyToStart == true)
        {
        }
    }*/


    /*[ClientRpc]
    void RpcSetNooneReady()
    {
        readyToStart = false;
    }
*/

    /*[Command]
    public void CmdSyncQuiet(string songName)
    {
        if (!isLocalPlayer)
        {
            Debug.Log("not local calling sync");
            return;
        }
        RpcSyncQuiet(songName);
    }*/

        /*
            [ClientRpc]
            void RpcSyncQuiet(string songName)
            {
                string msg = "Syncing " + songName;
                Logger.Event(msg);

            }*/


}
