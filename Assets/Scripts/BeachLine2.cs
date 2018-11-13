using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachLine
{
    BeachLineElement mRoot = null;

    public BeachLine() { }

    public BeachLineElement GetRoot() { return mRoot; }

    public void SetRoot(BeachLineElement root)
    {
        mRoot = root;
        mRoot.SetBeachLine(this);
    }

    public BeachLineArc SearchX(float x)
    {
        // Traverse child nodes to find arc at this x value
        if (mRoot != null)
        {
            return mRoot.SearchX(x);
        }
        return null;
    }

    public void Update(float sweepLine)
    {
        // Traverse child nodes to update sweepline for each one
        if (mRoot != null)
        {
            mRoot.Update(sweepLine);
        }
    }

    public void Draw()
    {
        if (mRoot != null)
        {
            mRoot.Draw();
            //mRoot.DrawTree(new Vector3(-15, 0, 0), 0);
        }
    }
}

//public abstract class BeachLineElement
//{
//    BeachLine mBeachLine; public BeachLine BeachLineRoot { get { return mBeachLine; } }
//    BeachLineElement mParent; public BeachLineElement Parent { get { return mParent; } }
//    BeachLineElement mLeftChild; public BeachLineElement LeftChild { get { return mLeftChild; } }
//    BeachLineElement mRightChild; public BeachLineElement RightChild { get { return mRightChild; } }
//    BeachLineElement mPrev; public BeachLineElement Prev {  get { return mPrev; } }
//    BeachLineElement mNext; public BeachLineElement Next {  get { return mNext; } }

//    public BeachLineElement Sibling
//    {
//        get
//        {
//            if (mParent == null)
//            {
//                return null;
//            }
//            if (mParent.LeftChild == this)
//            {
//                return mParent.RightChild;
//            }
//            else if (mParent.RightChild == this)
//            {
//                return mParent.LeftChild;
//            }
//            return null;
//        }
//    }

//    protected float mStartX = -10; public float StartX { get { return mStartX; } }
//    protected float mEndX = 10; public float EndX { get { return mEndX; } }

//    public abstract BeachLineArc SearchX(float x);

//    public abstract void Update(float sweepLine);

//    public BeachLineElement GetRoot()
//    {
//        if (Parent == null)
//        {
//            return this;
//        }
//        return Parent.GetRoot();
//    }

//    public void SetBeachLine(BeachLine beachLine)
//    {
//        mBeachLine = beachLine;
//    }

//    public void SetLeftChild(BeachLineElement newLeftChild)
//    {
//        mLeftChild = newLeftChild;
//    }

//    public void SetRightChild(BeachLineElement newRightChild)
//    {
//        mRightChild = newRightChild;
//    }

//    public void SetParent(BeachLineElement newParent)
//    {
//        mParent = newParent;
//    }

//    public void ReplaceWithSingleNode(BeachLineElement newNode)
//    {
//        if (Parent != null)
//        {
//            newNode.SetParent(Parent);
//            if (Parent.LeftChild == this)
//            {
//                Parent.SetLeftChild(newNode);
//            }
//            else
//            {
//                Parent.SetRightChild(newNode);
//            }
//        }
//        if (LeftChild != null)
//        {
//            newNode.SetLeftChild(LeftChild);
//            LeftChild.SetParent(newNode);
//        }
//        if (RightChild != null)
//        {
//            newNode.SetRightChild(RightChild);
//            RightChild.SetParent(newNode);
//        }
//        mParent = null;
//        mLeftChild = null;
//        mRightChild = null;
//    }

//    public void ReplaceWithSubTree(BeachLineElement newSubtreeParentNode)
//    {
//        if (Parent != null)
//        {
//            newSubtreeParentNode.SetParent(Parent);
//            if (Parent.LeftChild == this)
//            {
//                Parent.SetLeftChild(newSubtreeParentNode);
//            }
//            else
//            {
//                Parent.SetRightChild(newSubtreeParentNode);
//            }
//        }
//    }

//    public abstract void Draw(float sweepLine);
//    public void DrawTree(Vector3 offset, int iteration)
//    {

