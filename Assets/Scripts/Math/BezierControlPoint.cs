

using UnityEngine;

/// <summary>
/// Control point of Bezier curve.
/// </summary>
public class BezierControlPoint : MonoBehaviour
{
    public bool DebugPoint = false;

    /// <summary>
    /// Draws gizmo icon of control point.
    /// </summary>
    public void OnDrawGizmos()
    {
        if (!DebugPoint) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
