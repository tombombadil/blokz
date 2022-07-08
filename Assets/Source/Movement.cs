using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreObj
{
    [HideInInspector] public EventTouch EventDrag = new EventTouch();
    private Vector3 dragOffsetPos; //distance touch position of object center
    private bool isDragActive = false;
    private bool isEnable = false;


    public void SetEnable(bool _isEnable)
    {
        isEnable = _isEnable;
    }

    private void updateDrag(Vector3 pos)
    {
        if (isDragActive)
        {
            Vector3 p = pos - dragOffsetPos;
            p.z = 0;
            this.transform.position = p;
        }
    }

    public override void OnTouch(TouchInfo info)
    {
        base.OnTouch(info);


        if (isEnable)
        {
            if (info.Phase == TouchPhase.Began)
            {
                dragOffsetPos = (info.ScenePoints[0] - this.transform.position);
                isDragActive = true;
                EventDrag.Invoke(info);
            }

            if (info.Phase == TouchPhase.Ended)
            {
                isDragActive = false;
                EventDrag.Invoke(info);
            }

            if (info.Phase == TouchPhase.Moved)
            {
                updateDrag(info.ScenePoints[1]);
            }
        }
    }

    private bool getHasTouchHit(List<GameObject> hits)
    {
        bool r = false;
        if (hits.Count > 0)
            if (hits[0] == gameObject)
                r = true;
        return r;
    }
}