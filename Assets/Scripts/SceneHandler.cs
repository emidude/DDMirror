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
    bool readyToGo = false;

    public List<NetworkIdentity> PlayersNetIds_SH = new List<NetworkIdentity>();

    NetworkingPlayer NP;


    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        panelParent.SetActive(true);
        panelstart.SetActive(true);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        NP = networkIdentity.GetComponent<NetworkingPlayer>();
       
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if(e.target.tag == "identifiercube")
        {
           /* Debug.Log("id cube clicked");

            NetworkIdentity targetNetworkIdentity = e.target.gameObject.transform.root.GetComponent<NetworkIdentity>();

            playerManager = NetworkClient.connection.identity.GetComponent<PlayerManager>();

            Debug.Log("netid - " + targetNetworkIdentity);
            if (PlayersNetIds_SH.Count == 0)
            {
                PlayersNetIds_SH.Add(targetNetworkIdentity);
                
                playerManager.PlayersNetIds.Add(targetNetworkIdentity);
            }
            else
            {
                for (int i = 0; i < PlayersNetIds_SH.Count; i++)
                {
                    if(PlayersNetIds_SH[i] == targetNetworkIdentity)
                    {
                        return;
                    }
                }
                Debug.Log("should be unige net id added= "+ targetNetworkIdentity);
                PlayersNetIds_SH.Add(targetNetworkIdentity);

                playerManager.PlayersNetIds.Add(targetNetworkIdentity);
            }



            Debug.Log("playernet id in player manager count= " + playerManager.PlayersNetIds.Count);
            for (int i = 0; i < playerManager.PlayersNetIds.Count; i++)
            {
                Debug.Log(playerManager.PlayersNetIds[i]);
            }*/
        }

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
                //readyToGo = true;
                panelstart.SetActive(false);
                panelParent.SetActive(false);

                //player is ready to start
                //need issue cmd to play first song to all players
                //DONT NEEB BELWO COS ALREADY SET PLAYER MANAGER IN CLICKLING EACH OTHER SCENE
                /*ebug.Log("PlayersNetIds_SH.Count=" + PlayersNetIds_SH.Count);
                int numOtherPlayersReady = 0;
                for(int i = 0; i < PlayersNetIds_SH.Count; i++)
                {
                    if (PlayersNetIds_SH[i].GetComponent<SceneHandler>().readyToGo)
                    {
                        Debug.Log("ready in scen handler");
                        numOtherPlayersReady++;
                    }
                }
*/
                /*if(numOtherPlayersReady == PlayersNetIds_SH.Count)
                {*/
                    NetworkIdentity networkIdentity = NetworkClient.connection.identity;
                    playerManager = networkIdentity.GetComponent<PlayerManager>();
                    playerManager.CmdClickedSubmit();

                NP.DisactiveateBodyMarkers();
               // }
                
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
    






}