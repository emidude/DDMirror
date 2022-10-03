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
	public static Vector3 SphereSI(float a, float b, float c,  float u, float v)
	{
		Vector3 p;
		float r = 0.8f + Mathf.Sin(pi * (6f * u + a)) * 0.1f;
		r += Mathf.Sin(pi * (4f * v + b)) * 0.1f;
		float s = r * Mathf.Cos(pi * c * v );
		p.x = s * Mathf.Sin(pi * u);
		p.y = r * Mathf.Sin(pi * c * v);
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
	public static Vector3 TorusSI3(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot, float u, float v)
	{
		Vector3 p;
		float r1 = 0.65f + Mathf.Sin(pi * (6f * u + cLPos.x)) * 0.1f;
		float r2 = 0.2f + Mathf.Sin(pi * (4f * v + cLPos.y)) * 0.05f;
		float s = r2 * Mathf.Cos(pi * v + cLPos.z) + r1;
		p.x = s * Mathf.Sin(pi * u + cRRot.z);
		p.y = r2 * Mathf.Sin(pi * v + cRRot.y);
		p.z = s * Mathf.Cos(pi * u + cRRot.x);
		return p;
	}


	public static Vector3 TorusSI(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot,  float u, float v)
	{
		Vector3 p;
		float r1 = cLPos.x + Mathf.Sin(pi * (cLPos.y * u + cRRot.x)) * cLPos.z;
		float r2 = cRPos.x + Mathf.Sin(pi * (cLPos.y * v + cLRot.x)) * cRPos.z;
		float s = r2 * Mathf.Cos(pi * v) + r1;
		p.x = s * Mathf.Sin(pi * u);
		p.y = r2 * Mathf.Sin(pi * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}

	public static Vector3 TorusSI2(float a, float b, float c, float d, float e, float f, float g, float h, float u, float v)
	{
		Vector3 p;
		/*float r1 = a + Mathf.Sin(pi * (b * u + c)) * d;
		float r2 = e + Mathf.Sin(pi * (f* v + g)) * h;*/
		float r1 = e + Mathf.Sin(pi * (b * u + g)) * d;
		float r2 = a + Mathf.Sin(pi * (f* v + c)) * h;
		float s = r2 * Mathf.Cos(pi * v) + r1;
		p.x = s * Mathf.Sin(pi * u);
		p.y = r2 * Mathf.Sin(pi * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}

	public static Vector3 SimpleSymmetric(float x, float y, float z, float u, float v)
	{
		Vector3 p;
		p.x = Mathf.Sin(pi * (y + u));
		p.y = Mathf.Sin(pi * (z + v));
		p.z = Mathf.Sin(pi * (x + (u + v)));
		return p;
	}
	/*public static Vector3 BoysSurfaceSI(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot, Vector3 vL, Vector3 vR, Vector3 avL, Vector3 avR, float x, float z, float t)
	{
		Vector3 p;
		Quaternion qL = cLRot;
		Quaternion qR = cRRot;

		ComplexNumber Z = new ComplexNumber();

		Z.z.x = x;
		Z.z.y = z;

		Vector2 one = new Vector2(1, 0);
		Vector2 z2 = Z.multiply(Z.z, Z.z);
		Vector2 z3 = Z.multiply(z2, Z.z);
		Vector2 z4 = Z.multiply(z3, Z.z);
		Vector2 z6 = Z.multiply(z3, z3);

		*//*Debug.Log("sanity check:");
		Debug.Log("z4=" + z4 + ", other way to calculate z4=" + Z.multiply(z2, z2));
		Vector2 z6 = Z.multiply(z3, z3);
		Debug.Log("another sanity check:");
		Debug.Log("z6 (3+3)=" + z6 + ", z6 (2 + 4)=" + Z.multiply(z2, z4) + " z6 (4+2)="+ Z.multiply(z4,z2));*//*
		Vector2 denominator = z6 + Mathf.Sqrt(5) * z3 - one;
		Vector2 g1C = (-3 / 2) * Z.divide(Z.multiply(one - z4, Z.z), denominator);
		Vector2 g2C = (-3 / 2) * Z.divide(Z.multiply(one + z4, Z.z), denominator);
		Vector2 g3C = Z.divide(one + z6, denominator) - one / 2;

		float coef = 1 / (g1C.y * g1C.y + g2C.x * g2C.x + g3C.y * g3C.y);

		p.x = g1C.y / coef;
		p.y = g2C.x / coef;
		p.z = g3C.y / coef;




		return p;
	}*/


}
