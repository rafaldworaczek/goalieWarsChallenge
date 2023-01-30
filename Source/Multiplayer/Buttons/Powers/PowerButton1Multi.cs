using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalsNS;

public class PowerButton1Multi : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
	private bool clicked = false;
	private Button button;
	private Image buttonImage;
	private Color colorNonClicked;
	private Color colorClicked;
	private playerControllerMultiplayer playerMainScript;
	public PowersMulti powersScript;

	void Start()
	{
		button = GetComponent<Button>();
		buttonImage = button.GetComponent<Image>();
		//ColorUtility.TryParseHtmlString("#91DEE0", out colorNonClicked);
		//ColorUtility.TryParseHtmlString("#45B8FF", out colorNonClicked);
		//colorClicked.a = colorNonClicked.a = 0.6f;
		//ColorUtility.TryParseHtmlString("#91DEE0", out colorNonClicked);
		//ColorUtility.TryParseHtmlString("#B9F1F3", out colorNonClicked);
		//ColorUtility.TryParseHtmlString("#45B7FF", out colorNonClicked);
		ColorUtility.TryParseHtmlString("#46A9FF", out colorNonClicked);

		colorClicked = Color.yellow;
		playerMainScript = Globals.player1MainScript;
		powersScript = GameObject.Find("extraPowers").GetComponent<PowersMulti>();
	}

	void Update()
	{

		return;
		if (clicked)
		{
			buttonImage.color = colorClicked;
		}
		else
		{
			buttonImage.color = colorNonClicked;
		}
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		//print("BUTTONUN_CLICKED");
		//clicked = false;
	}

	public virtual void OnPointerDown(PointerEventData ped)
	{
		//if (playerMainScript.cpuPlayer.getShotActive() ||
		//    playerMainScript.cpuPlayer.getPreShotActive())
		//		return;

		powersScript.button1Action();

		clicked = !clicked;
		//print("BUTTONCLICKED");
	}

	public virtual void OnDrag(PointerEventData ped)
	{

	}

	public void changeNonClickedColorAlpha(float alpha)
	{
		colorNonClicked.a = alpha;
	}

	public void changeClickedColorAlpha(float alpha)
	{
		colorClicked.a = alpha;
	}

	public float getNonClickedColorAlpha()
	{
		return colorNonClicked.a;
	}

	public float getClickedColorAlpha() {
		return colorClicked.a;
	}

	public bool getButtonState()
    {
		return clicked;
    }

	public void setButtonState(bool state)
    {
		clicked = state;
    }
}
