using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGetButton : MonoBehaviour
{
    private ItemControl itemControl;
    public SaveItem saveItem;
    public PlayByAudioManager playByAudioManager;

    private void Awake()
    {
        if (itemControl == null)
        {
            itemControl = transform.parent.GetComponent<ItemControl>();
        }
    }

    private void Start()
    {
        if (itemControl._thisItem.isBought)
        {
            gameObject.SetActive(false);
        }
    }

    public void GetButton()
    {
        if (GameManager.Instance.UseGem(int.Parse(itemControl._thisItem.item.price)))
        {
            itemControl._thisItem.isBought = true;
            playByAudioManager.PlaySFXByAudioManager(1);
        }
        else
        {
            // 돈 없음
            int i = transform.parent.parent.parent.parent.childCount;
            transform.parent.parent.parent.parent.GetChild(i - 1).gameObject.SetActive(true);
            playByAudioManager.PlaySFXByAudioManager(2);
        }
        // 구매한 아이템 저장
        if (saveItem == null)
        {
            Debug.LogError("SaveItem 감지 안됨");
        }
        else
        {
            saveItem.SaveItemDataBuy(itemControl._thisItem.item.item_name);
            itemControl.UpdateThisItemUI();
        }
    }
}
