using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using Logic.Actions;
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

		[SerializeField] private bool _useStaticSeed;
		[SerializeField] private int _seed;
		
		public readonly GameState GameState;
				
		
		
		private Game _game;
		void Awake()
		{
			if (_useStaticSeed)
			{				
				_game = new Game(_size, _diceNumber, _seed);	
			}
			else
			{
				_game = new Game(_size, _diceNumber);
			}
			
			_grid.Initialize(_game.Grid.Size);	
			_grid.OnUserSwapFields.AddListener(TryToMakeSwap);
		}

		private void TryToMakeSwap(Vector2Int arg0, Vector2Int arg1)
		{
			if (_game.IsSwapPossible(arg0, arg1))
			{
				//make move
				List<DestroySpawnGravityAction> actions = _game.MakeSwap(arg0, arg1);								
				_grid.SwapDicesAndApplyActions(arg0, arg1, actions);
			}
			else
			{
				_grid.ShowWrongMove(arg0, arg1);
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
