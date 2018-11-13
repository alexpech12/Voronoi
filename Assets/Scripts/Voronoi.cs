using System.Collections.Generic;
using UnityEngine;

public class VoronoiSite
{
    Vector2 mPoint; public Vector2 Point { get { return mPoint; } }
    public float x { get { return mPoint.x; } }
    public float y { get { return mPoint.y; } }

    public VoronoiSite(Vector2 point)
    {
        mPoint = point;
    }

    public VoronoiSite(float x, float y)
    {
        mPoint = new Vector2(x,y);
    }

    public void Draw()
    {
        float size = 0.1f;
        Color color = Color.white;
        Debug.DrawRay(new Vector3(mPoint.x - size, mPoint.y, 0), Vector3.right * 2*size, color);
        Debug.DrawRay(new Vector3(mPoint.x, mPoint.y-size, 0), Vector3.up * 2*size, color);
        Debug.DrawRay(new Vector3(mPoint.x, mPoint.y, -size), Vector3.forward * 2*size, color);
    }
}

[System.Serializable]
public class Voronoi {

    public bool use_max_iterations = false;

    [Range(0, 100)]
    public int max_iterations = 2;

    [Range(-10f, 20.0f)]
    public float target_sweepline = 20f;

    float sweepLine; public float Sweepline { get { return sweepLine; } }
    List<VoronoiEvent> events; public List<VoronoiEvent> Events { get { return events; } }
    BeachLine beachLine; public BeachLine Beachline { get { return beachLine; } }

    public List<BeachLineEdge> DoVoronoi(List<VoronoiSite> siteList)
    {
        List<BeachLineEdge> outEdges = new List<BeachLineEdge>();

        beachLine = new BeachLine();
        events = new List<VoronoiEvent>();

        // Add site list to event list.
        foreach(VoronoiSite site in siteList)
        {
            events.Add(new SiteEvent(site));
        }

        // Sort site list.
        events.Sort();

        // Test for first two events being very close in y
        if (events.Count >= 2)
        {
            if (events[0].y - events[1].y < 0.01f)
            {
                // Add another site events far above them to prevent this from happening
                events.Insert(0, new SiteEvent(new VoronoiSite(new Vector2(0, -10))));
            }
        }

        // Start processing events
        int iterations = 0;
        while(events.Count != 0)
        {
            if (use_max_iterations && iterations == max_iterations)
            {
                break;
            }
            else
            {
                iterations++;
            }
            
            // Pop event off front of list
            VoronoiEvent thisEvent = events[0];


            // Set sweepline
            sweepLine = thisEvent.y;

            if (sweepLine >= target_sweepline)
            {
                sweepLine = target_sweepline;
                beachLine.Update(sweepLine);
                break;
            }

            beachLine.Update(sweepLine);

            if (thisEvent.IsValid())
            {
                // Check event type
                if (thisEvent is SiteEvent)
                {
                    SiteEvent siteEvent = (SiteEvent)thisEvent;

                    // Create a new arc for this event.
                    BeachLineArc newArc = new BeachLineArc(siteEvent.Site.Point, sweepLine);

                    // Find arc at this site
                    BeachLineArc arcToSplit = beachLine.SearchX(siteEvent.x);
                    if (arcToSplit == null)
                    {
                        // We could not find an arc, meaning this is the first one.
                        // Lets just add it to the beachline as root.
                        beachLine.SetRoot(newArc);
                    }
                    else
                    {
                        // Split the arc
                        arcToSplit.Split(newArc);

                        // The split will have added newArc to the tree and also created two new edges.
                        // We need to check those for intersections.
                        List<BeachLineArc> arcsToSqueeze = new List<BeachLineArc> { newArc.LeftArc, newArc.RightArc };
                        foreach (BeachLineArc arc in arcsToSqueeze)
                        {
                            CheckForNewEdgeEvents(arc);
                        }
                    }
                }
                else if (thisEvent is EdgeEvent)
                {
                    EdgeEvent edgeEvent = (EdgeEvent)thisEvent;

                    if (edgeEvent.SqueezedArc != null)
                    {
                        if (edgeEvent.SqueezedArc.LeftEdge != null && edgeEvent.SqueezedArc.RightEdge != null)
                        {

                            BeachLineEdge newEdge;
                            List<BeachLineEdge> outputEdges = edgeEvent.SqueezedArc.Squeeze(out newEdge);

                            // Add edges to output
                            foreach (BeachLineEdge edge in outputEdges)
                            {
                                outEdges.Add(edge);
                            }

                            // Squeeze function will have removed arc and edges from tree and created a new edge.
                            // We need to check that edge for any new intersections.
                            CheckForNewEdgeEvents(newEdge.LeftArc);
                            CheckForNewEdgeEvents(newEdge.RightArc);
                        }
                    }
                    // Otherwise, this must have been pre-empted but not invalidated for some reason

                }
                else
                {
                    Debug.LogError("Wrong event type! Should not happen!");
                }
            }
            events.RemoveAt(0);

        }
        if(iterations != max_iterations)
        {
            sweepLine = target_sweepline;
            beachLine.Update(sweepLine);
        }

        if(events.Count == 0)
        {
            // We completed the events list.
            // Add the remaining edges to the output.
            BeachLineElement node = beachLine.GetRoot();
            if (node != null)
            {
                // Mode node to start
                while (node.Prev != null) { node = node.Prev; }
                // Add all remaining edges
                while (node.Next != null)
                {
                    if (node is BeachLineEdge)
                    {
                        outEdges.Add(node as BeachLineEdge);
                    }
                    node = node.Next;
                }
            }
        }

        return outEdges;
    }

    bool CheckForNewEdgeEvents(BeachLineArc arc)
    {
        if (arc != null)
        {
            // Check left and right edge of this arc for intersections.
            BeachLineEdge leftEdge = arc.LeftEdge;
            BeachLineEdge rightEdge = arc.RightEdge;
            if (leftEdge != null && rightEdge != null)
            {
                // Check for edge intersection
                Vector2 intersection;
                if (leftEdge.CheckIntersection(rightEdge, out intersection))
                {
                    // These edges intersect. We need to add a new edge event.
                    
                    // This event may invalidate a previous one. Search for an existing event for this arc.
                    foreach(VoronoiEvent e in events)
                    {
                        if(e is EdgeEvent)
                        {
                            EdgeEvent edgeEvent = e as EdgeEvent;
                            if(edgeEvent.LeftEdge == leftEdge && edgeEvent.RightEdge == rightEdge)
                            {
                                e.Invalidate();
                            }
                        }
                    }

                    events.Add(new EdgeEvent(intersection, arc, leftEdge, rightEdge));
                    events.Sort();
                    return true;
                }
            }
        }
        return false;
    }

}
