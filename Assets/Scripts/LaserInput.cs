using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class LaserInput : MonoBehaviour
{
    public static GameObject currentObject;
    int currentID;

    // Start is called before the first frame update
    void Start()
    {
        currentObject = null;

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, 100f);
        for (int i = 0; i < hits.Length; i++)
        {
         //   if(currentID!=id)
        }
    }
}
