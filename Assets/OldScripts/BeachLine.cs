
#define OLD

#if !OLD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BeachLine
{
    public BeachLineElement root = null;

    public void SetDirectrix(float directrix)
    {
        if (root != null)
        {
            root.SetDirectrix(directrix);
        }
    }

    public void Add(BeachLineElement newElement)
    {
        if (root == null)
        {
            root = newElement;
        }
        else
        {
            // 
            // Find place for new element
            //if(newElement)
            //root.SetLeftChild(newElement);
        }
    }

    public BeachLineArc GetArcAtX(float x, out Vector2 intersection, out Vector2 direction)
    {
        // Traverse the tree looking for x value
        // We only need to look at leaf nodes because we are looking for arcs
        return root.GetArcAtX(x, out intersection, out direction);
    }

    public void Draw(Vector3 offset)
    {
        root.Draw(offset);
    }
}

abstract class BeachLineElement
{
    public BeachLineElement parent = null;
    public BeachLineElement leftChild = null;
    public BeachLineElement rightChild = null;

    public void SetParent(BeachLineElement newParent)
    {
        parent = newParent;
    }

    public void SetLeftChild(BeachLineElement newChild)
    {
        leftChild = newChild;
    }

    public void SetRightChild(BeachLineElement newChild)
    {
        rightChild = newChild;
    }

    public abstract void SetDirectrix(float directrix);
    public abstract void Draw(Vector3 offset);
    public abstract float GetX();
    public abstract BeachLineArc GetArcAtX(float x, out Vector2 intersection, out Vector2 direction);
}

class BeachLineArc : BeachLineElement
{
    public Arc arc;
    public BeachLineArc(Arc arc)
    {
        this.arc = arc;
    }

    public override void Draw(Vector3 offset)
    {
        if (Mathf.Abs(arc.focus.y - arc.directrix) < 0.001f)
        {
            Debug.DrawRay(arc.focus, Vector3.down, Color.red);
            return;
        }
        DrawQuadratic(arc, offset, Mathf.Clamp(arc.start_x, -5.0f, 5.0f), Mathf.Clamp(arc.end_x, -5.0f, 5.0f), 0.1f, Color.red);
    }

    public override BeachLineArc GetArcAtX(float x, out Vector2 intersection, out Vector2 direction)
    {
        // This is an arc leaf so we check the bounds for x.
        if (x >= arc.start_x && x < arc.end_x)
        {
            // This is the right arc. Set results.
            intersection = new Vector2(x, arc.y(x));
            direction = arc.Tangent(x);
            return this;
        }
        // This is not the arc we're looking for.
        intersection = Vector2.zero;
        direction = Vector2.zero;
        return null;
    }

    public override float GetX()
    {
        throw new NotImplementedException();
    }

    public override void SetDirectrix(float directrix)
    {
        this.arc.directrix = directrix;
    }

    public void Split()
    {

    }
}

class BeachLineEdge : BeachLineElement
{
    public Edge edge;
    public BeachLineEdge(Edge edge)
    {
        this.edge = edge;
    }

    public override void Draw(Vector3 offset)
    {
        edge.Draw(offset);
        leftChild.Draw(offset);
        rightChild.Draw(offset);
    }

    public override BeachLineArc GetArcAtX(float x, out Vector2 intersection, out Vector2 direction)
    {
        // This is not an arc. We just need to check child nodes
        BeachLineArc foundArc = leftChild.GetArcAtX(x, out intersection, out direction);
        if (foundArc != null)
        {
            // We found are arc. We are done and can return.
            return foundArc;
        }
        // Try right child
        return rightChild.GetArcAtX(x, out intersection, out direction);
    }

    public override float GetX()
    {
        throw new NotImplementedException();
    }

    public override void SetDirectrix(float directrix)
    {
        leftChild.SetDirectrix(directrix);
        rightChild.SetDirectrix(directrix);
    }
}
#endif