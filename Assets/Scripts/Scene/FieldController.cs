using UnityEngine;

namespace Scene
{
    public class FieldController : MonoBehaviour
    {
        public DiceController Dice { get; private set; }
        
        
        private Vector2Int _position;
        public void Initialize(Vector2Int position)
        {
            _position = position;
        }

        public void SetDice(DiceController dice)
        {
            SetDice(dice, 0);
        }

        public void SetDice(DiceController dice, float yOffset)
        {
            Dice = dice;
            dice.transform.SetParent(transform);
            SetDiceOffset(yOffset);
            dice.gameObject.SetActive(true);
        }

        public void SetDiceOffset(float yOffset)
        {
            Dice.transform.localPosition = new Vector3(0f, yOffset, 0f);
        }
    }
}