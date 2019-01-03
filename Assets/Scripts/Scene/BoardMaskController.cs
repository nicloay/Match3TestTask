using Match3.Scene;
using UnityEngine;

namespace Scene
{
    [RequireComponent(typeof(SpriteMask))]
    public class BoardMaskController : MonoBehaviour
    {
        private SpriteMask _spriteMask;
        void Awake()
        {
            _spriteMask = GetComponent<SpriteMask>();
            FindObjectOfType<GridController>().OnGridSizeChanged.AddListener(SetupMask);            
        }

        private void SetupMask(Vector2 boardSize)
        {
            Vector3 scale = new Vector3();
            scale.x = boardSize.x / _spriteMask.sprite.bounds.size.x;
            scale.y = boardSize.y / _spriteMask.sprite.bounds.size.y;
            transform.localScale = scale;
        }
    }
}