using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CsvReader : MonoBehaviour
{

    string timeSeriesFile = @"Assets/CSVFiles/20230202_183359participantNumber6session1conditionA.log.csv";
    //StreamReader reader;
    // Start is called before the first frame update

    float[] rightHandX = new float[29056];
    float[] rightHandY = new float[29056];
    float[] rightHandZ = new float[29056];

    float[] leftHandX = new float[29056];
    float[] leftHandY = new float[29056];
    float[] leftHandZ = new float[29056];

    float[] headX = new float[29056];
    float[] headY = new float[29056];
    float[] headZ = new float[29056];


    float[] ROT_rightHandX = new float[29056];
    float[] ROT_rightHandY = new float[29056];
    float[] ROT_rightHandZ = new float[29056];

    float[] ROT_leftHandX = new float[29056];
    float[] ROT_leftHandY = new float[29056];
    float[] ROT_leftHandZ = new float[29056];

    float[] ROT_headX = new float[29056];
    float[] ROT_headY = new float[29056];
    float[] ROT_headZ = new float[29056];


    int lineNumber = 0;
    int idx = 0;
    
    void Start()
    {
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
                        for (int i = 0; i < parts.Length; i++)
                        {
                            //Debug.Log(parts[i]);
                            headX[idx] = Single.Parse(parts[2]);
                            headY[idx] = Single.Parse(parts[3]);
                            headZ[idx] = Single.Parse(parts[4]);

                            ROT_headX[idx] = Single.Parse(parts[5]);
                            ROT_headY[idx] = Single.Parse(parts[6]);
                            ROT_headZ[idx] = Single.Parse(parts[7]);

                            leftHandX[idx] = Single.Parse(parts[8]);
                            leftHandY[idx] = Single.Parse(parts[9]);
                            leftHandZ[idx] = Single.Parse(parts[10]);

                            ROT_leftHandX[idx] = Single.Parse(parts[11]);
                            ROT_leftHandY[idx] = Single.Parse(parts[12]);
                            ROT_leftHandZ[idx] = Single.Parse(parts[13]);

                            rightHandX[idx] = Single.Parse(parts[20]);
                            rightHandY[idx] = Single.Parse(parts[21]);
                            rightHandZ[idx] = Single.Parse(parts[22]);

                            ROT_rightHandX[idx] = Single.Parse(parts[23]);
                            ROT_rightHandY[idx] = Single.Parse(parts[24]);
                            ROT_rightHandZ[idx] = Single.Parse(parts[25]);
                        }
                    }

                    idx++;
                }
               
                lineNumber++;
             
            }
        }
        Debug.Log(lineNumber);

        for (int i = 0; i < headX.Length; i++) 
        {
            Debug.Log("rhzrot=" + ROT_rightHandZ[i]);
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
