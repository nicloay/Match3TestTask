
using Match3.Scene;
using UnityEngine;
using UnityEngine.UI;

namespace Match3.Integrationtest
{
	
	[RequireComponent(typeof(Toggle))]
	public class AutoPlay : MonoBehaviour
	{
		private GameManager gameManager;
		
		void Awake()
		{
			gameManager = FindObjectOfType<GameManager>();
			Toggle toggle = GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(OnAutoplayChanged);
			OnAutoplayChanged(toggle.isOn);
		}

		private void OnAutoplayChanged(bool arg0)
		{
			if (arg0)
			{
				gameManager.Events.OnStateChange.AddListener(OnGameStateChange);
				OnGameStateChange(gameManager.GameState);
			}
			else
			{
				gameManager.Events.OnStateChange.RemoveListener(OnGameStateChange);				
			}
		}

		private void OnGameStateChange(GameState gameState)
		{
			if (gameState == GameState.WaitForInput)
			{
				gameManager.TryToMakeSwap(gameManager.RandomHint.First, gameManager.RandomHint.Second);
			} else if (gameManager.GameState == GameState.GameOver)
			{
				Debug.Log("GameOver");
			}
		}
	}
}
