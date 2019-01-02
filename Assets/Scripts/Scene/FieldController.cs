using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Scene
{

    [Serializable]
    public class UserSwapFieldsEvent : UnityEvent<Vector2Int, Vector2Int>
    {            
    }
    
    
    public class FieldController : MonoBehaviour, IDropHandler, IDragHandler
    {
        public UserSwapFieldsEvent OnUserSwapFields;
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

        public void OnDrop(PointerEventData eventData)
        {            
            FieldController otherObject = eventData.pointerDrag.GetComponent<FieldController>();
            if (otherObject != null && IsNeighbourWith(otherObject))
            {
                OnUserSwapFields.Invoke(_position, otherObject._position);
            }
        }

        public bool IsNeighbourWith(FieldController other)
        {
            if (other._position.x == _position.x && Mathf.Abs(other._position.y - _position.y) == 1)
            {
                return true;
            }else if (other._position.y == _position.y && Mathf.Abs(other._position.x - _position.x) == 1)
            {
                return true;
            }

            return false;
        }

        public void OnDrag(PointerEventData eventData)
        {
         
        }
    }
}