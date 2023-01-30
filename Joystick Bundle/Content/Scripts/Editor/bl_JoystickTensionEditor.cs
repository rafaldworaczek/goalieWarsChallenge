using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace Lovatto.Joystick
{
    [CustomEditor(typeof(bl_JoystickTension))]
    public class bl_JoystickTensionEditor : Editor
    {
        bl_JoystickTension script;

        private void OnEnable()
        {
            script = (bl_JoystickTension)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical("box");
            script.indicatorType = (bl_JoystickTension.IndicatorType)EditorGUILayout.EnumPopup("Indicator Type", script.indicatorType, EditorStyles.toolbarPopup);
            GUILayout.Space(2);

            if (script.indicatorType == bl_JoystickTension.IndicatorType.FourAxis)
            {
                if (script.axisIndicators.Length <= 0) { script.axisIndicators = new CanvasGroup[4]; }
                script.fadeCurve = EditorGUILayout.CurveField("Fade Curve", script.fadeCurve);
                script.axisIndicators[0] = EditorGUILayout.ObjectField("Left Indicator", script.axisIndicators[0], typeof(CanvasGroup), true) as CanvasGroup;
                script.axisIndicators[1] = EditorGUILayout.ObjectField("Right Indicator", script.axisIndicators[1], typeof(CanvasGroup), true) as CanvasGroup;
                script.axisIndicators[2] = EditorGUILayout.ObjectField("Up Indicator", script.axisIndicators[2], typeof(CanvasGroup), true) as CanvasGroup;
                script.axisIndicators[3] = EditorGUILayout.ObjectField("Down Indicator", script.axisIndicators[3], typeof(CanvasGroup), true) as CanvasGroup;
            }
            else if (script.indicatorType == bl_JoystickTension.IndicatorType.StickAngle)
            {
                script.angleIndicator = EditorGUILayout.ObjectField("Indicator Pivot", script.angleIndicator, typeof(RectTransform), true) as RectTransform;
            }
            script.sourceJoystick = EditorGUILayout.ObjectField("Source Joystick", script.sourceJoystick, typeof(bl_Joystick), true) as bl_Joystick;
            EditorGUILayout.EndVertical();
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }
    }
}