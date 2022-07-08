using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Block : CoreObj
{
    public GameObject PrefabBlockItem;
    public Transform ItemContainer;
    public BoxCollider collider;
    [HideInInspector] public Color ColorDefault;
    protected List<BlockItem> listItems = new List<BlockItem>();
    protected Vector3 defaultPos;
    private List<Color> blockColors = new List<Color>();
    private Vector2 blockSize;

    public virtual void Init(string id)
    {
        blockSize = GameCont.Instance.BlockSize;
        ColorDefault = getRandomColor();
        makeBlock(id);
    }

    public void InitDefaultPos()
    {
        defaultPos = this.transform.position;
    }

    private void makeBlock(string id)
    {
        BlockDataItem data = DataCont.Instance.GetBlock(id);
        if (data.Pattern.Length > 0)
        {
            int startRow = 0;
            int startColumn = 0;
            int[] nums = Array.ConvertAll(data.Pattern.Split(','), int.Parse);
            int rowCount = Mathf.FloorToInt(nums.Length / GameCont.Instance.BlockColumn);
            for (int i = 0; i < nums.Length; i++)
            {
                int num = nums[i];
                int col = Mathf.FloorToInt(num % GameCont.Instance.BlockColumn);
                int row = Mathf.FloorToInt(num / GameCont.Instance.BlockColumn);
                if (i == 0)
                {
                    startRow = row;
                    startColumn = col;
                }

                BlockItem item = GameCont.Instance.SpawnObj(PrefabBlockItem, ItemContainer) as BlockItem;
                if (item)
                {
                    Vector3 p = Vector3.zero;
                    p.x = (col * blockSize.x);
                    p.y = (-1 * row * blockSize.y);
                    item.transform.localPosition = p;
                    item.OwnerBlock = this;
                    item.SetStatus(EBlockItemStatus.UNFILL);
                    listItems.Add(item);
                    AddChild(item);
                    item.name = "item" + listItems.Count;
                }
            }

            reAlignContainer(startColumn, startRow);
            reSizeCollider(GameCont.Instance.BlockColumn, rowCount);
        }
    }

    private void reAlignContainer(int column, int row)
    {
        Vector3 containerPos = Vector3.zero;
        containerPos.x = 0 - (blockSize.x * column);
        containerPos.y = 0 + (blockSize.y * row);
        ItemContainer.transform.localPosition = containerPos;
    }

    private void reSizeCollider(int column, int row)
    {
        float w = blockSize.x * column;
        float h = blockSize.y * (row + 1);
        Bounds bounds = Utils.CalculateBoundsOfObject(this.gameObject);
        collider.size = bounds.size;
        collider.center = bounds.center;
    }


    private Color getRandomColor()
    {
        if (blockColors.Count == 0)
        {
            blockColors = new List<Color>(GameCont.Instance.BlockColors);
        }

        int rand = UnityEngine.Random.Range(0, blockColors.Count);
        Color c = blockColors[rand];
        blockColors.RemoveAt(rand);
        return c;
    }
}