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
    
    public GameObject trainingPanel;
    

    
   
    public LinearMapping trainingPreference;

   
    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

       /* panelParent.SetActive(true);
        trainingPanel.SetActive(true); */

        OriginalLaserPointerScale = laserPointer.transform.localScale;
        OriginalOtherHandScale = HandWithoutLaserPointer.transform.localScale;

    }

   

    public void PointerClick(object sender, PointerEventArgs e)
    {
        
        if (e.target.tag == "submit")
        {
            e.target.gameObject.GetComponent<Image>().color = Color.blue;

           

           
            

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

   

        
        

    }
    






