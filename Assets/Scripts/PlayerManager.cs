using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Valve.VR;


public class PlayerManager : NetworkBehaviour
{
    public PlayerSync playerSync;

    public int[] songOrdering = { 1, 8, 2 };

    [SerializeField] AudioClip audio;
    //bool readyToStart = false;
    GameObject audioObject;
    AudioHandler AudioHandler;
    int songIndx = 0;
   
    public GameObject guiObject;
    public SceneHandler SceneHndlr;
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
    /*SteamVR_TrackedObject trackedObjHead, trackedObjLeft, trackedObjRight;
    bool isLinkedToVR;*/

    GameObject defaultHead, defaultLeftHand, defaultRightHand;
    GameObject HeadGO, LeftHandGO, RightHandGO;

    SteamVR_Behaviour_Pose cL, cR;

    public GameObject cubePf;

    public bool ready = false;

    int resolution = 10;
    public GameObject[] points1, points2, points3;
    float step;
    Vector3 scale;

    public bool bodyShapes;
    public bool questionTime = true;

    // Players List to manage playerNumber  
    static readonly List<PlayerManager> playersList = new List<PlayerManager>();

    public GameObject[] vertices1Pf, vertices2Pf, vertices3Pf;
    bool hypercubeRotations = true;
   
    public GameObject Testpf1, TestPf2, TestPf3;

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

        
        /*//SET ORDERING: <- NO DONE ABOVE
        songOrdering = new int[] { 4, 1, 2, 3, 6, 9};
        //combinations = new int[] { 4, 2, 1, 3, 0, 0};*/

        //AUDIO:
        audioObject = GameObject.FindGameObjectWithTag("audioHndlr");
        AudioHandler = audioObject.GetComponent<AudioHandler>();
        //LOGGER:
        ContinuousLogger = audioObject.GetComponent<ContinuousLogger>();


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


        step = 2f / resolution;
        scale = Vector3.one * 0.2f;

        CmdSpawnCubes();
        //CmdUpdateCubes(cL.GetVelocity(),cR.GetVelocity());
        CmdDestroyCubes();

        CmdSpawnHeadAndHands();
        CmdDestroyHeadAndHands();

       
       
