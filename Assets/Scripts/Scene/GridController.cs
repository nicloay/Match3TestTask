using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using Match3.Utils;
using Scene;
using UnityEngine;
using UnityEngine.Events;

namespace Match3.Scene
{	
	public class GridController : MonoBehaviour
	{
		public UserSwapFieldsEvent OnUserSwapFields;
		
		[Serializable]
		public class GridSizeChangeEvent : UnityEvent<Vector2>{}

		public GridSizeChangeEvent OnGridSizeChanged;
		
		[SerializeField] private DicePool _dicePool; 
		
		[SerializeField] private Vector2 _bgTileOffset = new Vector2(2.7f, 2.7f);

		[SerializeField] private GameObject _fieldPrefab;		
		
		[SerializeField] private float _timePerCellTimeGravity;

		[SerializeField] private EaseType _gravityEaseType;
		
		[SerializeField] private float _wrongMoveDuration;
		
		[SerializeField] private EaseType _wrongMoveEaseType;
		
		
		private FieldController[,] _fields;
		private Vector2Int _boardSize;
		private Vector2 _boardPhysicalSize;
		public Vector2 BoardPhysicalSize
		{
			get { return _boardPhysicalSize; }
			set
			{
				if (value == _boardPhysicalSize)
				{
					return;
				}

				_boardPhysicalSize = value;
				OnGridSizeChanged.Invoke(value);
			}
		}

		public void Initialize(Vector2Int size)
		{
			_boardSize = size;
			Vector2 newBoardSize= _bgTileOffset;
			newBoardSize.x *= size.x;
			newBoardSize.y *= size.y;
			BoardPhysicalSize = newBoardSize;
			Vector2 startOffset = (BoardPhysicalSize - _bgTileOffset) / -2.0f;

			_fields = new FieldController[size.x, size.y];
			Vector2 position = startOffset;
			for (int rowId = 0; rowId < size.y; rowId++)
			{
				position.x = startOffset.x;
				for (int colId = 0; colId < size.x; colId++)
				{
					GameObject instance = Instantiate(_fieldPrefab, transform, false);
					instance.name = colId + "x" + rowId;
					instance.transform.localPosition = position;
					position.x += _bgTileOffset.x;
					_fields[colId, rowId] = instance.GetComponent<FieldController>();
					_fields[colId, rowId].Initialize(new Vector2Int(colId, rowId));					
					_fields[colId, rowId].OnUserSwapFields.AddListener(OnUserSwapFields.Invoke);
				}

				position.y += _bgTileOffset.y;
			}
		
			_dicePool.WarmUp(size.x * (size.y + 1));
		}

		public void HandleSpawns(List<SpawnDiceAction> actions)
		{
			StartCoroutine(DoHandleSpawns(actions));
		}

		
		// it's beter to use (MEC) instead of coroutines (as many junk in GC)
		private IEnumerator DoHandleSpawns(List<SpawnDiceAction> actions)
		{
			//1 find first empty field in the bottom which waiting for spawning
			Dictionary<int, int> bottomYByColumnId = new Dictionary<int, int>();
			actions.ForEach(action =>
			{
				int columnId = action.Destination.x;
				int rowId = action.Destination.y;
				if (bottomYByColumnId.ContainsKey(columnId))
				{
					bottomYByColumnId[columnId] = Mathf.Min(bottomYByColumnId[columnId], rowId);
				}
				else
				{
					bottomYByColumnId.Add(columnId, rowId);
				}				
			});

			Dictionary<int, OffsetWithTime> yOffsetByColumn = new Dictionary<int, OffsetWithTime>();
			foreach (KeyValuePair<int,int> keyValuePair in bottomYByColumnId)
			{
				int cellDicstance = _boardSize.y - keyValuePair.Value;

				OffsetWithTime value = new OffsetWithTime()
				{
					Offset = cellDicstance * _bgTileOffset.y,
					Duration = cellDicstance * _timePerCellTimeGravity
				};				
				
				yOffsetByColumn.Add(keyValuePair.Key, value);
			}
			
			// place dices with local offset and then start time 		
			HashSet<SpawnDiceAction> _fieldsInAction  = new HashSet<SpawnDiceAction>();
			foreach (var spawnAction in actions)
			{
				int columnId = spawnAction.Destination.x;
				int rowId = spawnAction.Destination.y;
				DiceController dice = _dicePool.Get();
				dice.SetDice(spawnAction.DiceId);
				_fields[columnId,rowId].SetDice(dice, yOffsetByColumn[columnId].Offset);
				_fieldsInAction.Add(spawnAction);
			}
						
			//animate fall down
			float time = 0.0f;
			List<SpawnDiceAction> actionToRemove = new List<SpawnDiceAction>();
			while (_fieldsInAction.Count > 0)
			{
				actionToRemove.Clear();
				foreach (var action in actions)
				{
					int columnId = action.Destination.x;
					int rowId = action.Destination.y;

					OffsetWithTime offsetWithTime = yOffsetByColumn[columnId];

					float easeTime = time;
					if (time > offsetWithTime.Duration)
					{
						easeTime = offsetWithTime.Duration;
						actionToRemove.Add(action);
					}

					float yOffset = Equations.ChangeFloat(easeTime, offsetWithTime.Offset, -offsetWithTime.Offset,
						offsetWithTime.Duration, _gravityEaseType);
					
					_fields[columnId,rowId].SetDiceOffset(yOffset);
				}


				foreach (var removeAction in actionToRemove)
				{
					_fieldsInAction.Remove(removeAction);
				}
								
				yield return null;
				time += Time.deltaTime;
			}											
		}

		public void ShwoWrongMove(Vector2Int from, Vector2Int to)
		{
			StartCoroutine(DoShwoWrongMove(from, to));
		}

		private IEnumerator DoShwoWrongMove(Vector2Int from, Vector2Int to)
		{
			FieldController fieldFrom = _fields[from.x, from.y];
			FieldController fieldTo = _fields[to.x, to.y];
			
			Vector2 diff = fieldFrom.transform.position - fieldTo.transform.position;

			float time = 0.0f;
			do
			{
				time += Time.deltaTime;
				time = Mathf.Min(time, _wrongMoveDuration);
				Vector3 offset =
					Equations.ChangeVector3PingPong(time, Vector3.zero, diff, _wrongMoveDuration, _wrongMoveEaseType);
				fieldFrom.Dice.transform.localPosition = -offset;
				fieldTo.Dice.transform.localPosition =  offset;
				yield return null;
			} while (time < _wrongMoveDuration);

		}
		
		struct OffsetWithTime
		{
			public float Offset;
			public float Duration;
		}
	}
}