//        Color color = this is BeachLineArc ? Color.green : Color.cyan;
//        // Draw square
//        float square_size = 0.5f;
//        Debug.DrawRay(new Vector3(-square_size, -square_size, 0) + offset, 2 * Vector3.right * square_size, color);
//        Debug.DrawRay(new Vector3(-square_size, square_size, 0) + offset, 2 * Vector3.right * square_size, color);
//        Debug.DrawRay(new Vector3(-square_size, square_size, 0) + offset, 2 * Vector3.down * square_size, color);
//        Debug.DrawRay(new Vector3(square_size, square_size, 0) + offset, 2 * Vector3.down * square_size, color);

//        float child_x_offset = 4f / Mathf.Pow(2, iteration);
//        Vector3 left_child_offset = new Vector3(offset.x + child_x_offset, offset.y - 1.5f, 0);
//        Vector3 right_child_offset = new Vector3(offset.x - child_x_offset, offset.y - 1.5f, 0);
//        if (LeftChild != null)
//        {
//            Debug.DrawLine(
//                offset + (new Vector3(0, -square_size, 0)),
//                left_child_offset + (new Vector3(0, square_size, 0)),
//                Color.white);
//            LeftChild.DrawTree(left_child_offset, iteration + 1);
//        }
//        if (RightChild != null)
//        {
//            Debug.DrawLine(
//                offset + (new Vector3(0, -square_size, 0)),
//                right_child_offset + (new Vector3(0, square_size, 0)),
//                Color.white);
//            RightChild.DrawTree(right_child_offset, iteration + 1);
//        }
//    }
//}

//public class BeachLineArc : BeachLineElement
//{
//    Vector2 mFocus; public Vector2 Focus { get { return mFocus; } }
//    float mDirectrix; public float Directrix { get { return mDirectrix; } }
//    BeachLineArc mLeftArc, mRightArc;
//    public BeachLineArc LeftArc { get { return mLeftArc; } }
//    public BeachLineArc RightArc { get { return mRightArc; } }
//    BeachLineEdge mLeftEdge, mRightEdge;
//    public BeachLineEdge LeftEdge { get { return mLeftEdge; } }
//    public BeachLineEdge RightEdge { get { return mRightEdge; } }

//    public BeachLineArc(Vector2 focus)
//    {
//        mFocus = focus;
//        colorIndex = 0;
//    }

//    public BeachLineArc(BeachLineArc arc, float endX)
//    {
//        mFocus = arc.Focus;
//        mStartX = arc.StartX;
//        mEndX = endX;
//        colorIndex = 0;
//    }

//    public BeachLineArc(float startX, BeachLineArc arc)
//    {
//        mFocus = arc.Focus;
//        mStartX = startX;
//        mEndX = arc.EndX;
//        colorIndex = 0;
//    }

//    public override void Update(float sweepLine)
//    {
//        mDirectrix = sweepLine;
        
//    }

//    public void SetStartX(float startX) { mStartX = startX; }
//    public void SetEndX(float endX) { mEndX = endX; }

//    public void SetLeftArc(BeachLineArc newLeftArc)
//    {
//        mLeftArc = newLeftArc;
//    }

//    public void SetRightArc(BeachLineArc newRightArc)
//    {
//        mRightArc = newRightArc;
//    }

//    public void SetLeftEdge(BeachLineEdge newLeftEdge)
//    {
//        mLeftEdge = newLeftEdge;
//    }

//    public void SetRightEdge(BeachLineEdge newRightEdge)
//    {
//        mRightEdge = newRightEdge;
//    }

//    public void Split(BeachLineArc newArc, float sweepLine)
//    {
//        float splitX = newArc.Focus.x;
//        BeachLineArc newLeftArc = new BeachLineArc(this, splitX);
//        BeachLineArc newRightArc = new BeachLineArc(splitX, this);
//        Vector2 splitPoint = new Vector2(splitX, y(splitX, sweepLine));
//        Vector2 edgeDirection = Tangent(splitX, sweepLine);
//        BeachLineEdge newLeftEdge = new BeachLineEdge(splitPoint, edgeDirection);
//        BeachLineEdge newRightEdge = new BeachLineEdge(splitPoint, -edgeDirection);

