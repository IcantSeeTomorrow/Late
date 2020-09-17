using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        /// <summary>
        /// 地图中的角色，以CharacterID为Key
        /// </summary>
        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();


        //刷怪管理器
        SpawnManager SpawnManager = new SpawnManager();
        //怪物管理器
        public MonsterManager MonsterManager = new MonsterManager();

        internal Map(MapDefine define)
        {
            this.Define = define;
            this.SpawnManager.Init(this);
            this.MonsterManager.Init(this);
        }

        internal void Update()
        {
            SpawnManager.Update();
        }


        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> sender, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);
            character.Info.mapId = this.ID;
            this.MapCharacters[character.Id] = new MapCharacter(sender, character);

            sender.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            sender.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            foreach (var kv in this.MapCharacters)
            {
                sender.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                if (kv.Value.character!=character)
                    this.AddCharacterEnterMap(kv.Value.connection, character.Info);
            }
            foreach (var kv in this.MonsterManager.Monsters)
            {
                sender.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.Info);
            }
            
            sender.SendResponse();
        }

        void AddCharacterEnterMap(NetConnection<NetSession> sender, NCharacterInfo character)
        {
            if (sender.Session.Response.mapCharacterEnter==null)
            {
                sender.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
                sender.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            }
            sender.Session.Response.mapCharacterEnter.Characters.Add(character);
            sender.SendResponse();
        }

        internal void CharacterLeave(Character character)
        {
            Log.InfoFormat("CharacterLeave: Map:{0} characterId:{1}", this.Define.ID, character.Id);
            foreach (var kv in this.MapCharacters)
            {
                this.SendCharacterLeaveMap(kv.Value.connection, character);
            }
            this.MapCharacters.Remove(character.Id);
        }

        private void SendCharacterLeaveMap(NetConnection<NetSession> sender, Character character)
        {
            Log.InfoFormat("SendCharacterLeaveMap To {0}:{1} : Map:{2} character:{3}{4}",sender.Session.Character.Id,sender.Session.Character.Info.Name, this.Define.ID, character.Id,character.Info.Name);
            sender.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            sender.Session.Response.mapCharacterLeave.entityId = character.entityId;
            sender.SendResponse();
        }

        internal void UpdateEntity(NEntitySync entity)
        {
            foreach (var kv in this.MapCharacters)
            {
                if (kv.Value.character.entityId == entity.Id)
                {
                    kv.Value.character.Position = entity.Entity.Position;
                    kv.Value.character.Direction = entity.Entity.Direction;
                    kv.Value.character.Speed = entity.Entity.Speed;
                }
                else
                {
                    MapService.Instance.SendEntityUpdate(kv.Value.connection, entity);
                }
            }
        }

        /// <summary>
        /// 怪物进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void MonsterEnter(Monster monster)
        {
            Log.InfoFormat("MonsterEnter: Map:{0} monsterId:{1}", this.Define.ID, monster.Id);
            foreach (var kv in this.MapCharacters)
            {
                this.AddCharacterEnterMap(kv.Value.connection, monster.Info);
            }

        }
    }
}
