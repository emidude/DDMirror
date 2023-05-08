using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CsvReader : MonoBehaviour
{

    string timeSeriesFileP1 = @"Assets/CSVFiles/20230202_183359participantNumber6session1conditionA.log.csv";
    string timeSeriesFileP2 = @"Assets/CSVFiles/20230202_183359participantNumber6session1conditionA.log.csv";

    //P1
    float[] P1rightHandX = new float[29056];
    float[] P1rightHandY = new float[29056];
    float[] P1rightHandZ = new float[29056];

    float[] P1leftHandX = new float[29056];
    float[] P1leftHandY = new float[29056];
    float[] P1leftHandZ = new float[29056];

    float[] P1headX = new float[29056];
    float[] P1headY = new float[29056];
    float[] P1headZ = new float[29056];


    float[] P1ROT_rightHandX = new float[29056];
    float[] P1ROT_rightHandY = new float[29056];
    float[] P1ROT_rightHandZ = new float[29056];

    float[] P1ROT_leftHandX = new float[29056];
    float[] P1ROT_leftHandY = new float[29056];
    float[] P1ROT_leftHandZ = new float[29056];

    float[] P1ROT_headX = new float[29056];
    float[] P1ROT_headY = new float[29056];
    float[] P1ROT_headZ = new float[29056];


    //P2
    float[] P2rightHandX = new float[29056];
    float[] P2rightHandY = new float[29056];
    float[] P2rightHandZ = new float[29056];

    float[] P2leftHandX = new float[29056];
    float[] P2leftHandY = new float[29056];
    float[] P2leftHandZ = new float[29056];

    float[] P2headX = new float[29056];
    float[] P2headY = new float[29056];
    float[] P2headZ = new float[29056];


    float[] P2ROT_rightHandX = new float[29056];
    float[] P2ROT_rightHandY = new float[29056];
    float[] P2ROT_rightHandZ = new float[29056];

    float[] P2ROT_leftHandX = new float[29056];
    float[] P2ROT_leftHandY = new float[29056];
    float[] P2ROT_leftHandZ = new float[29056];

    float[] P2ROT_headX = new float[29056];
    float[] P2ROT_headY = new float[29056];
    float[] P2ROT_headZ = new float[29056];


    int lineNumber = 0;
    int idx = 0;

    GameObject P1HeadGO, P1LeftHandGO, P1RightHandGO;
    public GameObject cubePf;
    
    void Start()
    {
        using (StreamReader reader = new StreamReader(timeSeriesFileP1))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (lineNumber % 2 == 0)
                {
                    Debug.Log(line);

                    if (lineNumber != 0)
                    {
                        string[] parts = line.Split(',');
                        for (int i = 0; i < parts.Length; i++)
                        {
                            //Debug.Log(parts[i]);
                            P1headX[idx] = Single.Parse(parts[2]);
                            P1headY[idx] = Single.Parse(parts[3]);
                            P1headZ[idx] = Single.Parse(parts[4]);

                            P1ROT_headX[idx] = Single.Parse(parts[5]);
                            P1ROT_headY[idx] = Single.Parse(parts[6]);
                            P1ROT_headZ[idx] = Single.Parse(parts[7]);

                            P1leftHandX[idx] = Single.Parse(parts[8]);
                            P1leftHandY[idx] = Single.Parse(parts[9]);
                            P1leftHandZ[idx] = Single.Parse(parts[10]);

                            P1ROT_leftHandX[idx] = Single.Parse(parts[11]);
                            P1ROT_leftHandY[idx] = Single.Parse(parts[12]);
                            P1ROT_leftHandZ[idx] = Single.Parse(parts[13]);

                            P1rightHandX[idx] = Single.Parse(parts[20]);
                            P1rightHandY[idx] = Single.Parse(parts[21]);
                            P1rightHandZ[idx] = Single.Parse(parts[22]);

                            P1ROT_rightHandX[idx] = Single.Parse(parts[23]);
                            P1ROT_rightHandY[idx] = Single.Parse(parts[24]);
                            P1ROT_rightHandZ[idx] = Single.Parse(parts[25]);
                        }
                    }

                    idx++;
                }
               
                lineNumber++;
             
            }
        }

        /*Debug.Log(lineNumber);

        for (int i = 0; i < P1headX.Length; i++) 
        {
            Debug.Log("rhzrot=" + P1ROT_rightHandZ[i]);
        }*/
        idx = 0;
        SpawnHeadAndHands();

    }

    // Update is called once per frame
    void Update()
    {
        if(idx< 29056)
        {
            UpdateHeadAndHands(idx);
            idx++;
        }
        
    }

    void SpawnHeadAndHands()
    {
        Debug.Log("spawning head n hands");
        P1HeadGO = Instantiate(cubePf);
        P1HeadGO.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        P1HeadGO.transform.SetParent(transform, false);


        P1LeftHandGO = Instantiate(cubePf);
        P1LeftHandGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        P1LeftHandGO.transform.SetParent(transform, false);


        P1RightHandGO = Instantiate(cubePf);
        P1RightHandGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        P1RightHandGO.transform.SetParent(transform, false);

    }

    void UpdateHeadAndHands(int idx)
    {
        P1HeadGO.transform.position = new Vector3(P1headX[idx], P1headY[idx], P1headZ[idx]);
        P1HeadGO.transform.eulerAngles = new Vector3(P1ROT_headX[idx], P1ROT_headY[idx], P1ROT_headZ[idx]);

        P1LeftHandGO.transform.position = new Vector3(P1leftHandX[idx], P1leftHandY[idx], P1leftHandZ[idx]);
        P1LeftHandGO.transform.eulerAngles = new Vector3(P1ROT_leftHandX[idx], P1ROT_leftHandY[idx], P1ROT_leftHandZ[idx]);

        P1RightHandGO.transform.position = new Vector3(P1rightHandX[idx], P1rightHandY[idx], P1rightHandZ[idx]);
        P1RightHandGO.transform.eulerAngles = new Vector3(P1ROT_rightHandX[idx], P1ROT_rightHandY[idx], P1ROT_rightHandZ[idx]);

    }

}
