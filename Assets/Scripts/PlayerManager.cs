using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Valve.VR;

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
   
    public GameObject guiObject;
    public SceneHandler SceneHndlr;
    [SyncVar]
    public bool started = false;


    /// </summary>
    //source gameobjects head, left and right controller object of local/host/first player
    [SerializeField] private GameObject localHead;
    [SerializeField] private GameObject localLeftHand;
    [SerializeField] private GameObject localRightHand;

    //prefabs to assign head, left, right controller for Network visbile LAN/client/second player
    [SerializeField] private GameObject networkedHead;
    [SerializeField] private GameObject networkedLeftHand;
    [SerializeField] private GameObject networkedRightHand;

    GameObject theLocalPlayer;
    SteamVR_TrackedObject trackedObjHead, trackedObjLeft, trackedObjRight;
    bool isLinkedToVR;

    GameObject defaultHead, defaultLeftHand, defaultRightHand;


    SteamVR_Behaviour_Pose cL, cR;

    public GameObject cubePf;

    public bool ready = false;

    int resolution = 10;
    GameObject[] points;

    /// <summary>
    /// //
    ///
    /// </summary>


    // Players List to manage playerNumber
    static readonly List<PlayerManager> playersList = new List<PlayerManager>();

    public int PlayerID;
    public bool sharing;

    public override void OnStartServer()
    {
        base.OnStartServer();
        // Add this to the static Players List

        playersList.Add(this);
        Debug.Log("players list, count: " + playersList.Count);
        for (int i = 0; i < playersList.Count;i++)
        {
            Debug.Log(playersList[i].netIdentity);
        }
        this.PlayerID = playersList.Count;
        Debug.Log("this.PlayerID ="+ this.PlayerID);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //SET ORDERING:
        songOrdering = new int[] { 4, 2, 2, 3, 6, 9};
        combinations = new int[] { 2, 4, 1, 3, 0, 0};

        //AUDIO:
        audioObject = GameObject.FindGameObjectWithTag("audioHndlr");
        AudioHandler = audioObject.GetComponent<AudioHandler>();
        //PANELS/////////////////////MISTAEK BELOW IDK?
        guiObject = GameObject.FindGameObjectWithTag("PanelParent");
        SceneHndlr = guiObject.GetComponent<SceneHandler>();


        // find the gaming rig in the scene and link to it
        if (theLocalPlayer == null)
        {
            theLocalPlayer = GameObject.Find("Local VR Rig");// find the rig in the scene
        }


        // now link localHMD, localHands to the Rig so that they are
        // automatically filled when the rig moves
        localHead = Camera.main.gameObject; // get HMD
        //localHead = theLocalPlayer.transform.Find("FolowHead").gameObject;
        //localHead = GameObject.FindWithTag("FolowHead");
        //localHead = theLocalPlayer.transform.Find("MainCamera").gameObject;
        Debug.Log("local head = " + localHead);
        //localLeftHand = theLocalPlayer.transform.Find("LeftHand").gameObject;
        localLeftHand = GameObject.FindWithTag("LeftHand");
        //localRightHand = theLocalPlayer.transform.Find("RightHand").gameObject;
        localRightHand = GameObject.FindWithTag("RightHand");

        //not sure if these tracked are neccary, delete later
        trackedObjRight = localRightHand.GetComponent<SteamVR_TrackedObject>();
        trackedObjLeft = localLeftHand.GetComponent<SteamVR_TrackedObject>();

        cL = localLeftHand.GetComponent<SteamVR_Behaviour_Pose>();
        cR = localRightHand.GetComponent<SteamVR_Behaviour_Pose>();


        //CmdSpawnCubes();
        /*Debug.Log("server active?" + NetworkServer.active);
        Debug.Log("song idx = " + songIndx);
        Debug.Log("song ordering(idx)=" +songOrdering[songIndx]);*/

    }

    void Update()
    {
        //TODO: FIX
        updateHeadAndHands();

        if (isLocalPlayer)
        {
            if (float.IsNaN(cL.GetVelocity().x) || float.IsNaN(cL.GetVelocity().y) || float.IsNaN(cL.GetVelocity().z))
            {
               // Debug.Log("NAN");
            }
            else if (sharing)
            {
                CmdUpdateCubes(cL.GetVelocity(), cR.GetVelocity());
            }
            else
            {
                LocalUpdateCubes(cL.GetVelocity(), cR.GetVelocity());
            }
        }

    }

    [Command]
    void CmdSpawnCubes()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        //transform.position = head.position; <-TODO:  need to fix
        transform.position = Vector3.zero;

        points = new GameObject[resolution * resolution];
        for (int i = 0; i < points.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = scale;
            point.transform.SetParent(transform, false);
            points[i] = point;
            NetworkServer.Spawn(point);
        }
    }

    [Command]
    public void CmdDestroyCubes()
    {
        for (int i = 0; i < points.Length; i++)
        {
            //NetworkServer.UnSpawn(points[i]);
            NetworkServer.Destroy(points[i]);
        }
    }

    public void LocalDestroyCubes()
    {
        for (int i = 0; i < points.Length; i++)
        {
            GameObject.Destroy(points[i]);
        }
    }

    [Command]
    void CmdUpdateCubes(Vector3 vL, Vector3 vR)
    {
        float t = Time.time;
        float step = 2f / resolution;
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;

                //TODO: if only 2 or 1 clients also need additonal automatic update of cubes to compensate for players
                points[i].transform.localPosition = Graphs.SimpleSin(vL, vR, u, v, t) * 5;


            }
        }
    }

    void LocalInstantiateCubes()
    {
        float step = 2f / resolution;
        Vector3 scale = Vector3.one * step;
        //transform.position = head.position; <-TODO:  need to fix
        transform.position = Vector3.zero;

        points = new GameObject[resolution * resolution];
        for (int i = 0; i < points.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = scale;
            point.transform.SetParent(transform, false);
            points[i] = point;
        }
    }

    void LocalUpdateCubes(Vector3 vL, Vector3 vR)
    {
        float t = Time.time;
        float step = 2f / resolution;
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;

                //TODO: if only 2 or 1 clients also need additonal automatic update of cubes to compensate for players
                points[i].transform.localPosition = Graphs.SimpleSin(vL, vR, u, v, t) * 5;


            }
        }
    }

    //TODO: need to fix head and hands not updating
    //https://forum.unity.com/threads/multiplayer-with-steamvr.535321/
    void updateHeadAndHands()
    {

        if (!isLocalPlayer)
        {
            // do nothing, net transform does all the work for us
        }
        else
        {
            // we are the local player.
            // we copy the values from the Rig's HMD
            // and hand positions so they can be
            // used for local positioning
            // prevent headless version of app from crashing
            // depends on SteamVR version if HMD is null or simply won't move
            if (localHead == null)
            {
                localHead = defaultHead;// when running as headless, provide default non-moving objects instead
                localLeftHand = defaultLeftHand;
                localRightHand = defaultRightHand;
                Debug.Log("HEADLESS detected");
            }

            networkedHead.transform.position = localHead.transform.position;
            networkedHead.transform.rotation = localHead.transform.rotation;

            if (localLeftHand) //we need to check in case player left the hand unconnected, should return true if left controller connected
            {
                networkedLeftHand.transform.position = localLeftHand.transform.position;
                networkedLeftHand.transform.rotation = localLeftHand.transform.rotation;
            }
            else
            {
                Debug.Log("left hand not connected");
            }

            if (localRightHand)// only if right hand is connected
            {
                networkedRightHand.transform.position = localRightHand.transform.position;
                networkedRightHand.transform.rotation = localRightHand.transform.rotation;
            }
            else
            {
                Debug.Log("right hand not connected");
            }
        }
    }


    [Command] //client tells server to run this method
    public void CmdNextSong()
    {
        RpcPlaySong();
    }

    [ClientRpc]
    void RpcPlaySong()
    {
        //sync in logger time
        PlayerManager PM = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        AudioHandler AH = PM.AudioHandler;
        
        int songIndex = PM.songOrdering[PM.songIndx];

        Debug.Log("rpc playing song" + AH.soundList[songIndex].name + " index=" + songIndex);
        int songID = AH.soundList[songIndex].ID;
        // string msg = "Syncing " + AudioHandler.soundList[songIndex].name;
        //Logger.Event(msg);

        AH.SetAudioToPlay(songID);
        PM.songIndx++;

    }

    [Command]
    public void CmdClickedSubmit()
    {
        ready = true; //THIS IS ON SERVER SCRIPT
        int numPlayersReady = 0;

        Debug.Log("Clicked Submit. num connections = " + NetworkServer.connections.Count);

        for (int i = 0; i< playersList.Count;i++)
        {
            if (playersList[i].ready)
            {
                Debug.Log("player list " + i + " = " +  playersList[i] + ", is ready? = " + playersList[i].ready);
                numPlayersReady++;
            }
        }
        Debug.Log("num player ready = " + numPlayersReady);
        if(numPlayersReady == NetworkServer.connections.Count)
        {
            RpcResetPlayers();
            numPlayersReady = 0;
            //ready = false; //THIS ONLY GETS CALLED ONCE FROM THE LAST PLAYER WHO WAS READY LAST TIME
            for (int i = 0; i < playersList.Count; i++)
            {
                playersList[i].ready = false;
            }


            Debug.Log("FINALLY EVERYONE READY!!!!!!! (songOrdering[songIndx]=" + songOrdering[songIndx]);
            
            RpcPlaySong();

            RpcSetNetworkedObjects();
        }     
    }

    [ClientRpc]
    void RpcResetPlayers()
    {
        PlayerManager PM = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        SceneHandler SH = PM.SceneHndlr;
        SH.SetCanvasInactive();

        PM.ready = false; //might no longer need
    }

    [ClientRpc]
    void RpcSetNetworkedObjects()
    {
        Debug.Log("RpcSetNetworkedObjects()");
        PlayerManager PM = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        if (PM.PlayerID == combinations[songIndx])
        {
            //this player is the player to dance alone
            PM.sharing = false;
            PM.LocalInstantiateCubes();

        }
        else if (combinations[songIndx] == 0)
        {
            //everyone dancing together
            CmdSpawnCubes();
        }
        else if (combinations[songIndx] == 4)
        {
            //everyone dancing alone
            PM.sharing = false;
            PM.LocalInstantiateCubes();
        }
        else
        {
            Debug.Log("error song index not inclucded, songIndx= " + songIndx);
        }
    }


}
