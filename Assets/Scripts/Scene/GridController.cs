using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using Logic.Actions;
using Logic.Physics;
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
		public class GridSizeChangeEvent : UnityEvent<Vector2, Vector2Int>{} //PhysicalSize, GridSize

		public GridSizeChangeEvent OnGridSizeChanged;
		
		[SerializeField] private DicePool _dicePool; 
		
		[SerializeField] private Vector2 _bgTileOffset = new Vector2(2.7f, 2.7f);

		[SerializeField] private GameObject _fieldPrefab;		
		
		[SerializeField] private GravityHandler _gravity;		
		
		[SerializeField] private EaseConfig _wrongMoveEaseconfig;

		[SerializeField] private EaseConfig _swapMoveConfig;

		[SerializeField] private DestroyDiceController _diceDestroyer;
		
		
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
				OnGridSizeChanged.Invoke(value, _boardSize);
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
			_diceDestroyer.Initialize(_dicePool);
		}

		public void HandleSpawns(List<SpawnDiceAction> actions)
		{
			StartCoroutine(DoHandleSpawns(actions));
		}

		
		// it's beter to use (MEC) instead of coroutines (as many junk in GC)
		private IEnumerator DoHandleSpawns(List<SpawnDiceAction> actions)
		{
			//1 find first empty field in the bottom which waiting for spawning
			SpawnDicesWithOffsets(actions);

			yield return StartCoroutine(_gravity.ApplyGravity(_fields));
		}

		private void SpawnDicesWithOffsets(List<SpawnDiceAction> actions)
		{
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
			foreach (KeyValuePair<int, int> keyValuePair in bottomYByColumnId)
			{
				int cellDicstance = _boardSize.y - keyValuePair.Value;

				OffsetWithTime value = new OffsetWithTime()
				{
					Offset = cellDicstance * _bgTileOffset.y,
				};

				yOffsetByColumn.Add(keyValuePair.Key, value);
			}

			// place dices with local offset and then start time 		
			HashSet<SpawnDiceAction> _fieldsInAction = new HashSet<SpawnDiceAction>();
			foreach (var spawnAction in actions)
			{
				int columnId = spawnAction.Destination.x;
				int rowId = spawnAction.Destination.y;
				DiceController dice = _dicePool.Get();
				dice.SetDice(spawnAction.DiceId);
				_fields[columnId, rowId].SetDice(dice, yOffsetByColumn[columnId].Offset);
				_fieldsInAction.Add(spawnAction);
			}
		}

		public void ShowWrongMove(Vector2Int from, Vector2Int to, Action onDone)
		{
			StartCoroutine(TweenDices(from, to, _wrongMoveEaseconfig, Equations.ChangeVector3PingPong, onDone));
		}

		

		public IEnumerator TweenDices(Vector2Int @from, Vector2Int to, EaseConfig easeConfig,
			Equations.ChangeVector3Delegate function, Action onDone)
		{
			FieldController fieldFrom = _fields[from.x, from.y];
			FieldController fieldTo = _fields[to.x, to.y];
			
			Vector2 diff = fieldFrom.transform.position - fieldTo.transform.position;

			float time = 0.0f;
			do
			{
				time += Time.deltaTime;
				time = Mathf.Min(time, easeConfig.Duration);
				Vector3 offset = function(time, Vector3.zero, diff, easeConfig.Duration, easeConfig.EaseType);
				fieldFrom.Dice.transform.localPosition = -offset;
				fieldTo.Dice.transform.localPosition =  offset;
				yield return null;
			} while (time < easeConfig.Duration);

			if (onDone != null)
			{
				onDone();
			}
		}


		public void SwapDicesAndApplyActions(Vector2Int from, Vector2Int to, List<DestroySpawnGravityAction> actions, Action onDone)
		{
			StartCoroutine(DoSwapDicesAndApplyActions(from, to, actions, onDone));
		}
				
		private IEnumerator DoSwapDicesAndApplyActions(Vector2Int @from, Vector2Int to,
			List<DestroySpawnGravityAction> actions, Action onDone)
		{
			yield return StartCoroutine(TweenDices(@from, to, _swapMoveConfig, Equations.ChangeVector3, null));						
			_fields[from.x, from.y].SwapDiceWith(_fields[to.x, to.y]);			
			foreach (var action in actions)
			{
				
				//destroy
				foreach (var actionDestroy in action.Destroys)
				{
					StartCoroutine( _diceDestroyer.DestroyDice(_fields[actionDestroy.CellPosition.x, actionDestroy.CellPosition.y]));					
				}

				while (_diceDestroyer.DestroyAction > 0)
				{
					yield return null;
				}
				
				
				//spawn + gravity				
				
				MoveDicesFromFieldToField(action.Moves);
				SpawnDicesWithOffsets(action.Spawns);
								
				yield return  StartCoroutine(_gravity.ApplyGravity(_fields));
				
				yield return null;
			}
			onDone();
		}

		private void MoveDicesFromFieldToField(List<DiceMovement> movements)
		{
			movements.ForEach(movement => _fields[movement.Source.x, movement.Source.y].SwapDiceWith(_fields[movement.Destination.x, movement.Destination.y]));
		}
		

		private void DestroyDice(Vector2Int position)
		{
			_dicePool.Release(_fields[position.x, position.y].Dice);
			_fields[position.x, position.y].ClearDice();
		}
		
		
		struct OffsetWithTime
		{
			public float Offset;			
		}
	}
}