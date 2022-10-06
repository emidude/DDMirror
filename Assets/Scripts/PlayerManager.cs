using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Valve.VR;

public class PlayerManager : NetworkBehaviour
{
    public PlayerSync playerSync;

    public int[] songOrdering; 

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
    //GameObject[] points;
    GameObject[] points1, points2, points3;
    float step = 0.2f;
    Vector3 scale;

    //public bool bodyShapes = false;
    public bool bodyShapes;
    public bool questionTime = true;

    // Players List to manage playerNumber  
    static readonly List<PlayerManager> playersList = new List<PlayerManager>();


    public GameObject[] vertices1Pf, vertices2Pf, vertices3Pf;
    // bool hypercubeRotations = false;

    public bool calibratingArmSpa;

    float maxLx, maxLy, maxLz, maxRy, maxRx, maxRz, minLx, minLy, minLz, minRx, minRy, minRz;
    const float TWOPI = Mathf.PI * 2;

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

        //AUDIO:
        audioObject = GameObject.FindGameObjectWithTag("audioHndlr");
        AudioHandler = audioObject.GetComponent<AudioHandler>();
        //LOGGER:
        ContinuousLogger = audioObject.GetComponent<ContinuousLogger>();

        //ORDERING:
        songOrdering = new int[] { 10, 2, 7 };

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


        //DO NOT UPDATE GLOBAL VARS HERE IF NEEDED IN A CMD -> LOCAL PLAYER HAS VARS, NOT SERVER!
        /*step = 2f / resolution;
        scale = Vector3.one * step;*/

        CmdSpawnCubes();
        //CmdUpdateCubes(cL.GetVelocity(),cR.GetVelocity());
        CmdDestroyCubes();

        CmdSpawnHeadAndHands();
        CmdDestroyHeadAndHands();

       
       
        CmdSetCubesCondition();

        //set min calibrating values to be really high:

