using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using GlobalsNS;

public class gkMoveDownButtonMulti : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler 
{
	private bool clicked = false;
	private playerControllerMultiplayer player;
	private bool buttonHold = false;

	void Start()
	{
		player = Globals.player1MainScript;
	}

	void Update()
	{
		if (buttonHold)
			player.gkMoveDownAnim();
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		buttonHold = false;
	}

	public virtual void OnPointerDown(PointerEventData ped)
	{
		buttonHold = true;
		//print("GKSIDELEFTPOINTERDOWN");
	}

	public virtual void OnDrag(PointerEventData ped)
    {

    }

	public bool isButtonPress()
	{
		return buttonHold;
	}

	public bool getButtonState()
    {
		return clicked;
    }
}
