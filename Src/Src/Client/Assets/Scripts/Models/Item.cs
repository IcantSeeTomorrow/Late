using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Item
    {
        public int ID;
        public int Count;
        public ItemDefine Define;
        public EquipDefine EquipInfo;

        public Item(NItemInfo item):
            this(item.Id,item.Count)
        {
        }
        public Item(int id,int count)
        {
            this.ID = id;
            this.Count = count;
            DataManager.Instance.Items.TryGetValue(this.ID, out this.Define);
            DataManager.Instance.Equips.TryGetValue(this.ID, out this.EquipInfo);
        }

        public override string ToString()
        {
            return string.Format("ID:{0},Count:{1}", this.ID, this.Count);
        }
    }
}
