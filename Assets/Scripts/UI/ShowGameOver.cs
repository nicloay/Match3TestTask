using System.Collections;
using System.Collections.Generic;
using GameView;
using UnityEngine;

public class ShowGameOver : MonoBehaviour
{
	[SerializeField]
	private GameObject _gameOverPopup;

	void Awake()
	{
		_gameOverPopup.SetActive(false);
		FindObjectOfType<GameManager>().Events.OnStateChange.AddListener(state =>
		{
			if (state == GameState.GameOver)
			{
				_gameOverPopup.SetActive(true);
			}
		});
	}
		
}