        /*minLx = 100f;
        minLy = 100f;
        minLz = 100f;
        minRx = 100f;
        minRy = 100f;
        minRz = 100f;
        calibratingArmSpa = true;*/
    }

    void Update()
    {

        if (isLocalPlayer)
        {
            /*if (calibratingArmSpa)
            {
                if (!(float.IsFinite(localHead.transform.position.x) || float.IsFinite(localHead.transform.position.y) || float.IsFinite(localHead.transform.position.z)
                   || float.IsFinite(cL.transform.position.x) || float.IsFinite(cL.transform.position.y) || float.IsFinite(cL.transform.position.z)
                   || float.IsFinite(cR.transform.position.x) || float.IsFinite(cR.transform.position.y) || float.IsFinite(cR.transform.position.z)
                   || float.IsFinite(localHead.transform.rotation.x) || float.IsFinite(localHead.transform.rotation.y) || float.IsFinite(localHead.transform.rotation.z) || float.IsFinite(localHead.transform.rotation.w)
                   || float.IsFinite(cL.transform.rotation.x) || float.IsFinite(cL.transform.rotation.y) || float.IsFinite(cL.transform.rotation.z) || float.IsFinite(cL.transform.rotation.w)
                   || float.IsFinite(cR.transform.rotation.x) || float.IsFinite(cR.transform.rotation.y) || float.IsFinite(cR.transform.rotation.z) || float.IsFinite(cR.transform.rotation.w)))
                {
                    // Debug.Log("NAN");
                    maxLx = Mathf.Max(maxLx, cL.transform.localPosition.x);
                    maxLy = Mathf.Max(maxLy, cL.transform.localPosition.y);
                    maxLz = Mathf.Max(maxLz, cL.transform.localPosition.z);
                    
                    minLx = Mathf.Min(minLx, cL.transform.localPosition.x);
                    minLy = Mathf.Min(minLy, cL.transform.localPosition.y);
                    minLz = Mathf.Min(minLz, cL.transform.localPosition.z);


                    maxRx = Mathf.Max(maxRx, cR.transform.localPosition.x);
                    maxLy = Mathf.Max(maxRy, cR.transform.localPosition.y);
                    maxLz = Mathf.Max(maxRz, cR.transform.localPosition.z);

                    minRx = Mathf.Min(minRx, cR.transform.localPosition.x);
                    minRy = Mathf.Min(minRy, cR.transform.localPosition.y);
                    minRz = Mathf.Min(minRz, cR.transform.localPosition.z);
                }
                
            }*/

            if (!questionTime)
            {
                if (! (float.IsFinite(localHead.transform.position.x) || float.IsFinite(localHead.transform.position.y) || float.IsFinite(localHead.transform.position.z) 
                    || float.IsFinite(cL.transform.position.x) || float.IsFinite(cL.transform.position.y) || float.IsFinite(cL.transform.position.z) 
                    || float.IsFinite(cR.transform.position.x) || float.IsFinite(cR.transform.position.y) || float.IsFinite(cR.transform.position.z)                     
                    || float.IsFinite(localHead.transform.rotation.x) || float.IsFinite(localHead.transform.rotation.y) || float.IsFinite(localHead.transform.rotation.z) || float.IsFinite(localHead.transform.rotation.w)
                    || float.IsFinite(cL.transform.rotation.x) || float.IsFinite(cL.transform.rotation.y) || float.IsFinite(cL.transform.rotation.z) || float.IsFinite(cL.transform.rotation.w)
                    || float.IsFinite(cR.transform.rotation.x) || float.IsFinite(cR.transform.rotation.y) || float.IsFinite(cR.transform.rotation.z) || float.IsFinite(cR.transform.rotation.w)))
                {
                   // Debug.Log("NAN");
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
                 //  CmdUpdateCubes(localHead.transform.position, localHead.transform.rotation, cL.transform.position, cL.transform.rotation, cR.transform.position, cR.transform.rotation);
                    CmdUpdateCubes(localHead.transform.localPosition, localHead.transform.rotation, cL.transform.localPosition, cL.transform.rotation, cR.transform.localPosition, cR.transform.rotation);

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
        Debug.Log("spoawing cubes");
       
        //transform.position = head.position; <-TODO:  need to fix
        transform.position = Vector3.zero;

        points1 = new GameObject[resolution * resolution];
        for (int i = 0; i < points1.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = Vector3.one*0.2f;
            //TODO: SET PARENT TO HEAD
            point.transform.SetParent(transform, false);
            points1[i] = point;
            NetworkServer.Spawn(point);
        }

        points2 = new GameObject[resolution * resolution];
        for (int i = 0; i < points2.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = Vector3.one * 0.2f;
            //TODO: SET PARENT TO HEAD
            point.transform.SetParent(transform, false);
            points2[i] = point;
            NetworkServer.Spawn(point);
        }

        points3 = new GameObject[resolution * resolution];
        for (int i = 0; i < points3.Length; i++)
        {
            GameObject point = Instantiate(cubePf);
            point.transform.localScale = Vector3.one * 0.2f;
            //TODO: SET PARENT TO HEAD
            point.transform.SetParent(transform, false);
            points3[i] = point;
            NetworkServer.Spawn(point);
        }

        vertices1Pf = new GameObject[16];
        vertices2Pf = new GameObject[16];
        vertices3Pf = new GameObject[16];


        for (int i = 0; i < vertices1Pf.Length; i++)
        {
            GameObject v1 = Instantiate(cubePf);
            v1.transform.localScale = Vector3.one * 0.2f;
            GameObject v2 = Instantiate(cubePf);
            v2.transform.localScale = Vector3.one * 0.2f;
            GameObject v3 = Instantiate(cubePf);
            v3.transform.localScale = Vector3.one * 0.2f;
            vertices1Pf[i] = v1;
            vertices2Pf[i] = v2;
            vertices3Pf[i] = v3;
            NetworkServer.Spawn(v1);
            NetworkServer.Spawn(v2);
            NetworkServer.Spawn(v3);

        }

        

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

        
            for (int i = 0; i < vertices1Pf.Length; i++)
            {
                NetworkServer.Destroy(vertices1Pf[i]);
                NetworkServer.Destroy(vertices2Pf[i]);
                NetworkServer.Destroy(vertices3Pf[i]);
            }
        
    }

    [Command]
    void CmdUpdateCubes(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot)
    {
        
            float distArmsApart = Vector3.Distance(cLPos, cRPos) + 3;
            /*float t = Time.time;*/
            //float step = 2f / resolution;
            /*step = 0.2f;
            for (int i = 0, z = 0; z < resolution; z++)
            {
                float v = (z + 0.5f) * step - 1f;
                for (int x = 0; x < resolution; x++, i++)
                {
                    float u = (x + 0.5f) * step - 1f;

                    points1[i].transform.localPosition = Graphs.SimpleSymmetric(HPos.x / distArmsApart, cLPos.y / distArmsApart, cRPos.z / distArmsApart, u, v) * (distArmsApart);
                    distArmsApart = Vector3.Distance(HPos, cLPos) + 3;
                    points2[i].transform.localPosition = Graphs.SimpleSymmetric(cLPos.x / distArmsApart, cRPos.y / distArmsApart, HPos.z / distArmsApart, u, v) * distArmsApart;
                    distArmsApart = Vector3.Distance(HPos, cRPos) + 3;
                    points3[i].transform.localPosition = Graphs.SimpleSymmetric(cRPos.x / distArmsApart, HPos.y / distArmsApart, cLPos.z / distArmsApart, u, v) * distArmsApart;
                }
            }*/


        Vector3 cL_Deg = cLRot.eulerAngles * Mathf.Deg2Rad;
        Vector3 cR_Deg = cRRot.eulerAngles * Mathf.Deg2Rad;
        Vector3 H_Deg = HRot.eulerAngles * Mathf.Deg2Rad;

        /*float rotLx = Mathf.Lerp(minLx, maxLx, cLPos.x) * TWOPI;
        float rotLy = Mathf.Lerp(minLy, maxLy, cLPos.y) * TWOPI;
        float rotLz = Mathf.Lerp(minLz, maxLz, cLPos.z) * TWOPI;

        float rotRx = Mathf.Lerp(minRx, maxRx, cRPos.x) * TWOPI;
        float rotRy = Mathf.Lerp(minRy, maxRy, cRPos.y) * TWOPI;
        float rotRz = Mathf.Lerp(minRz, maxRz, cRPos.z) * TWOPI;*/

        float rotLx = Mathf.LerpUnclamped(0, TWOPI, cLPos.x * 0.5f);
        float rotLy = Mathf.LerpUnclamped(0, TWOPI, cLPos.y * 0.5f);
        float rotLz = Mathf.LerpUnclamped(0, TWOPI, cLPos.z * 0.5f);

        float rotRx = Mathf.LerpUnclamped(0, TWOPI, cRPos.x * 0.5f);
        float rotRy = Mathf.LerpUnclamped(0, TWOPI, cRPos.y * 0.5f);
        float rotRz = Mathf.LerpUnclamped(0, TWOPI, cRPos.z * 0.5f);

        /*float rotLx = Mathf.LerpUnclamped(0, TWOPI, cLPos.x );
        float rotLy = Mathf.LerpUnclamped(0, TWOPI, cLPos.y );
        float rotLz = Mathf.LerpUnclamped(0, TWOPI, cLPos.z );

        float rotRx = Mathf.LerpUnclamped(0, TWOPI, cRPos.x );
        float rotRy = Mathf.LerpUnclamped(0, TWOPI, cRPos.y );
        float rotRz = Mathf.LerpUnclamped(0, TWOPI, cRPos.z );*/


        for (int i = 0; i < 16; i++)
        {
            //1
            /*vertices1Pf[i].transform.position = Hypercube.UpdateVertices(cLPos.x, cLPos.y, cLPos.z, cR_Deg.x, cR_Deg.y, cR_Deg.z, 3f, 2, i, HPos.y);
            vertices2Pf[i].transform.position = Hypercube.UpdateVertices(H_Deg.x, H_Deg.y, H_Deg.z, cRPos.x, cRPos.y, cRPos.z, 3f, 2, i, HPos.y);
            vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 3f, 2, i, HPos.y);*/

            //fast
            //vertices1Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, cR_Deg.x, cR_Deg.y, cR_Deg.z, 3f, 2, i, HPos.y);

            //not complete motions, right speed
            //vertices2Pf[i].transform.position = Hypercube.UpdateVertices(cLPos.x, cLPos.y, cLPos.z, cRPos.x, cRPos.y, cRPos.z, 3f, 2, i, HPos.y);

            //vertices3Pf[i].transform.position = Hypercube.UpdateVertices(rotLx, rotLy, rotLz, rotRx, rotRy, rotRz, 3f, 2, i, HPos.y);

            vertices1Pf[i].transform.position = Hypercube.UpdateVertices(rotLx, rotLy, rotLz, rotRx, rotRy, rotRz, Vector3.Distance(cLPos,cRPos)* 3f, 2, i, HPos);
            vertices1Pf[i].transform.rotation = cLRot;

            vertices2Pf[i].transform.position = Hypercube.UpdateVertices( rotRx, rotRy, rotRz, rotLx, rotLy, rotLz, Vector3.Distance( cRPos, cLPos) * 3f, 2, i, HPos);
            vertices2Pf[i].transform.rotation = cRRot;


            /*vertices1Pf[i].transform.position = Hypercube.UpdateVertices(cLPos.x, cLPos.y, cLPos.z, cR_Deg.x, cR_Deg.y, cR_Deg.z, 1, 2, i);
            vertices2Pf[i].transform.position = Hypercube.UpdateVertices(H_Deg.x, H_Deg.y, H_Deg.z, cRPos.x, cRPos.y, cRPos.z, 1, 2, i);
            vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 1, 2, i);*/

            //too fast
            /*vertices1Pf[i].transform.position = Hypercube.UpdateVertices(cLPos.x, cLPos.y, cLPos.z, cR_Deg.x, cR_Deg.y, cR_Deg.z, 4, 5, i);
            vertices2Pf[i].transform.position = Hypercube.UpdateVertices(H_Deg.x, H_Deg.y, H_Deg.z, cRPos.x, cRPos.y, cRPos.z, 4, 5, i);
            vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 4, 5, i);*/

            /* vertices1Pf[i].transform.position = Hypercube.UpdateVertices(HRot.x, HRot.y, cLRot.x, cLRot.y, cLRot.z, cLRot.w, 0, 1, i);
             vertices2Pf[i].transform.position = Hypercube.UpdateVertices(HRot.z, HRot.w, cRRot.x, cRRot.y, cRRot.z, cRRot.w, 0, 1, i);
             vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 0, 1, i);
*/

            //super glitchy!
            /* vertices1Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x * 0.1f, cL_Deg.y * 0.1f, cL_Deg.z * 0.1f, cR_Deg.x * 0.1f, cR_Deg.y * 0.1f, cR_Deg.z * 0.1f, 3, 3, i);
             vertices2Pf[i].transform.position = Hypercube.UpdateVertices(H_Deg.x * 0.1f, H_Deg.y * 0.1f, H_Deg.z * 0.1f, cL_Deg.x * 0.1f, cL_Deg.y * 0.1f, cL_Deg.z * 0.1f, 3, 3, i);
             vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cR_Deg.x * 0.1f, cR_Deg.y * 0.1f, cR_Deg.z * 0.1f, H_Deg.x * 0.1f, H_Deg.y * 0.1f, H_Deg.z * 0.1f, 3, 3, i);*/

            //best so far
            /*vertices1Pf[i].transform.position = Hypercube.UpdateVertices(HRot.x, HRot.y, cLRot.x, cLRot.y, cLRot.z, cLRot.w, 1.5f, 2, i);
            vertices2Pf[i].transform.position = Hypercube.UpdateVertices(HRot.z, HRot.w, cRRot.x, cRRot.y, cRRot.z, cRRot.w, 1.5f, 2, i);
            vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 1.5f, 2, i);
*/
            //cant tell diff between this and above
            /*vertices1Pf[i].transform.position = Hypercube.UpdateVertices(HRot.x, HRot.y, cLRot.x, cLRot.y, cLRot.z, cLRot.w, 2, 2, i);
            vertices2Pf[i].transform.position = Hypercube.UpdateVertices(HRot.z, HRot.w, cRRot.x, cRRot.y, cRRot.z, cRRot.w, 2, 2, i);
            vertices3Pf[i].transform.position = Hypercube.UpdateVertices(cL_Deg.x, cL_Deg.y, cL_Deg.z, HPos.x, HPos.y, HPos.z, 2, 2, i);*/

        }

        /*if (hypercubeRotations)
        {
            UpdateSimpleSinPoints(cLPos, cRPos, HPos);
            Update4DPoints(cLRot, cRRot, HRot, 0.01f, localHead.transform, cL.transform, cR.transform);
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
        }*/






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


            Debug.Log("FINALLY EVERYONE READY!!!!!!! (songOrdering[songIndx]="+songOrdering[songIndx] + " index=" + songIndx);
            for(int i = 0; i< songOrdering.Length; i++)
            {
                Debug.Log(songOrdering[i]);
            }
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
        }
        else
        {
            PM.CmdSpawnCubes();
        }
    }

    void UpdateSimpleSinPoints(Vector3 cLPos, Vector3 cRPos, Vector3 HPos)
    {
        float distArmsApart = Vector3.Distance(cLPos, cRPos) + 3;
        /*float t = Time.time;
        float step = 2f / resolution;*/
        for (int i = 0, z = 0; z < resolution; z++)
        {
            float v = (z + 0.5f) * step - 1f;
            for (int x = 0; x < resolution; x++, i++)
            {
                float u = (x + 0.5f) * step - 1f;

                points1[i].transform.localPosition = Graphs.SimpleSymmetric(HPos.x / distArmsApart, cLPos.y / distArmsApart, cRPos.z / distArmsApart, u, v) * (distArmsApart);
                distArmsApart = Vector3.Distance(HPos, cLPos) + 3;
                points2[i].transform.localPosition = Graphs.SimpleSymmetric(cLPos.x / distArmsApart, cRPos.y / distArmsApart, HPos.z / distArmsApart, u, v) * distArmsApart;
                distArmsApart = Vector3.Distance(HPos, cRPos) + 3;
                points3[i].transform.localPosition = Graphs.SimpleSymmetric(cRPos.x / distArmsApart, HPos.y / distArmsApart, cLPos.z / distArmsApart, u, v) * distArmsApart;
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
