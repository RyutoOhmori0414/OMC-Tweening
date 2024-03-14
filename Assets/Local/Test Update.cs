using System;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.UI;
using OMC.Tweening;
using UnityEngine.Serialization;

public class TestUpdate : MonoBehaviour
{
    [FormerlySerializedAs("_textUI")] [SerializeField]
    private Text _textUI1;
    
    [SerializeField]
    private Text _textUI2;

    [SerializeField, CurveClamp01] 
    private AnimationCurve _curve;

    private float _elapsed = 0;
    
    void Start()
    {
        ParticleSystem p;
        
        Tweener.To(1111,
            x => _textUI1.text = x.ToString("00.00"),
            -100, 
            5);
    }

    private void Update()
    {
        if (_elapsed >= 5)
        {
            _textUI2.text = 5.ToString("00.00");
            
            return;
        }
        
        _elapsed += Time.deltaTime;

        _textUI2.text = _elapsed.ToString("00.00");
    }
}
