using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexNumber : MonoBehaviour
{
    /*public float real;
    public float imaginary;*/

    public Vector2 z;

    public Vector2 multiply (Vector2 c1, Vector2 c2)
    {
        float x = c1.x;
        float y = c1.y;

        float u = c2.x;
        float v = c2.y;

        //(x + iy)(u + iv) = (xu-yv) + (xv + yu)i
        Vector2 product;
        product.x = (x * u - y * v);
        product.y = (x * v + y * u);
        return product;
    }

    public Vector2 exponentPolarCoords (Vector2 c, int exp)
    {
        //c = real + im*i
        //c = r * cos(angle) + r * i * sin(angle)

        //c*z = rc * rz * ( cos(ac + az) + i * sin(ac + az) )
        //c = vec2, let c.x = r, c.y = a, for polar coords;
        
       //c.x = r;
       //c.y = angle;
        

        float rSqrd = 1;
        float angle =0;
        for(int i = 0; i < exp; i++)
        {
            rSqrd *= c.x;
            angle += c.y;
        }
        return new Vector2(rSqrd, angle); //returns polar coords
        //return new Vector2(rSqrd * Mathf.Cos(angle), rSqrd * Mathf.Sin(angle));
    }

    public Vector2 exponentDividePolarCoords(Vector2 cTop, Vector2 cBottom)
    {
        //assumingPolarCoordinates
        //cTop.x = rad;
        //cTop.y = angle;

        float r = cTop.x / cBottom.x;
        float angle = cTop.y = cBottom.y;
        //return new Vector2(r * Mathf.Cos(angle), r * Mathf.Sin(angle) );
        return new Vector2(r, angle);
    }

    public Vector2 cartesianToPolar(Vector2 cart)
    {
        float r = Mathf.Sqrt(cart.x * cart.x + cart.y * cart.y);
        float angle = Mathf.Atan(cart.y / cart.x);
        return new Vector2(r, angle);
    }

    public Vector2 polarToCartesian(Vector2 polar)
    {
        float r = polar.x;
        float angle = polar.y;

        return new Vector2(r * Mathf.Cos(angle), r * Mathf.Sin(angle));
    }


    public Vector2 divide (Vector2 c1, Vector2 c2)
    {
        // c1/c2
        float a = c1.x;
        float b = c1.y;

        float c = c2.x;
        float d = c2.y;

        float re = (a * c + b * d) / (c * c + d * d);
        float im = ( b*c - a*d ) / (c * c + d * d);

        return new Vector2(re, im);
    }

    public float norm(Vector2 c)
    {
        return c.x * c.x + c.y * c.y;
    }
}
