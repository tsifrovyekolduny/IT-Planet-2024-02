using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public Color BackgroundColor;
    public float Duration;
    public bool FadeIn;
    public int LoopTimes = 1;    

    public AnimationCurve Curve = new AnimationCurve(
        new Keyframe(0, 1), 
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), 
        new Keyframe(1, 0)
    );

    // Debug only //
    [SerializeField]
    private bool _startFadeFromInspector;

    private Texture2D _texture;
    private float _alpha;
    private float _direction;
    private float _timer;
    private int _loopCounter = 0;

    private void Start()
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
        DrawToTexture();
    }

    private void DrawToTexture()
    {
        _texture.SetPixel(0, 0, new Color(BackgroundColor.r, BackgroundColor.g, BackgroundColor.b, _alpha));
        _texture.Apply();
    }

    public void StartFade()
    {
        _loopCounter = LoopTimes;
    }

    void Update()
    {
        if (_startFadeFromInspector)
        {
            _startFadeFromInspector = false;
            StartFade();
        }

        if (_loopCounter > 0 && _direction == 0)
        {
            if (_alpha >= 1f)
            {                
                _alpha = 0f;
                _timer = 0f;
                _direction = 1;
            }
            else
            {                
                _alpha = 0f;
                _timer = 1f;    
                _direction = -1;
            }
        }
    }    

    public void OnGUI()
    {
        if(_alpha > 0f)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);            
        }
        if (_direction != 0)
        {
            _timer += _direction * Time.deltaTime * Duration;            
            _alpha = Curve.Evaluate(_timer);            
            DrawToTexture();
            if (_alpha <= 0f || _alpha >= 1f)
            {
                _loopCounter -= 1;
                _direction = 0;
            }
        }
    }
}
