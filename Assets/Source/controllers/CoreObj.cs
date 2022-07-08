using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class CoreObj : MonoBehaviour
{
    [HideInInspector] public string Id;
    [HideInInspector] public EventResultClass EventResult = new EventResultClass();
    public bool IsListenTouchEvent;
    private List<CoreObj> listChildren = new List<CoreObj>();
    private bool _result;

    public virtual void OnTouch(TouchInfo info)
    {
    }

    public void AddChild(CoreObj child)
    {
        listChildren.Add(child);
    }

    public List<CoreObj> GetChildren()
    {
        return listChildren;
    }

    public virtual bool CheckResult()
    {
        bool r = GetResult();
        EventResult.Invoke(this, r);
        return r;
    }

    public virtual bool GetResult()
    {
        bool r = (listChildren.Count > 0) ? true : false;
        foreach (CoreObj child in listChildren)
        {
            if (!GetChildResult(child))
                r = false;
        }

        return r;
    }

    public virtual bool GetChildResult(CoreObj child)
    {
        bool r = child.GetResult();
        return r;
    }
}