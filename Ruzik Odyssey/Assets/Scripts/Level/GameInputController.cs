﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using RuzikOdyssey.Characters;
using System;

namespace RuzikOdyssey
{

	public class GameInputController : MonoBehaviour 
	{
		private IList<TouchButton> buttons;
		private GameCharacterController playerController;

		private int movementTouchId;

		private void Start()
		{
			buttons = GameObject.FindGameObjectsWithTag("TouchButton")
				.Select(x => x.GetComponent<TouchButton>())
				.ToList();

			playerController = GameObject.FindGameObjectWithTag("Player")
				.GetComponent<GameCharacterController>();
			if (playerController == null) throw new UnityException("Failed to initialize player controller");

			movementTouchId = -1;
		}

		private void Update() 
		{	
			if (Input.touchCount > 2 && Input.touchCount <= 0) 
			{
				playerController.Stop();
				return;
			}

			foreach (var touch in Input.touches)
			{
				var isButtonTouch = false;
				foreach (var button in buttons)
				{
					if (button.HitTest(touch.position)) 
					{
						isButtonTouch = true;
						if (touch.phase == TouchPhase.Began)
						{
							button.Touch();
						}
						else if (touch.fingerId == movementTouchId) ProcessMovementTouch(touch);
					}
				}

				if (isButtonTouch && Input.touchCount == 1) playerController.Stop();
				if (isButtonTouch || Environment.IsGameOver) continue;


				if (movementTouchId < 0 && (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved))
				{
					movementTouchId = touch.fingerId;
					playerController.Stop();
					playerController.Move(touch.position, touch.deltaPosition);
				}
				else if (touch.fingerId == movementTouchId) ProcessMovementTouch(touch);
		    }
		}

		private void ProcessMovementTouch(Touch touch)
		{
			if (Environment.IsGameOver) return;

			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
			{
				playerController.AttackWithMainWeapon();
				playerController.Move(touch.position, touch.deltaPosition);
			}
			else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				movementTouchId = -1;
				playerController.Stop();
			}
		}
	}
}