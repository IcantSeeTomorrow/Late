using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class UITest : UIWindow
{
    public Text title;

    private void Start()
    {
        
    }

    public void SetTitle(string title)
    {
        this.title.text = title;
    }
}
