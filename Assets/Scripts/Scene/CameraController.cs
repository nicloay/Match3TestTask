
using GameView;
using Match3.Scene;
using UnityEngine;
using UnityEngine.Assertions;

namespace Match3.Scene
{
	

}

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	private Camera _camera;
	void Awake()
	{
		_camera = GetComponent<Camera>();
		Assert.IsNotNull(_camera);
		FindObjectOfType<GridController>().OnGridSizeChanged.AddListener(OnBoardSizeChange);
	}

	public void OnBoardSizeChange(Vector2 boardSize)
	{
		if (boardSize.y * _camera.aspect > boardSize.x)
		{
			_camera.orthographicSize = boardSize.y / 2.0f;
		}
		else
		{
			_camera.orthographicSize = boardSize.x / _camera.aspect / 2.0f;
		}
	}
}