//        newLeftEdge.SetLeftChild(newLeftArc);
//        newLeftEdge.SetRightChild(newRightEdge);
//        newLeftEdge.SetParent(Parent);
//        newLeftEdge.SetLeftArc(newLeftArc);
//        newLeftEdge.SetRightArc(newArc);

//        newRightEdge.SetLeftChild(newArc);
//        newRightEdge.SetRightChild(newRightArc);
//        newRightEdge.SetParent(newLeftEdge);
//        newRightEdge.SetLeftArc(newArc);
//        newRightEdge.SetRightArc(newRightArc);

//        newLeftArc.SetLeftArc(LeftArc);
//        newLeftArc.SetLeftEdge(LeftEdge);
//        newLeftArc.SetRightArc(newArc);
//        newLeftArc.SetRightEdge(newLeftEdge);
//        newLeftArc.SetParent(newLeftEdge);

//        newRightArc.SetLeftArc(newArc);
//        newRightArc.SetLeftEdge(newRightEdge);
//        newRightArc.SetRightArc(RightArc);
//        newRightArc.SetRightEdge(RightEdge);
//        newRightArc.SetParent(newRightEdge);

//        newArc.SetLeftArc(newLeftArc);
//        newArc.SetRightArc(newRightArc);
//        newArc.SetLeftEdge(newLeftEdge);
//        newArc.SetRightEdge(newRightEdge);
//        newArc.SetParent(newRightEdge);

//        BeachLineElement parent = Parent;

//        if (parent == null)
//        {
//            // This is root. Replace this with left edge.
//            BeachLineRoot.SetRoot(newLeftEdge);
//        }
//        else
//        {
//            // Set parent to point to new construct
//            if (parent.LeftChild == this)
//            {
//                parent.SetLeftChild(newLeftEdge);
//            }
//            else if (parent.RightChild == this)
//            {
//                parent.SetRightChild(newLeftEdge);
//            }
//            else
//            {
//                Debug.LogError("Error in Split - couldn't find child in parent.");
//            }
//        }

//        // We can now remove this node
//        SetLeftArc(null);
//        SetRightArc(null);
//        SetLeftEdge(null);
//        SetRightEdge(null);
//        SetLeftChild(null);
//        SetRightChild(null);
//        SetParent(null);

//    }

//    public List<BeachLineEdge> Squeeze(out BeachLineEdge newEdge)
//    {
//        // These are the output edges
//        BeachLineEdge leftEdge = GetLeftEdge();
//        BeachLineEdge rightEdge = GetRightEdge();

//        // Create new edge
//        // To create it, we need the intersection point of the two output edges plus the
//        // focuses of the two arcs.
//        Vector2 intersection;
//        leftEdge.CheckIntersection(rightEdge, out intersection);
//        Vector2 focus1 = GetLeftArc().Focus;
//        Vector2 focus2 = GetRightArc().Focus;
//        Vector2 perpendicular = focus2 - focus1;
//        Vector2 newEdgeDirection = new Vector2(perpendicular.y, -perpendicular.x);
//        if (newEdgeDirection.y < 0)
//        {
//            newEdgeDirection = -newEdgeDirection;
//        }

//        newEdge = new BeachLineEdge(intersection, newEdgeDirection.normalized);


//        BeachLineEdge edgeToReplace = leftEdge == Parent ? rightEdge : leftEdge;
//        newEdge.SetLeftArc(LeftArc);
//        newEdge.SetRightArc(RightArc);
//        // Replace other edge with new edge
//        edgeToReplace.ReplaceWithSingleNode(newEdge);

//        // Replace parent with sibling

//        BeachLineElement sibling = Sibling;
//        sibling.SetParent(null);
//        bool iAmLeftChild = this == Parent.LeftChild;
//        if (iAmLeftChild)
//        {
//            Parent.SetRightChild(null);
//        }
//        else
//        {
//            Parent.SetLeftChild(null);
//        }

//        BeachLineElement parentsParent = Parent.Parent;
//        bool parentIsLeftChild = Parent == parentsParent.LeftChild;



//        if (parentIsLeftChild)
//        {
//            parentsParent.SetLeftChild(sibling);
//        }
//        else
//        {
//            parentsParent.SetRightChild(sibling);
//        }
//        sibling.SetParent(parentsParent);

