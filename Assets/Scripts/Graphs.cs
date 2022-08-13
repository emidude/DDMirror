using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphs 
{
	const float pi = Mathf.PI;

	public static Vector3 SimpleSin(Vector3 vL, Vector3 vR, float u, float v, float t)
	{
		Vector3 p;
		p.x = Mathf.Sin(vL.x * u);
		p.y = Mathf.Sin(pi * (vR.y * u));
		p.z = Mathf.Sin((vL.z + vR.z) * v);
		return p;
	}
}
