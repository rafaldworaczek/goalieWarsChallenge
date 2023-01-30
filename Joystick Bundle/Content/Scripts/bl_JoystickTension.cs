using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lovatto.Joystick
{
	public class bl_JoystickTension : MonoBehaviour
	{
		public IndicatorType indicatorType = IndicatorType.FourAxis;
		public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);
		public RectTransform angleIndicator;
		public CanvasGroup[] axisIndicators = new CanvasGroup[4]; //0 = left, 1 = right, 2 = up, 3 = down
		public bl_Joystick sourceJoystick;

		/// <summary>
		/// 
		/// </summary>
		void Update()
		{
			if (sourceJoystick == null) return;
			if (indicatorType == IndicatorType.FourAxis)
			{
				FourAxis();
			}
			else if (indicatorType == IndicatorType.StickAngle)
			{
				StickAngle();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		void FourAxis()
		{
			//left
			float value = sourceJoystick.Horizontal > 0 ? 0 : Mathf.Abs(sourceJoystick.Horizontal);
			if (axisIndicators[0] != null)
				axisIndicators[0].alpha = fadeCurve.Evaluate(value);
			//right
			value = sourceJoystick.Horizontal < 0 ? 0 : sourceJoystick.Horizontal;
			if (axisIndicators[1] != null)
				axisIndicators[1].alpha = fadeCurve.Evaluate(value);
			//up
			value = sourceJoystick.Vertical < 0 ? 0 : sourceJoystick.Vertical;
			if (axisIndicators[2] != null)
				axisIndicators[2].alpha = fadeCurve.Evaluate(value);
			//down
			value = sourceJoystick.Vertical > 0 ? 0 : Mathf.Abs(sourceJoystick.Vertical);
			if (axisIndicators[3] != null)
				axisIndicators[3].alpha = fadeCurve.Evaluate(value);
		}

		/// <summary>
		/// 
		/// </summary>
		void StickAngle()
		{
			angleIndicator.up = sourceJoystick.StickRect.position - angleIndicator.position;
		}

		[Serializable]
		public enum IndicatorType
		{
			FourAxis,
			StickAngle,
		}
	}
}