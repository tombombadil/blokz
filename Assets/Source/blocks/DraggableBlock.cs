using UnityEngine;


public class DraggableBlock : Block
{
    private Movement movement;

    public override void Init(string id)
    {
        base.Init(id);
        movement = GetComponent<Movement>();
        movement.SetEnable(true);
        movement.EventDrag.AddListener(onTouchEvent);
    }

    private void onTouchEvent(TouchInfo e)
    {
        if (e.Phase == TouchPhase.Began)
        {
            setFillKeyBlocks(false);
        }

        if (e.Phase == TouchPhase.Ended)
        {
            if (CheckResult())
            {
                startPlacement();
            }
            else
            {
                backToPlacement();
            }
        }
    }

    private void startPlacement()
    {
        BlockItem itemFirst = listItems[0];
        Vector3 pos = itemFirst.TriggeredBlockItem.transform.position;
        // this.transform.position = pos;
        LeanTween.move(gameObject, pos, 0.2f).setEase(LeanTweenType.easeOutExpo);
        setFillKeyBlocks(true);
    }

    private void setFillKeyBlocks(bool isFilled)
    {
        foreach (BlockItem item in listItems)
        {
            if (item.TriggeredBlockItem)
            {
                if (isFilled)
                {
                    item.SetColor(ColorDefault);
                    item.TriggeredBlockItem.SetStatus(EBlockItemStatus.FILL);
                }

                else
                {
                    item.TriggeredBlockItem.SetStatus(EBlockItemStatus.UNFILL);
                }
            }
        }
    }

    private void backToPlacement()
    {
        // this.transform.position = defaultPos;
        LeanTween.move(gameObject, defaultPos, 0.2f).setEase(LeanTweenType.easeInSine);
    }
}