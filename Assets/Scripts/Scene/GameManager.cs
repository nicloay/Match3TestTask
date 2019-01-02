using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using Match3.Logic;
using Match3.Scene;
using UnityEngine;
using UnityEngine.Events;


namespace GameView
{	
	public enum GameState
	{
		Initialization,
		WaitForInput,
		ActionInProgress,
		Paused
	}

	[Serializable]
	public class GameStateChangeEvent : UnityEvent<GameState>
	{		
	}

	[Serializable]
	public class GameOverEvent : UnityEvent
	{		
	}


	[Serializable]
	public class EventsConfig
	{
		public GameStateChangeEvent OnStateChange;
		public GameOverEvent OnGameOver;
	}
	
	
	[DefaultExecutionOrder(int.MaxValue)]
	public class GameManager : MonoBehaviour
	{
		public EventsConfig Events;
		
		[SerializeField] private int _diceNumber = 5;

		[SerializeField] private GridController _grid;

		[SerializeField] private Vector2Int _size;
		
		
		public readonly GameState GameState;
				
		
		
		private Game _game;
		void Awake()
		{
			_game = new Game(_size, _diceNumber);	
			_grid.Initialize(_game.Grid.Size);	
			_grid.OnUserSwapFields.AddListener(TryToMakeSwap);
		}

		private void TryToMakeSwap(Vector2Int arg0, Vector2Int arg1)
		{
			if (_game.IsMovePossible(arg0, arg1))
			{
				//make move
			}
			else
			{
				_grid.ShwoWrongMove(arg0, arg1);
			}
		}

		IEnumerator Start()
		{
			yield return new WaitForSeconds(0.2f);
			var spawnActions = _game.FillEmptyGrid();
			_grid.HandleSpawns(spawnActions);
		}
		
	}

	
}
