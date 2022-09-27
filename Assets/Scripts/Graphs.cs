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

	public static Vector3 Sine2DFunctionSI(float x, float y, float z, Quaternion rot, float u, float v, float w, float scale)
	{
		Vector3 p;

		p.x = Mathf.Sin(pi* (y + u));		
		p.x += Mathf.Sin(rot.eulerAngles.x*Mathf.Deg2Rad);

		p.y = Mathf.Sin(pi * (z + v));
		p.y += Mathf.Sin(rot.eulerAngles.y * Mathf.Deg2Rad);

		p.z = Mathf.Sin(pi * (x + w));
		p.z = Mathf.Sin(rot.eulerAngles.z * Mathf.Deg2Rad);
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

	public static Vector3 TorusSI(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot, float u, float v, float t)
	{
		Vector3 p;
		float r1 = cLPos.x + Mathf.Sin(pi * (cRPos.y * u + cRRot.x)) * cLPos.z;
		float r2 = cRPos.x + Mathf.Sin(pi * (cLPos.y * v + cLRot.x)) * cRPos.z;
		float s = r2 * Mathf.Cos(pi * v) + r1;
		p.x = s * Mathf.Sin(pi * u);
		p.y = r2 * Mathf.Sin(pi * v);
		p.z = s * Mathf.Cos(pi * u);
		return p;
	}

	//static void Vector3 calcPos()

	public static Vector3 movingFigure8(Vector3 HPos, Quaternion HRot, Vector3 cLPos, Quaternion cLRot, Vector3 cRPos, Quaternion cRRot, float u, float v, float t)
	{
		Vector3 p;
		float r = HRot.x;
		t /= 10f;
		/*u = pi * (u + t);
		v = pi * (v + t);*/
		u = pi * (cLPos.x * cRPos.y + cLPos.z * cRPos.z + u);
		v = pi * (cRPos.x * cLPos.y + cLPos.z + v);
		p.x = (r + Mathf.Cos(u / 2f) * Mathf.Sin(v) - Mathf.Sin(u / 2f) * Mathf.Sin(2 * v)) * Mathf.Cos(u);
		p.y = (r + Mathf.Cos(u / 2f) * Mathf.Sin(v) - Mathf.Sin(u / 2f) * Mathf.Sin(2 * v)) * Mathf.Sin(u);
		p.z = Mathf.Sin(u / 2f) * Mathf.Sin(v) + Mathf.Cos(u / 2f) * Mathf.Sin(2 * v);
		return p;
	}
	
	public static Vector3 movingFigure8Sym(float x, float y, float z, Quaternion Rot, float u, float v, float scale)
	{
		Vector3 p;
		float r = z;
		/*t /= 10f;
		u = pi * (u + t);
		v = pi * (v + t);*/
		u = pi * (x + u);
		v = pi * (y + v);
		p.x = (r + Mathf.Cos(u / 2f) * Mathf.Sin(v) - Mathf.Sin(u / 2f) * Mathf.Sin(2 * v)) * Mathf.Cos(u) * scale ;
		p.y = (r + Mathf.Cos(u / 2f) * Mathf.Sin(v) - Mathf.Sin(u / 2f) * Mathf.Sin(2 * v)) * Mathf.Sin(u)  *scale ;
		p.z = Mathf.Sin(u / 2f) * Mathf.Sin(v) + Mathf.Cos(u / 2f) * Mathf.Sin(2 * v) * scale;
		return p;
	}

	public static Vector3 SimpleSymmetric(float x, float y, float z, float u, float v)
    {
		Vector3 p;
        /*p.x = Mathf.Sin(pi * (x + u));
        p.y = Mathf.Sin(pi * (y + v));
        p.z = Mathf.Sin(pi * (z + u + v));*/
        p.x = Mathf.Sin(pi * (y + u));
        p.y = Mathf.Sin(pi * (z + v));
        p.z = Mathf.Sin(pi * (x + (u + v)));
        return p;
    }

	public static Vector3 SimpleSymmetric3D(float x, float y, float z, float u, float v, float w)
	{
		Vector3 p;
		/*p.x = Mathf.Sin(pi * (x + u));
        p.y = Mathf.Sin(pi * (y + v));
        p.z = Mathf.Sin(pi * (z + u + v));*/
		p.x = Mathf.Sin(pi * (y + u));
		p.y = Mathf.Sin(pi * (z + v));
		p.z = Mathf.Sin(pi * (x + w));
		return p;
	}
	//not good
	/*public static Vector3 TestMove2D(float x, float y, float z, float u, float v)
	{
		Vector3 p;
		p.x = x+ u;
		p.y = y + v;
		p.z = u;
		return p;
	}*/

	//made wierd circle
	public static Vector3 RotToPos(float x, float y, float z, float u, float v, float scale)
	{
		Vector3 p;
        /*p.x = Mathf.Sin(pi * (x + u));
        p.y = Mathf.Sin(pi * (y + v));
        p.z = Mathf.Sin(pi * (z + u + v));*/

        /*p.x =  Mathf.Sin(pi * (q.eulerAngles.y*0.01f + z + x +  u)) * scale;
		p.y =  Mathf.Sin(pi * (q.eulerAngles.z * 0.01f + x + y + v)) * scale ;
		p.z = Mathf.Sin(pi * (q.eulerAngles.x * 0.01f + y + z + u + v)) * scale ;*/


        /*p.x = Mathf.Sin(pi * (q.y + z + u)) * scale; 
		p.y = Mathf.Sin(pi * (q.z + x + v)) *scale;
		p.z =  Mathf.Sin(pi * (q.x+ y + u + v) *scale);*/

        p.x = Mathf.Sin(pi * (y + u)) * scale;
        p.y = Mathf.Sin(pi * (z + v)) * scale;
        p.z = Mathf.Sin(pi * (x + u + v) * scale);

        return p;
	}

	public static Quaternion PosToRot(float x, float y, float z, float u, float v, float scale)
    {
		Quaternion q;
		q.x = u;
		q.y = x * 0.1f;
		q.z = y * 0.1f;
		q.w = z * 0.1f;
		return q;

	}

	public static Vector3 SphereSI(float x, float y, float z, Quaternion q, float u, float v)
	{
		Vector3 p;
		float t = q.eulerAngles.x;
		float a = 0.8f;
		a = q.eulerAngles.y;
		float b = 6f;
		b = q.eulerAngles.z;
		float c = 0.1f;
		c = x;
		float d = 0.5f;
		d = y;
		float e = 4f;
		e = z;

		float r = a + Mathf.Sin(pi * (b * u + t)) * c;
		r += Mathf.Sin(pi * (e * v + t)) * c;
		float s = r * Mathf.Cos(pi * d * v);
		p.x = s * Mathf.Sin(pi * u);
		p.y = r * Mathf.Sin(pi * d * v);
		p.z = s * Mathf.Cos(pi * u);
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
