using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CsvReader : MonoBehaviour
{

    string timeSeriesFileP1 = @"Assets/CSVFiles/20230202_183359participantNumber6session1conditionA.log.csv";
    string timeSeriesFileP2 = @"Assets/CSVFiles/20230202_182214participantNumber6session3conditionH.log.csv";

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
    int idxP1 = 0;
    int idxP2 = 0;

    GameObject P1HeadGO, P1LeftHandGO, P1RightHandGO, P2HeadGO, P2LeftHandGO, P2RightHandGO;
    public GameObject cubePf;
    
    void Start()
    {
        using (StreamReader reader1 = new StreamReader(timeSeriesFileP1))
        {
            string line;
            while ((line = reader1.ReadLine()) != null)
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

        Debug.Log(lineNumber);
        Debug.Log(idx);
        lineNumber = 0;
        idx = 0;

        using (StreamReader reader2 = new StreamReader(timeSeriesFileP2))
        {
            string line;
            while ((line = reader2.ReadLine()) != null)
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
                            P2headX[idx] = Single.Parse(parts[2]);
                            P2headY[idx] = Single.Parse(parts[3]);
                            P2headZ[idx] = Single.Parse(parts[4]);

                            P2ROT_headX[idx] = Single.Parse(parts[5]);
                            P2ROT_headY[idx] = Single.Parse(parts[6]);
                            P2ROT_headZ[idx] = Single.Parse(parts[7]);

                            P2leftHandX[idx] = Single.Parse(parts[8]);
                            P2leftHandY[idx] = Single.Parse(parts[9]);
                            P2leftHandZ[idx] = Single.Parse(parts[10]);

                            P2ROT_leftHandX[idx] = Single.Parse(parts[11]);
                            P2ROT_leftHandY[idx] = Single.Parse(parts[12]);
                            P2ROT_leftHandZ[idx] = Single.Parse(parts[13]);

                            P2rightHandX[idx] = Single.Parse(parts[20]);
                            P2rightHandY[idx] = Single.Parse(parts[21]);
                            P2rightHandZ[idx] = Single.Parse(parts[22]);

                            P2ROT_rightHandX[idx] = Single.Parse(parts[23]);
                            P2ROT_rightHandY[idx] = Single.Parse(parts[24]);
                            P2ROT_rightHandZ[idx] = Single.Parse(parts[25]);
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
            UpdateHeadAndHands(idxP1, idxP2);
            idxP1++;
            idxP2++;
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


        P2HeadGO = Instantiate(cubePf);
        P2HeadGO.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        P2HeadGO.transform.SetParent(transform, false);

        P2LeftHandGO = Instantiate(cubePf);
        P2LeftHandGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        P2LeftHandGO.transform.SetParent(transform, false);

        P2RightHandGO = Instantiate(cubePf);
        P2RightHandGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        P2RightHandGO.transform.SetParent(transform, false);

    }

    void UpdateHeadAndHands(int idxP1, int idxP2)
    {
        P1HeadGO.transform.position = new Vector3(P1headX[idxP1], P1headY[idxP1], P1headZ[idxP1]);
        P1HeadGO.transform.eulerAngles = new Vector3(P1ROT_headX[idxP1], P1ROT_headY[idxP1], P1ROT_headZ[idxP1]);

        P1LeftHandGO.transform.position = new Vector3(P1leftHandX[idxP1], P1leftHandY[idxP1], P1leftHandZ[idxP1]);
        P1LeftHandGO.transform.eulerAngles = new Vector3(P1ROT_leftHandX[idxP1], P1ROT_leftHandY[idxP1], P1ROT_leftHandZ[idxP1]);

        P1RightHandGO.transform.position = new Vector3(P1rightHandX[idxP1], P1rightHandY[idxP1], P1rightHandZ[idxP1]);
        P1RightHandGO.transform.eulerAngles = new Vector3(P1ROT_rightHandX[idxP1], P1ROT_rightHandY[idxP1], P1ROT_rightHandZ[idxP1]);



        P2HeadGO.transform.position = new Vector3(P2headX[idxP2], P2headY[idxP2], P2headZ[idxP2]);
        P2HeadGO.transform.eulerAngles = new Vector3(P2ROT_headX[idxP2], P2ROT_headY[idxP2], P2ROT_headZ[idxP2]);

        P2LeftHandGO.transform.position = new Vector3(P2leftHandX[idxP2], P2leftHandY[idxP2], P2leftHandZ[idxP2]);
        P2LeftHandGO.transform.eulerAngles = new Vector3(P2ROT_leftHandX[idxP2], P2ROT_leftHandY[idxP2], P2ROT_leftHandZ[idxP2]);

        P2RightHandGO.transform.position = new Vector3(P2rightHandX[idxP2], P2rightHandY[idxP2], P2rightHandZ[idxP2]);
        P2RightHandGO.transform.eulerAngles = new Vector3(P2ROT_rightHandX[idxP2], P2ROT_rightHandY[idxP2], P2ROT_rightHandZ[idxP2]);

    }

}
