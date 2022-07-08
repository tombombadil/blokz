using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class InputCont : CoreCont<InputCont>
{
    [HideInInspector] public CoreObj HitObject;
    private TouchInfo touchInfo;
    private Plane virtualPlane;

    public void Awake()
    {
        virtualPlane = new Plane(Vector3.forward, Vector3.zero);
    }


    public void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                startTouch(UnityEngine.Input.mousePosition);
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                endTouch(UnityEngine.Input.mousePosition);
            }

            if (UnityEngine.Input.GetMouseButton(0))
            {
                updateTouch(UnityEngine.Input.mousePosition);
            }
        }
        else
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                for (int i = 0; i < UnityEngine.Input.touchCount; i++)
                {
                    UnityEngine.Touch touch = UnityEngine.Input.GetTouch(i);
                    Vector2 pos;
                    if (touch.phase == TouchPhase.Began)
                    {
                        pos = touch.position;
                        startTouch(pos, i);
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        pos = touch.position;
                        endTouch(pos, i);
                    }

                    if (touch.phase == TouchPhase.Moved)
                    {
                        pos = touch.position;
                        updateTouch(pos, i);
                    }
                }
            }
        }
    }

    public CoreObj UpdateTouch(TouchInfo _touch)
    {
        CoreObj result = null;
        bool isAnyhitResult = false;
        RaycastHit[] hits = getSceneHitsFromScreen(_touch.GetScreenPoint());
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                GameObject go = hit.collider.gameObject;
                if (go)
                {
                    foreach (CoreObj coreObj in go.GetComponents<CoreObj>())
                    {
                        if (coreObj.IsListenTouchEvent)
                        {
                            result = coreObj;
                        }
                    }
                }
            }
        }

        return result;
    }

    private RaycastHit[] getSceneHitsFromScreen(Vector2 _screenPos)
    {
        RaycastHit[] hits = new RaycastHit[0];
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(_screenPos.x, _screenPos.y, 0));
        hits = Physics.RaycastAll(ray, 100.0F);
        return hits;
    }


    protected void startTouch(Vector2 _screenPos, int _touchIndex = 0)
    {
        touchInfo = new TouchInfo();
        touchInfo.Reset();
        touchInfo.Points[0] = Camera.main.ScreenToViewportPoint(_screenPos);
        touchInfo.ScenePoints[0] = getOnPlaneHit(_screenPos);
        touchInfo.Phase = TouchPhase.Began;
        HitObject = UpdateTouch(touchInfo);
        if (HitObject) HitObject.OnTouch(touchInfo);
             
    }

    protected void updateTouch(Vector2 _screenPos, int _touchIndex = 0)
    {
        touchInfo.Points[1] = Camera.main.ScreenToViewportPoint(_screenPos);
        touchInfo.ScenePoints[1] = getOnPlaneHit(_screenPos);
        touchInfo.Phase = TouchPhase.Moved;
        if (HitObject)
            HitObject.OnTouch(touchInfo);
    }

    protected void endTouch(Vector2 _screenPos, int _touchIndex = 0)
    {
        touchInfo.Points[1] = Camera.main.ScreenToViewportPoint(_screenPos);
        touchInfo.ScenePoints[1] = getOnPlaneHit(_screenPos);
        touchInfo.Phase = TouchPhase.Ended;
        if (HitObject) HitObject.OnTouch(touchInfo);
        HitObject = null;
    }


    private Vector3 getOnPlaneHit(Vector2 _screenPos)
    {
        Vector3 r = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(_screenPos.x, _screenPos.y, 0));
        float enter = 0.0f;
        if (virtualPlane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            r = hitPoint;
        }

        return r;
    }
}