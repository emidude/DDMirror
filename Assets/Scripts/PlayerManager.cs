using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public PlayerSync playerSync;
    public int[] combinations = new int[6];
    public int[] songOrdering = new int[6];
   //private System.Random _random = new System.Random();
    [SerializeField] AudioClip audio;
    //bool readyToStart = false;
    GameObject audioObject;
    AudioHandler AudioHandler;
    int songIndx = 0;
    [SyncVar]
    int numberOfTimesReadyClicked = 0;
    public GameObject guiObject;
    public SceneHandler SceneHandler;
    [SyncVar]
    public bool started = false;
    NetworkIdentity firstNetworkId;
    int test;

   
 /*   public override void OnStartServer()
    {
        base.OnStartServer();


        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        Debug.Log("NetworkIdentity local" + networkIdentity.netId);

  
       
    }*/


  /*  public override void OnStartClient()
    {
        base.OnStartClient();
        *//*Debug.Log("called on start client: NetworkServer.connections.count=" + NetworkServer.connections.Count);
        Debug.Log("LOCAL (REMOTE) NET id??" + NetworkClient.connection.identity.netId);
        Debug.Log("on start clinet test =" + test);*//*
    }*/

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //SET ORDERING:
        songOrdering = new int[] { 1, 0, 2, 3, 6, 9};
        combinations = new int[] { 4, 2, 1, 3, 0, 0};

        //AUDIO:
        audioObject = GameObject.FindGameObjectWithTag("audioHndlr");
        AudioHandler = audioObject.GetComponent<AudioHandler>();
        //PANELS/////////////////////MISTAEK BELOW IDK?
        /*guiObject = GameObject.FindGameObjectWithTag("PanelParent");
        SceneHandler = guiObject.GetComponent<SceneHandler>();*/

       
    }


    [Command] //client tells server to run this method
    public void CmdNextSong()
    {


        //play next song
        Debug.Log("songOrdering[songIndx]=" + songOrdering[songIndx]);
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
        /*if (isServer)
        {*/
            numberOfTimesReadyClicked++;
            //Debug.Log("numberOfTimesReadyClicked= " + numberOfTimesReadyClicked);
            Debug.Log("NetworkServer.connections.Count= " + NetworkServer.connections.Count);
            if (numberOfTimesReadyClicked >= NetworkServer.connections.Count)
            {
                Debug.Log("about to rpcplaysong");
                //three readys! letsGo!
                RpcPlaySong(songOrdering[songIndx]);

                songIndx++;

                //reset counter
                numberOfTimesReadyClicked = 0;
            }
       // }
        
        



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
