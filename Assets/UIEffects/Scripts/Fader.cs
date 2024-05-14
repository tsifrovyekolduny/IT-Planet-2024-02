using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public Color BackgroundColor;
    public float Duration;
    public bool FadeIn;
    public bool StartFade;

    private Texture2D _texture;
    private float _alpha;
    private float _direction;

    private void Awake()
    {      
        if (FadeIn)
        {
            _alpha = 0f;
        }
        else
        {
            _alpha = 1f;
        }

        _texture = new Texture2D(1, 1);
        _texture.SetPixel(0, 0, new Color(BackgroundColor.r, BackgroundColor.g, BackgroundColor.b, _alpha));
        _texture.Apply();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (StartFade)
        {
            if (_alpha >= 1f)
            {
                _alpha = 0f;
            }
            else
            {
                _alpha = 0f;                
            }
        }
    }
    public void OnGUI()
    {
        if(_alpha > 0f)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);
            if(_direction != 0)
            {

            }
        }
    }
}
