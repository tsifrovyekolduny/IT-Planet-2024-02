using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceScript : MonoBehaviour
{
    [SerializeField]
    private Material FaceMaterial;

    [SerializeField]
    private GameObject _goodMusic;

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
        if(shiftY == 0.69f)
        {
            TurnHappyMusic();
        }
        FaceMaterial.mainTextureOffset = new Vector2(0, shiftY);
    }

    void TurnHappyMusic()
    {        
        _goodMusic.SetActive(true);
    }
}
