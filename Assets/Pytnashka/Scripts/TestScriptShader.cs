using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestScriptShader : MonoBehaviour
{
    //SP = Shader property
    private static int SP_PlayerWorldPosition = Shader.PropertyToID("_CustomAlphaValue");
    //private float value = ;

    private void Start()
    {
        //_transform = transform;
    }

    private void Update()
    {
        Shader.SetGlobalFloat(SP_PlayerWorldPosition, CreateBoard.alpha_value);
    }
}
