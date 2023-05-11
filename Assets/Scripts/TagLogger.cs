using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;


public class TagLogger : MonoBehaviour
{

   /* public string p1;
    public string p2;
    public string expmntCondition;
    public string p1tMapping;
    public string p2Mapping;*/
   

    private StreamWriter tagWriter;
   

    static string[] tagHeader = {
        "p1csvIdx",
        "p1time",
        "p2csvIdx",
        "p2time",
        "tag"
    };

    //string[][] answers = new string[6][];

   /* struct Tagg
    {
      *//*  public string p1;
        public string p2;
        public string expmntCondition;
        public string p1tMapping;
        public string p2Mapping;*//*
        public string p1csvIdx;
        public string p1time;
        public string p2csvIdx;
        public string p2time;
        public string text;

    }

    Tagg[] tags = new Tagg[20];*/

   

    void OnDestroy()
    {
        if (null != tagWriter)
        {
            Debug.Log("destroying tag logger at " + Time.time.ToString());
            tagWriter.Close();
        }
      
    }

   

    public void WriteTagHeader(string p1, string p2, string expmntCondition,string p1tMapping, string p2Mapping)
    {
        string date = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filename = date + "___" + p1 + p1tMapping + "__" + p2 + p2Mapping + "___C" + expmntCondition;
        tagWriter = new StreamWriter(filename + ".csv");
        tagWriter.WriteLine(String.Join(",", tagHeader));
        //tagWriter.WriteLine(String.Join(",", tagHeader) + "\n");
    }

    public void TagTextInput(string p1csvIdx, string p1time,string p2csvIdx, string p2time,string text)
    {
        string[] values = {
            p1csvIdx, // actual csv line number = (p1csvIdx + 1)*2 or sth like that check csv reader code for mapping from linenumber to csvid since there were gaps and a title in csv.
            p1time,
            p2csvIdx,
            p2time,
            text
        };
   
        string csv = String.Join(",", values);
        //tagWriter.WriteLine(csv + "\n");
        tagWriter.WriteLine(csv );
    }
}
