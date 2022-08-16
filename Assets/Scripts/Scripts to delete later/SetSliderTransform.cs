using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSliderTransform : MonoBehaviour
{
    public Transform parent;
    public Transform sliderbar;
    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        sliderbar.transform.SetParent(parent, false);

    }

    // Update is called once per frame
    void Update()
    {
        sliderbar.position = pos;   
    }
}
