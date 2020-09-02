using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 单个物品图标数量 
/// </summary>
public class UIIconItem : MonoBehaviour
{
    public Image mainImage;
    public Image secondImage;

    public Text mainText;

    public void SetMainIcon(string iconName,string text)
    {
        this.mainImage.overrideSprite = Resloader.Load<Sprite>(iconName);//define的"Icon": "UI/Items/hongp",已完整
        this.mainText.text = text;
    }
}
