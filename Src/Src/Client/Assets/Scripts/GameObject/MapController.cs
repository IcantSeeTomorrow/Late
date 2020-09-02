using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class MapController : MonoBehaviour
{
    public Collider minimapBoundingBox;
    private void Start()
    {
        MinimapManager.Instance.UpdateMinimap(minimapBoundingBox);
    }


}
