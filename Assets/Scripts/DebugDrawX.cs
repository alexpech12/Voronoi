using UnityEngine;
using System.Collections;

public static class DebugDrawX
{
    public static void DrawCross(Vector2 point, float size, Color color)
    {
        float debugDrawSize = size;
        Color debugColor = color;
        Vector2 intersection = point;
        Debug.DrawLine(intersection + new Vector2(debugDrawSize, debugDrawSize),
            intersection + new Vector2(-debugDrawSize, -debugDrawSize), debugColor);
        Debug.DrawLine(intersection + new Vector2(-debugDrawSize, debugDrawSize),
            intersection + new Vector2(debugDrawSize, -debugDrawSize), debugColor);
    }
}
