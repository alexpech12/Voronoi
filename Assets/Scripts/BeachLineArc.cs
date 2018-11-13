using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BeachLineArc : BeachLineElement
{
    public Vector2 mFocus; public Vector2 Focus { get { return mFocus; } }
    public float mDirectrix; public float Directrix { get { return mDirectrix; } }

    public BeachLineArc LeftArc
    {
        get
        {
            if (Prev != null)
            {
                return (BeachLineArc)Prev.Prev;
            }
            return null;
        }
    }

    public BeachLineArc RightArc
    {
        get
        {
            if (Next != null)
            {
                return (BeachLineArc)Next.Next;
            }
            return null;
        }
    }

    public BeachLineEdge LeftEdge { get { return (BeachLineEdge)Prev; } }
    public BeachLineEdge RightEdge { get { return (BeachLineEdge)Next; } }

    public BeachLineArc(Vector2 focus, float directrix)
    {
        mFocus = focus;
        mDirectrix = directrix;
        colorIndex = 0;
    }

    public BeachLineArc(BeachLineArc arc)
    {
        mFocus = arc.Focus;
        mDirectrix = arc.mDirectrix;
    }

    //public BeachLineArc(BeachLineArc arc, float endX)
    //{
    //    mFocus = arc.Focus;
    //    mStartX = arc.StartX;
    //    mEndX = endX;
    //    colorIndex = 0;
    //}

    //public BeachLineArc(float startX, BeachLineArc arc)
    //{
    //    mFocus = arc.Focus;
    //    mStartX = startX;
    //    mEndX = arc.EndX;
    //    colorIndex = 0;
    //}

    public override BeachLineArc SearchX(float x)
    {
        // If the search has reached an arc, this is the one we want.
        // Just return this.
        return this;
    }

    public override void Update(float sweepLine)
    {
        mDirectrix = sweepLine;
    }

    public void SetStartX(float startX) { mStartX = startX; }
    public void SetEndX(float endX) { mEndX = endX; }

    public void Split(BeachLineArc newArc)
    {
        float splitX = newArc.Focus.x;
        float sweepLine = newArc.Focus.y;
      
        BeachLineArc newLeftArc = new BeachLineArc(this);
        BeachLineArc newRightArc = new BeachLineArc(this);
        Vector2 splitPoint = new Vector2(splitX, y(splitX));
        Vector2 edgeDirection = Tangent(splitX);
        // Make edgeDirection always point in the positive x direction
        edgeDirection = edgeDirection.x < 0 ? -edgeDirection : edgeDirection;
        BeachLineEdge newLeftEdge = new BeachLineEdge(splitPoint, -edgeDirection);
        BeachLineEdge newRightEdge = new BeachLineEdge(splitPoint, edgeDirection);

        newLeftEdge.SetLeftChild(newLeftArc);
        newLeftEdge.SetRightChild(newRightEdge);
        newLeftEdge.SetParent(Parent);
        newRightEdge.SetLeftChild(newArc);
        newRightEdge.SetRightChild(newRightArc);
        newRightEdge.SetParent(newLeftEdge);
        newLeftArc.SetParent(newLeftEdge);
        newRightArc.SetParent(newRightEdge);
        newArc.SetParent(newRightEdge);

        // Change next and prev links
        BeachLineEdge prevEdge = (BeachLineEdge)Prev;
        BeachLineEdge nextEdge = (BeachLineEdge)Next;

        LinkRemove();
        if(prevEdge != null)
        {
            prevEdge.LinkInsertAfter(newLeftArc);
        }
        else
        {
            // newLeftArc is going to be the new start of the list
            if(nextEdge != null)
            {
                newLeftArc.SetNext(nextEdge);
                nextEdge.SetPrev(newLeftArc);
            }
        }
        newLeftArc.LinkInsertAfter(newLeftEdge)
                  .LinkInsertAfter(newArc)
                  .LinkInsertAfter(newRightEdge)
                  .LinkInsertAfter(newRightArc);
        //if (nextEdge != null)
        //{
        //    newRightArc.SetNext(nextEdge);
        //    nextEdge.SetPrev(newRightArc);
        //}

        BeachLineElement parent = Parent;

        if (parent == null)
        {
            // This is root. Replace this with left edge.
            BeachLineRoot.SetRoot(newLeftEdge);
        }
        else
        {
            // Set parent to point to new construct
            if (parent.LeftChild == this)
            {
                parent.SetLeftChild(newLeftEdge);
            }
            else if (parent.RightChild == this)
            {
                parent.SetRightChild(newLeftEdge);
            }
            else
            {
                Debug.LogError("Error in Split - couldn't find child in parent.");
            }
        }

        // We can now remove this node
        //SetLeftArc(null);
        //SetRightArc(null);
        //SetLeftEdge(null);
        //SetRightEdge(null);
        SetLeftChild(null);
        SetRightChild(null);
        SetParent(null);

    }

    public List<BeachLineEdge> Squeeze(out BeachLineEdge newEdge)
    {
        // These are the output edges
        BeachLineEdge leftEdge = (BeachLineEdge)Prev;
        BeachLineEdge rightEdge = (BeachLineEdge)Next;
        BeachLineArc leftArc = (BeachLineArc)leftEdge.Prev;
        BeachLineArc rightArc = (BeachLineArc)rightEdge.Next;

        // Create new edge
        // To create it, we need the intersection point of the two output edges plus the
        // focuses of the two arcs.
        Vector2 intersection;
        leftEdge.CheckIntersection(rightEdge, out intersection);
        Vector2 focus1 = leftArc.Focus;
        Vector2 focus2 = rightArc.Focus;
        Vector2 perpendicular = focus2 - focus1;
        Vector2 newEdgeDirection = new Vector2(perpendicular.y, -perpendicular.x);
        if(Vector2.Dot(leftEdge.Direction.normalized + rightEdge.Direction.normalized, -newEdgeDirection) > 0)
        //if (newEdgeDirection.y < 0)
        {
            newEdgeDirection = -newEdgeDirection;
        }

        newEdge = new BeachLineEdge(intersection, newEdgeDirection.normalized);


        BeachLineEdge edgeToReplace = leftEdge == Parent ? rightEdge : leftEdge;
        //newEdge.SetLeftArc(LeftArc);
        //newEdge.SetRightArc(RightArc);
        // Replace other edge with new edge
        edgeToReplace.ReplaceWithSingleNode(newEdge);

        // Replace parent with sibling

        BeachLineElement sibling = Sibling;
        sibling.SetParent(null);
        bool iAmLeftChild = this == Parent.LeftChild;
        if (iAmLeftChild)
        {
            Parent.SetRightChild(null);
        }
        else
        {
            Parent.SetLeftChild(null);
        }

        BeachLineElement parentsParent = Parent.Parent;
        bool parentIsLeftChild = Parent == parentsParent.LeftChild;

        if (parentIsLeftChild)
        {
            parentsParent.SetLeftChild(sibling);
        }
        else
        {
            parentsParent.SetRightChild(sibling);
        }
        sibling.SetParent(parentsParent);

        //Parent.ReplaceWith(Sibling);

        //Parent.SetLeftChild(null);
        //Parent.SetRightChild(null);

        // Set next/prev for altered nodes
        leftArc.SetNext(newEdge);
        newEdge.SetPrev(leftArc);
        newEdge.SetNext(rightArc);
        rightArc.SetPrev(newEdge);

        // Remove this node from tree
        SetParent(null);
        SetLeftChild(null);
        SetRightChild(null);
        SetNext(null);
        SetPrev(null);
        leftEdge.SetParent(null);
        leftEdge.SetLeftChild(null);
        leftEdge.SetRightChild(null);
        leftEdge.SetNext(null);
        leftEdge.SetPrev(null);
        rightEdge.SetParent(null);
        rightEdge.SetLeftChild(null);
        rightEdge.SetRightChild(null);
        rightEdge.SetNext(null);
        rightEdge.SetPrev(null);

        leftEdge.SetEndpoint(intersection);
        rightEdge.SetEndpoint(intersection);

        List<BeachLineEdge> outputList = new List<BeachLineEdge>
        {
            leftEdge,
            rightEdge
        };

        return outputList;

    }


    public float y(float x)
    {
        return (1 / (2 * (Focus.y - Directrix))) * (x - Focus.x) * (x - Focus.x) + (Focus.y + Directrix) / 2;
    }

    public float dy(float x)
    {
        return (1 / (Focus.y - Directrix)) * (x - Focus.x);
    }

    public Vector2 Point(float x)
    {
        return new Vector2(x, y(x));
    }

    public Vector2 Tangent(float x)
    {
        //return new Vector2(x, dy(x) * x); // * x ??? 
        return new Vector2(1, dy(x));
    }

    static int colorIndex = 0;
    public override void Draw()
    {
        Color color = Color.green;
        Color[] colorArray = new Color[] { Color.red, Color.blue, Color.yellow, Color.green, Color.cyan, Color.magenta, Color.white, Color.grey };

        color = colorArray[colorIndex];
        colorIndex++;
        if (colorIndex == 8) { colorIndex = 0; }
        Draw(color);
    }
    public void Draw(Color color)
    {
        float step_size = 0.1f;
        Vector3 offset = Vector3.zero;

        if (Directrix == Focus.y)
        {
            // Vertical
            Debug.DrawRay(Focus, Vector3.down * 20, color);
        }
        else
        {

            //BeachLineArc highArc = mLeftArc;
            //BeachLineArc lowArc = mRightArc;

            //if(highArc == null && lowArc != null)
            //{
            //    return;
            //}

            BeachLineEdge prevEdge = (BeachLineEdge)Prev;
            BeachLineEdge nextEdge = (BeachLineEdge)Next;
            float startX = prevEdge != null ? (prevEdge.Start + prevEdge.Direction).x : -10;
            float endX = nextEdge != null ? (nextEdge.Start + nextEdge.Direction).x : 10;

            //float startX = -10;
            //if (lowArc != null)
            //{
            //    //Vector2 i1, i2;
            //    //int res = Geometry.Intersection2D(out i1, out i2, mFocus, sweepLine, lowArc.Focus, sweepLine);
            //    //if (res == 2)
            //    //{
            //    //    //mStartX = Mathf.Max(i1.x, i2.x);
            //    //    // Find result between these arcs
            //    //    if (lowArc.Focus.x < i1.x && i1.x < Focus.x)
            //    //    {
            //    //        startX = i1.x;
            //    //    }
            //    //    else if (lowArc.Focus.x < i2.x && i2.x < Focus.x)
            //    //    {
            //    //        startX = i2.x;
            //    //    }
            //    //}
            //    startX = mStartX;
            //}

            //float endX = 10;
            //if (highArc != null)
            //{
            //    //Vector2 i1, i2;
            //    //int res = Geometry.Intersection2D(out i1, out i2, mFocus, sweepLine, highArc.Focus, sweepLine);
            //    //if (res == 2)
            //    //{
            //    //    //mEndX = Mathf.Min(i1.x, i2.x);
            //    //    // Find result between these arcs
            //    //    if (Focus.x < i1.x && i1.x < highArc.Focus.x)
            //    //    {
            //    //        endX = i1.x;
            //    //    }
            //    //    else if (Focus.x < i2.x && i2.x < highArc.Focus.x)
            //    //    {
            //    //        endX = i2.x;
            //    //    }
            //    //}
            //    endX = mEndX;
            //}


            int lines = Mathf.RoundToInt((endX - startX) / step_size);

            float t0 = 0f;
            float x0 = Mathf.Lerp(startX, endX, t0);
            float y0 = y(x0);
            for (int i = 0; i < lines; i++)
            {
                float t1 = (float)(i + 1) / lines;
                float x1 = Mathf.Lerp(startX, endX, t1);
                float y1 = y(x1);
                if (y1 == float.PositiveInfinity)
                {
                    y1 = float.MaxValue;
                }
                if (y1 == float.NegativeInfinity)
                {
                    y1 = float.MinValue;
                }

                Vector3 start = offset + new Vector3(x0, y0, 0);
                Vector3 end = offset + new Vector3(x1, y1, 0);

                Debug.DrawLine(start, end, color);

                t0 = t1;
                x0 = x1;
                y0 = y1;
            }
        }
    }

    public void Draw2(Color color)
    {
        Vector3 coeff = Geometry.QuadraticFocusDirectrixToABCCoefficents(Focus, Directrix);
        float a = coeff.x;
        float b = coeff.y;
        float c = coeff.z;
        float t = 0f;
        while(t < 10f)
        {

            Vector2 point = new Vector2(t, a * t * t + b * t + c);
            t += 0.1f;
            Vector2 nextPoint = new Vector2(t, a * t * t + b * t + c);
            Debug.DrawLine(point, nextPoint, color);
        }
    }
}

