using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTouch : UnityEvent<TouchInfo>{};

public class EventResultClass : UnityEvent<CoreObj, bool>{};

public enum EBlockItemStatus
{
    UNFILL, FILL, DISABLE
}
public class EventBlockItemStatus : UnityEvent<EBlockItemStatus> {  };


[System.Serializable]
public class Grid
{
    public int Column;
    public int Row;
    public Vector2 Size;
    public int Count
    {
        get => Row * Column;
    }
}


public struct TouchInfo
{
    public Vector2[] Points; //screen viewport points (0~1)
    public Vector3[] ScenePoints; //world points
    public List<GameObject> HitObjects;
    public TouchPhase Phase;

    public void Reset()
    {
        Points = new Vector2[2];
        ScenePoints = new Vector3[2];
        HitObjects = new List<GameObject>();
    }
    

    public Vector2 GetScreenPoint(int _index = 0)
    {
        return Camera.main.ViewportToScreenPoint(Points[_index]);
    }
}