        CmdSetCubesCondition();


    }

    void Update()
    {

        if (isLocalPlayer)
        {
            if (!questionTime)
            {
                if (float.IsNaN(localHead.transform.position.x) || float.IsNaN(localHead.transform.position.y) || float.IsNaN(localHead.transform.position.z) || float.IsNaN(cL.transform.position.x) || float.IsNaN(cL.transform.position.y) || float.IsNaN(cL.transform.position.z) || float.IsNaN(cR.transform.position.x) || float.IsNaN(cR.transform.position.y) || float.IsNaN(cR.transform.position.z)
                    || float.IsInfinity(localHead.transform.position.x) || float.IsInfinity(localHead.transform.position.y) || float.IsInfinity(localHead.transform.position.z)
                    || float.IsInfinity(cL.transform.position.x) || float.IsInfinity(cL.transform.position.y) || float.IsInfinity(cL.transform.position.z)
                    || float.IsInfinity(cR.transform.position.x) || float.IsInfinity(cR.transform.position.y) || float.IsInfinity(cR.transform.position.z)
                    || float.IsInfinity(localHead.transform.rotation.x) || float.IsInfinity(localHead.transform.rotation.y) || float.IsInfinity(localHead.transform.rotation.z) || float.IsInfinity(localHead.transform.rotation.w)
                    || float.IsInfinity(cL.transform.rotation.x) || float.IsInfinity(cL.transform.rotation.y) || float.IsInfinity(cL.transform.rotation.z) || float.IsInfinity(cL.transform.rotation.w)
                    || float.IsInfinity(cR.transform.rotation.x) || float.IsInfinity(cR.transform.rotation.y) || float.IsInfinity(cR.transform.rotation.z) || float.IsInfinity(cR.transform.rotation.w))
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
                    //CmdUpdateCubes(localHead.transform.position, localHead.transform.rotation, cL.transform.position, cL.transform.rotation, cR.transform.position, cR.transform.rotation,cL.GetVelocity(), cR.GetVelocity(),cL.GetAngularVelocity(),cR.GetAngularVelocity() );
                    CmdUpdateCubes(localHead.transform.position, localHead.transform.rotation, cL.transform.position, cL.transform.rotation, cR.transform.position, cR.transform.rotation);

                }
            }           
        }
    }
    [Command]
    void CmdSpawnTest()
    {
        vertices1Pf = new GameObject[16];
        for (int i = 0; i < 16; i++)
        {
            //red
            GameObject v1 = Instantiate(Testpf1);
            v1.transform.localScale = Vector3.one * 0.2f;
            vertices1Pf[i] = v1;
            NetworkServer.Spawn(v1);
            //blue
            GameObject v2 = Instantiate(TestPf2);
            v2.transform.localScale = Vector3.one * 0.2f;
            vertices2Pf[i] = v2;
            NetworkServer.Spawn(v2);
            //green
            GameObject v3 = Instantiate(TestPf3);
            v3.transform.localScale = Vector3.one * 0.2f;
            vertices3Pf[i] = v3;
            NetworkServer.Spawn(v3);
        }
       
       
    }
    [Command]
    public void CmdDestroyTest()
    {
        for (int i = 0; i < vertices1Pf.Length; i++)
        {
            NetworkServer.Destroy(vertices1Pf[i]);
            NetworkServer.Destroy(vertices2Pf[i]);
            NetworkServer.Destroy(vertices3Pf[i]);

        }
    }
    

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
        PlayerManager PM = NetworkClient.connection.identity.GetComponent<PlayerManager>();

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
        Debug.Log("spoawing cubesjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjj");
       
        //transform.position = head.position; <-TODO:  need to fix
        transform.position = Vector3.zero;
        scale = Vector3.one * 0.2f;
        points1 = new GameObject[resolution * resolution];
        Debug.Log("points1.Length=" + points1.Length);
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

        //if (hypercubeRotations)
        //{
            // _vertices = new Vector3[UtilsGeom4D.kTesseractPoints.Length];
            vertices1Pf = new GameObject[16];
            vertices2Pf = new GameObject[16];
            vertices3Pf = new GameObject[16];


            for (int i = 0; i < vertices1Pf.Length; i++)
            {
                GameObject v1 = Instantiate(cubePf);
                v1.transform.localScale = scale;
                GameObject v2 = Instantiate(cubePf);
                v2.transform.localScale = scale;
                GameObject v3 = Instantiate(cubePf);
                v3.transform.localScale = scale;
                vertices1Pf[i] = v1;
                vertices2Pf[i] = v2;
                vertices3Pf[i] = v3;
                NetworkServer.Spawn(v1);
                NetworkServer.Spawn(v2);
                NetworkServer.Spawn(v3);

            }
        //}

    }


    [Command]
    public void CmdDestroyCubes()
    {
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

        if (hypercubeRotations)
        {
            for (int i = 0; i < vertices1Pf.Length; i++)
            {
                NetworkServer.Destroy(vertices1Pf[i]);
                NetworkServer.Destroy(vertices2Pf[i]);
                NetworkServer.Destroy(vertices3Pf[i]);
            }
        }
    }

    [Command]
    //void CmdUpdateCubes(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot, Vector3 vL, Vector3 vR, Vector3 avL, Vector3 avR)
    void CmdUpdateCubes(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot)
    {
        if (points1 == null || points2 == null || points3 == null )
        {
            Debug.Log("points array nullhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh");
        }
        else
        {       
            if (hypercubeRotations)
            {                
                Vector3 cL_Deg = cLRot.eulerAngles * Mathf.Deg2Rad ;
                Vector3 cR_Deg = cRRot.eulerAngles * Mathf.Deg2Rad ;
                Vector3 H_Deg = HRot.eulerAngles * Mathf.Deg2Rad ;

               
                for (int i= 0; i< 16; i++)
                {
                    /*vertices1Pf[i].transform.position = Hypercube.UpdateVertices(cLPos.x, cLPos.y, cLPos.z, cR_Deg.x, cR_Deg.y, cR_Deg.z, 1.5f, 2, i);
                    vertices2Pf[i].transform.position = Hypercube.UpdateVertices(H_Deg.x, H_Deg.y, H_Deg.z, cRPos.x, cRPos.y, cRPos.z, 1.5f, 2, i);
                    vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 1.5f, 2, i);*/

                    /*vertices1Pf[i].transform.position = Hypercube.UpdateVertices(cLPos.x, cLPos.y, cLPos.z, cR_Deg.x, cR_Deg.y, cR_Deg.z, 1, 2, i);
                    vertices2Pf[i].transform.position = Hypercube.UpdateVertices(H_Deg.x, H_Deg.y, H_Deg.z, cRPos.x, cRPos.y, cRPos.z, 1, 2, i);
                    vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 1, 2, i);*/

                    /*vertices1Pf[i].transform.position = Hypercube.UpdateVertices(cLPos.x, cLPos.y, cLPos.z, cR_Deg.x, cR_Deg.y, cR_Deg.z, 4, 5, i);
                    vertices2Pf[i].transform.position = Hypercube.UpdateVertices(H_Deg.x, H_Deg.y, H_Deg.z, cRPos.x, cRPos.y, cRPos.z, 4, 5, i);
                    vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 4, 5, i);*/

                    /* vertices1Pf[i].transform.position = Hypercube.UpdateVertices(HRot.x, HRot.y, cLRot.x, cLRot.y, cLRot.z, cLRot.w, 0, 1, i);
                     vertices2Pf[i].transform.position = Hypercube.UpdateVertices(HRot.z, HRot.w, cRRot.x, cRRot.y, cRRot.z, cRRot.w, 0, 1, i);
                     vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 0, 1, i);
 */
                    vertices1Pf[i].transform.position = Hypercube.UpdateVertices(HRot.x, HRot.y, cLRot.x, cLRot.y, cLRot.z, cLRot.w, 1, 2, i);
                    vertices2Pf[i].transform.position = Hypercube.UpdateVertices(HRot.z, HRot.w, cRRot.x, cRRot.y, cRRot.z, cRRot.w, 1, 2, i);
                    vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 1, 2, i);


                }


                //UpdateSimpleSinPoints(cLPos,cRPos,HPos);
                float distArmsApart = Vector3.Distance(cLPos, cRPos) * 2f + 3;
                /*float t = Time.time;
                float step = 2f / resolution;*/
                for (int i = 0, z = 0; z < resolution; z++)
                {
                    float v = ((z + 0.5f) * step - 1f) * 3;
                    for (int x = 0; x < resolution; x++, i++)
                    {
                        float u = ((x + 0.5f) * step - 1f) * 3;


                        points1[i].transform.position = Graphs.SimpleSymmetric(HPos.x, cLPos.y, cRPos.z, u, v) * distArmsApart;
                        distArmsApart = Vector3.Distance(HPos, cLPos) * 2 + 3;
                        points2[i].transform.position = Graphs.SimpleSymmetric(cLPos.x / distArmsApart, cRPos.y / distArmsApart, HPos.z / distArmsApart, u, v) * distArmsApart;
                        distArmsApart = Vector3.Distance(HPos, cRPos) * 2 + 3;
                        points3[i].transform.position = Graphs.SimpleSymmetric(cRPos.x / distArmsApart, HPos.y / distArmsApart, cLPos.z / distArmsApart, u, v) * distArmsApart;

                        /* points1[i].transform.localPosition = Graphs.SimpleSymmetric(HPos.x , cLPos.y , cRPos.z , u, v) ;
                         distArmsApart = Vector3.Distance(HPos, cLPos) * 2 + 3;
                         points2[i].transform.localPosition = Graphs.SimpleSymmetric(cLPos.x , cRPos.y , HPos.z , u, v) ;
                         distArmsApart = Vector3.Distance(HPos, cRPos) * 2 + 3;
                         points3[i].transform.localPosition = Graphs.SimpleSymmetric(cRPos.x / distArmsApart, HPos.y / distArmsApart, cLPos.z / distArmsApart, u, v) ;*/


                    }
                }

                // UpdateTorusPoints(cLPos, cRPos, HPos, HRot, cLRot, cRRot, 8);

                /* for (int i = 0, z = 0; z < resolution; z++)
                 {
                     float v = (z + 0.5f) * step - 1f;
                     for (int x = 0; x < resolution; x++, i++)
                     {
                         float u = (x + 0.5f) * step - 1f;

                         float m = 8;
                         // points1[i].transform.localPosition = Graphs.SphereSI(HPos.z, cLPos.x, cRPos.y,  u, v) * 5;
                         float dist = Vector3.Distance(cLPos, cRPos);
                         //points1[i].transform.localPosition = Graphs.TorusSI2(dist, HPos.x/dist, HPos.y/dist, HPos.z/dist, HRot.x/dist, HRot.y/dist, HRot.z/dist, HRot.w/dist, u, v) * 10;
                         //points1[i].transform.localPosition = Graphs.TorusSI2(dist, HPos.x / m, HPos.y / m, HPos.z / m, HRot.x/m, HRot.y/m, HRot.z/m, HRot.w/m, u, v) * m*2;
                         dist = Vector3.Distance(HPos, cRPos);
                         //points2[i].transform.localPosition = Graphs.TorusSI2(dist, cLPos.x/dist, cLPos.y/dist, cLPos.z/dist, cLRot.x, cLRot.y, cLRot.z, cLRot.w, u, v) * 5;
                         points2[i].transform.localPosition = Graphs.TorusSI2(dist, cLPos.x / m, cLPos.y / m, cLPos.z / m, cLRot.x / m, cLRot.y / m, cLRot.z / m, cLRot.w / m, u, v) * m;

                         dist = Vector3.Distance(HPos, cLPos);
                         points3[i].transform.localPosition = Graphs.TorusSI2(dist, cRPos.x / m, cRPos.y / m, cRPos.z / m, cRRot.x / m, cRRot.y / m, cRRot.z / m, cRRot.w / m, u, v) * m;


                     }
                 }*/

                /*for (int i = 0, z = 0; z < resolution; z++)
                {
                    float v = (z + 0.5f) * step - 1f;
                    for (int x = 0; x < resolution; x++, i++)
                    {
                        float u = (x + 0.5f) * step - 1f;

                        float m = 8;
                         points1[i].transform.localPosition = Graphs.SphereSI(HPos.z, cLPos.x, cRPos.y,  u, v) * 5;
                        points2[i].transform.localPosition = Graphs.SphereSI(cLPos.z, cRPos.x, HPos.y, u, v) * 5;
                        points3[i].transform.localPosition = Graphs.SphereSI(cRPos.z, HPos.x, cLPos.y, u, v) * 5;
                       

                    }
                }*/
            }
            else
            {
                for (int i = 0, z = 0; z < resolution; z++)
                {
                    float v = (z + 0.5f) * step - 1f;
                    for (int x = 0; x < resolution; x++, i++)
                    {
                        float u = (x + 0.5f) * step - 1f;

                        float m = 5;
                        //points1[i].transform.localPosition = Graphs.TorusSI( HPos,  HRot,  cLPos, cLRot,  cRPos, cRRot,  vL,  vR,  avL,  avR, u, v,t) * 5;
                        float dist = Vector3.Distance(cLPos, cRPos);
                        //points1[i].transform.localPosition = Graphs.TorusSI2(dist, HPos.x/dist, HPos.y/dist, HPos.z/dist, HRot.x, HRot.y, HRot.z, HRot.w, u, v) * 5;
                        points1[i].transform.localPosition = Graphs.TorusSI2(dist, HPos.x / m, HPos.y / m, HPos.z / m, HRot.x, HRot.y, HRot.z, HRot.w, u, v) * m;
                        dist = Vector3.Distance(HPos, cRPos);
                        //points2[i].transform.localPosition = Graphs.TorusSI2(dist, cLPos.x/dist, cLPos.y/dist, cLPos.z/dist, cLRot.x, cLRot.y, cLRot.z, cLRot.w, u, v) * 5;
                        points2[i].transform.localPosition = Graphs.TorusSI2(dist, cLPos.x / m, cLPos.y / m, cLPos.z / m, cLRot.x, cLRot.y, cLRot.z, cLRot.w, u, v) * m;

                        dist = Vector3.Distance(HPos, cLPos);
                        points3[i].transform.localPosition = Graphs.TorusSI2(dist, cRPos.x / m, cRPos.y / m, cRPos.z / m, cRRot.x, cRRot.y, cRRot.z, cRRot.w, u, v) * m;
                    }
                }
            }
            

            
            
        }
        
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
        PlayerManager PM = NetworkClient.connection.identity.GetComponent<PlayerManager>();
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
            Debug.Log("spawning cubes");
             PM.CmdSpawnCubes();
            //PM.CmdSpawnTest();
        }
    }

    void UpdateSimpleSinPoints(Vector3 cLPos, Vector3 cRPos, Vector3 HPos)
    {
        float distArmsApart = Vector3.Distance(cLPos, cRPos)*2f + 3;
        /*float t = Time.time;
        float step = 2f / resolution;*/
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = ((z + 0.5f) * step - 1f)*3;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = ((x + 0.5f) * step - 1f)*3;


                points1[i].transform.localPosition = Graphs.SimpleSymmetric(HPos.x, cLPos.y , cRPos.z , u, v) * distArmsApart;
                distArmsApart = Vector3.Distance(HPos, cLPos) * 2 + 3;
                points2[i].transform.localPosition = Graphs.SimpleSymmetric(cLPos.x / distArmsApart, cRPos.y / distArmsApart, HPos.z / distArmsApart, u, v) * distArmsApart;
                distArmsApart = Vector3.Distance(HPos, cRPos) * 2 + 3;
                points3[i].transform.localPosition = Graphs.SimpleSymmetric(cRPos.x / distArmsApart, HPos.y / distArmsApart, cLPos.z / distArmsApart, u, v) * distArmsApart;

                /* points1[i].transform.localPosition = Graphs.SimpleSymmetric(HPos.x , cLPos.y , cRPos.z , u, v) ;
                 distArmsApart = Vector3.Distance(HPos, cLPos) * 2 + 3;
                 points2[i].transform.localPosition = Graphs.SimpleSymmetric(cLPos.x , cRPos.y , HPos.z , u, v) ;
                 distArmsApart = Vector3.Distance(HPos, cRPos) * 2 + 3;
                 points3[i].transform.localPosition = Graphs.SimpleSymmetric(cRPos.x / distArmsApart, HPos.y / distArmsApart, cLPos.z / distArmsApart, u, v) ;*/


            }
        }
    }

    void UpdateTorusPoints(Vector3 cLPos, Vector3 cRPos, Vector3 HPos, Quaternion HRot, Quaternion cLRot, Quaternion cRRot, float m)
    {
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;

                //points1[i].transform.localPosition = Graphs.TorusSI( HPos,  HRot,  cLPos, cLRot,  cRPos, cRRot,  vL,  vR,  avL,  avR, u, v,t) * 5;
                float dist = Vector3.Distance(cLPos, cRPos);
                //points1[i].transform.localPosition = Graphs.TorusSI2(dist, HPos.x/dist, HPos.y/dist, HPos.z/dist, HRot.x, HRot.y, HRot.z, HRot.w, u, v) * 5;
                points1[i].transform.localPosition = Graphs.TorusSI2(dist, HPos.x / m, HPos.y / m, HPos.z / m, HRot.x/m, HRot.y/m, HRot.z/m, HRot.w/m, u, v) * m;
                dist = Vector3.Distance(HPos, cRPos);
                //points2[i].transform.localPosition = Graphs.TorusSI2(dist, cLPos.x/dist, cLPos.y/dist, cLPos.z/dist, cLRot.x, cLRot.y, cLRot.z, cLRot.w, u, v) * 5;
                points2[i].transform.localPosition = Graphs.TorusSI2(dist, cLPos.x / m, cLPos.y / m, cLPos.z / m, cLRot.x/m, cLRot.y/m, cLRot.z/m, cLRot.w/m, u, v) * m;

                dist = Vector3.Distance(HPos, cLPos);
                points3[i].transform.localPosition = Graphs.TorusSI2(dist, cRPos.x / m, cRPos.y / m, cRPos.z / m, cRRot.x/m, cRRot.y/m, cRRot.z/m, cRRot.w/m, u, v) * m;
            }
        }        
    }

    

    /* void UpdateCubes4d(Quaternion q1, Quaternion q2, float ddd, Transform viewPoint)
     {
         *//*viewPoint = localHead.transform;
         viewPoint.position *= ddd;*/
    /*rotationXW = cL.transform.rotation.eulerAngles.x;
    rotationXY = cL.transform.rotation.eulerAngles.y;
    rotationYW = cL.transform.rotation.eulerAngles.z;
    rotationYZ = cR.transform.rotation.eulerAngles.x;
    rotationZW = cR.transform.rotation.eulerAngles.y;
    rotationZX = cR.transform.rotation.eulerAngles.z;*//*
    float rotationXW = q1.eulerAngles.x * ddd;
    float rotationXY = q1.eulerAngles.y * ddd;
    float rotationYW = q1.eulerAngles.z * ddd;
    float rotationYZ = q2.eulerAngles.x * ddd;
    float rotationZW = q2.eulerAngles.y * ddd;
    float rotationZX = q2.eulerAngles.z * ddd;
    GenerateVertices(_vertices, rotationXY, rotationYZ, rotationZX, rotationXW, rotationYW, rotationZW, viewPoint);      
}*/
    /*void Update4DPoints(Quaternion cLRot, Quaternion cRRot, Quaternion HRot, float scaling, Transform headT, Transform LT, Transform RT)
    {
        
        UpdateCubes4d(cLRot, cLRot, scaling, headT);
        for (int a = 0; a < vertices1Pf.Length; a++)
        {
            vertices1Pf[a].transform.position = _vertices[a] * scaling;
        }
        UpdateCubes4d(cRRot, HRot, scaling, LT);
        for (int a = 0; a < vertices2Pf.Length; a++)
        {
            vertices2Pf[a].transform.position = _vertices[a] * scaling;
        }
        UpdateCubes4d(cLRot, HRot, scaling, RT);
        for (int a = 0; a < vertices3Pf.Length; a++)
        {
            vertices3Pf[a].transform.position = _vertices[a] * scaling;
        }
    }*/

    /* public void GenerateVertices(Vector3[] vertices, float rotationXY, float rotationYZ, float rotationZX, float rotationXW, float rotationYW, float rotationZW , Transform viewPoint )
     {
         // setup rotations
         Matrix4x4 matrixXY = UtilsGeom4D.CreateRotationMatrixXY(rotationXY * Mathf.Deg2Rad);
         Matrix4x4 matrixYZ = UtilsGeom4D.CreateRotationMatrixYZ(rotationYZ * Mathf.Deg2Rad);
         Matrix4x4 matrixZX = UtilsGeom4D.CreateRotationMatrixZX(rotationZX * Mathf.Deg2Rad);

         Matrix4x4 matrixXW = UtilsGeom4D.CreateRotationMatrixXW(rotationXW * Mathf.Deg2Rad);
         Matrix4x4 matrixYW = UtilsGeom4D.CreateRotationMatrixYW(rotationYW * Mathf.Deg2Rad);
         Matrix4x4 matrixZW = UtilsGeom4D.CreateRotationMatrixZW(rotationZW * Mathf.Deg2Rad);

         Matrix4x4 matrix = matrixXY * matrixYZ * matrixZX * matrixXW * matrixYW * matrixZW;

         // calculate view point vectors
         Vector3 tp = transform.position;
         *//*Vector3 co = viewPoint.right;
         Vector3 cp = viewPoint.position;    
         Vector3 cu = viewPoint.up;*//*
         Vector3 cp = new Vector3(0, 1, -1);
         Vector3 cu = Vector3.up;
         Vector3 co = Vector3.right;



         Vector4 toDir = new Vector4(tp.x, tp.y, tp.z, 0);
         Vector4 fromDir = new Vector4(cp.x, cp.y, cp.z, 0);
         Vector4 upDir = new Vector4(cu.x, cu.y, cu.z, 0);
         Vector4 overDir = new Vector4(co.x, co.y, co.z, 0);
         float viewingAngle = 20;
         // update mesh vertices based on rotations and view directions
         UtilsGeom4D.ProjectTo3DPerspective(UtilsGeom4D.kTesseractPoints, matrix, ref vertices, viewingAngle, fromDir, toDir, upDir, overDir);
     }
 */
    /*  public void LogSong(string song)
      {
          if (isLocalPlayer)
          {
              CL.songName = song;
              Debug.Log("logged song + " + CL.songName);
          }
      }*/




}
