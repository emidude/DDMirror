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
    public string sessionString;
    public string condition;
    public string songJustFinished;

    public string participantNumber;
    public int sessionNumber;
    public int pcNumber;
    public int studyOrder;

    private StreamWriter answersWriter;
    int currentAnswer = 0;

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

    static string[] answersHeader = {
        "particpantNumber",
        "studyOrder",
        "session",
        "condition",
        "songname",
        "musicPreference",
        "dancePreference",
        "correctlyIdHuman"
    };

    //string[][] answers = new string[6][];

    struct answer
    {
        public string particpantNum;
        public string studyOrder;
        public string sesh;
        public string cond;
        public string songnam;
        public string musicPref;
        public string dancePref;
        public string humanId;
    }

    answer[] answers = new answer[20];

    void Start()
    {   
        sessionString = sessionNumber.ToString();
        CalculateCondition();

        string date = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        //string filename = date+"participantNumber" + participantNumber + "session" + session + "condition" + condition;
        string filename = date + "participantNumber" + participantNumber + "session" + sessionString + "condition" + condition + ".log";
        Logger.filename = filename;
        /*Logger.songName = songName;
        Logger.condition = condition;
        Logger.sessionString = sessionString;
        Logger.participantNumber = participantNumber;*/

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
        //LOG ANSWERS HRERE:
        LogAnswers();
    }

    void LogAnswers()
    {
        string filename = "ANSWERS_" + "participantNumber" + participantNumber + "_Robot_OR_human";
        answersWriter = new StreamWriter(filename + ".csv");
        answersWriter.WriteLine(String.Join(",", answersHeader) + "\n");

        string[] values = new string[8];
        for (int s = 0; s < 4; s++)
        {
            values[0] = answers[s].particpantNum;
            values[1] = answers[s].studyOrder;
            values[2] = answers[s].sesh;
            values[3] = answers[s].cond;
            values[4] = answers[s].songnam;
            values[5] = answers[s].musicPref;
            values[6] = answers[s].dancePref;
            values[7] = answers[s].humanId;

            string csv = String.Join(",", values);
            answersWriter.WriteLine(csv + "\n");
        }
        answersWriter.Close();

    }

    public void UpdateAnswers(float musicPreference, float dancePreference, int pickedHuman, int currentSong)
    {
        answers[currentAnswer].particpantNum = participantNumber;
        answers[currentAnswer].studyOrder = studyOrder.ToString();
        answers[currentAnswer].sesh = sessionString;
        answers[currentAnswer].cond = condition;
        answers[currentAnswer].songnam = songJustFinished;
        answers[currentAnswer].musicPref = musicPreference.ToString();
        answers[currentAnswer].dancePref = dancePreference.ToString();
        answers[currentAnswer].humanId = pickedHuman.ToString();

        Debug.Log("currentSong=" + currentSong);
        Debug.Log("currentAnswer="+currentAnswer);
        currentAnswer++;
    }
    public void CalculateCondition()
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
