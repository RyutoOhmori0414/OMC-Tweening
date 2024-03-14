using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OMC.Tweening
{
    [CustomPropertyDrawer(typeof(CurveClamp01Attribute))]
    public sealed class CurveClamp01AttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.AnimationCurve) return;

            var curve = property.animationCurveValue;

            Debug.Log(curve.keys.Length);
            
            if (curve.keys.Length < 2)
            {
                curve.keys = new[] { new Keyframe(0, 0), new Keyframe(1, 1) };
            }
            else
            {
                var first = curve.keys[0];
                var last = curve.keys[^1];
                curve.keys[0] = new Keyframe(0.0F, first.value, first.inTangent, first.outTangent, first.inWeight,
                    first.outWeight);
                curve.keys[^1] = new Keyframe(1.0F, last.value, last.inTangent, last.outTangent, last.inWeight,
                    last.outWeight);
            }
            
            
            
            var ranges = new Rect(0, 0, 1, 1);
            

            EditorGUI.CurveField(position, property, Color.cyan, ranges);
        }
    }   
}