using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;




public class BlockItem : CoreObj
{
   public bool IsKey;
   public Color ColorDisable;
   public Color ColorSuccTrigger;
   
   private EBlockItemStatus status;
   private Renderer renderer;
   [HideInInspector] public Block OwnerBlock;
   [HideInInspector] public Block TriggeredBlock;
   [HideInInspector] public BlockItem TriggeredBlockItem;
   public EventBlockItemStatus EventStatus = new EventBlockItemStatus();
   protected List<BlockItem> listTriggeredItems = new List<BlockItem>();
   private void Awake()
   {
      renderer = GetComponentInChildren<Renderer>();
   }



   #region Status
   public void SetStatus(EBlockItemStatus _status)
   {
      status = _status;
      EventStatus.Invoke(status);
      switch (status)
      {
         case EBlockItemStatus.DISABLE:
            SetColor(ColorDisable);
            break;
         case EBlockItemStatus.UNFILL:
         {
            if(!IsKey) SetColor(OwnerBlock.ColorDefault);
         }
            
            break;
         case EBlockItemStatus.FILL:
            SetColor(ColorSuccTrigger);
            break;
         
      }
   }

   public EBlockItemStatus GetStatus() => status;

   #endregion

   private void OnTriggerEnter(Collider collider)
   {
      if (IsKey) return;
      
      BlockItem otherItem = collider.GetComponent<BlockItem>();
      if (otherItem)
      {
         if (otherItem.IsKey)
         {
            if (otherItem.GetStatus() == EBlockItemStatus.UNFILL)
            {
               if (!listTriggeredItems.Contains(otherItem))
               {
                  TriggeredBlockItem = otherItem;
                  listTriggeredItems.Add(otherItem);
                  TriggeredBlock = otherItem.OwnerBlock;
                  SetStatus(EBlockItemStatus.FILL);
               }
              
            }
            
         }
      }
   }

   void OnTriggerExit(Collider collider)
   {
      if (IsKey) return;
      
      BlockItem otherItem = collider.GetComponent<BlockItem>();
      if (otherItem)
      {
         if (otherItem.IsKey )
         {
            if (listTriggeredItems.Contains(otherItem))
            {
               listTriggeredItems.Remove(otherItem);
            }

           if(listTriggeredItems.Count == 0)
            {
               TriggeredBlock = null;
               SetStatus(EBlockItemStatus.UNFILL);
            }
            
         }
      }
   }
   
   public override bool GetResult()
   {
      bool r = (GetStatus() == EBlockItemStatus.FILL);
      return r;
   }



   public void SetColor(Color newColor)
   {
      if (renderer && !IsKey)
         renderer.material.color = newColor;
   }
}