//        //Parent.ReplaceWith(Sibling);

//        //Parent.SetLeftChild(null);
//        //Parent.SetRightChild(null);

//        // Remove this node from tree
//        SetParent(null);
//        SetLeftChild(null);
//        SetRightChild(null);
//        leftEdge.SetParent(null);
//        leftEdge.SetLeftChild(null);
//        leftEdge.SetRightChild(null);
//        rightEdge.SetParent(null);
//        rightEdge.SetLeftChild(null);
//        rightEdge.SetRightChild(null);

//        leftEdge.SetEndpoint(intersection);
//        rightEdge.SetEndpoint(intersection);

//        List<BeachLineEdge> outputList = new List<BeachLineEdge>
//        {
//            leftEdge,
//            rightEdge
//        };

//        return outputList;

//    }

//    public override BeachLineArc SearchX(float x)
//    {
//        // If the search has reached an arc, this is the one we want.
//        // Just return this.
//        return this;
//    }

//    public float y(float x, float sweepLine)
//    {
//        return (1 / (2 * (Focus.y - sweepLine))) * (x - Focus.x) * (x - Focus.x) + (Focus.y + sweepLine) / 2;
//    }

//    public float dy(float x, float sweepLine)
//    {
//        return (1 / (Focus.y - sweepLine)) * (x - Focus.x);
//    }

//    public Vector2 Point(float x, float sweepLine)
//    {
//        return new Vector2(x, y(x, sweepLine));
//    }

//    public Vector2 Tangent(float x, float sweepLine)
//    {
//        return new Vector2(x, dy(x, sweepLine) * x);
//    }

//    static int colorIndex = 0;
//    public override void Draw(float sweepLine)
//    {
//        float step_size = 0.1f;
//        Vector3 offset = Vector3.zero;
//        Color color = Color.green;
//        Color[] colorArray = new Color[] { Color.red, Color.blue, Color.yellow, Color.green, Color.cyan, Color.magenta, Color.white, Color.grey };

//        color = colorArray[colorIndex];
//        colorIndex++;
//        if (colorIndex == 8) { colorIndex = 0; }

//        if (sweepLine == Focus.y)
//        {
//            // Vertical
//            Debug.DrawRay(Focus, Vector3.down * 20, color);
//        }
//        else
//        {

//            BeachLineArc highArc = mLeftArc;
//            BeachLineArc lowArc = mRightArc;

//            //if(highArc == null && lowArc != null)
//            //{
//            //    return;
//            //}

//            float startX = -10;
//            if (lowArc != null)
//            {
//                //Vector2 i1, i2;
//                //int res = Geometry.Intersection2D(out i1, out i2, mFocus, sweepLine, lowArc.Focus, sweepLine);
//                //if (res == 2)
//                //{
//                //    //mStartX = Mathf.Max(i1.x, i2.x);
//                //    // Find result between these arcs
//                //    if (lowArc.Focus.x < i1.x && i1.x < Focus.x)
//                //    {
//                //        startX = i1.x;
//                //    }
//                //    else if (lowArc.Focus.x < i2.x && i2.x < Focus.x)
//                //    {
//                //        startX = i2.x;
//                //    }
//                //}
//                startX = mStartX;
//            }

//            float endX = 10;
//            if (highArc != null)
//            {
//                //Vector2 i1, i2;
//                //int res = Geometry.Intersection2D(out i1, out i2, mFocus, sweepLine, highArc.Focus, sweepLine);
//                //if (res == 2)
//                //{
//                //    //mEndX = Mathf.Min(i1.x, i2.x);
//                //    // Find result between these arcs
//                //    if (Focus.x < i1.x && i1.x < highArc.Focus.x)
//                //    {
//                //        endX = i1.x;
//                //    }
//                //    else if (Focus.x < i2.x && i2.x < highArc.Focus.x)
//                //    {
//                //        endX = i2.x;
//                //    }
//                //}
//                endX = mEndX;
//            }


//            int lines = Mathf.RoundToInt((endX - startX) / step_size);

