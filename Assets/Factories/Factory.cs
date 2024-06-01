using System.Collections.Generic;
using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    public Transform SpawnPoint;
    public abstract GameObject CreateObject();

    public Bounds GetBoundsOf(Transform gmObj)
    {
        MeshRenderer meshR;
        meshR = gmObj.GetComponent<MeshRenderer>();
        if (meshR == null)
        {
            List<MeshRenderer> meshs = new List<MeshRenderer>();
            gmObj.GetComponentsInChildrenRecursively<MeshRenderer>(meshs);
            meshR = meshs[0];
            Debug.Log($"No meshR in this parent! First meshR of children: {meshR.ToString()}");
        }
        return meshR.localBounds;
    }    
    public float PlaceOn(Transform child, Transform platform)
    {
        float halfHeightOfPlatform = (GetBoundsOf(platform).size.y + platform.localScale.y) / 2 - 0.03f;

        return halfHeightOfPlatform;
    }
}
