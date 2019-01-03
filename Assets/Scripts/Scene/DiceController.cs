using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Scene
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DiceController : MonoBehaviour
    {
        [SerializeField]        
        private List<Sprite> _spriteById;        
        private SpriteRenderer _spriteRenderer;

        public int DiceId { get; private set; }

        public float Alpha
        {
            set
            {
                _spriteRenderer.color = new Color(1f, 1f, 1f, value);                 
            }
        }        
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(_spriteRenderer);            
        }
        
        public void SetDice(int diceId)
        {
            Assert.IsTrue(diceId>=0 && diceId < _spriteById.Count);            
            _spriteRenderer.sprite = _spriteById[diceId];
            transform.localPosition = Vector3.zero;
            DiceId = diceId;            
        }
    

    }
}