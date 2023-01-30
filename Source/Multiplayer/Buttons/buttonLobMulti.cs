using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalsNS;

public class buttonLobMulti : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler 
{
	private bool clicked = false;
	private Button button;
	private Image buttonImage;
	private Color colorNonClicked;
	private Color colorClicked;
	private playerControllerMultiplayer playerMainScript;

	void Start()
	{
		playerMainScript = Globals.player1MainScript;
		button = GetComponent<Button>();
		buttonImage = button.GetComponent<Image>();

		//ColorUtility.TryParseHtmlString("#46A9FF", out colorNonClicked);
		//ColorUtility.TryParseHtmlString("#B9F1F3", out colorNonClicked);
		//ColorUtility.TryParseHtmlString("#91DEE0", out colorNonClicked);
		//ColorUtility.TryParseHtmlString("#B9F1F3", out colorNonClicked);
		//ColorUtility.TryParseHtmlString("#27FAFD", out colorNonClicked);
		//ColorUtility.TryParseHtmlString("#88D2D9", out colorNonClicked);
		ColorUtility.TryParseHtmlString("#FFFFFF", out colorNonClicked);

		//95CDD1

		//colorClicked.a = colorNonClicked.a = 0.6f;
		colorClicked = Color.yellow;
	}

	void Update()
	{
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
	
	}

	public virtual void OnPointerDown(PointerEventData ped)
	{
		if (playerMainScript.peerPlayer.getShotActive() ||
			playerMainScript.peerPlayer.getPreShotActive())
			return;

		clicked = !clicked;
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

	public float getClickedColorAlpha()
	{
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
