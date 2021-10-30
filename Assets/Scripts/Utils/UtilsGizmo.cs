using System;
using UnityEngine;

namespace Mans
{
    public static partial class Utils
    {
        static public void LineGizmo(Vector3 startPosition, Vector3 endPosition,float thick,Color color)
        {
            if (startPosition == endPosition) return;
            Gizmos.color = color;
            Quaternion rotate = new Quaternion();
            var pos = (startPosition + endPosition) / 2;
            var size = new Vector3(thick, thick, Vector3.Distance(startPosition, endPosition));
            rotate.SetLookRotation(startPosition - endPosition, Vector3.up);
            Gizmos.matrix = Matrix4x4.TRS(pos, rotate, Vector3.one);
            Gizmos.DrawCube(Vector3.zero, size);
        }
    }
}