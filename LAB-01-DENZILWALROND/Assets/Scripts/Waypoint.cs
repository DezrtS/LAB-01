using System;
using UnityEngine;

[Serializable]
public class Waypoint
{
    [SerializeField]
    private Vector3 pos;
    public Vector3 Pos { get { return pos; } set { pos = value; } }    

    public Waypoint()
    {
        Pos = new Vector3(0, 1, 1);
    }
}