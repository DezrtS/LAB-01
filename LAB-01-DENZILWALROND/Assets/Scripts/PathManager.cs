using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [HideInInspector]
    [SerializeField]
    public List<Waypoint> path;

    public GameObject prefab;
    int currrentPointIndex = 0;

    public List<GameObject> prefabPoints;

    private void Start()
    {
        prefabPoints = new List<GameObject>();
        foreach (Waypoint p in path)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = p.Pos;
            prefabPoints.Add(go);
        }
    }

    private void Update()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Waypoint p = path[i];
            GameObject g = prefabPoints[i];
            g.transform.position = p.Pos;
        }
    }

    public List<Waypoint> GetPath()
    {
        if (path == null)
        {
            path = new List<Waypoint>();
        }
        return path;
    }

    public void CreateAddPoint()
    {
        Waypoint go = new Waypoint();
        path.Add(go);
    }

    public Waypoint GetNextTarget() 
    {
        int nextPointIndex = (currrentPointIndex + 1) % (path.Count);
        currrentPointIndex = nextPointIndex;
        return path[nextPointIndex];
    }


}
