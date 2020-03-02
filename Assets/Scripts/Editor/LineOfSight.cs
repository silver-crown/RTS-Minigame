using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom Editor script used to draw the line of sight of the drone.
/// </summary>
/// 
/*
[CustomEditor(typeof(DroneSight))]
public class LineOfSight : Editor
{
    /// <summary>
    /// not idea how this function works
    /// </summary>
    private void OnSceneGUI()
    {
        DroneSight lineOfSight = (DroneSight)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(
            lineOfSight.transform.position, // The center of the circle.
            Vector3.up,            // The normal of the circle.
            Vector3.forward,       // (from vector) The direction of the point on the circle circumference, relative to the center, where the arc begins.
            360,                   // The angle of the arc, in degrees.
            lineOfSight.viewRadius // radius
            );
    }

}*/