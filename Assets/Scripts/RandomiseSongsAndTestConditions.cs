using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomiseSongsAndTestConditions : MonoBehaviour
{
    private System.Random _random = new System.Random();
    public int[] testConditions = new int[4]; //4 conditions: 0= abstract + abstract; 1=abstract + head&hands; 2=head&hands+abstract; 3=head&hands+head&hands
    public int[] songOrdering = new int[16];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            songOrdering[i] = i;
        }
        //randomise Song order:
        Shuffle(songOrdering);

        for (int i = 0; i <4; i++)
        {
            testConditions[i] = i;
        }
        //randomise Song order:
        Shuffle(testConditions);
    }

    // Update is called once per frame
    void Update()
    {
        
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
