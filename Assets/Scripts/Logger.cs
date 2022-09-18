using System;
using System.IO;
using UnityEngine;


public class Logger {

    static public string filename;
    // static int questionNumber; //can be 0 or 1, logging 2 qn ansers per song

    /*public static string songName;
    string participantNumber;
    public static string sessionString;
    public static string condition;
*/
    /* static string[] answersHeader = {
         "particpantNumber",
         "session",
         "condition",
         "songname",
         "musicPreference",
         "dancePreference"
         };*/

    //static string[] answers = new string[120];


    static public void Event(string msg)
    {
        Debug.Log(msg);
        StreamWriter writer = new StreamWriter(filename, append: true);
        writer.WriteLine(Time.time.ToString() + ": " + msg + "\n");
        writer.Close();
    }

    /*static public void LogAnswers()
    {
        string answersFileName = filename + "Answers";
        StreamWriter writer = new StreamWriter(answersFileName + ".csv" );
        writer.WriteLine(String.Join(",", answersHeader) + "\n");
    }*/

    static public void UpdateAnswers()
    {
        //string answer = particpantNumber + "," + session;
        /*condition + "," +
        songname + "," +
        musicPreference + +"," +
        dancePreference;*/
    }

    /*writer.WriteLine(Time.time.ToString() + ": " + msg + "\n");
    }*/
}
