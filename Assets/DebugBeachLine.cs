using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBeachLine : MonoBehaviour {

    public BeachLineArc leftArc;
    public BeachLineEdge leftEdge;
    public BeachLineArc newArc;
    public BeachLineEdge rightEdge;
    public BeachLineArc rightArc;

    // Use this for initialization
    void Start () {
        leftArc.LinkInsertAfter(leftEdge).LinkInsertAfter(newArc).LinkInsertAfter(rightEdge).LinkInsertAfter(rightArc);
	}
	
	// Update is called once per frame
	void Update () {
        //float sweepLine = 5.0f;

        Vector2 intersection;
        if (GetValidIntersection(out intersection, leftArc, leftEdge))
        {
            // Set bounds on arc and edge
            leftEdge.SetEndpoint(intersection);
            //leftArc.SetNext(leftEdge);
            leftArc.Draw(Color.green);
            leftEdge.Draw(Color.cyan);
        }
        if (GetValidIntersection(out intersection, newArc, rightEdge))
        {
            // Set bounds on arc and edge
            rightEdge.SetEndpoint(intersection);
            //leftArc.SetNext(leftEdge);
            newArc.Draw(Color.yellow);
            rightEdge.Draw(Color.blue);
            rightArc.Draw(Color.magenta);
        }



        //newArc.Draw(sweepLine);
    }

    public void DrawCross(Vector2 point, float size, Color color)
    {
        float debugDrawSize = size;
        Color debugColor = color;
        Vector2 intersection = point;
        Debug.DrawLine(intersection + new Vector2(debugDrawSize, debugDrawSize),
            intersection + new Vector2(-debugDrawSize, -debugDrawSize), debugColor);
        Debug.DrawLine(intersection + new Vector2(-debugDrawSize, debugDrawSize),
            intersection + new Vector2(debugDrawSize, -debugDrawSize), debugColor);
    }

    public bool GetValidIntersection(out Vector2 intersection, BeachLineArc arc, BeachLineEdge edge)
    {

        //Vector3 arcCoeff = Geometry.QuadraticFocusDirectrixToABCCoefficents(arc.Focus, arc.Directrix);
        //// Coefficients = [x^2, x, y]

        // y = ax^2 + bx + c
        // => ax^2 + bx - y = c
        //
        // dx + ey = f

        // Input matrix / output vector
        // | a b -1 | c |
        // | 0 d  e | f |

        // Get line equation in homogeneous coordinates
        //Vector3 line = Vector3.Cross(new Vector3(edge.Start.x, edge.Start.y, 1), new Vector3(edge.End.x, edge.End.y, 1));

        //const int dimension = 3;
        //double[,] inputMatrix = new double[,] { 
        //    { arcCoeff.x,   arcCoeff.y,     -1      }, 
        //    { 0,            line.x,         line.y  } };
        //double[] outputVector = new double[] { arcCoeff.z, line.z };
        //SystemOfLinearEquations.SolveUsingLU(inputMatrix, outputVector, dimension);








        Vector2 i1, i2;
        int interX = Geometry.Intersection(out i1, out i2, arc, edge);


        intersection = i1;
        bool intersectionValid = false;
        if (interX != 0)
        {
            if (interX == 1)
            {
                intersection = i1;
                if (Vector2.Dot(edge.Direction, i1 - edge.Start) > 0)
                {
                    intersectionValid = true;
                }
            }
            else
            {
                bool i1_valid = false;
                bool i2_valid = false;
                if (Vector2.Dot(edge.Direction, i1 - edge.Start) > 0)
                {
                    i1_valid = true;
                    intersection = i1;
                }
                if (Vector2.Dot(edge.Direction, i2 - edge.Start) > 0)
                {
                    i2_valid = true;
                    intersection = i2;
                }
                if (i1_valid && i2_valid)
                {
                    // Use closest
                    intersection = Vector2.Distance(i1, edge.Start) < Vector2.Distance(i2, edge.Start) ? i1 : i2;
                }
                intersectionValid = i1_valid || i2_valid;

                DrawCross(i1, 0.1f, Color.blue);
                DrawCross(i2, 0.1f, Color.magenta);
            }

            if (intersectionValid)
            {
                DrawCross(intersection, 0.2f, Color.white);
                return true;
            }
        }
        return false;
    }
}

