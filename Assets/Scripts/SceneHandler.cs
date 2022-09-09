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

    int currentQn = 0;
    bool preFirstSong = true;



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

            e.target.gameObject.GetComponent<Image>().color = Color.white;
            //log answer


            numPlayersPanel.SetActive(false);
            musicPrefPanel.SetActive(true);
            currentQn++;


        }
        else if (e.target.tag == "one")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.white;

            numPlayersPanel.SetActive(false);
            musicPrefPanel.SetActive(true);
            currentQn++;
        }
        else if (e.target.tag == "two")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.white;

            numPlayersPanel.SetActive(false);
            musicPrefPanel.SetActive(true);
            currentQn++;
        }
        else if (e.target.tag == "submit")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.white;

            if (preFirstSong)
            {
                preFirstSong = false;

                panelstart.SetActive(false);
                panelParent.SetActive(false);

                //player is ready to start
                //need issue cmd to play first song to all players

                NetworkIdentity networkIdentity = NetworkClient.connection.identity;
                playerManager = networkIdentity.GetComponent<PlayerManager>();
                playerManager.CmdClickedSubmit();
            }

            else if (currentQn == 1) {
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
                currentQn = 0;
                NetworkIdentity networkIdentity = NetworkClient.connection.identity;
                playerManager = networkIdentity.GetComponent<PlayerManager>();
                playerManager.CmdClickedSubmit();
            }

        }
    }

    public void SetCanvasInactive()
    {
        answeredQnPanel.SetActive(false);
        panelParent.SetActive(false);
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
       
        if (e.target.tag == "zero")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.yellow;
        }
        else if (e.target.tag == "one")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.yellow;
        }
        else if (e.target.tag == "two")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.yellow;
        }
        else if(e.target.tag == "submit")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.yellow;
        }
        
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.tag == "zero")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.white;

        }
        else if (e.target.tag == "one")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.white;
        }
        else if (e.target.tag == "two")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.white;
        }
        
        else if(e.target.tag == "submit")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.white;
        }

    }

    public void FinishedSong()
    {
        Debug.Log("finished song");
        panelParent.SetActive(true);

        panelstart.SetActive(false);
        musicPrefPanel.SetActive(false);
        dancePrefPanel.SetActive(false);
        answeredQnPanel.SetActive(false);

        numPlayersPanel.SetActive(true);

        //disable visuals;
        PlayerManager PM = NetworkClient.connection.identity.GetComponent<PlayerManager>();
        PM.questionTime = true;
        if (PM.bodyShapes)
        {
            //destroy body shapes
            //PM.CmdDeactivateBodyShapes(); //did not deacviate on remote client for some reason
            //PM.CmdDestroyHeadAndHands();
            PM.CmdDestroyTest();
        }
        else
        {
            PM.CmdDestroyCubes();
        }

    }
    






}