//            float t0 = 0f;
//            float x0 = Mathf.Lerp(startX, endX, t0);
//            float y0 = y(x0, sweepLine);
//            for (int i = 0; i < lines; i++)
//            {
//                float t1 = (float)(i + 1) / lines;
//                float x1 = Mathf.Lerp(startX, endX, t1);
//                float y1 = y(x1, sweepLine);
//                if (y1 == float.PositiveInfinity)
//                {
//                    y1 = float.MaxValue;
//                }
//                if (y1 == float.NegativeInfinity)
//                {
//                    y1 = float.MinValue;
//                }

//                Vector3 start = offset + new Vector3(x0, y0, 0);
//                Vector3 end = offset + new Vector3(x1, y1, 0);

//                Debug.DrawLine(start, end, color);

//                t0 = t1;
//                x0 = x1;
//                y0 = y1;
//            }
//        }
//    }

//    public override BeachLineArc GetLeftLeaf()
//    {
//        return this;
//    }

//    public override BeachLineArc GetRightLeaf()
//    {
//        return this;
//    }
//}

//public class BeachLineEdge : BeachLineElement
//{
//    Vector2 mStart; public Vector2 Start { get { return mStart; } }
//    Vector2 mDirection; public Vector2 Direction { get { return mDirection; } }

//    BeachLineArc mLeftArc, mRightArc;
//    public BeachLineArc LeftArc { get { return mLeftArc; } }
//    public BeachLineArc RightArc { get { return mRightArc; } }

//    public void SetLeftArc(BeachLineArc newLeftArc)
//    {
//        mLeftArc = newLeftArc;
//    }

//    public void SetRightArc(BeachLineArc newRightArc)
//    {
//        mRightArc = newRightArc;
//    }

//    public BeachLineEdge(Vector2 start, Vector2 direction)
//    {
//        mStart = start;
//        mDirection = direction;
//    }

//    public override void Update(float sweepLine)
//    {
//        LeftArc.Update(sweepLine);
//        RightArc.Update(sweepLine);
//        // Get arcs on either left or right and find intersection between it and this edge.
//        Vector2 i1, i2;
//        Vector3 arcABC = Geometry.QuadraticFocusDirectrixToABCCoefficents(LeftArc.Focus, LeftArc.Directrix);
//        Vector3 lineAB = Geometry.RayToABCoefficients(mStart, mDirection);
//        int intNum = Geometry.Intersection2D(out i1, out i2, arcABC.x, arcABC.y, arcABC.z, 0, lineAB.x, lineAB.y);
//        if (intNum == 0)
//        {
//            mDirection = mDirection.normalized;
//        }
//        else if (intNum == 1)
//        {
//            mDirection = i1 - mStart;
//        }
//        else
//        {
//            // One of these intersections will lie on the direction vector.
//            // We will find the closest and then scale the vector.
//            float dist1 = Vector2.Distance((i1 - mStart).normalized, mDirection.normalized);
//            float dist2 = Vector2.Distance((i2 - mStart).normalized, mDirection.normalized);
//            if (dist1 < dist2)
//            {
//                mDirection = i1 - mStart;
//            }
//            else
//            {
//                mDirection = i2 - mStart;
//            }
//        }

//        // Also update the arcs here
//        LeftArc.SetEndX((mStart + mDirection).x);
//        RightArc.SetStartX((mStart + mDirection).x);
//    }

//    public float Length(float sweepLine)
//    {
//        return 1f;
//    }

//    public void SetEndpoint(Vector2 endPoint)
//    {
//        mDirection = endPoint - mStart;
//    }

//    public bool CheckIntersection(BeachLineEdge otherEdge, out Vector2 intersection)
//    {
//        //// Check if edges intersect. If they do, return true and set Vector2 intersection.
//        //// Get line equations
//        //Vector3 l1 = Vector3.Cross(
//        //    new Vector3(1, mStart.x, mStart.y), 
//        //    new Vector3(1, (mStart+mDirection).x, (mStart + mDirection).y));
//        //Vector3 l2 = Vector3.Cross(
//        //    new Vector3(1, otherEdge.Start.x, otherEdge.Start.y), 
//        //    new Vector3(1, (otherEdge.Start + otherEdge.Direction).x, (otherEdge.Start + otherEdge.Direction).y));

