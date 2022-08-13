using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class Graphs
{
	const float pi = Mathf.PI;
=======
public class Graphs 
{
	const float pi = Mathf.PI;

>>>>>>> tryAgain
	public static Vector3 SimpleSin(Vector3 vL, Vector3 vR, float u, float v, float t)
	{
		Vector3 p;
		p.x = Mathf.Sin(vL.x * u);
		p.y = Mathf.Sin(pi * (vR.y * u));
<<<<<<< HEAD
		p.z = Mathf.Sin(vL.z * v);
=======
		p.z = Mathf.Sin((vL.z + vR.z) * v);
>>>>>>> tryAgain
		return p;
	}
}
