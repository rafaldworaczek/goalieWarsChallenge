using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class joystick1 : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

	private Image joystickBG;
	private Image joystickS;
	public Vector2 pos;
	public static Vector3 inputVector;
	GameObject imageObject;
	private bool joystickUsed = false;
	private int pointerId = -1;

	void Awake()
	{
		joystickBG = GetComponent<Image>();
		joystickS = transform.GetChild(0).GetComponent<Image>();

		//print("JoystickSTART1 " + joystickS);
	}

	public virtual void OnDrag(PointerEventData ped)
	{ 
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBG.rectTransform, 
			ped.position, 
			ped.pressEventCamera, 
			out pos))
		{
			joystickUsed = true;
			pointerId = ped.pointerId;

			pos.x = (pos.x / joystickBG.rectTransform.sizeDelta.x);
			pos.y = (pos.y / joystickBG.rectTransform.sizeDelta.y);


			//Debug.Log("POS + " + pos);

			inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
			inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;


			//print("INPUT VECTOR " + inputVector);	

			joystickS.rectTransform.anchoredPosition =
				new Vector3(inputVector.x * (joystickBG.rectTransform.sizeDelta.x / 3.8f)
					, inputVector.z * (joystickBG.rectTransform.sizeDelta.y / 3.8f));
		}
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		inputVector = Vector3.zero;
		joystickS.rectTransform.anchoredPosition = Vector3.zero;
		joystickUsed = false;
		pointerId = -1;
	}

	public virtual void OnPointerDown(PointerEventData ped)
	{
		OnDrag(ped);
	}

	public bool isJoystickUsed()
    {
		return joystickUsed;
    }

	public int getPointerId()
    {
		return pointerId;
	}

	public float Horizontal() {
		if (inputVector.x != 0) {
			return inputVector.x;
		} else {
			return Input.GetAxis ("Horizontal");
		}		
	}
		
	public float Vertical() {
		if (inputVector.z != 0) {
			return inputVector.z;
		} else {
			return Input.GetAxis ("Vertical");
		}		
	}

	public void changeColorButton(Color32 color)
    {
		joystickS.color = color;
	}

	public void setDefaultColorButton()
    {
		joystickS.color = new Color32(17, 59, 77, 255);
	}

	public void zeroPosition()
    {
		inputVector = Vector3.zero;
		joystickS.rectTransform.anchoredPosition = Vector3.zero;
		pointerId = -1;
	}
}
