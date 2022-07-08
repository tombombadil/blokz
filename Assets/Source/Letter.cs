using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : CoreObj
{
    private List<KeyBlock> listKeyBlocks = new List<KeyBlock>();
    [HideInInspector]public List<DraggableBlock> listDraggables = new List<DraggableBlock>();
    public Transform ContainerKeys;
    public Transform ContainerDraggables;
    public void Init(string id)
    {
        Id = id;
        LetterItem dataLetter =  DataCont.Instance.GetLetter(id);
        if (dataLetter.blocks.Count > 0)
        {
            int i = 0;
            foreach (LetterBlock dataBlock in dataLetter.blocks)
            {
               
                {//create keys
                    int x = Mathf.FloorToInt(dataBlock.PositionIndex % GameCont.Instance.BlockColumn);
                    int y = Mathf.FloorToInt(dataBlock.PositionIndex / GameCont.Instance.BlockColumn);
                    KeyBlock keyBlock = GameCont.Instance.SpawnObj(GameCont.Instance.PrefabKeyBlock, ContainerKeys) as KeyBlock;
                    keyBlock.Init(dataBlock.Id);
                    Vector3 localPos = Vector3.zero;
                    localPos.x = x * GameCont.Instance.BlockSize.x;
                    localPos.y = 0 - (y * GameCont.Instance.BlockSize.y);
                    keyBlock.transform.localPosition = localPos;
                    listKeyBlocks.Add(keyBlock);
                    keyBlock.name = "key" + listKeyBlocks.Count.ToString() + "-" + dataBlock.Id;
                }
               
                
                {//create draggables
                    DraggableBlock draggable = GameCont.Instance.SpawnObj(GameCont.Instance.PrefabDraggableBlock, ContainerDraggables) as DraggableBlock;
                    draggable.Init(dataBlock.Id);
                    listDraggables.Add(draggable);
                    AddChild(draggable);
                    draggable.name = "draggable" + listDraggables.Count.ToString();
                    draggable.EventResult.AddListener(onDraggableEventResult);
                    i++;
                }
            }
        }
    }
    
    private void onDraggableEventResult(CoreObj block, bool result)
    {
        CheckResult();
    }

    /* override -> just fror loak at logs.. */
    public override bool CheckResult()
    {
        bool r = base.CheckResult();

        if (r)
        {
            Debug.Log("LETTER " + Id + " BASARILI!");
        }
        else
        {
            Debug.Log("denemeye devam :/");
        }

        return r;
    }



   
    



    
    

}
