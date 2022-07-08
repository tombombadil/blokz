using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils
{
    /* places to objects on grid according to the values given to it */
    public static void PlaceToObjectsOnGrid(List<CoreObj> objects, Grid grid, float space = 0, bool isShuffle = false)
    {
        int total = Mathf.Min(grid.Count, objects.Count);
        for (int i = 0; i < total; i++)
        {
            CoreObj obj = objects[i];
            int col = Mathf.FloorToInt(i % grid.Column);
            int row = Mathf.FloorToInt(i / grid.Column);
            float x = col * (grid.Size.x + space);
            float y = row * (grid.Size.y + space) * -1;
            Vector3 pos = new Vector3(x, y, 0);
            obj.transform.localPosition = pos;
        }
    }

    /* this method get ramdom positions from sliced by the number of relevant column and row count box */
    public static Vector3[] GetGridPositionsInBox(BoxCollider collider, int columnCount, int rowCount)
    {
        Bounds bounds = collider.bounds;
        Vector3 boundsMin = bounds.center - bounds.extents;
        Vector3 boundsMax = bounds.center + bounds.extents;
        int total = columnCount * rowCount;
        float w = (boundsMax.x - boundsMin.x) / rowCount;
        float h = (boundsMax.y - boundsMin.y) / columnCount;


        Vector3[] result = new Vector3[total];
        for (int i = 0; i < total; i++)
        {
            int index = i;
            float xStart = boundsMin.x + (Mathf.FloorToInt(i % columnCount) * w);
            float yStart = boundsMin.y - (Mathf.FloorToInt(i / columnCount) * h);

            float xEnd = xStart + w;
            float yEnd = yStart + h;

            Vector3 min = new Vector3(xStart, yStart, 0);
            Vector3 max = new Vector3(xEnd, yEnd, 0);

            Vector3 v = new Vector3(xStart, yStart, 0);
            result[i] = v;
        }


        return result;
    }

    public static Vector3 GetRandomPositionInVector(Vector3[] _ranges)
    {
        Vector3 min = Vector3.zero;
        Vector3 max = Vector3.zero;
        if (_ranges.Length > 0) min = _ranges[0];
        if (_ranges.Length > 1) max = _ranges[1];
        float x = UnityEngine.Random.Range(min.x, max.x);
        float y = UnityEngine.Random.Range(min.y, max.y);
        float z = UnityEngine.Random.Range(min.z, max.z);
        return new Vector3(x, y, z);
    }

    public static Bounds CalculateBoundsOfObject(GameObject go)
    {
        Bounds bounds = new Bounds(go.transform.position, Vector3.zero);
        foreach (Renderer renderer in go.GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        Vector3 localCenter = bounds.center - go.transform.position;
        bounds.center = localCenter;
        return bounds;
    }
}