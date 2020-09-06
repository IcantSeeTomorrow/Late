using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class UIShop : UIWindow
{
    public Text title;
    public Text moeny;

    public GameObject shopItem;
    ShopDefine shop;
    public Transform[] itemRoot;

    private void Start()
    {
        StartCoroutine(InitItems());
    }

    IEnumerator InitItems()
    {
        int count = 0;
        int page = 0;
        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            if (kv.Value.Status > 0)
            {
                GameObject go = Instantiate(shopItem, itemRoot[page]);
                UIShopItem ui = go.GetComponent<UIShopItem>();
                ui.SetShopItem(kv.Key, kv.Value, this);
                count++;
                if (count>=10)
                {
                    count = 0;
                    page++;
                    itemRoot[page].gameObject.SetActive(true);
                }
            }
        }
        yield return null;
    }

    public void SetShop(ShopDefine shop)
    {
        this.shop = shop;
        this.title.text = shop.Name;
        this.moeny.text = User.Instance.CurrentCharacter.Gold.ToString();
    }

    private UIShopItem selectedItem;
    public void SelectShopItem(UIShopItem item)
    {
        if (selectedItem!=null)
            selectedItem.Selected = false;//取消之前选中
        selectedItem = item;
    }

    public void OnClickBuy()
    {
        if (this.selectedItem==null)
        {
            MessageBox.Show("请选择要购买的道具", "购买提示"); ;
            return;
        }
        if (!ShopManager.Instance.BuyItem(this.shop.ID,this.selectedItem.ShopItemID))
        {

        }
    }
}
