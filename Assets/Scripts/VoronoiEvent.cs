using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VoronoiEvent : System.IComparable<VoronoiEvent>
{
    VoronoiSite mSite; public VoronoiSite Site { get { return mSite; } }
    public float x { get { return mSite.x; } }
    public float y { get { return mSite.y; } }

    bool valid = true;

    public VoronoiEvent(VoronoiSite site)
    {
        mSite = site;
    }

    public abstract void Draw();

    public int CompareTo(VoronoiEvent other)
    {
        return mSite.y.CompareTo(other.Site.y);
    }

    public bool IsValid() { return valid; }

    public void Invalidate() { valid = false; }
}

public class SiteEvent : VoronoiEvent
{
    public SiteEvent(VoronoiSite site) : base(site)
    {

    }

    public override void Draw()
    {
        Debug.DrawLine(new Vector3(-10, y), new Vector3(10, y), Color.red);
    }
}

public class EdgeEvent : VoronoiEvent
{
    BeachLineArc mSqueezedArc; public BeachLineArc SqueezedArc { get { return mSqueezedArc; } }
    BeachLineEdge mLeftEdge; public BeachLineEdge LeftEdge { get { return mLeftEdge; } }
    BeachLineEdge mRightEdge; public BeachLineEdge RightEdge { get { return mRightEdge; } }
    Vector2 mIntersection; public Vector2 Intersection { get { return mIntersection; } }

    public EdgeEvent(Vector2 intersection, BeachLineArc squeezedArc, BeachLineEdge leftEdge, BeachLineEdge rightEdge)
        : base(new VoronoiSite(intersection.x, 
            intersection.y + (intersection - squeezedArc.Focus).magnitude))
    {
        mSqueezedArc = squeezedArc;
        mLeftEdge = leftEdge;
        mRightEdge = rightEdge;
        mIntersection = intersection;
    }

    public override void Draw()
    {
        Color color = Color.blue;
        if (!IsValid())
        {
            color = Color.grey;
        }
        Debug.DrawLine(new Vector3(-10, y), new Vector3(10, y), color);
        DebugDrawX.DrawCross(Intersection, 0.1f, color);
    }
}