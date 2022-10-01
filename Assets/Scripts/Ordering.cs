using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Ordering: MonoBehaviour
{
    private System.Random _random = new System.Random();
    //public int[] combinations = new int[6];
    public int[] sessionOrdering = new int[4];
    public int[] songOrdering = new int[12];
    /*public int[] embodiedCombinations = new int[6];
    public int[] disembodiedCombinations = new int[6];
    public int[] embodiedSongOrdering = new int[6];
    public int[] disembodiedSongOrdering = new int[6];*/

    int[] session_AA_SongOrdering = new int[3];
    int[] session_AE_SongOrdering = new int[3];
    int[] session_EA_SongOrdering = new int[3];
    int[] session_EE_SongOrdering = new int[3];


    string[] sessionOrderingString =
    {
        "A_A",
        "A_E",
        "E_A",
        "E_E"
    };

    private StreamWriter OrderWriter;

    private string[] OrderHeader = {
        "order",
        "session number",
        "session name",
        "song order"
    };

    private void Start()
    {
        //set song ordering for this session
        for (int i = 0; i < 12; i++)
        {
            songOrdering[i] = i;
        }
        //randomise Song order:
        Shuffle(songOrdering);
        Debug.Log("song ordering is :");
        for (int i = 0; i < songOrdering.Length; i++)
        {
            Debug.Log(songOrdering[i]);
        }

        //SESSION ORDERING:
        // 1 = A + A
        // 2 = A + E
        // 3 = E + A
        // 4 = E + E
        for (int i = 0; i < 4; i++)
        {
            sessionOrdering[i] = i;
        }
        //randomise Song order:
        Shuffle(sessionOrdering);
        Debug.Log("song ordering is :");
        for (int i = 0; i < sessionOrdering.Length; i++)
        {
            Debug.Log(sessionOrdering[i]);
        }


        
        for (int i = 0, s =0 ; i < songOrdering.Length; i+=3)
        {
            Debug.Log("Order = " + s + ", Session = " +  sessionOrdering[s] + " - " + sessionOrderingString[sessionOrdering[s]] + ": {" +
                songOrdering[i] + " , " + 
                songOrdering[i+1] + " , " + 
                songOrdering[i+2] + " };"
               );

            s++;
        }

        string date = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filename = "ORDERING_" +  date ;
        OrderWriter = new StreamWriter(filename);
        OrderWriter.WriteLine(String.Join(",", OrderHeader) + "\n");

        string[] values = new string[4];

        for (int i = 0, s = 0; i < songOrdering.Length; i += 3)
        {
            values[0] = s.ToString();
            values[1] = sessionOrdering[s].ToString();
            values[2] = sessionOrderingString[sessionOrdering[s]].ToString();
            values[3] = "{ " + songOrdering[i].ToString() + " , " +
                songOrdering[i + 1] + " , " +
                songOrdering[i + 2] + " };";

            string csv = String.Join(",", values);
            OrderWriter.WriteLine(csv + "\n");
            s++;
        }
        OrderWriter.Close();

        //BELOW - OLD REQUIRES MATCH SETUP 
            /*//set Player combinations for 10 songs, each player will do: 4 '3 player' songs, 4 '2 player' songs and 4 '1 player' songs.
            //numerical code for combinations array:
            //0 means all players are dancing together
            //1 => player 1 is dancing alone, player 2 & 3 dancing together
            //2 => p2 alone, 3=> p3 alone,
            //4 => all players dancing alone
            *//* for (int i = 0; i < 4; i++)
             {
                 combinations[i] = 0;
             }
             for (int i = 4; i < 6; i++)
             {
                 combinations[i] = 1;
             }
             for (int i = 6; i < 8; i++)
             {
                 combinations[i] = 2;
             }
             for (int i = 8; i < 10; i++)
             {
                 combinations[i] = 3;
             }
             for (int i = 10; i < 12; i++)
             {
                 combinations[i] = 4;
             }*//*
            embodiedCombinations = new int[] { 0,0,1,2,3,4};
            disembodiedCombinations = new int[] { 0, 0, 1, 2, 3, 4 };

            //randomise player combinations 
            //Shuffle(combinations);
            Shuffle(embodiedCombinations);
            Shuffle(disembodiedCombinations);

           *//* Debug.Log("player combinations ordering is :");
            for (int i = 0; i < combinations.Length; i++)
            {
                Debug.Log(combinations[i]);
            }*//*

            //Embodied first?
            int e = _random.Next(0, 2);
            Debug.Log("embodied first =0 ; disembodied first = 1 :::" + e);

            if (e == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    embodiedSongOrdering[i] = songOrdering[i];
                }
                for (int i = 0; i < 6; i++)
                {
                    disembodiedSongOrdering[i] = songOrdering[i + 6];
                }

                Debug.Log("embodied case first:");
                Debug.Log("embodied song ordering:}");
                Debug.Log("int[] {" + embodiedSongOrdering[0] + ", " + embodiedSongOrdering[1] + ", " + embodiedSongOrdering[2] + ", " +
                    embodiedSongOrdering[3] + ", " + embodiedSongOrdering[4] + ", " + embodiedSongOrdering[5] + "};");

                Debug.Log("embodied combinatins:");
                Debug.Log("int[] {" + embodiedCombinations[0] + ", " + embodiedCombinations[1] + ", " + embodiedCombinations[2] + ", " +
                    embodiedCombinations[3] + ", " + embodiedCombinations[4] + ", " + embodiedCombinations[5] +"};" );

                Debug.Log("------------------------------------------------------");
                Debug.Log("disembodied song ordering:");
                Debug.Log("int[] {" + disembodiedSongOrdering[0] + " , " + disembodiedSongOrdering[1] + " , " + disembodiedSongOrdering[2] + " , " +
                    disembodiedSongOrdering[3] + " , " + disembodiedSongOrdering[4] + " , "+ disembodiedSongOrdering[5]+"};");
                Debug.Log("disembodied combinatins:");
                Debug.Log("int[] {" + disembodiedCombinations[0] + " , " + disembodiedCombinations[1] + " , " + disembodiedCombinations[2] + " , " +
                     disembodiedCombinations[3] + " , " + disembodiedCombinations[4] + " , " + disembodiedCombinations[5]+"};");
            }
            else if (e == 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    disembodiedSongOrdering[i] = songOrdering[i];
                }
                for (int i = 0; i < 6; i++)
                {
                    embodiedSongOrdering[i] = songOrdering[i + 6];
                }

                Debug.Log("disembodied case first:");
                Debug.Log("disembodied song ordering:" + disembodiedSongOrdering[0] + " , " + disembodiedSongOrdering[1] + " , " + disembodiedSongOrdering[2] + " , " +
                    disembodiedSongOrdering[3] + " , " + disembodiedSongOrdering[4] + " , " + disembodiedSongOrdering[5]);
                Debug.Log("disembodied combinatins:" + disembodiedCombinations[0] + " , " + disembodiedCombinations[1] + " , " + disembodiedCombinations[2] + " , " +
                     disembodiedCombinations[3] + " , " + disembodiedCombinations[4] + " , " + disembodiedCombinations[5]);

                Debug.Log("------------------------------------------------------");
                Debug.Log("embodied song ordering: {" + embodiedSongOrdering[0] + ", " + embodiedSongOrdering[1] + ", " + embodiedSongOrdering[2] + ", " +
                   embodiedSongOrdering[3] + ", " + embodiedSongOrdering[4] + ", " + embodiedSongOrdering[5] + "};");
                Debug.Log("embodied combinatins:" + embodiedCombinations[0] + ", " + embodiedCombinations[1] + ", " + embodiedCombinations[2] + ", " +
                    embodiedCombinations[3] + ", " + embodiedCombinations[4] + ", " + embodiedCombinations[5]);
            }*/


    }

    void Shuffle(int[] array)
    {
        int p = array.Length;
        for (int n = p - 1; n > 0; n--)
        {
            int r = _random.Next(0, n + 1);
            int t = array[r];
            array[r] = array[n];
            array[n] = t;
        }
    }
}
