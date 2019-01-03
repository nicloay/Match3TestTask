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

        private void SetupMask(Vector2 physicalSize, Vector2Int gridSize)
        {
            Vector3 scale = new Vector3();
            scale.x = physicalSize.x / _spriteMask.sprite.bounds.size.x;
            scale.y = physicalSize.y / _spriteMask.sprite.bounds.size.y;
            transform.localScale = scale;
        }
    }
}