using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class UpdateSliderPosition : MonoBehaviour
{
    public LinearMapping LinearMapping;
    public Transform sphere;
    public Transform start;
    public Transform end;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sphere.position = Vector3.Lerp(start.position, end.position, LinearMapping.value);
    }
}