//        //// Find intersection
//        //float a1, b1, c1, a2, b2, c2;
//        //a1 = l1.x;
//        //b1 = l1.y;
//        //c1 = l1.z;
//        //a2 = l2.x;
//        //b2 = l2.y;
//        //c2 = l2.z;

//        //intersection = new Vector2(
//        //    (b1 * c2 - b2 * c1) / (a1 * b2 - a2 * b1),
//        //    (a1 * c2 - a2 * c1) / (a2 * b1 - a1 * b2));

//        intersection = Geometry.Intersection2D(mStart, mDirection, otherEdge.mStart, otherEdge.mDirection);

//        // Check if intersection is in right direction
//        if (intersection.x >= mStart.x && mDirection.x >= 0 || intersection.x < mStart.x && mDirection.x < 0)
//        {
//            // first edge intersection is valid
//            if (intersection.x >= otherEdge.Start.x && otherEdge.Direction.x >= 0 || intersection.x < otherEdge.Start.x && otherEdge.Direction.x < 0)
//            {
//                // second edge intersection is valid
//                float debugDrawSize = 0.1f;
//                Color debugColor = Color.blue;
//                Debug.DrawLine(intersection + new Vector2(debugDrawSize, debugDrawSize),
//                    intersection + new Vector2(-debugDrawSize, -debugDrawSize), debugColor);
//                Debug.DrawLine(intersection + new Vector2(-debugDrawSize, debugDrawSize),
//                    intersection + new Vector2(debugDrawSize, -debugDrawSize), debugColor);
//                return true;
//            }
//        }
//        return false;

//    }

//    public override void Draw(float sweepLine)
//    {
//        //Debug.DrawRay(new Vector3(mStart.x, mStart.y, 0), new Vector3(mDirection.x, mDirection.y, 0) * Length(sweepLine), Color.cyan);
//        Vector2 i1, i2;

//        int iNum = Geometry.Intersection2D(out i1, out i2, RightArc.Focus, sweepLine, LeftArc.Focus, sweepLine);
//        bool i1_intersects = false;
//        bool i2_intersects = false;
//        Vector2 end = mStart + mDirection.normalized;
//        if (mDirection.x > 0)
//        {
//            if (i1.x > mStart.x)
//            {
//                i1_intersects = true;
//                end = i1;
//            }
//            if (i2.x > mStart.x)
//            {
//                i2_intersects = true;
//                end = i2;
//            }
//            if (i1_intersects && i2_intersects)
//            {
//                if (i1.x < i2.x)
//                {
//                    end = i1;
//                }
//                else
//                {
//                    end = i2;
//                }
//            }
//        }
//        else
//        {
//            if (i1.x < mStart.x)
//            {
//                i1_intersects = true;
//                end = i1;
//            }
//            if (i2.x < mStart.x)
//            {
//                i2_intersects = true;
//                end = i2;
//            }
//            if (i1_intersects && i2_intersects)
//            {
//                if (i1.x < i2.x)
//                {
//                    end = i2;
//                }
//                else
//                {
//                    end = i1;
//                }
//            }
//        }

//        Debug.DrawLine(mStart, end, Color.cyan);

//        if (LeftChild != null)
//        {
//            LeftChild.Draw(sweepLine);
//        }
//        if (RightChild != null)
//        {
//            RightChild.Draw(sweepLine);
//        }
//    }

//    //public BeachLineArc GetLeftArc()
//    //{
//    //    return LeftChild.GetRightLeaf();
//    //}

//    //public BeachLineArc GetRightArc()
//    //{
//    //    return RightChild.GetLeftLeaf();
//    //}

//    public override BeachLineArc SearchX(float x)
//    {
//        // Left points in positive x direction
//        // If start point is less than x and pointing in positive direction, go left
//        if (mStart.x < x && mDirection.x > 0)
//        {
//            // Go left
//            return LeftChild.SearchX(x);
//        }
//        else
//        {
//            // Go right
//            return RightChild.SearchX(x);
//        }
//    }

//    public override BeachLineArc GetLeftLeaf()
//    {
//        return LeftChild.GetLeftLeaf();
//    }

//    public override BeachLineArc GetRightLeaf()
//    {
//        return RightChild.GetRightLeaf();
//    }
//}