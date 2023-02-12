/* SceneHandler.cs*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

using Valve.VR;


public class SceneHandlerTraining : MonoBehaviour
{
   

    public SteamVR_LaserPointer laserPointer;
    Vector3 OriginalLaserPointerScale;
    public SteamVR_Behaviour_Pose HandWithoutLaserPointer;
    Vector3 OriginalOtherHandScale;
    public LinearMapping linMap;
    public GameObject panelParent;
    
    public GameObject sliderTrainingPanel;
    public GameObject naturalMappingPanel;
    public GameObject distributedMappingPanel;
    public GameObject FinishedPanel;

    public GameObject empyPF;
    public GameObject cubePf;
    GameObject HeadGO, LeftHandGO, RightHandGO;
   

    public GameObject localHead;


    public GameObject[] rightHandCubes, leftHandCubes, headCubes;
    public SteamVR_Behaviour_Pose localLeftHand, localRightHand;

    public int trainingRound = 0;
   
    public LinearMapping trainingPreference;

    public Vector3[] R_PosCenters, L_PosCenters, H_PosCenters;
    int size = 4;
    //int totalCubes=125; //hardcoded becasue cant be fucked to find the right place to declare totalCubes=size*size*size, getting weird netwroking errors
    int totalCubes = 64;

    void Awake()
    {
        localHead = Camera.main.gameObject;
        localLeftHand = localLeftHand.GetComponent<SteamVR_Behaviour_Pose>();
        localRightHand = localRightHand.GetComponent<SteamVR_Behaviour_Pose>();

        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

       /* panelParent.SetActive(true);
        trainingPanel.SetActive(true); */

        OriginalLaserPointerScale = laserPointer.transform.localScale;
        OriginalOtherHandScale = HandWithoutLaserPointer.transform.localScale;

        SetTransformsGrid();

    }

    private void Update()
    {
        if (trainingRound == 1)
        {
            UpdateHeadAndHands(
                localHead.transform.position, localHead.transform.rotation, 
                localLeftHand.transform.position, localLeftHand.transform.rotation, 
                localRightHand.transform.position, localRightHand.transform.rotation);
        }
        else if (trainingRound == 2)
        {
            UpdateCubesDistrBS(
                localHead.transform.position, localHead.transform.rotation,
                localLeftHand.transform.position, localLeftHand.transform.rotation,
                localRightHand.transform.position, localRightHand.transform.rotation);
        }
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        
        if (e.target.tag == "submit")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.green;

            if(trainingRound == 0)
            {
                sliderTrainingPanel.SetActive(false);
                naturalMappingPanel.SetActive(true);
                //HideLaserPointerHandKeepingLaser();
               // HideVRHands();
                SpawnHeadAndHands();
                trainingRound++;
            }
            else if (trainingRound == 1)
            {
                naturalMappingPanel.SetActive(false);
                distributedMappingPanel.SetActive(true);
                trainingRound++;
                DestroyHeadAndHands();
                SpawnDistributedCubes();
                
            }
            else
            {
                trainingRound++;
                DestroyDestributed();
                
                distributedMappingPanel.SetActive(false);
                FinishedPanel.SetActive(true);

            }
            
            
            



           
            

        }
    }

    //CAUSES GAME TO CRAsh,ALSO SETTING ACTIVE FALSE, ASLO OTHER STUFF I CANT REMEMBER
   /* void HideVRHands()
    {
        localLeftHand.GetComponentInParent<Hand>().renderModelPrefab.GetComponent<RenderModel>().handPrefab = null;
        localRightHand.GetComponentInParent<Hand>().renderModelPrefab.GetComponent<RenderModel>().handPrefab = null;
    }*/

    void HideLaserPointerHandKeepingLaser()
    {
        laserPointer.transform.localScale = Vector3.one*0.07f;
        //laserPointer.active = false;
        HandWithoutLaserPointer.transform.localScale = Vector3.zero;
    }
    void ShowLaserPointer()
    {
        laserPointer.transform.localScale = OriginalLaserPointerScale;
        laserPointer.active = true;
        HandWithoutLaserPointer.transform.localScale = OriginalLaserPointerScale;
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

   
    void SpawnHeadAndHands()
    {
        //transform.position = Vector3.zero;
        Debug.Log("spawing HH");
        HeadGO = Instantiate(cubePf);
        HeadGO.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        //HeadGO.transform.SetParent(transform, false);
        

        LeftHandGO = Instantiate(cubePf);
        LeftHandGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //LeftHandGO.transform.SetParent(transform, false);
        

        RightHandGO = Instantiate(cubePf);
        RightHandGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        //RightHandGO.transform.SetParent(transform, false);
       
    }
        
    void SpawnDistributedCubes()
    {
        rightHandCubes = new GameObject[totalCubes - 1];
        leftHandCubes = new GameObject[totalCubes - 1];
        headCubes = new GameObject[totalCubes - 1];

        for (int i = 0; i < rightHandCubes.Length; i++)
        {
            GameObject vR = Instantiate(cubePf);
            vR.transform.localScale = Vector3.one * 0.2f;
            
            GameObject vL = Instantiate(cubePf);
            vL.transform.localScale = Vector3.one * 0.2f;
           
            GameObject vH = Instantiate(cubePf);
            vH.transform.localScale = Vector3.one * 0.4f;
           
            rightHandCubes[i] = vR;
            leftHandCubes[i] = vL;
            headCubes[i] = vH;
        }

    }

    void DestroyHeadAndHands()
    {
        GameObject.Destroy(HeadGO);
        GameObject.Destroy(LeftHandGO);
        GameObject.Destroy(RightHandGO);
    }

    void DestroyDestributed()
    {
        for (int i = 0; i < rightHandCubes.Length; i++)
        {
            GameObject.Destroy(rightHandCubes[i]);
            GameObject.Destroy(leftHandCubes[i]);
            GameObject.Destroy(headCubes[i]);
        }
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

    void UpdateCubesDistrBS(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot)
    {
        //Debug.Log("total cubes=" + totalCubes + "rightHandCubes.lenght=" + rightHandCubes.Length + "R_PosCenters.lngth=" + R_PosCenters.Length);
        for (int i = 0; i < totalCubes - 1; i++)
        {
            rightHandCubes[i].transform.position = R_PosCenters[i] + cRPos;
            leftHandCubes[i].transform.position = L_PosCenters[i] + cLPos;
            headCubes[i].transform.position = H_PosCenters[i] + HPos;

            rightHandCubes[i].transform.rotation = cRRot;
            leftHandCubes[i].transform.rotation = cLRot;
            headCubes[i].transform.rotation = HRot;



        }

    }

    void SetTransformsGrid()
    {
        //SET TRANSFORMS GRID:
        R_PosCenters = new Vector3[totalCubes - 1];
        L_PosCenters = new Vector3[totalCubes - 1];
        H_PosCenters = new Vector3[totalCubes - 1];

        float centeringAdjust = size / 2;
        float stretch = 2f;

        for (int i = 0, t = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {

                    if (i - centeringAdjust == 0 && j - centeringAdjust == 0 && k - centeringAdjust == 0)
                    {
                        Debug.Log("removing 0 point so no cubes at body positon, occurs at i=" + i + "j=" + j + "k=" + k);
                    }
                    else
                    {
                        R_PosCenters[t] = new Vector3(i - centeringAdjust, j - centeringAdjust, k - centeringAdjust) * stretch;
                        L_PosCenters[t] = new Vector3(i - centeringAdjust, j - centeringAdjust, k - centeringAdjust) * stretch;
                        H_PosCenters[t] = new Vector3(i - centeringAdjust, j - centeringAdjust, k - centeringAdjust) * stretch;
                        t++;
                    }


                }
            }
        }
    }

}
    






