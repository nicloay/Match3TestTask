using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Scene
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DiceController : MonoBehaviour
    {
        public List<Sprite> SpriteById;
        private SpriteRenderer _spriteRenderer;

        public int DiceId { get; private set; }
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Assert.IsNotNull(_spriteRenderer);            
        }
        
        public void SetDice(int diceId)
        {
            Assert.IsTrue(diceId>=0 && diceId < SpriteById.Count);            
            _spriteRenderer.sprite = SpriteById[diceId];
            DiceId = diceId;
        }
    }
}