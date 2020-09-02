using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class UIManager : Singleton<UIManager>
{
    class UIElement
    {
        public string Resource;
        public bool Cache;
        public GameObject Instance;
    }

    private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();

    /// <summary>
    /// 构造UIManager时加载各UI组件
    /// </summary>
    public UIManager()
    {
        //UIResources.Add(typeof(UITest), new UIElement() { Resource = "UI/UITest", Cache = true });
        UIResources.Add(typeof(UIBag), new UIElement() { Resource = "UI/UIBag", Cache = false });
        UIResources.Add(typeof(UIShop), new UIElement() { Resource = "UI/UIShop", Cache = false });


    }

    public T Show<T>()
    {
        //SoundManager.Instance.PlaySound("ui_open");
        Type type = typeof(T);
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if (info.Instance != null)
            {
                info.Instance.SetActive(true);
            }
            else
            {
                UnityEngine.Object prefab = Resources.Load(info.Resource);
                if (prefab == null)
                {
                    return default(T);
                }
                info.Instance = (GameObject)UnityEngine.Object.Instantiate(prefab);
            }
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }

    public void Close(Type type)
    {
        //SoundManager.Instance.PlaySound("ui_close");
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if (info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                UnityEngine.Object.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }
}
