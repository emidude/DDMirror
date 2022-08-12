using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    /* public Transform cubepf;
     Transform cube;*/
    public GameObject cubepf;
    GameObject cube;
    public GameObject Canvas, Token;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("server started");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    public void CmdCreateCube()
    {
        GameObject cube = Instantiate(cubepf);
        NetworkServer.Spawn(cube, connectionToClient);
    }

    [ClientRpc]
    public void RpcUpdateCube(GameObject cube)
    {
        float t = Time.time;
        //
        if (hasAuthority)
        {
            Debug.Log("we have authrority");
            Vector3 testPos = new Vector3(0, 6, 0);
            cube.transform.position = testPos;
        }
        else
        {
            Debug.Log("no authority here");
            cube.GetComponent<Material>().color = Color.red;
        }
    }

    void CreateGui()
    {
        Canvas = GameObject.Find("Main Canvas");
        GameObject token = Instantiate(Token, new Vector2(-200f, -400f), Quaternion.identity);
        token.GetComponent<Transform>().SetParent(Canvas.transform, false);
    }


}
