
using Match3.Scene;
using UnityEngine;

namespace Match3.UI
{
	public class ShowGameOver : MonoBehaviour
	{
		[SerializeField] private GameObject _gameOverPopup;

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
}