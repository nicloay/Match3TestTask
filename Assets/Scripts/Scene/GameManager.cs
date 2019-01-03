using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using Logic.Actions;
using Match3.Logic;
using Match3.Logic.MatchFinder;
using UnityEngine;
using UnityEngine.Events;


namespace Match3.Scene
{	
	public enum GameState
	{
		Unknown,
		Initialization,
		WaitForInput,
		ActionInProgress,
		GameOver
	}

	[Serializable]
	public class GameStateChangeEvent : UnityEvent<GameState>
	{		
	}	

	[Serializable]
	public class TurnDoneEvent : UnityEvent
	{
	}	
	
	[Serializable]
	public class EventsConfig
	{
		public GameStateChangeEvent OnStateChange;		
		public TurnDoneEvent OnTurnDone;
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


		private GameState _gameState;

		public GameState GameState
		{
			get
			{
				return _gameState; 				
			}

			set
			{
				if (value != _gameState)
				{
					_gameState = value;
					Events.OnStateChange.Invoke(value);
				}
			}
		}
		public Hint[] Hints { get; private set; }		
		public Hint RandomHint { get; private set; }
		
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

			GameState = GameState.Initialization;
			_grid.Initialize(_game.Grid.Size);
			_grid.OnUserSwapFields.AddListener(TryToMakeSwap);
		}

		public void TryToMakeSwap(Vector2Int arg0, Vector2Int arg1)
		{
			GameState = GameState.ActionInProgress;
			if (_game.IsSwapPossible(arg0, arg1))
			{
				//make move
				List<DestroySpawnGravityAction> actions = _game.MakeSwap(arg0, arg1);								
				_grid.SwapDicesAndApplyActions(arg0, arg1, actions, OnAllVisualActionsDone);
				Events.OnTurnDone.Invoke();
			}
			else
			{
				_grid.ShowWrongMove(arg0, arg1, () => GameState = GameState.WaitForInput);
			}
		}

		private void OnAllVisualActionsDone()
		{
			UpdateHints();		
		}

		private void UpdateHints()
		{
			Hints = _game.GetHints();
			if (Hints.Length > 0)
			{				
				RandomHint = Hints[UnityEngine.Random.Range(0, Hints.Length)];
				GameState = GameState.WaitForInput;
			}
			else
			{
				GameState = GameState.GameOver;
			}			
		}
				

		IEnumerator Start()
		{
			yield return new WaitForSeconds(0.2f);
			var spawnActions = _game.FillEmptyGrid();
			_grid.HandleSpawns(spawnActions);
			UpdateHints();
		}
		
	}

	
	
	
}
