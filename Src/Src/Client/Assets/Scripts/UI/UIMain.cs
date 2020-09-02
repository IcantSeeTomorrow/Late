using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : MonoSingleton<UIMain>
{

    public Text avatarName;
    public Text avatarLevel;

    protected override void OnStart()
    {
        this.UpdateAvatar();
    }

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    void Update()
    {

    }

    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }

    public void OnClickTest()
    {
        UITest test = UIManager.Instance.Show<UITest>();
        test.SetTitle("OMG你打开了这个界面！");
        test.OnClose += Test_OnClose;
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        //(sender as UITest).title.text = "龟龟";
        MessageBox.Show("点击了对话框的： " + result, "对话框相应结果： " + MessageBoxType.Information);
    }

    private bool BagOpen = false;
    public void OnClickBag()
    {
        if (!BagOpen)
        {
            UIManager.Instance.Show<UIBag>();
            BagOpen = true;
        }          
        else
        {
            UIManager.Instance.Close(typeof(UIBag));
            BagOpen = false;
        }
    }

    private bool EquipPanelOpen = false;
    public void OnClickCharEquip()
    {
        if (!EquipPanelOpen)
        {
            UIManager.Instance.Show<UICharEquip>();
            EquipPanelOpen = true;
        }
        else
        {
            UIManager.Instance.Close(typeof(UICharEquip));
            EquipPanelOpen = false;
        }

    }
}
