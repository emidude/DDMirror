using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CsvReader : MonoBehaviour
{
    public float slowness = 0.005f;
    float timeInterval;

    string timeSeriesFileP1 = @"Assets/CSVFiles/20230202_183359participantNumber6session1conditionA.log.csv";
    string timeSeriesFileP2 = @"Assets/CSVFiles/20230202_182214participantNumber6session3conditionH.log.csv";

    string P1Name = "P6_";
    string P2Name = "P7_";

    string P1DanceCondition = "ED";
    string P2DanceCondition = "HH";

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

    GameObject[] P1rightHandCubes, P1leftHandCubes, P1headCubes;
    GameObject[] P2rightHandCubes, P2leftHandCubes, P2headCubes;
    const float TWOPI = Mathf.PI * 2;
    Vector3 P1startingHeadPos, P2startingHeadPos;

    void Start()
    {
        using (StreamReader reader1 = new StreamReader(timeSeriesFileP1))
        {
            string line;
            while ((line = reader1.ReadLine()) != null)
            {
                if (lineNumber % 2 == 0)
                {
                   // Debug.Log(line);

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
                   // Debug.Log(line);

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

        Debug.Log(lineNumber);
        Debug.Log(idx);

        int P1StartIdx = GetPsongStartIdx(timeSeriesFileP1);
        int P2StartIdx = GetPsongStartIdx(timeSeriesFileP2);

        idxP1 = P1StartIdx;
        idxP2 = P2StartIdx;
        /*Debug.Log(lineNumber);

        for (int i = 0; i < P1headX.Length; i++) 
        {
            Debug.Log("rhzrot=" + P1ROT_rightHandZ[i]);
        }*/
        idx = 0;
       // SpawnHeadAndHands();

        P1startingHeadPos = GetStartingHeadPos(timeSeriesFileP1);
        P2startingHeadPos = GetStartingHeadPos(timeSeriesFileP2);

        /* if (P1DanceCondition == "HH")
         {
             //SpawnHeadAndHands(P1HeadGO, );
             string cubeName = "";

             P1HeadGO = Instantiate(cubePf);
             P1HeadGO.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
             P1HeadGO.transform.SetParent(transform, false);
             cubeName = P1Name + "head";
             P1HeadGO.name = cubeName;

             P1LeftHandGO = Instantiate(cubePf);
             P1LeftHandGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
             P1LeftHandGO.transform.SetParent(transform, false);
             cubeName = P1Name + "leftHand";
             P1LeftHandGO.name = cubeName;

             P1RightHandGO = Instantiate(cubePf);
             P1RightHandGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
             P1RightHandGO.transform.SetParent(transform, false);
             cubeName = P1Name + "rightHand";
             P1RightHandGO.name = cubeName;
         }
         else if (P1DanceCondition == "ED")
         {

         }
         SpawnDistributedCubes();*/

        SpawnAllCubes(P1DanceCondition, P2DanceCondition);

    }

    // Update is called once per frame
    void Update()
    {
        if(timeInterval > 0)
        {
            timeInterval -= Time.deltaTime;
        }
        else if(idx< 29056)
        {
            UpdateHeadAndHands(idxP1, idxP2);

            //P1
            Vector3 P1HV3 = new Vector3(P1headX[idxP1], P1headY[idxP1], P1headZ[idxP1]);
            Quaternion P1HQ = Quaternion.identity;
            P1HQ.eulerAngles = new Vector3(P1ROT_headX[idxP1], P1ROT_headY[idxP1], P1ROT_headZ[idxP1]);

            Vector3 P1LV3 = new Vector3(P1leftHandX[idxP1], P1leftHandY[idxP1], P1leftHandZ[idxP1]);
            Quaternion P1LQ = Quaternion.identity;
            P1LQ.eulerAngles = new Vector3(P1ROT_leftHandX[idxP1], P1ROT_leftHandY[idxP1], P1ROT_leftHandZ[idxP1]);

            Vector3 P1RV3 = new Vector3(P1rightHandX[idxP1], P1rightHandX[idxP1], P1rightHandX[idxP1]);
            Quaternion P1RQ = Quaternion.identity;
            P1RQ.eulerAngles = new Vector3(P1ROT_rightHandX[idxP1], P1ROT_rightHandX[idxP1], P1ROT_rightHandX[idxP1]);

            //P2
            Vector3 P2HV3 = new Vector3(P2headX[idxP2], P2headY[idxP2], P2headZ[idxP2]);
            Quaternion P2HQ = Quaternion.identity;
            P2HQ.eulerAngles = new Vector3(P2ROT_headX[idxP2], P2ROT_headY[idxP2], P2ROT_headZ[idxP2]);

            Vector3 P2LV3 = new Vector3(P2leftHandX[idxP2], P2leftHandY[idxP2], P2leftHandZ[idxP2]);
            Quaternion P2LQ = Quaternion.identity;
            P2LQ.eulerAngles = new Vector3(P2ROT_leftHandX[idxP2], P2ROT_leftHandY[idxP2], P2ROT_leftHandZ[idxP2]);

            Vector3 P2RV3 = new Vector3(P2rightHandX[idxP2], P2rightHandX[idxP2], P2rightHandX[idxP2]);
            Quaternion P2RQ = Quaternion.identity;
            P2RQ.eulerAngles = new Vector3(P2ROT_rightHandX[idxP2], P2ROT_rightHandX[idxP2], P2ROT_rightHandX[idxP2]);

            UpdateDistribuitedCubesP(P1HV3, P1HQ, P1LV3, P1LQ, P1RV3, P1RQ, P1startingHeadPos, P2HV3, P2HQ, P2LV3, P2LQ, P2RV3, P2RQ, P2startingHeadPos );

            idxP1++;
            idxP2++;
            timeInterval = slowness;
        }
        
    }

    void SpawnHeadAndHands()
    {
        Debug.Log("spawning head n hands");
        string cubeName = "";

        P1HeadGO = Instantiate(cubePf);
        P1HeadGO.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        P1HeadGO.transform.SetParent(transform, false);
        cubeName = P1Name + "head";
        P1HeadGO.name = cubeName;

        P1LeftHandGO = Instantiate(cubePf);
        P1LeftHandGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        P1LeftHandGO.transform.SetParent(transform, false);
        cubeName = P1Name + "leftHand";
        P1LeftHandGO.name = cubeName;

        P1RightHandGO = Instantiate(cubePf);
        P1RightHandGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        P1RightHandGO.transform.SetParent(transform, false);
        cubeName = P1Name + "rightHand";
        P1RightHandGO.name = cubeName;


        P2HeadGO = Instantiate(cubePf);
        P2HeadGO.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        P2HeadGO.transform.SetParent(transform, false);
        cubeName = P2Name + "head";
        P2HeadGO.name = cubeName;

        P2LeftHandGO = Instantiate(cubePf);
        P2LeftHandGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        P2LeftHandGO.transform.SetParent(transform, false);
        cubeName = P2Name + "leftHand";
        P2LeftHandGO.name = cubeName;

        P2RightHandGO = Instantiate(cubePf);
        P2RightHandGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        P2RightHandGO.transform.SetParent(transform, false);
        cubeName = P2Name + "rightHand";
        P2RightHandGO.name = cubeName;


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

    int GetPsongStartIdx(string timeSeriesFile)
    {
        int lineNumber = 0;
        int idx = 0;
        bool notFoundStartYet = true;
        int songStartIdx = 0;

        using (StreamReader reader = new StreamReader(timeSeriesFile))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (lineNumber % 2 == 0)
                {
                    //Debug.Log(line);

                    if (lineNumber != 0)
                    {
                        string[] parts = line.Split(',');
                     
                        if(parts[0] != "" && notFoundStartYet)
                        {
                            Debug.Log("song starts here! + Pidx = " + idx);
                            notFoundStartYet = false;
                            songStartIdx = idx;
                        }
                        
                    }

                    idx++;
                }

                lineNumber++;

            }
        }

        return songStartIdx;
    }

    Vector3 GetStartingHeadPos(string timeSeriesFile)
    {
        int lineNumber = 0;
        int idx = 0;
        bool notFoundStartYet = true;
        int songStartIdx = 0;
        Vector3 startHeadPos = Vector3.zero;

        using (StreamReader reader = new StreamReader(timeSeriesFile))
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

                        if (parts[0] != "" && notFoundStartYet)
                        {
                            Debug.Log("song starts here! + Pidx = " + idx);
                            notFoundStartYet = false;
                           // songStartIdx = idx;
                           startHeadPos = new Vector3( Single.Parse(parts[2]),  Single.Parse(parts[3]), Single.Parse(parts[4]));
                            Debug.Log("startHeadPos=" + startHeadPos);
                        }

                    }

                    idx++;
                }

                lineNumber++;

            }
        }

        return startHeadPos;
    }

    void SpawnDistributedCubes()
    {
        Debug.Log("spoawing cubes");

        string cubeName = "";

        //transform.position = head.position; <-TODO:  need to fix
        transform.position = Vector3.zero;

       
        P1rightHandCubes = new GameObject[16];
        P1leftHandCubes = new GameObject[16];
        P1headCubes = new GameObject[16];
        P2rightHandCubes = new GameObject[16];
        P2leftHandCubes = new GameObject[16];
        P2headCubes = new GameObject[16];

        /*  RParents = new GameObject[16];
          LParents = new GameObject[16];
          HParents = new GameObject[16];*/

        for (int i = 0; i < P1rightHandCubes.Length; i++)
        {
            GameObject vR = Instantiate(cubePf);
            vR.transform.localScale = Vector3.one * 0.2f;
            /*
                        GameObject PR = new GameObject();
                        RParents[i] = PR;
                        //RParents[i].transform.rotation = Quaternion.LookRotation(new Vector3(0,0,1),  Vector3.up);
                        vR.transform.SetParent(RParents[i].transform,false);*/

            GameObject vL = Instantiate(cubePf);
            vL.transform.localScale = Vector3.one * 0.2f;
            /* GameObject PL = new GameObject();
             LParents[i] = PL;
             vL.transform.SetParent(LParents[i].transform, false);*/


            GameObject vH = Instantiate(cubePf);
            vH.transform.localScale = Vector3.one * 0.2f;
            /* GameObject PH = new GameObject();
             HParents[i] = PH;
             vH.transform.SetParent(HParents[i].transform, false);*/

            P1rightHandCubes[i] = vR;
            P1leftHandCubes[i] = vL;
            P1headCubes[i] = vH;

            cubeName = P1Name + "head_distributed";
            P1headCubes[i].name = cubeName;

            cubeName = P1Name + "rightHand_distributed";
            P1rightHandCubes[i].name = cubeName;

            cubeName = P1Name + "leftHand_distributed";
            P1leftHandCubes[i].name = cubeName;

        }

        for (int i = 0; i < P1rightHandCubes.Length; i++)
        {
            GameObject vR = Instantiate(cubePf);
            vR.transform.localScale = Vector3.one * 0.2f;
            
            GameObject vL = Instantiate(cubePf);
            vL.transform.localScale = Vector3.one * 0.2f;

            GameObject vH = Instantiate(cubePf);
            vH.transform.localScale = Vector3.one * 0.2f;
            
            P2rightHandCubes[i] = vR;
            P2leftHandCubes[i] = vL;
            P2headCubes[i] = vH;

            cubeName = P2Name + "head_distributed";
            P2headCubes[i].name = cubeName;

            cubeName = P2Name + "rightHand_distributed";
            P2rightHandCubes[i].name = cubeName;

            cubeName = P2Name + "leftHand_distributed";
            P2leftHandCubes[i].name = cubeName;
        }



    }

    void UpdateDistribuitedCubesP(Vector3 P1HPos, Quaternion P1HRot, Vector3 P1cLPos, Quaternion P1cLRot, Vector3 P1cRPos, Quaternion P1cRRot, Vector3 P1startingHeadPos, Vector3 P2HPos, Quaternion P2HRot, Vector3 P2cLPos, Quaternion P2cLRot, Vector3 P2cRPos, Quaternion P2cRRot, Vector3 P2startingHeadPos)
    {
        //P1:

        //NOTE: pos = NOT local pos, needs to be global in space
        float distArms = Vector3.Distance(P1cLPos, P1cRPos);
        float distLHead = Vector3.Distance(P1cLPos, P1HPos); //this could be local pos of cLPos???
        float distRHead = Vector3.Distance(P1cRPos, P1HPos);

        int res = P1rightHandCubes.Length;
        float tStep = TWOPI / (float)res;
        float t = 0;
        float scale = 3;

        float[] positionScaleFactors = { -4, -3, -2, -1, 0, 1, 2, 3, 4, 3, 2, 1, 0, -1, -2, -3 };
        float[] rotationLerpParam = { 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1, 0, 1, 2, 3 };
        for (int i = 0; i < 16; i++)
        {
            rotationLerpParam[i] = rotationLerpParam[i] / 8;
        }

        /*Quaternion q;
        Vector3 e;*/

        //BASIC WORKING
        for (int i = 0; i < res; i++)
        {
            /* //setting relative body distances
             rightHandCubes[i].transform.position = new Vector3(distArms * Mathf.Sin(t), 0, distRHead * Mathf.Cos(t)) * scale + startingHeadPos;
             leftHandCubes[i].transform.position = new Vector3(distLHead * Mathf.Sin(t), distArms * Mathf.Cos(t), 0) * scale + startingHeadPos;
             headCubes[i].transform.position = new Vector3(0, distRHead * Mathf.Sin(t), distLHead * Mathf.Cos(t)) * scale + startingHeadPos;
             *//*rightHandCubes[i].transform.position = new Vector3(distArms * Mathf.Sin(t), 0, distRHead * Mathf.Cos(t)) * scale + startingHeadPos;
             leftHandCubes[i].transform.position = new Vector3(distLHead * Mathf.Sin(t), 0, distArms * Mathf.Cos(t)) * scale + startingHeadPos;
             headCubes[i].transform.position = new Vector3(distRHead * Mathf.Cos(t), 0, distLHead * Mathf.Sin(t)) * scale + startingHeadPos;*/
            P1rightHandCubes[i].transform.position = new Vector3(distArms * Mathf.Sin(t), 0, distRHead * Mathf.Cos(t)) * scale + P1startingHeadPos;
            P1leftHandCubes[i].transform.position = new Vector3(distLHead * Mathf.Cos(t), 0, distArms * Mathf.Sin(t)) * scale + P1startingHeadPos;
            P1headCubes[i].transform.position = new Vector3(distRHead * Mathf.Cos(t), 0, distLHead * Mathf.Sin(t)) * scale + P1startingHeadPos;
            t += tStep;

            //rotate elipse so more visible//DID NOT WORK WENT GLITCHY LOCAL ROTS
            /* rightHandCubes[i].transform.RotateAround(startingHeadPos, new Vector3(1, 0, 0),90 ); //YZ plane
             leftHandCubes[i].transform.RotateAround(startingHeadPos, new Vector3(0, 1, 0), 90); //XZ plane
             rightHandCubes[i].transform.RotateAround(startingHeadPos, new Vector3(0, 0, 1), 90); //XY plane*/
            /*rightHandCubes[i].transform.RotateAround(startingHeadPos, Vector3.forward, 90); //YZ plane
            leftHandCubes[i].transform.RotateAround(startingHeadPos, Vector3.forward, 90); //XZ plane
            rightHandCubes[i].transform.RotateAround(startingHeadPos, Vector3.forward, 90); //XY plane*/

            //updating postiion of cubes based on device position
            P1rightHandCubes[i].transform.position += (P1cRPos - P1startingHeadPos) * positionScaleFactors[i];
            //leftHandCubes[i].transform.position += (cLPos - Vector3.up * startingHeadPos.y) * positionScaleFactors[i];
            P1leftHandCubes[i].transform.position += (P1cLPos - P1startingHeadPos) * positionScaleFactors[i];

            P1headCubes[i].transform.position += (P1HPos - P1startingHeadPos) * positionScaleFactors[i];

            //rotation
            P1rightHandCubes[i].transform.rotation = P1cRRot * Quaternion.Slerp(P1cRRot, Quaternion.Inverse(P1cRRot), rotationLerpParam[i]);
            P1leftHandCubes[i].transform.rotation = P1cLRot * Quaternion.Slerp(P1cLRot, Quaternion.Inverse(P1cLRot), rotationLerpParam[i]);
            P1headCubes[i].transform.rotation = P1HRot * Quaternion.Slerp(P1HRot, Quaternion.Inverse(P1HRot), rotationLerpParam[i]);

            //euler rotation:
            /*Vector3 increment = Vector3.one * rotInc * i;
            q = cRRot;
            q.eulerAngles += q.eulerAngles + increment;
            rightHandCubes[i].transform.rotation = q;

            q = cLRot;
            q.eulerAngles += q.eulerAngles + increment;
            leftHandCubes[i].transform.rotation = q;

            q = HRot;
            q.eulerAngles += q.eulerAngles + increment;
            headCubes[i].transform.rotation = q;*/
        }

        ////////////////////////////////////
        ///P2:
        ///

        //NOTE: pos = NOT local pos, needs to be global in space
        distArms = Vector3.Distance(P2cLPos, P2cRPos);
        distLHead = Vector3.Distance(P2cLPos, P2HPos); //this could be local pos of cLPos???
        distRHead = Vector3.Distance(P2cRPos, P2HPos);

        /*int res = rightHandCubes.Length;
        float tStep = TWOPI / (float)res;*/
        t = 0;
        scale = 3;

        /*float[] positionScaleFactors = { -4, -3, -2, -1, 0, 1, 2, 3, 4, 3, 2, 1, 0, -1, -2, -3 };
        float[] rotationLerpParam = { 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1, 0, 1, 2, 3 };*/
       /* for (int i = 0; i < 16; i++)
        {
            rotationLerpParam[i] = rotationLerpParam[i] / 8;
        }*/

      
        //BASIC WORKING
        for (int i = 0; i < res; i++)
        {

            P2rightHandCubes[i].transform.position = new Vector3(distArms * Mathf.Sin(t), 0, distRHead * Mathf.Cos(t)) * scale + P2startingHeadPos;
            P2leftHandCubes[i].transform.position = new Vector3(distLHead * Mathf.Cos(t), 0, distArms * Mathf.Sin(t)) * scale + P2startingHeadPos;
            P2headCubes[i].transform.position = new Vector3(distRHead * Mathf.Cos(t), 0, distLHead * Mathf.Sin(t)) * scale + P2startingHeadPos;
            t += tStep;

            //updating postiion of cubes based on device position
            P2rightHandCubes[i].transform.position += (P2cRPos - P2startingHeadPos) * positionScaleFactors[i];
            //leftHandCubes[i].transform.position += (cLPos - Vector3.up * startingHeadPos.y) * positionScaleFactors[i];
            P2leftHandCubes[i].transform.position += (P2cLPos - P2startingHeadPos) * positionScaleFactors[i];

            P2headCubes[i].transform.position += (P2HPos - P2startingHeadPos) * positionScaleFactors[i];

            //rotation
            P2rightHandCubes[i].transform.rotation = P2cRRot * Quaternion.Slerp(P2cRRot, Quaternion.Inverse(P2cRRot), rotationLerpParam[i]);
            P2leftHandCubes[i].transform.rotation = P2cLRot * Quaternion.Slerp(P2cLRot, Quaternion.Inverse(P2cLRot), rotationLerpParam[i]);
            P2headCubes[i].transform.rotation = P2HRot * Quaternion.Slerp(P2HRot, Quaternion.Inverse(P2HRot), rotationLerpParam[i]);

           
        }

    }


    void SpawnAllCubes(string P1DanceCondition, string P2DanceCondition)
    {
        SpawnHeadAndHands();
        SpawnDistributedCubes();

        if (P1DanceCondition == "HH")
        {
            SmolifyEDCubes(P1headCubes, P1leftHandCubes, P1rightHandCubes);
        }
        else if(P1DanceCondition == "ED")
        {
            SmolifyHHCubes(P1HeadGO, P1LeftHandGO, P1RightHandGO);
        }
        else
        {
            Debug.Log("Error, P1 dance condition not properly set");
        }

        if (P2DanceCondition == "HH")
        {
            SmolifyEDCubes(P2headCubes, P2leftHandCubes, P2rightHandCubes);
        }
        else if (P2DanceCondition == "ED")
        {
            SmolifyHHCubes(P2HeadGO, P2LeftHandGO, P2RightHandGO);
        }
        else
        {
            Debug.Log("Error, P2 dance condition not properly set");
        }


    }

    void SmolifyEDCubes(GameObject[] headCubes, GameObject[] leftHandCubes, GameObject[] rightHandCubes)
    {
        for (int i = 0; i < headCubes.Length; i++)
        {
            headCubes[i].transform.localScale = Vector3.zero;
            Debug.Log("smolifying " + headCubes[i].name);
        }
        for (int i = 0; i < leftHandCubes.Length; i++)
        {
            leftHandCubes[i].transform.localScale = Vector3.zero;
            Debug.Log("smolifying " + leftHandCubes[i].name);
        }
        for (int i = 0; i < rightHandCubes.Length; i++)
        {
            rightHandCubes[i].transform.localScale = Vector3.zero;
            Debug.Log("smolifying " + rightHandCubes[i].name);

        }

    }

    void SmolifyHHCubes(GameObject headCube, GameObject leftHandCube, GameObject rightHandCube)
    {
        
            headCube.transform.localScale = Vector3.zero;
        Debug.Log("smolifying " + headCube.name);


        leftHandCube.transform.localScale = Vector3.zero;
        Debug.Log("smolifying " + leftHandCube.name);

        rightHandCube.transform.localScale = Vector3.zero;
        Debug.Log("smolifying " + rightHandCube.name);


    }

    void BigifyEDCubes(GameObject[] headCubes, GameObject[] leftHandCubes, GameObject[] rightHandCubes)
    {
        for (int i = 0; i < headCubes.Length; i++)
        {
            headCubes[i].transform.localScale = Vector3.one * 0.2f;
        }
        for (int i = 0; i < leftHandCubes.Length; i++)
        {
            leftHandCubes[i].transform.localScale = Vector3.one * 0.2f;
        }
        for (int i = 0; i < rightHandCubes.Length; i++)
        {
            rightHandCubes[i].transform.localScale = Vector3.one * 0.2f;
        }

    }

    void BigifyHHCubes(GameObject headCube, GameObject leftHandCube, GameObject rightHandCube)
    {

        headCube.transform.localScale = Vector3.one * 0.4f;

        leftHandCube.transform.localScale = Vector3.one * 0.1f;

        rightHandCube.transform.localScale = Vector3.one * 0.1f;


    }


}
