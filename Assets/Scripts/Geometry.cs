using UnityEngine;
using System.Collections;

public static class Geometry
{
    public static bool GetValidIntersection(out Vector2 intersection, BeachLineArc arc, BeachLineEdge edge)
    {
        Vector2 i1, i2;
        int interX = Geometry.Intersection(out i1, out i2, arc, edge);

        intersection = i1;
        bool intersectionValid = false;
        if (interX != 0)
        {
            if (interX == 1)
            {
                intersection = i1;
                if (Vector2.Dot(edge.Direction, i1 - edge.Start) > 0)
                {
                    intersectionValid = true;
                }
            }
            else
            {
                bool i1_valid = false;
                bool i2_valid = false;
                if (Vector2.Dot(edge.Direction, i1 - edge.Start) > 0)
                {
                    i1_valid = true;
                    intersection = i1;
                }
                if (Vector2.Dot(edge.Direction, i2 - edge.Start) > 0)
                {
                    i2_valid = true;
                    intersection = i2;
                }
                if (i1_valid && i2_valid)
                {
                    // Use closest
                    intersection = Vector2.Distance(i1, edge.Start) < Vector2.Distance(i2, edge.Start) ? i1 : i2;
                }
                intersectionValid = i1_valid || i2_valid;
            }

            if (intersectionValid)
            {
                return true;
            }
        }
        return false;
    }

    public static Vector3 QuadraticFocusDirectrixToABCCoefficents(Vector2 focus, float directrix)
    {
        float a, b, c;
        a = 1 / (2.0f * (focus.y - directrix));
        b = a * -2 * focus.x;
        c = a * (focus.x * focus.x + focus.y * focus.y - directrix * directrix);
        return new Vector3(a, b, c);
    }

    public static Vector2 RayToABCoefficients(Vector2 start, Vector2 direction)
    {
        Vector2 p1 = start;
        Vector2 p2 = start + direction;
        float dy = p1.y - p2.y;
        float dx = p1.x - p2.x;
        float m = dy / dx;
        float c = p1.y - (m * p1.x);
        return new Vector2(m, c);
    }

    public static int Intersection(out Vector2 intersection1, out Vector2 intersection2, BeachLineArc arc, BeachLineEdge edge)
    {
        if (edge.Direction.x == 0)
        {
            // If vertical, just return quadratic at x
            intersection1 = new Vector2(edge.Start.x, arc.y(edge.Start.x));
            intersection2 = intersection1;
            return 1;

        }

        Vector3 quadraticAbc = QuadraticFocusDirectrixToABCCoefficents(arc.Focus, arc.Directrix);
        Vector2 rayAb = RayToABCoefficients(edge.Start, edge.Direction);
        return Intersection2D(out intersection1, out intersection2, quadraticAbc.x, quadraticAbc.y, quadraticAbc.z, 0, rayAb.x, rayAb.y);
    }

    public static int Intersection2D(out Vector2 intersection1, out Vector2 intersection2, Vector2 focus1, float directrix1, Vector2 focus2, float directrix2)
    {
        Vector3 arc1 = QuadraticFocusDirectrixToABCCoefficents(focus1, directrix1);
        Vector3 arc2 = QuadraticFocusDirectrixToABCCoefficents(focus2, directrix2);

        return Intersection2D(out intersection1, out intersection2, arc1.x, arc1.y, arc1.z, arc2.x, arc2.y, arc2.z);
    }

    public static int Intersection2D(out Vector2 intersection1, out Vector2 intersection2, float a1, float b1, float c1, float a2, float b2, float c2)
    {
        float a = a1 - a2;
        float b = b1 - b2;
        float c = c1 - c2;

        float disc = b * b - 4 * a * c;
        if (disc < 0)
        {
            // No intersection
            intersection1 = Vector2.zero;
            intersection2 = Vector2.zero;
            return 0;
        }
        else if (disc == 0)
        {
            // One intersection
            float x = -b / (2 * a);
            float y = a1 * x * x + b1 * x + c1;
            intersection1 = new Vector2(x, y);
            intersection2 = Vector2.positiveInfinity;
            return 1;
        }

        // Two intersections
        float x1 = (-b + Mathf.Sqrt(disc)) / (2 * a);
        float x2 = (-b - Mathf.Sqrt(disc)) / (2 * a);
        float y1 = a1 * x1 * x1 + b1 * x1 + c1;
        float y2 = a2 * x2 * x2 + b2 * x2 + c2;
        intersection1 = new Vector2(x1, y1);
        intersection2 = new Vector2(x2, y2);
        return 2;
    }

    public static Vector2 Intersection2D(Vector2 line1Start, Vector2 line1Direction, Vector2 line2Start, Vector2 line2Direction)
    {
        // Should check here for parallel lines
        if(line1Direction == line2Direction || line1Direction == -line2Direction)
        {
            //return new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            line2Direction = new Vector2(line2Direction.x, line2Direction.y + 0.001f);
        }

        Vector2 line1End = line1Start + line1Direction;
        Vector2 line2End = line2Start + line2Direction;

        float x1, y1, x2, y2, x3, y3, x4, y4;
        x1 = line1Start.x;
        y1 = line1Start.y;
        x2 = line1End.x;
        y2 = line1End.y;
        x3 = line2Start.x;
        y3 = line2Start.y;
        x4 = line2End.x;
        y4 = line2End.y;


        float intersectionX = 
                Determinant(
                    Determinant(x1,y1,x2,y2), Determinant(x1,1,x2,1),
                    Determinant(x3,y3,x4,y4), Determinant(x3,1,x4,1)
            ) / Determinant(
                    Determinant(x1, 1, x2, 1), Determinant(y1, 1, y2, 1),
                    Determinant(x3, 1, x4, 1), Determinant(y3, 1, y4, 1)
                );

        float intersectionY =
                Determinant(
                    Determinant(x1, y1, x2, y2), Determinant(y1, 1, y2, 1),
                    Determinant(x3, y3, x4, y4), Determinant(y3, 1, y4, 1)
            ) / Determinant(
                    Determinant(x1, 1, x2, 1), Determinant(y1, 1, y2, 1),
                    Determinant(x3, 1, x4, 1), Determinant(y3, 1, y4, 1)
                );


        return new Vector2(intersectionX, intersectionY);
    }

    // Determinant
    public static float Determinant(float a, float b, float c, float d)
    {
        return a * d - b * c;
    }
}
