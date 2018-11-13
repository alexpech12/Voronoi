
#define OLD

#if !OLD
using UnityEngine;
using System.Collections;

class VoronoiUtil
{
    public class Arc
    {
        public float a;
        public float b;
        public float c;
        public float start_x = float.NegativeInfinity;
        public float end_x = float.PositiveInfinity;
        public Vector2 focus;
        public float directrix;

        public Arc(Vector2 focus, float directrix)
        {
            this.focus = focus;
            this.directrix = directrix;
        }

        public float y(float x)
        {
            return (1 / (2 * (focus.y - directrix))) * (x - focus.x) * (x - focus.x) + (focus.x + directrix) / 2;
        }

        public float dy(float x)
        {
            return (1 / (focus.y - directrix)) * (x - focus.x);
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

    class Edge
    {
        public Vector2 start;
        public Vector2 direction;
        public float length;

        public Edge(Vector2 start, Vector2 direction, float length)
        {
            this.start = start;
            this.direction = direction;
            this.length = length;
        }

        public void Draw(Vector2 offset)
        {
            Debug.DrawRay(start, direction.normalized, Color.blue);
        }
    }

    public static Transform CreateLine(Transform parent, Vector2 start, Vector2 direction, float length, float width)
    {
        return CreateLine(parent, start, start + direction.normalized * length, width);
    }

    public static Transform CreateLine(Transform parent, Vector2 start, Vector2 end, float width)
    {
        Transform newLine = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        newLine.parent = parent.transform;

        newLine.localScale = new Vector3(width, (start - end).magnitude, width);
        newLine.Translate(start + (end - start) / 2);
        newLine.Rotate(0, 0, Vector3.Angle(Vector3.up, start - end));

        return newLine;
    }

    public static void DrawQuadratic(Arc arc, Vector3 offset, float start_x, float end_x, float step_size, Color color)
    {
        int lines = Mathf.RoundToInt((end_x - start_x) / step_size);

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
#endif