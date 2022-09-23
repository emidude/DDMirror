using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Valve.VR;

public class PlayerManagerGraphTesting : NetworkBehaviour
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
    public SceneHandlerGraphTesting SceneHndlr;
    public ContinuousLogger ContinuousLogger;
    public Logger AnswersLogger;


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
    GameObject[] rulerPoints;
    GameObject[] points1, points2, points3;
    GameObject[] points3DTest;
    float step;
    Vector3 scale;

    //public bool bodyShapes = false;
    public bool bodyShapes;
    public bool questionTime = false;

    // Players List to manage playerNumber  
    static readonly List<PlayerManagerGraphTesting> playersList = new List<PlayerManagerGraphTesting>();

  
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
        songOrdering = new int[] { 2, 3, 0, 3, 6, 9};
        //combinations = new int[] { 4, 2, 1, 3, 0, 0};

        //AUDIO:
        audioObject = GameObject.FindGameObjectWithTag("audioHndlr");
        AudioHandler = audioObject.GetComponent<AudioHandler>();
        //LOGGER:
        ContinuousLogger = audioObject.GetComponent<ContinuousLogger>();


        //PANELS/////////////////////MISTAEK BELOW IDK?
        guiObject = GameObject.FindGameObjectWithTag("PanelParent");
        SceneHndlr = guiObject.GetComponent<SceneHandlerGraphTesting>();


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


       
        CmdSetCubesCondition();


        questionTime = false;

        ////////////////////TODO: PUT IN REAL CODE LATER
        ///
        ///////////////////////

        step = 2f / resolution;
        scale = Vector3.one * step;

        Debug.Log("step set to:" + step);
        CmdSpawnCubes();
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
                    CmdUpdateHeadAndHands(localHead.transform.position, localHead.transform.rotation, cL.transform.position, cL.transform.rotation, cR.transform.position, cR.transform.rotation);
                    //CmdUpdateTest(cL.transform.position, cL.transform.rotation);
                }
                else
                {
                    CmdUpdateCubes(localHead.transform.position, localHead.transform.rotation, cL.transform.position, cL.transform.rotation, cR.transform.position, cR.transform.rotation);
                    //CmdUpdateCubes(localHead.transform.position, localHead.transform.rotation, cL.transform.position, cL.transform.rotation, cR.transform.position, cR.transform.rotation,cL.GetVelocity(), cR.GetVelocity(),cL.GetAngularVelocity(),cR.GetAngularVelocity() );
                //    UpdateRulerVals(localHead.transform.position, localHead.transform.rotation, cL.transform.position, cL.transform.rotation, cR.transform.position, cR.transform.rotation);
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
    void CmdSetCubesCondition()
    {
        //Debug.Log("CmdSetCubesCondition()");
        RpcSetCubesCondition();
    }
    [ClientRpc]
    void RpcSetCubesCondition()
    {
        //Debug.Log("RpcSetCubesCondition()");
        PlayerManagerGraphTesting PM = NetworkClient.connection.identity.GetComponent<PlayerManagerGraphTesting>();

        if (PM.ContinuousLogger.condition == "A")
        {
            Debug.Log("ContinuousLogger.condition == A--------------------------------------------------------");
            PM.bodyShapes = false;

        }
        else if (PM.ContinuousLogger.condition == "H")
        {
            Debug.Log("ContinuousLogger.condition == H----------------------------------------------------------");
            PM.bodyShapes = true;
        }
        else if(PM.ContinuousLogger.condition == null)
        {
            Debug.Log("NUUUUUULLLLL----------------------------------------------------");
        }
    }

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
       /* Debug.Log("spoawing cubes");
        //float step = 2f / resolution;
        Debug.Log("step=" + step);*/
        Vector3 scale = Vector3.one * step;
        transform.position = localHead.transform.position;// <-TODO:  need to fix
        //transform.position = Vector3.zero;

       /* points = new GameObject[resolution * resolution];
        for (int i = 0; i < points.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = scale;
            //TODO: SET PARENT TO HEAD
            point.transform.SetParent(transform, false);
            points[i] = point;
            NetworkServer.Spawn(point);
        }*/

        /* rulerPoints = new GameObject[21];
         for (int i = 0; i < 21; i++)
         {
             GameObject point = Instantiate(cubePf);
             point.transform.localScale = scale;
             point.transform.SetParent(transform, false);
             point.transform.position = new Vector3(i-10, 2, 1);
             rulerPoints[i] = point;
             NetworkServer.Spawn(point);
         }*/
        /*if(points == null)
        {
            Debug.Log("points == nulll");
            for(int i = 0; i < points.Length; i++)
            {
                Debug.Log("put in points i x corrd= " + points[i].transform.position.x);
            }
        }*/


        points1 = new GameObject[resolution * resolution];
        for (int i = 0; i < points1.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = scale;
            //TODO: SET PARENT TO HEAD
            point.transform.SetParent(transform, false);
            points1[i] = point;
            NetworkServer.Spawn(point);
        }

        points2 = new GameObject[resolution * resolution];
        for (int i = 0; i < points2.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = scale;
            //TODO: SET PARENT TO HEAD
            point.transform.SetParent(transform, false);
            points2[i] = point;
            NetworkServer.Spawn(point);
        }

        points3 = new GameObject[resolution * resolution];
        for (int i = 0; i < points3.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = scale;
            //TODO: SET PARENT TO HEAD
            point.transform.SetParent(transform, false);
            points3[i] = point;
            NetworkServer.Spawn(point);
        }

        points3DTest = new GameObject[resolution * resolution*resolution];
        for (int i = 0; i < points3.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = scale;
            //TODO: SET PARENT TO HEAD
            point.transform.SetParent(transform, false);
            points3[i] = point;
            NetworkServer.Spawn(point);
        }
    }

    [Command]
    public void CmdDestroyCubes()
    {
        /*for (int i = 0; i < points.Length; i++)
        {
         //   NetworkServer.UnSpawn(points[i]); //leaves visibele but unresponsive
            NetworkServer.Destroy(points[i]);
        }*/

       /* for (int i = 0; i < rulerPoints.Length; i++)
        {
            NetworkServer.Destroy(rulerPoints[i]);
        }*/
        for (int i = 0; i < points1.Length; i++)
        {
            NetworkServer.Destroy(points1[i]);
        }
        for (int i = 0; i < points2.Length; i++)
        {
            NetworkServer.Destroy(points2[i]);
        }
        for (int i = 0; i < points3.Length; i++)
        {
            NetworkServer.Destroy(points3[i]);
        }
        for (int i = 0; i < points3DTest.Length; i++)
        {
            NetworkServer.Destroy(points3[i]);
        }
    }



    [Command]
    //void CmdUpdateCubes(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot, Vector3 vL, Vector3 vR, Vector3 avL, Vector3 avR)
    void CmdUpdateCubes(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot)
    {
        if (points1 == null || points2 == null || points3 == null)
        {
            Debug.Log("points array nullhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh");
        }
        else
        {
            //float t = Time.time;
            //float step = 2f / resolution;
            for (int i = 0, z = 0; z < resolution; z++)
            {
                float v = (z + 0.5f) * step - 1f;
                for (int x = 0; x < resolution; x++, i++)
                {
                    float u = (x + 0.5f) * step - 1f;

                    //TODO: if only 2 or 1 clients also need additonal automatic update of cubes to compensate for players
                    //points[i].transform.localPosition = Graphs.TorusSI( HPos,  HRot,  cLPos, cLRot,  cRPos, cRRot,  u, v,t) * 5;
                    //points[i].transform.localPosition = Graphs.movingFigure8(HPos, HRot, cLPos, cLRot, cRPos, cRRot, u, v, t) * 5;


                    points1[i].transform.localPosition = Graphs.SimpleSymmetric(HPos.x, cLPos.y, cRPos.z, HRot, u, v) * 7;
                    //points1[i].transform.localRotation = HRot;
                    points1[i].transform.rotation = HRot;

                    points2[i].transform.localPosition = Graphs.SimpleSymmetric(cLPos.x, cRPos.y, HPos.z, cLRot, u, v) * 7;
                    //points2[i].transform.localRotation = cLRot;
                    points2[i].transform.rotation = cLRot;

                    points3[i].transform.localPosition = Graphs.SimpleSymmetric(cRPos.x, HPos.y, cLPos.z, cRRot, u, v) * 7;
                    points3[i].transform.localRotation = cRRot;


                    /*//hmmmm very bad
                    points1[i].transform.localPosition = Graphs.SphereSI(HPos.x, cLPos.y, cRPos.z, HRot, u, v);
                    points2[i].transform.localPosition = Graphs.SphereSI(cLPos.x, cRPos.y, HPos.z, cLRot, u, v);
                    points3[i].transform.localPosition = Graphs.SphereSI(cRPos.x, HPos.y, cLPos.z, cRRot, u, v);

                    //aslo quite bad
                    points1[i].transform.localPosition = Graphs.movingFigure8Sym(HPos.x, cLPos.y, cRPos.z, HRot, u, v, 7);
                    points2[i].transform.localPosition = Graphs.movingFigure8Sym(cLPos.x, cRPos.y, HPos.z, cLRot, u, v, 7);
                    points3[i].transform.localPosition = Graphs.movingFigure8Sym(cRPos.x, HPos.y, cLPos.z, cRRot, u, v, 7);*/



                }
            }


            // UpdateRulerVals(HPos, HRot, cLPos, cLRot, cRPos, cRRot);


            /*for (int i = 0, a = 0; a < resolution; a++)
            {
                float v = (a + 0.5f) * step - 1f;
                for (int b = 0; b < resolution; b++)
                {
                    float u = (b + 0.5f) * step - 1f;
                    for (int c = 0; c < resolution; c++, i++)
                    {
                        float w = (c + 0.5f) * step - 1f;
                        points3DTest[i].transform.localPosition = Graph.Sine2DFunctionSI(HPos.x, cLPos.y, cRPos.z, HRot, u, v, w, 7);
                    }
                }

            }*/
        }
    }

    void UpdateRulerVals(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot)
    {
        float[] values = { HPos.x , HPos.y, HPos.z,
            cLPos.x, cLPos.y, cLPos.z,
            cRPos.x, cRPos.y, cRPos.z,
        HRot.x,  HRot.y,  HRot.z,  HRot.w,
        cLRot.x,  cLRot.y,  cLRot.z,  cLRot.w,
        cRRot.x,  cRRot.y,  cRRot.z,  cRRot.w,
        };
            rulerPoints[0].transform.localScale = new Vector3 (0.2f, HPos.x, 0.2f);
            rulerPoints[1].transform.localScale = new Vector3(0.2f, HPos.y, 0.2f);
            rulerPoints[2].transform.localScale = new Vector3(0.2f, HPos.z, 0.2f);

        rulerPoints[3].transform.localScale = new Vector3(0.2f, cLPos.x, 0.2f);
        rulerPoints[4].transform.localScale = new Vector3(0.2f, cLPos.y, 0.2f);
        rulerPoints[5].transform.localScale = new Vector3(0.2f, cLPos.z, 0.2f);

        rulerPoints[6].transform.localScale = new Vector3(0.2f, cRPos.x, 0.2f);
        rulerPoints[7].transform.localScale = new Vector3(0.2f, cRPos.y, 0.2f);
        rulerPoints[8].transform.localScale = new Vector3(0.2f, cRPos.z, 0.2f);

        rulerPoints[9].transform.localScale = new Vector3(0.2f, HRot.x, 0.2f);
        rulerPoints[10].transform.localScale = new Vector3(0.2f, HRot.y, 0.2f);
        rulerPoints[11].transform.localScale = new Vector3(0.2f, HRot.z, 0.2f);
        rulerPoints[12].transform.localScale = new Vector3(0.2f, HRot.w, 0.2f);

        rulerPoints[13].transform.localScale = new Vector3(0.2f, cLRot.x, 0.2f);
        rulerPoints[14].transform.localScale = new Vector3(0.2f, cLRot.y, 0.2f);
        rulerPoints[15].transform.localScale = new Vector3(0.2f, cLRot.z, 0.2f);
        rulerPoints[16].transform.localScale = new Vector3(0.2f, cLRot.w, 0.2f);

        rulerPoints[17].transform.localScale = new Vector3(0.2f, cRRot.x, 0.2f);
        rulerPoints[18].transform.localScale = new Vector3(0.2f, cRRot.y, 0.2f);
        rulerPoints[19].transform.localScale = new Vector3(0.2f, cRRot.z, 0.2f);
        rulerPoints[20].transform.localScale = new Vector3(0.2f, cRRot.w, 0.2f);
    }
        

    //from this thread
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

    [Command]
    void CmdUpdateHeadAndHands(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot)
    { 
            HeadGO.transform.localPosition = HPos;
            HeadGO.transform.rotation = HRot;
        
            LeftHandGO.transform.localPosition = cLPos;
            LeftHandGO.transform.rotation = cLRot;
       
            RightHandGO.transform.position = cRPos;
            RightHandGO.transform.rotation = cRRot;  
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
        PlayerManagerGraphTesting PM = NetworkClient.connection.identity.GetComponent<PlayerManagerGraphTesting>();
        AudioHandler AH = PM.AudioHandler;
        
        int songIndex = PM.songOrdering[PM.songIndx];

        Debug.Log("rpc playing song" + AH.soundList[songIndex].name + " index=" + songIndex);
        int songID = AH.soundList[songIndex].ID;
        // string msg = "Syncing " + AudioHandler.soundList[songIndex].name;
        //Logger.Event(msg);
       // LogSong(AH.soundList[songIndex].name);

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

        PlayerManagerGraphTesting PM = NetworkClient.connection.identity.GetComponent<PlayerManagerGraphTesting>();
        SceneHandlerGraphTesting SH = PM.SceneHndlr;
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

  /*  public void LogSong(string song)
    {
        if (isLocalPlayer)
        {
            CL.songName = song;
            Debug.Log("logged song + " + CL.songName);
        }
    }*/

   


}
