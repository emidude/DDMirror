using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[System.Serializable]
public class Match{
    public string matchID;
    //public SyncListGameObject players = new SyncListGameObject();
    public List<GameObject> players = new List<GameObject>();

    public Match(string matchID, GameObject player)
    {
        this.matchID = matchID;
        players.Add(player);
    }

    public Match() { }
}


[System.Serializable]
public class SyncListMatch : SyncList<Match>
{

}

public class MatchMaker : NetworkBehaviour
{
    public static MatchMaker instance;

    public SyncListMatch matches = new SyncListMatch();

    public SyncList<string> matchIDs = new SyncList<string>();

    private void Start()
    {
        instance = this;
    }
    public static string GetRandomMatchID()
    {
        string _id = string.Empty;
        _id += (char)Random.Range(0, 3); //adds a random between number to string
        return _id;
    }
   public bool HostGame(string _matchID, GameObject _player)
    {
        if(!matchIDs.Contains(_matchID))
        {
            matchIDs.Add(_matchID);
            matches.Add(new Match(_matchID, _player));
            return true;
        }
        else
        {
            Debug.Log("match id already exists");
            return false;
        }
        

    }
    
}
