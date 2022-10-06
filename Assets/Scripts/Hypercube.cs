using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ADAPTED CODE FROM
// Daniel Shiffman
// http://youtube.com/thecodingtrain
// http://codingtra.in
// JavaScript transcription: Chuck England
// Coding Challenge #113: 4D Hypercube
// https://youtu.be/XE3YDVdQSPo
// Matrix Multiplication
// https://youtu.be/tzsgS19RRc8

public class Hypercube : MonoBehaviour
{
	static Vector4[] points = new Vector4[]
	{
	new Vector4(1, 1, 1, 1),
	new Vector4(1, 1, 1, -1),
	new Vector4(1, 1, -1, 1),
	new Vector4(1, 1, -1, -1),
	new Vector4(1, -1, 1, 1),
	new Vector4(1, -1, 1, -1),
	new Vector4(1, -1, -1, 1),
	new Vector4(1, -1, -1, -1),
	new Vector4(-1, 1, 1, 1),
	new Vector4(-1, 1, 1, -1),
	new Vector4(-1, 1, -1, 1),
	new Vector4(-1, 1, -1, -1),
	new Vector4(-1, -1, 1, 1),
	new Vector4(-1, -1, 1, -1),
	new Vector4(-1, -1, -1, 1),
	new Vector4(-1, -1, -1, -1)
	};


//	public static Vector3 UpdateVertices(float XY, float YZ, float ZW, float XW, float YW, float XZ, float dist, float scale, int vIdx, float yTrans)

	public static Vector3 UpdateVertices(float XY, float YZ, float ZW, float XW, float YW, float XZ, float dist, float scale, int vIdx, Vector3 head) 
	{
		//Debug.Log("points[vIdx]= " + points[vIdx]);
		Vector4 rotated = rotate(XY, YZ, ZW, XW, YW, XZ, points[vIdx]);
	//Debug.Log(vIdx + " rotated = " + rotated);
		return project(dist, rotated, scale, head);
	}
	//dist = 2
    static Vector3 project(float dist, Vector4 v, float scale, Vector3 head)
    {
		float w = 1 / (dist - v.w);
		//scale upwards:
		return new Vector3(v.x * w * scale + head.x, v.y * w * scale + head.y, v.z * w * scale + head.z);
    }

    static Vector4 rotate(float XY, float YZ, float ZW, float XW, float YW, float XZ, Vector4 v)
    {
	//	Debug.Log("points[vIdx]= " + v);
		Vector4 rotatedV = rotationXY(XY, v);
		rotatedV = rotationYZ(YZ, rotatedV);
		rotatedV = rotationXZ(XZ, rotatedV);
		rotatedV = rotationXW(XW, rotatedV);
		rotatedV = rotationYW(YW, rotatedV);
		rotatedV = rotationZW(ZW, rotatedV);
		return rotatedV;
	}

    static Vector4 rotationXY(float angle, Vector4 v)
    {
		float cos = Mathf.Cos(angle);
		float sin = Mathf.Sin(angle);
		Vector4 rotatedVec;
		rotatedVec.x = cos * v.x - sin * v.y;
		rotatedVec.y = sin * v.x + cos * v.y;
		rotatedVec.z = v.z;
		rotatedVec.w = v.w;
		return rotatedVec;
    }
	static Vector4 rotationYZ(float angle, Vector4 v)
	{
		float cos = Mathf.Cos(angle);
		float sin = Mathf.Sin(angle);
		Vector4 rotatedVec;
		rotatedVec.x = v.x;
		rotatedVec.y = cos * v.y - sin * v.z;
		rotatedVec.z = sin * v.y + cos * v.z; 
		rotatedVec.w = v.w;
		return rotatedVec;
	}

	static Vector4 rotationZW(float angle, Vector4 v)
	{
		float cos = Mathf.Cos(angle);
		float sin = Mathf.Sin(angle);
		Vector4 rotatedVec;
		rotatedVec.x = v.x;
		rotatedVec.y = v.y;
		rotatedVec.z = cos * v.z - sin * v.w;
		rotatedVec.w = sin * v.z + cos * v.w;
		return rotatedVec;
	}
	static Vector4 rotationXW(float angle, Vector4 v)
	{
		float cos = Mathf.Cos(angle);
		float sin = Mathf.Sin(angle);
		Vector4 rotatedVec;
		rotatedVec.x = cos * v.x - sin * v.w;
		rotatedVec.y = v.y;
		rotatedVec.z = v.z;
		rotatedVec.w = sin * v.x + cos * v.w;
		return rotatedVec;
	}
	static Vector4 rotationYW(float angle, Vector4 v)
	{
		float cos = Mathf.Cos(angle);
		float sin = Mathf.Sin(angle);
		Vector4 rotatedVec;
		rotatedVec.x = v.x;
		rotatedVec.y = cos * v.y - sin * v.w;
		rotatedVec.z = v.z;
		rotatedVec.w = sin * v.y + cos * v.w;
		return rotatedVec;
	}
	static Vector4 rotationXZ(float angle, Vector4 v)
	{
		float cos = Mathf.Cos(angle);
		float sin = Mathf.Sin(angle);
		Vector4 rotatedVec;
		rotatedVec.x = cos * v.x - sin * v.z;
		rotatedVec.y = v.y;
		rotatedVec.z = sin * v.x + cos * v.z;
		rotatedVec.w = v.w;
		return rotatedVec;
	}


}
