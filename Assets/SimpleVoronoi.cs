using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleVoronoi : MonoBehaviour {
    
    public List<Vector2> sites = new List<Vector2> {
        new Vector2(1f, 1f),
        new Vector2(3f, 1.5f),
        new Vector2(2f, 2f),
        new Vector2(2.5f, 3f)
    };


    public abstract class VEvent
    {
    }

    public class SiteEvent : VEvent
    {

    }

    public class EdgeEvent : VEvent
    {

    }


    public struct Quadratic
    {
        public Vector2 mFocus;
        public float mDirectrix;

        public Quadratic(Vector2 focus, float directrix)
        {
            mFocus = focus;
            mDirectrix = directrix;
        }

        public float y(float x)
        {
            return (1 / (2 * (mFocus.y - mDirectrix))) * (x - mFocus.x) * (x - mFocus.x) + (mFocus.x + mDirectrix) / 2;
        }

        public float dy(float x)
        {
            return (1 / (mFocus.y - mDirectrix)) * (x - mFocus.x);
        }

        public Vector2 Point(float x)
        {
            return new Vector2(x, y(x));
        }

        public Vector2 Tangent(float x)
        {
            return new Vector2(x, dy(x));
        }
    }

    public struct Edge
    {
        public Vector2 mStart;
        public Vector2 mDirection;
        public float mLength;

        public Edge(Vector2 start, Vector2 direction, float length)
        {
            mStart = start;
            mDirection = direction;
            mLength = length;
        }

        public float y(float x)
        {
            Vector2 end = mStart + (mDirection.normalized * mLength);
            if (mStart.x == end.x)
            {
                // Vertical
                end.x += float.MinValue;
            }
            float m = (mStart.y - end.y) / (mStart.x - end.x);
            float c = mStart.y - m * mStart.x;
            float y_val = m * x + c;
            return y_val;
        }

        //public float dy(float x)
        //{
        //    return (1 / (mFocus.y - mDirectrix)) * (x - mFocus.x);
        //}

        //public Vector2 Point(float x)
        //{
        //    return new Vector2(x, y(x));
        //}

        //public Vector2 Tangent(float x)
        //{
        //    return new Vector2(x, dy(x));
        //}
    }

    List<VEvent> events;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        events = new List<VEvent>();
        foreach (Vector2 point in sites)
        {
            //events.Add(new SiteEvent(point));
        }

        // Draw sites
        foreach (Vector2 point in sites)
        {
            DrawPoint(point);
        }
	}

    public void DrawPoint(Vector2 point)
    {
        float size = 0.1f;
        Color color = Color.white;
        Debug.DrawRay(new Vector3(point.x - size, point.y, 0), Vector3.right * 2 * size, color);
        Debug.DrawRay(new Vector3(point.x, point.y - size, 0), Vector3.up * 2 * size, color);
        Debug.DrawRay(new Vector3(point.x, point.y, -size), Vector3.forward * 2 * size, color);
    }



    public static void DrawQuadratic(Vector2 focus, float directrix, float start_x, float end_x)
    {
        float step_size = 0.1f;
        Vector3 offset = Vector3.zero;
        Color color = Color.green;
        int lines = Mathf.RoundToInt((end_x - start_x) / step_size);

        Quadratic arc = new Quadratic(focus, directrix);

        float t0 = 0f;
        float x0 = Mathf.Lerp(start_x, end_x, t0);
        float y0 = arc.y(x0);
        for (int i = 0; i < lines; i++)
        {
            float t1 = (float)(i + 1) / lines;
            float x1 = Mathf.Lerp(start_x, end_x, t1);
            float y1 = arc.y(x1);
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
