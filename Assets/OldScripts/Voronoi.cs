
#define OLD

#if !OLD
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Voronoi : MonoBehaviour {

    public float x_range = 10;
    public float y_range = 10;

    public float edge_size = 2;

    public int num_points = 5;
    public float point_size = 0.5f;
    public float line_size = 0.2f;

    public bool randomize = false;
    public int seed = 1;

    public List<Vector2> points;
    List<Transform> point_objects;
    List<Transform> line_objects;

    public int max_iterations = 2;





    class VoronoiEvent : IComparable<VoronoiEvent>
    {
        public float y_value;
        public Vector2 site;
        bool site_event;

        public VoronoiEvent(Vector2 site)
        {
            this.site = site;
            y_value = site.y;
            site_event = true;
        }

        public bool IsSiteEvent()
        {
            return site_event;
        }

        public int Compare(VoronoiEvent x, VoronoiEvent y)
        {
            return x.y_value.CompareTo(y.y_value);
        }

        public int CompareTo(VoronoiEvent other)
        {
            return y_value.CompareTo(other.y_value);
        }
    }


    List<VoronoiEvent> events;

    BeachLine beachLine;

	// Use this for initialization
	void Start ()
    {
        //points = new List<Vector2>();
        point_objects = new List<Transform>();
        line_objects = new List<Transform>();
        events = new List<VoronoiEvent>();
        beachLine = new BeachLine();
    }
	
	// Update is called once per frame
	void Update () {

        // Destroy existing gameobjects
        foreach(Transform t in point_objects)
        {
            Destroy(t.gameObject);
        }
        point_objects.Clear();

        foreach (Transform t in line_objects)
        {
            Destroy(t.gameObject);
        }
        line_objects.Clear();


        if (randomize)
        {
            UnityEngine.Random.InitState(seed);

            points.Clear();
            // Generate points
            for (int i = 0; i < num_points; i++)
            {
                Vector2 point = new Vector2(
                    UnityEngine.Random.Range(
                        (-x_range / 2) - edge_size, (x_range / 2) + edge_size
                        ),
                    UnityEngine.Random.Range(
                        (-y_range / 2) - edge_size, (y_range / 2) + edge_size
                        ));

                points.Add(point);
            }
        }







        // Add points to event list
        foreach (Vector2 point in points)
        {
            events.Add(new VoronoiEvent(point));
        }

        // Sort events by y_value
        events.Sort();

        bool complete_early = false;
        int iterations = 0;
        beachLine = new BeachLine();
        while(!(events.Count == 0) && !complete_early)
        {
            VoronoiEvent e = events[0]; // Get first event

            // Update directrix for beach line
            beachLine.SetDirectrix(e.y_value);

            if(beachLine.root == null)
            {
                // This is the first event
                beachLine.Add(new BeachLineArc(new Arc(e.site, e.y_value)));
            }
            // If site event
            else if(e.IsSiteEvent())
            {
                // Create new arc
                //Arc newArc = new Arc(e.site, e.y_value);

                Vector2 intersection = Vector2.zero;
                Vector2 direction = Vector2.zero;

                BeachLineArc arcToSplit = beachLine.GetArcAtX(e.site.x, out intersection, out direction);

                if (arcToSplit != null)
                {
                    Arc newLeftArc = new Arc(arcToSplit.arc.focus, arcToSplit.arc.directrix);
                    newLeftArc.start_x = arcToSplit.arc.start_x;
                    newLeftArc.end_x = e.site.x;
                    BeachLineArc newLeftBLArc = new BeachLineArc(newLeftArc);

                    Arc newRightArc = new Arc(arcToSplit.arc.focus, arcToSplit.arc.directrix);
                    newRightArc.start_x = e.site.x;
                    newRightArc.end_x = arcToSplit.arc.end_x;
                    BeachLineArc newRightBLArc = new BeachLineArc(newRightArc);

                    Edge newLeftEdge = new Edge(intersection, direction, 0);
                    Edge newRightEdge = new Edge(intersection, -direction, 0);
                    BeachLineEdge newLeftBLEdge = new BeachLineEdge(newLeftEdge);
                    BeachLineEdge newRightBLEdge = new BeachLineEdge(newRightEdge);

                    BeachLineElement parentElement = arcToSplit.parent;
                    bool isLeft = false;
                    if(parentElement != null)
                    {
                        if(parentElement.leftChild == arcToSplit)
                        {
                            isLeft = true;
                        }
                    }

                    arcToSplit.parent = newRightBLEdge;
                    newRightBLEdge.leftChild = arcToSplit;
                    newRightBLArc.parent = newRightBLEdge;
                    newRightBLEdge.rightChild = newRightBLArc;
                    newRightBLEdge.parent = newLeftBLEdge;
                    newLeftBLEdge.rightChild = newRightBLEdge;
                    newLeftBLArc.parent = newLeftBLEdge;
                    newLeftBLEdge.leftChild = newLeftBLArc;

                    if(parentElement == null)
                    {
                        beachLine.root = newLeftBLEdge;
                        newLeftBLEdge.parent = null;
                    }
                    else
                    {
                        newLeftBLEdge.parent = parentElement;
                        if (isLeft)
                        {
                            parentElement.leftChild = newLeftBLEdge;
                        }
                        else
                        {
                            parentElement.rightChild = newLeftBLEdge;
                        }
                    }
                    
                }

                //Arc leftArc = arcToSplit.SplitLeft(e.site.x);
                //Arc rightArc = arcToSplit.SplitRight(e.site.x);

                //// Direction should always point left
                //BeachLineEdge leftEdge = new BeachLineEdge(new Edge(intersection, direction, 0));
                //BeachLineEdge rightEdge = new BeachLineEdge(new Edge(intersection, -direction, 0));

                ////beachLine.Add(new BeachLineArc(newArc));

                //arcToSplit.ReplaceWith(leftEdge);
                //leftEdge.SetLeftChild(new BeachLineArc(leftArc));
                //leftEdge.SetRightChild(rightEdge);
                //rightEdge.SetLeftChild(new BeachLineArc(rightArc));

                // This arc is going to split existing arc
                // We will end of with a left/right arc, plus a left right edge
            }

            // Event handled. Remove event from list.
            events.RemoveAt(0);
            iterations++;


            if (iterations == max_iterations)
            {
                complete_early = true;
            }
        }
        beachLine.Draw(transform.position);






        foreach (Vector2 point in points)
        { 
            Transform point_object = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            point_object.parent = transform;
            point_object.localScale = new Vector3(point_size, point_size, point_size);
            point_object.localPosition = new Vector3(point.x, point.y, 0f);

            point_objects.Add(point_object);

        }

        // TEST
        Transform newLine = CreateLine(points[0], points[1]);
        line_objects.Add(newLine);

        //foreach(Vector2 point in points)
        //{
        //    if(point != points[0])
        //    {
        //        Transform newLine = CreateLine(point, points[0]);
        //        line_objects.Add(newLine);
        //    }
        //}


    }




}
#endif