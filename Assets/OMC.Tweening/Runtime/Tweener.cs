using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace OMC.Tweening
{
    public class Tweener
    {
        private float _start;
        
        private float _end;

        private Action<float> _tweenAction;

        private float _duration;

        private float _diff;
        
        private float _elapsed;

        private GameObject _linkedGameObject;

        private Tweener(float start, Action<float> tweenAction, float end, float duration)
        {
            _start = start;
            _tweenAction = tweenAction;
            _end = end;
            _duration = duration;
            _diff = end - start;
            
            OnTweenStart();
        }
        
        public static void To(float start, Action<float> tweenAction, float end, float duration)
        {
            var tweener = new Tweener(start, tweenAction, end, duration);
        }

        private void OnTweenStart()
        {
            TweeningUpdateCore.OnOMCUpdateEvent += Tween;
        }

        private void Tween(float deltaTime)
        {
            Debug.Log("Test");
            
            _elapsed += deltaTime;

            if (!_linkedGameObject)
            {
            }
            if (_elapsed < _duration)
            {
                _tweenAction.Invoke(_elapsed / _duration * (_end - _start) + _start);
            }
            else
            {
                _tweenAction.Invoke(_end);
                OnTweenEnd();
            }
        }

        private void OnTweenEnd()
        {
            TweeningUpdateCore.OnOMCUpdateEvent -= Tween;
        }
    }
}
