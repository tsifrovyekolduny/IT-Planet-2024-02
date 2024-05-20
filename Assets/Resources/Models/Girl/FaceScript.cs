using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceScript : MonoBehaviour
{
    [SerializeField]
    private Material FaceMaterial;
    // Start is called before the first frame update
    void Start()
    {
        SkinnedMeshRenderer meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        
        foreach(var material in meshRenderer.materials)
        {
            Debug.Log(material.name);
            if(material.name.Contains("Face"))
            {
                FaceMaterial = material;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeFace(float shiftY)
    {
        FaceMaterial.mainTextureOffset = new Vector2(0, shiftY);
    }
}
