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
		p.z = Mathf.Sin(vL.z * v);

		return p;
	}

	//Previously 
	public static Vector3 MultiSineFunctionSI(
		//SteamVR_Behaviour_Pose cL,
		//SteamVR_Behaviour_Pose cR, 
		float x, float z, float t)
	//public static Vector3 MultiSineFunctionSI(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot, Vector3 vL, Vector3 vR, Vector3 avL, Vector3 avR)
	{
		/* Vector3 p;
		 p.x = x;
		 p.y = Mathf.Sin(pi * (cL.GetVelocity().x * x));
		 //p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
		 p.y += Mathf.Sin(pi * (2f * cR.GetVelocity().x * x + t) / 2f);
		 p.y *= 2f / 3f;
		 p.z = z;
		 return p;*/

		//BELOW IS JUST TO PREVENT ERRORS FOR NOW - SWITCH BACK TO ABOVE WITHOUT CL POSES
		Vector3 p = Vector3.one;
		return p;
    }

	public static Vector3 SineFunction(float x, float z, float t)
	{
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + t));
		p.z = z;
		return p;
	}

	public static Vector3 MultiSineFunction(float x, float z, float t)
	{
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + t));
		p.y += Mathf.Sin(2f * pi * (x + 2f * t)) / 2f;
		p.y *= 2f / 3f;
		p.z = z;
		return p;
	}

	public static Vector3 Sine2DFunction(float x, float z, float t)
	{
		Vector3 p;
		p.x = x;
		p.y = Mathf.Sin(pi * (x + t));
		p.y += Mathf.Sin(pi * (z + t));
		p.y *= 0.5f;
		p.z = z;
		return p;
	}

	public static Vector3 MultiSine2DFunction(float x, float z, float t)
	{
		Vector3 p;
		p.x = x;
		p.y = 4f * Mathf.Sin(pi * (x + z + t / 2f));
		p.y += Mathf.Sin(pi * (x + t));
		p.y += Mathf.Sin(2f * pi * (z + 2f * t)) * 0.5f;
		p.y *= 1f / 5.5f;
		p.z = z;
		return p;
	}

	public static Vector3 Ripple(float x, float z, float t)
	{
		Vector3 p;
		float d = Mathf.Sqrt(x * x + z * z);
		p.x = x;
		p.y = Mathf.Sin(pi * (4f * d - t));
		p.y /= 1f + 10f * d;
		p.z = z;
		return p;
	}

	public static Vector3 Cylinder(float u, float v, float t)
	{
		Vector3 p;
		float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
		p.x = r * Mathf.Sin(pi * u);
		p.y = v;
		p.z = r * Mathf.Cos(pi * u);
		return p;
	}

	public static Vector3 Sphere(float u, float v, float t)
	{
		Vector3 p;
		float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
		r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
		float s = r * Mathf.Cos(pi * 0.5f * v);
		p.x = s * Mathf.Sin(pi * u);
		p.y = r * Mathf.Sin(pi * 0.5f * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}

	public static Vector3 Torus(float u, float v, float t)
	{
		Vector3 p;
		float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
		float r2 = 0.2f + Mathf.Sin(pi * (4f * v + t)) * 0.05f;
		float s = r2 * Mathf.Cos(pi * v) + r1;
		p.x = s * Mathf.Sin(pi * u);
		p.y = r2 * Mathf.Sin(pi * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}
}
