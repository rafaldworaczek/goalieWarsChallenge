using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class buttonCamera : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler 
{
	private bool clicked = false;
	private controllerRigid playerMainScript;
	private Button button;
	private Image buttonImage;
	private Color colorNonClicked;
	private Color colorClicked;
	public Text cameraText;
	private float lastCameraUpdate;
	private int cameraIdx = 0;
	private int CAMERAS_MAX = 2;

	void Start()
	{	
		button = GetComponent<Button>();
		buttonImage = button.GetComponent<Image>();
		ColorUtility.TryParseHtmlString("#FF5577", out colorNonClicked);
		playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();
		colorClicked = Color.yellow;
		cameraText.text = "";
	}

	void Update()
    {
		/*if (clicked)
		{
			buttonImage.color = colorClicked;
		}
		else
		{
			buttonImage.color = colorNonClicked;
		}*/

		if (Time.time - lastCameraUpdate > 2.0f)
			cameraText.text = "";
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		if (playerMainScript.cpuPlayer.getShotActive())
			return;

		//clicked = false;

	}
	public virtual void OnPointerDown(PointerEventData ped)
	{
		if (playerMainScript.cpuPlayer.getShotActive())
			return;

		clicked = !clicked;

		cameraIdx++;
		cameraIdx = cameraIdx % CAMERAS_MAX;
		cameraText.text = "CAMERA" + (cameraIdx + 1).ToString();
		playerMainScript.cameraChanged(true);
		StartCoroutine(playerMainScript.flashBackground(0.01f));
		
	
		lastCameraUpdate = Time.time;
	}

	public virtual void OnDrag(PointerEventData ped)
    {

    }

	public void changeNonClickedColorAlpha(float alpha)
	{
		colorNonClicked.a = alpha;
	}

	public float getNonClickedColorAlpha()
	{
		return colorNonClicked.a;
	}
	
	public bool getButtonState()
    {
		return clicked;
    }

	public void setButtonState(bool state)
	{
		clicked = state;
	}

	public int getCameraIdx()
    {
		return cameraIdx;
	}

	public void setCameraIdx(int idx)
    {
		cameraIdx = idx;
    }
}
