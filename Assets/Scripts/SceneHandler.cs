/* SceneHandler.cs*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;
using Mirror;


public class SceneHandler : NetworkBehaviour
{
    public PlayerManager playerManager;

    public SteamVR_LaserPointer laserPointer;
    public LinearMapping linMap;
    public GameObject panelParent;
    public GameObject panelstart;
    public GameObject numPlayersPanel;
    public GameObject musicPrefPanel;
    public GameObject dancePrefPanel;
    public GameObject answeredQnPanel;
    int currentSong = 0;
    int currentQn = 0;
    bool preFirstSong = true;

    //public GameObject spawnedSyncObj;
    
    [SerializeField] GameObject spawnableObjForSync;
    GameObject syncObj;

    void Awake()
    {
        


        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        panelParent.SetActive(true);
        panelstart.SetActive(true);
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.tag == "zero")
        {
         
            Debug.Log("Zero was clicked");
            e.target.gameObject.GetComponent<Image>().color = Color.red;
            //log answer


            numPlayersPanel.SetActive(false);
            musicPrefPanel.SetActive(true);
            currentQn++;


        }
        else if (e.target.tag == "one")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.red;

            Debug.Log("Button 1 was clicked");
            numPlayersPanel.SetActive(false);
            musicPrefPanel.SetActive(true);
            currentQn++;

            

        }
        else if (e.target.tag == "two")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.red;

            Debug.Log("2 clicked");
            numPlayersPanel.SetActive(false);
            musicPrefPanel.SetActive(true);
            currentQn++;

            

        }
        else if (e.target.tag == "submit")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.red;

            if (preFirstSong)
            {
                preFirstSong = false;

                panelstart.SetActive(false);
                panelParent.SetActive(false);

                //player is ready to start
                //need issue cmd to play first song to all players
                
               /* if (syncObj == null) 
                {
                    Debug.Log("no spawn obj");
                }
                syncObj.GetComponent<IncrementClick>().IncrementClicks();*/


                NetworkIdentity networkIdentity = NetworkClient.connection.identity;
                playerManager = networkIdentity.GetComponent<PlayerManager>();
                playerManager.CmdClickedSubmit();
                //playerManager.CmdIncrementClick(syncObj);

                networkIdentity.GetComponent<NetworkingPlayer>().CmdIncrementClick(syncObj);
                
                
            }
            else if(currentQn==1){
                //just answered numplayers qn, now on music pref panel
                musicPrefPanel.SetActive(false);
                //TODO: coroutine here
                dancePrefPanel.SetActive(true);
                //LOGANSWER

                currentQn++;
            }
            else if (currentQn == 2)
            {
                //LOGANSWER
                dancePrefPanel.SetActive(false);
                answeredQnPanel.SetActive(true);
                currentQn++;
            }
            else if (currentQn == 3)
            {
                answeredQnPanel.SetActive(false);
                panelParent.SetActive(false);
                //reset qns
                currentQn = 0;
                //server play next song
                NetworkIdentity networkIdentity = NetworkClient.connection.identity;
                playerManager = networkIdentity.GetComponent<PlayerManager>();
                playerManager.CmdClickedSubmit();
            }
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
       
        if (e.target.tag == "zero")
        {
            Debug.Log("zero inside");
            e.target.gameObject.GetComponent<Image>().color = Color.yellow;

        }
        else if (e.target.tag == "one")
        {
            Debug.Log("Button 1 was clicked");
            e.target.gameObject.GetComponent<Image>().color = Color.yellow;
        }
        else if (e.target.tag == "two")
        {
            Debug.Log("2 clicked");
            e.target.gameObject.GetComponent<Image>().color = Color.yellow;
        }
        else if(e.target.tag == "submit")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.yellow;
        }
        
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        //e.target.gameObject.GetComponent<Image>().color = Color.blue;
        if (e.target.tag == "zero")
        {
            Debug.Log("zero inside");
            e.target.gameObject.GetComponent<Image>().color = Color.white;

        }
        else if (e.target.tag == "one")
        {
            Debug.Log("Button 1 was clicked");
            e.target.gameObject.GetComponent<Image>().color = Color.white;
        }
        else if (e.target.tag == "two")
        {
            Debug.Log("2 clicked");
            e.target.gameObject.GetComponent<Image>().color = Color.white;
        }
        
        else if(e.target.tag == "submit")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.white;
        }

    }

    public void FinishedSong()
    {
        panelParent.SetActive(true);
        numPlayersPanel.SetActive(true);

        //disable visuals script;

    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        syncObj = Instantiate(spawnableObjForSync);
        NetworkServer.Spawn(syncObj);
    }
    






}