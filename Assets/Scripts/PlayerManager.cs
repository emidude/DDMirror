using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Valve.VR;

public class PlayerManager : NetworkBehaviour
{
    public PlayerSync playerSync;
    //public int[] combinations = new int[6];
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
    GameObject HeadGO, LeftHandGO, RightHandGO;

    SteamVR_Behaviour_Pose cL, cR;

    public GameObject cubePf;

    public bool ready = false;

    int resolution = 10;
    GameObject[] points;

    //public bool bodyShapes = false;
    public bool bodyShapes = true;
    public bool questionTime = true;

    // Players List to manage playerNumber  
    static readonly List<PlayerManager> playersList = new List<PlayerManager>();

    ///test vars:
    /*public GameObject testPF;
    GameObject testGO;*/

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
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        //SET ORDERING:
        songOrdering = new int[] { 2, 2, 2, 3, 6, 9};
        //combinations = new int[] { 4, 2, 1, 3, 0, 0};

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
        /*trackedObjRight = localRightHand.GetComponent<SteamVR_TrackedObject>();
        trackedObjLeft = localLeftHand.GetComponent<SteamVR_TrackedObject>();*/

        cL = localLeftHand.GetComponent<SteamVR_Behaviour_Pose>();
        cR = localRightHand.GetComponent<SteamVR_Behaviour_Pose>();


        CmdSpawnCubes();
        CmdDestroyCubes();
        /*Debug.Log("server active?" + NetworkServer.active);
        Debug.Log("song idx = " + songIndx);
        Debug.Log("song ordering(idx)=" +songOrdering[songIndx]);*/

        CmdSpawnHeadAndHands();
        CmdDestroyHeadAndHands();

        /*CmdSpawnTest();
        CmdDestroyTest();*/
    }

    void Update()
    {

        if (isLocalPlayer)
        {
            if (!questionTime)
            {
                if (float.IsNaN(cL.GetVelocity().x) || float.IsNaN(cL.GetVelocity().y) || float.IsNaN(cL.GetVelocity().z) || float.IsNaN(cR.GetVelocity().x) || float.IsNaN(cR.GetVelocity().y) || float.IsNaN(cR.GetVelocity().z))
                {
                    Debug.Log("NAN");
                }
                else if (bodyShapes)
                {
                    //Debug.Log("updating head and hands");
                    //CmdUpdateHeadAndHands(localHead.transform.position, localHead.transform.rotation, cL.transform.position, cL.transform.rotation, cR.transform.position, cR.transform.rotation);
                    //CmdUpdateTest(cL.transform.position, cL.transform.rotation);
                    UpdateHeadAndHands(localHead.transform.position, localHead.transform.rotation, cL.transform.position, cL.transform.rotation, cR.transform.position, cR.transform.rotation);
                }
                else
                {
                    //Debug.Log("still doing cubes, bodyShapes = " + bodyShapes);
                    CmdUpdateCubes(cL.GetVelocity(), cR.GetVelocity());
                }
            }           
        }
    }
   /* [Command]
    void CmdSpawnTest()
    {
        testGO = Instantiate(testPF);
        testGO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        testGO.transform.SetParent(transform, false);
        NetworkServer.Spawn(testGO);
    }
    [Command]
    public void CmdDestroyTest()
    {
        NetworkServer.Destroy(testGO);
    }
    [Command]
    void CmdUpdateTest(Vector3 pos, Quaternion rot)
    {
        testGO.transform.position = pos;
        testGO.transform.rotation = rot;
    }*/

    [Command]
    void CmdSpawnHeadAndHands()
    {
        Debug.Log("spawning head n hands");
        HeadGO = Instantiate(cubePf);
        HeadGO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        HeadGO.transform.SetParent(transform, false);
        NetworkServer.Spawn(HeadGO);

        LeftHandGO = Instantiate(cubePf);
        LeftHandGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        LeftHandGO.transform.SetParent(transform, false);
        NetworkServer.Spawn(LeftHandGO);

        RightHandGO = Instantiate(cubePf);
        RightHandGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        RightHandGO.transform.SetParent(transform, false);
        NetworkServer.Spawn(RightHandGO);
    }

    [Command]
    public void CmdDestroyHeadAndHands()
    {
        NetworkServer.Destroy(HeadGO);
        NetworkServer.Destroy(LeftHandGO);
        NetworkServer.Destroy(RightHandGO);
    }


    [Command]
    void CmdSpawnCubes()
    {
        Debug.Log("spoawing cubes");
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
        if(points == null)
        {
            Debug.Log("points == nulll");
            for(int i = 0; i < points.Length; i++)
            {
                Debug.Log("put in points i x corrd= " + points[i].transform.position.x);
            }
        }
        
    }

    [Command]
    public void CmdDestroyCubes()
    {
        for (int i = 0; i < points.Length; i++)
        {
         //   NetworkServer.UnSpawn(points[i]); //leaves visibele but unresponsive
            NetworkServer.Destroy(points[i]);
        }
    }

    

    [Command]
    void CmdUpdateCubes(Vector3 vL, Vector3 vR)
    {
        if (points == null)
        {
            Debug.Log("points array nullhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh");
        }
        else
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
        
    }

    //from this thread
    //https://forum.unity.com/threads/multiplayer-with-steamvr.535321/
    /*void updateHeadAndHands()
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
    }*/

[Command]//client tells server to run this method
void CmdUpdateHeadAndHands(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot)
    { 
            HeadGO.transform.localPosition = HPos;
            HeadGO.transform.rotation = HRot;
        
            LeftHandGO.transform.localPosition = cLPos;
            LeftHandGO.transform.rotation = cLRot;
       
            RightHandGO.transform.position = cRPos;
            RightHandGO.transform.rotation = cRRot;  
    }

    
    void UpdateHeadAndHands(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot)
    {
        HeadGO.transform.localPosition = HPos;
        HeadGO.transform.rotation = HRot;

        LeftHandGO.transform.localPosition = cLPos;
        LeftHandGO.transform.rotation = cLRot;

        RightHandGO.transform.position = cRPos;
        RightHandGO.transform.rotation = cRRot;
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
            RpcSetPlayersDanceMode();
            numPlayersReady = 0;
            //ready = false; //THIS ONLY GETS CALLED ONCE FROM THE LAST PLAYER WHO WAS READY LAST TIME
            for (int i = 0; i < playersList.Count; i++)
            {
                playersList[i].ready = false;
            }


            Debug.Log("FINALLY EVERYONE READY!!!!!!! (songOrdering[songIndx]="+songOrdering[songIndx]);
            RpcPlaySong();
        }     
    }

    [ClientRpc]  
    void RpcSetPlayersDanceMode()
    {
        
        PlayerManager PM = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        SceneHandler SH = PM.SceneHndlr;
        SH.SetCanvasInactive();
        PM.questionTime = false;
        PM.ready = false; //might no longer need

        if (PM.bodyShapes)
        {
            PM.CmdSpawnHeadAndHands();
            //PM.CmdSpawnTest();
        }
        else
        {
            PM.CmdSpawnCubes();
        }
    }

   


}
