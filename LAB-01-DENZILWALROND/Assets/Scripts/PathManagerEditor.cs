using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{

    [SerializeField]
    PathManager pathManager;

    [SerializeField]
    List<Waypoint> thePath;
    List<int> toDelete;

    Waypoint selectedPoint = null;
    bool doRepaint = true;

    private void OnSceneGUI()
    {
        thePath = pathManager.GetPath();
        DrawPath(thePath);
    }


    private void OnEnable()
    {
        pathManager = target as PathManager;
        toDelete = new List<int>();
    }

    override public void OnInspectorGUI()
    {
        this.serializedObject.Update();
        thePath = pathManager.GetPath();

        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path");

        DrawGUIForPoints();

        // Button for adding a point to the path
        if (GUILayout.Button("Add Point to Path"))
        {
            pathManager.CreateAddPoint();
        }

        EditorGUILayout.EndVertical();
        SceneView.RepaintAll();
    }

    void DrawGUIForPoints()
    {
        if (thePath != null && thePath.Count > 0)
        {
            for (int i = 0; i < thePath.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                Waypoint p = thePath[i];

                Color c = GUI.color;
                if (selectedPoint == p)
                {
                    GUI.color = Color.green;
                }

                Vector3 oldPos = p.Pos;
                Vector3 newPos = EditorGUILayout.Vector3Field("", oldPos);
                if (EditorGUI.EndChangeCheck())
                {
                    p.Pos = (newPos);
                }

                // the Delete button
                if (GUILayout.Button("-", GUILayout.Width(25)))
                {
                    toDelete.Add(i);
                    // do somethign for deletion
                }

                GUI.color = c;
                EditorGUILayout.EndHorizontal();
            }
        }
        if (toDelete.Count > 0)
        {
            foreach (int i in toDelete)
            {
                thePath.RemoveAt(i);
            }
            toDelete.Clear();
        }
    }

    public void DrawPath(List<Waypoint> path)
    {
        if (path != null)
        {
            List<Vector3> splinePoints = new List<Vector3>();
            foreach (Waypoint wp in path)
            {
                doRepaint = DrawPoint(wp);
            }
            for (int i = 0; i < path.Count; i++)
            {
                splinePoints.AddRange(CatmullRomSpline(path[i % path.Count].Pos, path[(i + 1) % path.Count].Pos, path[(i + 2) % path.Count].Pos, path[(i + 3) % path.Count].Pos, pathManager.numOfSplinePoints, pathManager.alpha));
            }
            for (int i = 0; i < splinePoints.Count - 1; i++)
            {
                DrawPathLine(splinePoints[i], splinePoints[(i + 1) % splinePoints.Count]);
            }
        }
        if (doRepaint)
        {
            Repaint();
        }
    }

    private Vector3[] CatmullRomSpline(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int numOfPoints, float alpha)
    {
        float t0 = 0;
        float t1 = ComputeT(t0, p0, p1, alpha);
        float t2 = ComputeT(t1, p1, p2, alpha);
        float t3 = ComputeT(t2, p2, p3, alpha);

        Vector3[] splinePoints = new Vector3[numOfPoints];

        for (int i = 0; i < numOfPoints; i++)
        {
            float t = t1 + (t2 - t1) * i / (numOfPoints - 1);

            float a1 = (t1 - t) / (t1 - t0);
            float a2 = (t - t0) / (t1 - t0);
            Vector3 A1 = a1 * p0 + a2 * p1;

            float a3 = (t2 - t) / (t2 - t1);
            float a4 = (t - t1) / (t2 - t1);
            Vector3 A2 = a3 * p1 + a4 * p2;

            float a5 = (t3 - t) / (t3 - t2);
            float a6 = (t - t2) / (t3 - t2);
            Vector3 A3 = a5 * p2 + a6 * p3;

            float b1 = (t2 - t) / (t2 - t0);
            float b2 = (t - t0) / (t2 - t0);
            Vector3 B1 = b1 * A1 + b2 * A2;

            float b3 = (t3 - t) / (t3 - t1);
            float b4 = (t - t1) / (t3 - t1);
            Vector3 B2 = b3 * A2 + b4 * A3;

            float b5 = (t2 - t) / (t2 - t1);
            float b6 = (t - t1) / (t2 - t1);
            splinePoints[i] = b5 * B1 + b6 * B2;
        }

        return splinePoints;
    }

    private float ComputeT(float t, Vector3 p0, Vector3 p1, float alpha)
    {
        return Mathf.Pow(Vector3.Distance(p1, p0), alpha) + t;
    }

    public void DrawPathLine(Vector3 p1, Vector3 p2)
    {
        // draw a line between current and next point
        Color c = Handles.color;
        Handles.color = Color.gray;
        Handles.DrawLine(p1, p2);
        Handles.color = c;
    }

    public bool DrawPoint(Waypoint p)
    {
        bool isChanged = false;
        if (selectedPoint == p)
        {
            Color c = Handles.color;
            Handles.color = Color.green;

            EditorGUI.BeginChangeCheck();
            Vector3 oldPos = p.Pos;
            Vector3 newPos = Handles.PositionHandle(oldPos, Quaternion.identity);

            float handleSize = HandleUtility.GetHandleSize(newPos);

            Handles.SphereHandleCap(-1, newPos, Quaternion.identity, 0.25f * handleSize, EventType.Repaint);
            if (EditorGUI.EndChangeCheck())
            {
                p.Pos = newPos;
            }

            Handles.color = c;
        }
        else
        {
            Vector3 currPos = p.Pos;
            float handleSize = HandleUtility.GetHandleSize(currPos);
            if (Handles.Button(currPos, Quaternion.identity, 0.25f * handleSize, 0.25f * handleSize, Handles.SphereHandleCap))
            {
                isChanged = true;
                selectedPoint = p;
            }
        }
        return isChanged;
    }
}