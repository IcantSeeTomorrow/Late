﻿using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TeleporterObject : MonoBehaviour
{
    public int ID;
    Mesh mesh = null;

    private void Start()
    {
        this.mesh = this.GetComponent<MeshFilter>().sharedMesh;
    }

     void OnTriggerEnter(Collider other)
    {
        PlayerInputController playerInputController = other.GetComponent<PlayerInputController>();
        if (playerInputController != null && playerInputController.isActiveAndEnabled)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
            if (td == null)
            {
                Debug.LogErrorFormat("TeleporterObject: Character [{0}] Enter Teleporter [{1}], But TeleporterDefine not existed", playerInputController.character.Info.Name, this.ID);
                return;
            }
            Debug.LogFormat("TeleporterObject: Character [{0}] Enter Teleporter [{1}];{2}", playerInputController.character.Info.Name, td.ID, td.Name);
            if (td.LinkTo > 0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                    MapService.Instance.SendMapTeleport(this.ID);
                else
                    Debug.LogErrorFormat("Teleporter ID:{0} LinkID {1} error!", td.ID, td.LinkTo);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (this.mesh != null)
        {
            Gizmos.DrawWireMesh(this.mesh, this.transform.position /*+ Vector3.up * this.transform.localScale.y*0.5f*/ , this.transform.rotation, this.transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, this.transform.position, this.transform.rotation, 1f, EventType.Repaint);
    }
#endif
    
}
