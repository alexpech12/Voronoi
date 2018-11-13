using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class BeachLineElement
{
    BeachLine mBeachLine         = null; public BeachLine BeachLineRoot { get { return mBeachLine; } }

    BeachLineElement mParent     = null; public BeachLineElement Parent { get { return mParent; } }
    BeachLineElement mLeftChild  = null; public BeachLineElement LeftChild { get { return mLeftChild; } }
    BeachLineElement mRightChild = null; public BeachLineElement RightChild { get { return mRightChild; } }
    BeachLineElement mPrev       = null; public BeachLineElement Prev { get { return mPrev; } }
    BeachLineElement mNext       = null; public BeachLineElement Next { get { return mNext; } }

    public BeachLineElement Sibling
    {
        get
        {
            if (mParent == null)
            {
                return null;
            }
            if (mParent.LeftChild == this)
            {
                return mParent.RightChild;
            }
            else if (mParent.RightChild == this)
            {
                return mParent.LeftChild;
            }
            return null;
        }
    }

    protected float mStartX = -10; public float StartX { get { return mStartX; } }
    protected float mEndX = 10; public float EndX { get { return mEndX; } }

    public abstract BeachLineArc SearchX(float x);

    public abstract void Update(float sweepLine);

    public BeachLineElement GetRoot()
    {
        if (Parent == null)
        {
            return this;
        }
        return Parent.GetRoot();
    }

    public void SetBeachLine(BeachLine beachLine)
    {
        mBeachLine = beachLine;
    }

    public void SetLeftChild(BeachLineElement newLeftChild)
    {
        mLeftChild = newLeftChild;
    }

    public void SetRightChild(BeachLineElement newRightChild)
    {
        mRightChild = newRightChild;
    }

    public void SetParent(BeachLineElement newParent)
    {
        mParent = newParent;
    }

    public void LinkRemove()
    {
        if(Prev != null)
        {
            Prev.SetNext(Next);
        }
        if(Next != null)
        {
            Next.SetPrev(Prev);
        }
        SetNext(null);
        SetPrev(null);
    }

    public BeachLineElement LinkInsertAfter(BeachLineElement newElement)
    {
        if (Next != null)
        {
            Next.SetPrev(newElement);
            newElement.SetNext(Next);
        }
        SetNext(newElement);
        newElement.SetPrev(this);
        return newElement;
    }

    public void SetNext(BeachLineElement next) { mNext = next; }
    public void SetPrev(BeachLineElement prev) { mPrev = prev; }

    public void ReplaceWithSingleNode(BeachLineElement newNode)
    {
        if (Parent != null)
        {
            newNode.SetParent(Parent);
            if (Parent.LeftChild == this)
            {
                Parent.SetLeftChild(newNode);
            }
            else
            {
                Parent.SetRightChild(newNode);
            }
        }
        else
        {
            // This is the root node
            mBeachLine.SetRoot(newNode);
        }

        if (LeftChild != null)
        {
            newNode.SetLeftChild(LeftChild);
            LeftChild.SetParent(newNode);
        }
        if (RightChild != null)
        {
            newNode.SetRightChild(RightChild);
            RightChild.SetParent(newNode);
        }
        mParent = null;
        mLeftChild = null;
        mRightChild = null;
    }

    public void ReplaceWithSubTree(BeachLineElement newSubtreeParentNode)
    {
        if (Parent != null)
        {
            newSubtreeParentNode.SetParent(Parent);
            if (Parent.LeftChild == this)
            {
                Parent.SetLeftChild(newSubtreeParentNode);
            }
            else
            {
                Parent.SetRightChild(newSubtreeParentNode);
            }
        }
    }

    public abstract void Draw();
    public void DrawTree(Vector3 offset, int iteration)
    {

        Color color = this is BeachLineArc ? Color.green : Color.cyan;
        // Draw square
        float square_size = 0.5f;
        Debug.DrawRay(new Vector3(-square_size, -square_size, 0) + offset, 2 * Vector3.right * square_size, color);
        Debug.DrawRay(new Vector3(-square_size, square_size, 0) + offset, 2 * Vector3.right * square_size, color);
        Debug.DrawRay(new Vector3(-square_size, square_size, 0) + offset, 2 * Vector3.down * square_size, color);
        Debug.DrawRay(new Vector3(square_size, square_size, 0) + offset, 2 * Vector3.down * square_size, color);

        float child_x_offset = 4f / Mathf.Pow(2, iteration);
        Vector3 left_child_offset = new Vector3(offset.x + child_x_offset, offset.y - 1.5f, 0);
        Vector3 right_child_offset = new Vector3(offset.x - child_x_offset, offset.y - 1.5f, 0);
        if (LeftChild != null)
        {
            Debug.DrawLine(
                offset + (new Vector3(0, -square_size, 0)),
                left_child_offset + (new Vector3(0, square_size, 0)),
                Color.white);
            LeftChild.DrawTree(left_child_offset, iteration + 1);
        }
        if (RightChild != null)
        {
            Debug.DrawLine(
                offset + (new Vector3(0, -square_size, 0)),
                right_child_offset + (new Vector3(0, square_size, 0)),
                Color.white);
            RightChild.DrawTree(right_child_offset, iteration + 1);
        }
    }
}
