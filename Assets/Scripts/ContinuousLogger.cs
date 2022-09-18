using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;


public class ContinuousLogger : MonoBehaviour {

    public Transform head;
    /*public Transform leftHand;
    public Transform rightHand;*/
    public SteamVR_Behaviour_Pose leftHand, rightHand;

    public string songName;
    public string participantNumber;
    public string sessionString;
    public string condition;
    
    public int sessionNumber;
    public int pcNumber;
    


    private StreamWriter continuousWriter;
    private string[] continuousHeader = {
        "songname",
        "t", 
        "headX", "headY", "headZ",
        "headRotX", "headRotY", "headRotZ",
        "leftHandX", "leftHandY", "leftHandZ",
        "leftHandRotX", "leftHandRotY", "leftHandRotZ", 
        "leftHandVelX","leftHandVelY","leftHandVelZ",
        "leftHandAngVelX","leftHandAngVelY","leftHandAngVelZ",
        "rightHandX", "rightHandY", "rightHandZ",
        "rightHandRotX", "rightHandRotY", "rightHandRotZ",
        "rightHandVelX","rightHandVelY","rightHandVelZ",
        "rightHandAngVelX","rightHandAngVelY","rightHandAngVelZ",
    };


    void Start()
    {   
        sessionString = sessionNumber.ToString();
        CalculateCondition();

        string date = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        //string filename = date+"participantNumber" + participantNumber + "session" + session + "condition" + condition;
        string filename = date + "participantNumber" + participantNumber + "session" + sessionString + "condition" + condition + ".log";
        Logger.filename = filename;
        continuousWriter = new StreamWriter(filename + ".csv");
        continuousWriter.WriteLine(String.Join(",", continuousHeader) + "\n");
    }


    void Update()
    {
        string[] values = {
            songName,
            Time.time.ToString(),
            //head
            head.position.x.ToString(),
            head.position.y.ToString(),
            head.position.z.ToString(),
            head.eulerAngles.x.ToString(),
            head.eulerAngles.y.ToString(),
            head.eulerAngles.z.ToString(),
            /*leftHand.position.x.ToString(),
            leftHand.position.y.ToString(),
            leftHand.position.z.ToString(),
            leftHand.eulerAngles.x.ToString(),
            leftHand.eulerAngles.y.ToString(),
            leftHand.eulerAngles.z.ToString(),
            rightHand.position.x.ToString(),
            rightHand.position.y.ToString(),
            rightHand.position.z.ToString(),
            rightHand.eulerAngles.x.ToString(),
            rightHand.eulerAngles.y.ToString(),
            rightHand.eulerAngles.z.ToString(),*/
            //left hand
            leftHand.transform.position.x.ToString(),
            leftHand.transform.position.y.ToString(),
            leftHand.transform.position.z.ToString(),
            leftHand.transform.eulerAngles.x.ToString(),
            leftHand.transform.eulerAngles.y.ToString(),
            leftHand.transform.eulerAngles.z.ToString(), 
            leftHand.GetVelocity().x.ToString(),
            leftHand.GetVelocity().y.ToString(),
            leftHand.GetVelocity().z.ToString(),
            leftHand.GetAngularVelocity().x.ToString(),
            leftHand.GetAngularVelocity().y.ToString(),
            leftHand.GetAngularVelocity().z.ToString(),
            //right hand
            rightHand.transform.position.x.ToString(),
            rightHand.transform.position.y.ToString(),
            rightHand.transform.position.z.ToString(),
            rightHand.transform.eulerAngles.x.ToString(),
            rightHand.transform.eulerAngles.y.ToString(),
            rightHand.transform.eulerAngles.z.ToString(),
            rightHand.GetVelocity().x.ToString(),
            rightHand.GetVelocity().y.ToString(),
            rightHand.GetVelocity().z.ToString(),
            rightHand.GetAngularVelocity().x.ToString(),
            rightHand.GetAngularVelocity().y.ToString(),
            rightHand.GetAngularVelocity().z.ToString(),

        };
        string csv = String.Join(",", values);
        continuousWriter.WriteLine(csv + "\n");
    }


    void OnDestroy()
    {
        if (null != continuousWriter)
        {
            Debug.Log("destroying cts logger at " + Time.time.ToString());
            continuousWriter.Close();
        }
    }

    void CalculateCondition()
    {
        Debug.Log("calculating conditions in ctslogger, participant numner " + participantNumber);
        if (sessionNumber == 0)
        {
            Debug.Log("condition = A");
            condition = "A";
        }
        else if (sessionNumber == 1)
        {
            if (pcNumber == 1)
            {
                condition = "A";
                Debug.Log("condition = A");
            }
            else
            {
                condition = "H";
                Debug.Log("condition = H");
            }
        }
        else if (sessionNumber == 2)
        {
            if (pcNumber == 1)
            {
                condition = "H";
                Debug.Log("condition = H");
            }
            else
            {
                condition = "A";
                Debug.Log("condition = A");
            }
        }
        else if (sessionNumber == 3)
        {
            condition = "H";
            Debug.Log("condition = H");
        }
        else
        {
            Debug.Log("ERRPR!!!!!!!!!!!!!!! SESSION NUMBER NOT 0-3");
        }
    }
}
