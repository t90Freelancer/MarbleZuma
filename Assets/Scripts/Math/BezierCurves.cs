
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bezier curves. Calculating.
/// Documentation: http://devmag.org.za/2011/04/05/bzier-curves-a-tutorial/
/// </summary>
public static class BezierCurves {
    
    /// <summary>
    /// Calculate Bezier curve's point.
    /// </summary>
    /// <param name="t">Fraction of curve.</param>
    /// <param name="p0">Control point.</param>
    /// <param name="p1">Control point.</param>
    /// <param name="p2">Control point.</param>
    /// <param name="p3">Control point.</param>
    /// <returns>Point of Bezier curve.</returns>
    public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var u = 1 - t;
        var tt = t*t;
        var uu = u*u;
        var uuu = uu*u;
        var ttt = tt*t;

        return uuu*p0 + 3*uu*t*p1 + 3*u*tt*p2 + ttt*p3;
    }

    /// <summary>
    /// Calculates the drawing points of Bezier curve. This drawing points must be connected by lines.
    /// </summary>
    /// <param name="controlPoints"></param>
    /// <param name="segmentsPerCurve"></param>
    /// <returns></returns>
    public static List<Vector3> GetDrawingPoints(List<Vector3> controlPoints, int segmentsPerCurve)
    {
        var drawingPoints = new List<Vector3>();
        for (var i = 0; i < controlPoints.Count - 3; i += 3)
        {
            var p0 = controlPoints[i];
            var p1 = controlPoints[i + 1];
            var p2 = controlPoints[i + 2];
            var p3 = controlPoints[i + 3];

            if (i == 0) //Only do this for the first endpoint.
            //When i != 0, this coincides with the end
            //point of the previous segment
            {
                drawingPoints.Add(CalculateBezierPoint(0, p0, p1, p2, p3));
            }

            for (var j = 1; j <= segmentsPerCurve; j++)
            {
                var t = j / (float)segmentsPerCurve;
                drawingPoints.Add(CalculateBezierPoint(t, p0, p1, p2, p3));
            }
        }
        return drawingPoints;
    }

}
