using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Valve.VR;


public class NetworkingPlayer : NetworkBehaviour
{
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

    int resolution = 10;
    GameObject[] points;


    void Update()
    {
        //TODO: FIX
        updateHeadAndHands();

        if (isLocalPlayer)
        {
            if (float.IsNaN(cL.GetVelocity().x) || float.IsNaN(cL.GetVelocity().y) || float.IsNaN(cL.GetVelocity().z))
            {
                Debug.Log("NAN");
            }
            else
            {
                CmdUpdateCubes(cL.GetVelocity(), cR.GetVelocity());
            }
        }

        /*Debug.Log("head trandfoprm = " + localHead.transform.position + " nwhead= " + networkedHead.transform.position);
        Debug.Log("left hand= " + localLeftHand.transform.position + " nwLH =" + networkedLeftHand.transform.position);
        Debug.Log("right hand=" + localRightHand.transform.position + " nwRH =" + networkedRightHand.transform.position);*/
        
    }


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // this is ONLY called on local player
        // connect to rig

        Debug.Log(gameObject.name + "Entered local start player, locating rig objects");

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


        CmdSpawnCubes();
        Debug.Log("server active?" + NetworkServer.active);
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

    [Command]
    void CmdReadyForNextSong()
    {
        if (isLocalPlayer)
        {
          //get player id, set ready on server
          
           //when all ready server does rpc - play next song
        }
    }

    [ClientRpc]
    void RpcPlayNextSong()
    {
        //get next song from list

        //sync start time in synclogger

        //start ctslogger with song in title

        //divide players into rooms

        //start song
    }


}
