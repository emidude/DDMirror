using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionAndSongOrder : MonoBehaviour
{
    private System.Random _random = new System.Random();
   // public int[] testConditions = new int[4]; //4 conditions: 0= abstract + abstract; 1=abstract + head&hands; 2=head&hands+abstract; 3=head&hands+head&hands
    public int[] songOrdering = new int[16];
    // Start is called before the first frame update
    void Awake()
    {
        //song orders
        for (int i = 0; i < songOrdering.Length; i++)
        {
            songOrdering[i] = i;
        }
        //randomise Song order:
        Shuffle(songOrdering);

        if (songOrdering.Length == 16)
        {
            Debug.Log("SONG ORDERING:");
            Debug.Log(songOrdering[0] + ", " + songOrdering[1] + ", " + songOrdering[2] + ", " + songOrdering[3] +
                ", " + songOrdering[4] + ", " + songOrdering[5] + ", " + songOrdering[6] + ", " + songOrdering[7] +
                ",  " + songOrdering[8] + " ," + songOrdering[9] + "," + songOrdering[10] + ", " + songOrdering[11] +
                ", " + songOrdering[12] + ", " + songOrdering[13] + " ," + songOrdering[14] + " ," + songOrdering[15]);
        }
        else if (songOrdering.Length == 20)
        {
            Debug.Log("SONG ORDERING:");
            Debug.Log(songOrdering[0] + " ," + songOrdering[1] + ", " + songOrdering[2] + ", " + songOrdering[3] +
                " ," + songOrdering[4] + " ," + songOrdering[5] + ", " + songOrdering[6] + ", " + songOrdering[7] +
                " , " + songOrdering[8] + " ," + songOrdering[9] + "," + songOrdering[10] + ", " + songOrdering[11] +
                ", " + songOrdering[12] + " ," + songOrdering[13] + " ," + songOrdering[14] + ", " + songOrdering[15] + 
            "," + songOrdering[16] + "," + songOrdering[17] + ", " + songOrdering[18] + ", " + songOrdering[19]);
        }
        

        /*for (int i = 0; i <4; i++)
        {
            testConditions[i] = i;
        }
        //randomise test conditioons - these are AA; AH; HA; HH:
        Shuffle(testConditions);
        Debug.Log("TEST CONDITIONS:");
        Debug.Log(testConditions[0] + ", " + testConditions[1] + ", " + testConditions[2] + "," + testConditions[3]);*/
        

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
