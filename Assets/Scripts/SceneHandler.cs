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
using Valve.VR;


public class SceneHandler : NetworkBehaviour
{
    public PlayerManager playerManager;

    public SteamVR_LaserPointer laserPointer;
    Vector3 OriginalLaserPointerScale;
    public SteamVR_Behaviour_Pose HandWithoutLaserPointer;
    Vector3 OriginalOtherHandScale;
    public LinearMapping linMap;
    public GameObject panelParent;
    public GameObject panelstart;
    public GameObject numPlayersPanel;
    public GameObject musicPrefPanel;
    public GameObject dancePrefPanel;
    public GameObject answeredQnPanel;
    public GameObject finishedSessionPanel;

    int currentQn = 0;
    bool preFirstSong = true;

    /*public AudioHandler AH;
    GameObject AHO;*/
    public ContinuousLogger CLogger;
    public LinearMapping musicPreference, dancePreference;

    int curentSong = 0;

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        panelParent.SetActive(true);
        panelstart.SetActive(true); //TODO:RETURN LATER
        /*panelstart.SetActive(false);
        musicPrefer*/

        OriginalLaserPointerScale = laserPointer.transform.localScale;
        OriginalOtherHandScale = HandWithoutLaserPointer.transform.localScale;

    }

   

    public void PointerClick(object sender, PointerEventArgs e)
    {
        //FOR MULTIPLE CHOICE PANEL:
 /*       if (e.target.tag == "zero")
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
        }*/
        if (e.target.tag == "submit")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.white;

            if (preFirstSong)
            {
                preFirstSong = false;

                panelstart.SetActive(false);
                panelParent.SetActive(false);

                
                HideLaserPointer();

                //player is ready to start
                //need issue cmd to play first song to all players

                NetworkIdentity networkIdentity = NetworkClient.connection.identity;
                playerManager = networkIdentity.GetComponent<PlayerManager>();
                playerManager.calibratingArmSpa = false;
                playerManager.CmdClickedSubmit();
            }

            else if (currentQn == 0) {
                //just answered numplayers qn, now on music pref panel

               // musicPreference = 
                   // musicPrefPanel.gameObject.GetComponentInChildren<LinearMapping>().value;

                musicPrefPanel.SetActive(false);                
                dancePrefPanel.SetActive(true);

                

                currentQn++;
            }
            else if (currentQn == 1)
            {
               //SAVE ANSWER
                CLogger.UpdateAnswers(musicPreference.value, dancePreference.value);
                //reset slider values
                musicPreference.value = 0.5f;
                dancePreference.value = 0.5f;
                //kill ui panels
                dancePrefPanel.SetActive(false);
                
                PlayerManager PM = NetworkClient.connection.identity.GetComponent<PlayerManager>();

                if (curentSong < PM.songOrdering.Length)
                {
                    answeredQnPanel.SetActive(true);
                    currentQn = 0;
                    PM.CmdClickedSubmit();
                }
                else
                {
                    finishedSessionPanel.SetActive(true);
                }


                HideLaserPointer();
                
                
            }

        }
    }

    void HideLaserPointer()
    {
        laserPointer.transform.localScale = Vector3.zero;
        laserPointer.active = false;
        HandWithoutLaserPointer.transform.localScale = Vector3.zero;
    }
    void ShowLaserPointer()
    {
        laserPointer.transform.localScale = OriginalLaserPointerScale;
        laserPointer.active = true;
        HandWithoutLaserPointer.transform.localScale = OriginalLaserPointerScale;
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
        curentSong++;

        Debug.Log("finished song");
        panelParent.SetActive(true);

        PlayerManager PM = NetworkClient.connection.identity.GetComponent<PlayerManager>();

        panelstart.SetActive(false);
        musicPrefPanel.SetActive(true);
        dancePrefPanel.SetActive(false);
        answeredQnPanel.SetActive(false);
        ShowLaserPointer();

        //BELOW DOES NOT WORK -LAST SONG GOES straight to finished session panel without qn panels
        /*if (curentSong < PM.songOrdering.Length)
        {
            //numPlayersPanel.SetActive(true);
            panelstart.SetActive(false);
            musicPrefPanel.SetActive(true);
            dancePrefPanel.SetActive(false);
            answeredQnPanel.SetActive(false);

            ShowLaserPointer();
        }
        else
        {
            finishedSessionPanel.SetActive(true);
        }*/

        //disable visuals;        
        PM.questionTime = true;
        if (PM.bodyShapes)
        {
            //destroy body shapes
            //PM.CmdDeactivateBodyShapes(); //did not deacviate on remote client for some reason
            PM.CmdDestroyHeadAndHands();
            //PM.CmdDestroyTest();
        }
        else
        {
            PM.CmdDestroyCubes();
        }

        
        

    }
    






}