using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using SkillBridge.Message;

namespace Managers
{
    class BagManager : Singleton<BagManager>
    {
        public int Unlocked;
        public BagItem[] Items;//物品的集合

        NBagInfo Info;

        unsafe public void Init(NBagInfo info)
        {
            this.Info = info;
            this.Unlocked = Info.Unlocked;
            Items = new BagItem[this.Unlocked];//按解锁格数初始化物品
            if (info.Items != null && info.Items.Length >= this.Unlocked)
            {
                Analyze(info.Items);
            }
            else
            {
                info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }

        public void Reset()
        {
            int i = 0;
            foreach (var kv in ItemManager.Instance.Items)
            {
                if (kv.Value.Count <= kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count > kv.Value.Define.StackLimit)
                    {
                        this.Items[i].ItemId = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                        i++;
                        count -= kv.Value.Define.StackLimit;
                    }
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)count;
                }
                i++;
            }
        }

        /// <summary>
        /// 将数据库字节数组记录转换为结构体数组
        /// </summary>
        /// <param name="data"></param>
        unsafe void Analyze(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    Items[i] = *item;
                }
            }
        }
        /// <summary>
        /// 将结构体数组记录转换为字节数组
        /// </summary>
        /// <returns></returns>
        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = Info.Items)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return this.Info;
        }

        public void AddItem(int itemId, int count)
        {
            ushort addCounnt = (ushort)count;
            for (int i = 0; i < Items.Length; i++)
            {
                if (this.Items[i].ItemId == itemId)
                {
                    ushort canAdd = (ushort)(DataManager.Instance.Items[itemId].StackLimit - this.Items[i].Count);
                    if (canAdd >= addCounnt)
                    {
                        this.Items[i].Count += addCounnt;
                        addCounnt = 0;
                        break;
                    }
                    else
                    {
                        this.Items[i].Count += canAdd;
                        addCounnt -= canAdd;
                    }
                }
            }
            if (addCounnt > 0)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (this.Items[i].ItemId == 0)
                    {
                        this.Items[i].ItemId = (ushort)itemId;
                        ushort canAdd = (ushort)(DataManager.Instance.Items[itemId].StackLimit - addCounnt);
                        if (canAdd >= addCounnt)
                        {
                            this.Items[i].Count = addCounnt;
                            break;
                        }
                        else
                        {
                            this.Items[i].Count = canAdd;
                            addCounnt -= canAdd;
                        }

                    }
                }
            }
        }

        public void RemoveItem(int itemId, int count)
        {

        }
    }
}
