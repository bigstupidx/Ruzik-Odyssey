using System;
using UnityEngine;
using RuzikOdyssey.Common;

namespace RuzikOdyssey.Level
{
	public class MenuController : MonoBehaviour
	{
		public static MenuController Instance { get; private set; }

		private GameObject ui;

		private void Awake()
		{
			if (Instance != null) throw new UnityException("Multiple menu controllers detected");
			Instance = this;
		}

		private void Start()
		{
			ui = GameObject.Find("UI");
			if (ui == null) throw new Exception("Failed to find game object named 'UI' in the hierarchy");
		}

		public void ShowGameOverMenu()
		{
			if (!GameEnvironment.IsGameOver)
			{
				GameEnvironment.GameOver();
				ui.AddComponent<GameOverMenu>();
			}
		}
	}
}

