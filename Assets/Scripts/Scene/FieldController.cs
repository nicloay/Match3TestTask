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

        private DiceController _dice;
        public DiceController Dice
        {
            get { return _dice;}
            private set
            {
                if (value != _dice)
                {
                    _dice = value;
                    _dice.transform.SetParent(transform, true);
                }
            }
        }


        private Vector2Int _position;                
        public void Initialize(Vector2Int position)
        {
            _position = position;
        }


        public void SetDice(DiceController dice, float yOffset)
        {
            Dice = dice;
            SetDiceOffset(yOffset);
            dice.gameObject.SetActive(true);
        }
        
        public void SwapDiceWith(FieldController anotherField)
        {
            DiceController dice = Dice;
            Dice = anotherField._dice;
            anotherField.Dice = dice;            
        }
        
        private void SetDice(DiceController dice)
        {            
            Dice = dice;
            dice.transform.SetParent(transform);
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