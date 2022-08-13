/* SceneHandler.cs*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

public class SceneHandler : MonoBehaviour
{
    public SteamVR_LaserPointer laserPointer;

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        if (e.target.tag == "zero")
        {
            Debug.Log("Zero was clicked");
        }
        else if (e.target.tag == "one")
        {
            Debug.Log("Button 1 was clicked");
        }
        else if (e.target.tag == "two")
        {
            Debug.Log("2 clicked");
        }
       
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
       
        if (e.target.tag == "zero")
        {
            Debug.Log("zero inside");
            e.target.gameObject.GetComponent<Image>().color = Color.red;

        }
        else if (e.target.tag == "one")
        {
            Debug.Log("Button 1 was clicked");
            e.target.gameObject.GetComponent<Image>().color = Color.red;
        }
        else if (e.target.tag == "two")
        {
            Debug.Log("2 clicked");
            e.target.gameObject.GetComponent<Image>().color = Color.red;
        }


    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        e.target.gameObject.GetComponent<Image>().color = Color.blue;
        /*if (e.target.tag == "zero")
        {
            Debug.Log("zero inside");
            e.target.gameObject.GetComponent<Image>().color = Color.blue;

        }
        else if (e.target.tag == "one")
        {
            Debug.Log("Button 1 was clicked");
            e.target.gameObject.GetComponent<Image>().color = Color.blue;
        }
        else if (e.target.tag == "two")
        {
            Debug.Log("2 clicked");
            e.target.gameObject.GetComponent<Image>().color = Color.blue;
        }*/

    }

   
}