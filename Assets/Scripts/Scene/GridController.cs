using System;
using Scene;
using UnityEngine;
using UnityEngine.Events;

namespace Match3.Scene
{
	
	public class DicePool : ObjectPool<DiceController>{}
	
	
	public class GridController : MonoBehaviour
	{		
		[Serializable]
		public class GridSizeChangeEvent : UnityEvent<Vector2>{}

		public GridSizeChangeEvent OnGridSizeChanged;
		
		[SerializeField] private DicePool _dicePool; 
		
		[SerializeField] private Vector2 _bgTileOffset = new Vector2(2.7f, 2.7f);

		[SerializeField] private GameObject _bgPrefab;

		[SerializeField] private GameObject _backgroundTilesParent;

		[SerializeField] private GameObject _dicePrefab;

		[SerializeField] private GameObject _diceParent;

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
			Vector2 newBoardSize= _bgTileOffset;
			newBoardSize.x *= size.x;
			newBoardSize.y *= size.y;
			BoardPhysicalSize = newBoardSize;
			Vector2 startOffset = (BoardPhysicalSize - _bgTileOffset) / -2.0f;


			Vector2 position = startOffset;
			for (int rowId = 0; rowId < size.y; rowId++)
			{
				position.x = startOffset.x;
				for (int colId = 0; colId < size.x; colId++)
				{
					GameObject instance = Instantiate(_bgPrefab, transform, false);
					instance.name = colId + "x" + rowId;
					instance.transform.localPosition = position;
					position.x += _bgTileOffset.x;
				}

				position.y += _bgTileOffset.y;
			}
		
			_dicePool.WarmUp(size.x * (size.y + 1));
		}
	}
}