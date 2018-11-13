using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BeachLineEdge : BeachLineElement
{
    public Vector2 mStart; public Vector2 Start { get { return mStart; } }
    public Vector2 mDirection; public Vector2 Direction { get { return mDirection; } }

    public BeachLineArc LeftArc { get { return (BeachLineArc)Prev; } }
    public BeachLineArc RightArc { get { return (BeachLineArc)Next; } }

    public BeachLineEdge(Vector2 start, Vector2 direction)
    {
        mStart = start;
        mDirection = direction;
    }

    public override void Update(float sweepLine)
    {
        LeftChild.Update(sweepLine);
        RightChild.Update(sweepLine);

        // Get arcs on either left or right and find intersection between it and this edge.
        Vector2 intersection;
        if(LeftArc == null || RightArc == null)
        {
            Debug.LogError("Should not happen!");
        }
        if(Geometry.GetValidIntersection(out intersection, LeftArc, this))
        {
            SetEndpoint(intersection);
        }
        else if(Geometry.GetValidIntersection(out intersection, RightArc, this))
        {
            SetEndpoint(intersection);
        }
        else
        {
            //throw new System.Exception("Invalid intersection between edge (" + mStart + "," + mDirection + ") and arc (" + LeftArc.Focus + "," + LeftArc.Directrix + ")");
            mDirection = mDirection.normalized;
        }

        // Also update the arcs here
        //LeftArc.SetEndX((mStart + mDirection).x);
        //RightArc.SetStartX((mStart + mDirection).x);
    }
    
    public override BeachLineArc SearchX(float x)
    {
        // The endpoint represents the point between the left and right arc
        float endX = mStart.x + mDirection.x;
        if (x <= endX)
        {
            // Go left
            return LeftChild.SearchX(x);
        }
        else
        {
            // Go right
            return RightChild.SearchX(x);
        }
    }

    public float Length
    {
        get
        {
            return mDirection.magnitude;
        }
    }

    public Vector2 End
    {
        get
        {
            return mStart + mDirection.normalized * Length;
        }
    }

    public void SetEndpoint(Vector2 endPoint)
    {
        mDirection = endPoint - mStart;
    }

    public bool CheckIntersection(BeachLineEdge otherEdge, out Vector2 intersection)
    {
        intersection = Geometry.Intersection2D(mStart, mDirection, otherEdge.mStart, otherEdge.mDirection);

        // Check if intersection is in right direction
        if (intersection.x >= mStart.x && mDirection.x >= 0 || intersection.x < mStart.x && mDirection.x < 0)
        {
            // first edge intersection is valid
            if (intersection.x >= otherEdge.Start.x && otherEdge.Direction.x >= 0 || intersection.x < otherEdge.Start.x && otherEdge.Direction.x < 0)
            {
                // second edge intersection is valid
                return true;
            }
        }
        return false;

    }

    public override void Draw()
    {
        Draw(Color.cyan);
    }

    public void Draw(Color color)
    {
        Debug.DrawLine(Start, End, color);

        if (LeftChild != null)
        {
            LeftChild.Draw();
        }
        if (RightChild != null)
        {
            RightChild.Draw();
        }
    }

    public void Draw2(Color color)
    {
        Vector2 coeff = Geometry.RayToABCoefficients(Start, Direction);
        float m = coeff.x;
        float c = coeff.y;
        float t = 0f;
        while (t < 10f)
        {

            Vector2 point = new Vector2(t, m * t + c);
            t += 0.1f;
            Vector2 nextPoint = new Vector2(t, m * t + c);
            Debug.DrawLine(point, nextPoint, color);
        }
    }

}