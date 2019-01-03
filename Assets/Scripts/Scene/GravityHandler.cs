using System;
using System.Collections;
using System.Collections.Generic;
using Match3.Utils;
using UnityEngine;

namespace Match3.Scene
{
    
    /// <summary>
    /// This class move all dices to the center of the fields
    /// </summary>
    [Serializable]       
    public class GravityHandler
    {
        public EaseType EaseType = Match3.Utils.EaseType.EaseOutBounce;
        
        [SerializeField] private float _timePerCellSpeed = 0.2f;

        [SerializeField] private float _lagPerRow = 0.1f;

        [SerializeField] private float _lagPerColumn = 0.3f;

        


        public IEnumerator ApplyGravity(FieldController[,] _fields)
        {
            List<GravityItem> items = GatherAllFloatingGems(_fields);
            float cellHeight = (_fields[0, 1].transform.position - _fields[0, 0].transform.position).y;
            SetDurationAndLag(items, cellHeight);
            
            float time = 0.0f;
            List<GravityItem> actionToRemove = new List<GravityItem>();
            while (items.Count > 0)
            {
                actionToRemove.Clear();
                foreach (var gravityItem in items)
                {
                    
                    if (time < gravityItem.Lag)
                    {
                        continue;
                    }

                    float noLagTime = time - gravityItem.Lag;
                    
                    if (noLagTime > gravityItem.Duration)
                    {
                        noLagTime = gravityItem.Duration;
                        actionToRemove.Add(gravityItem);
                    }

                    float yOffset = Equations.ChangeFloat(noLagTime, gravityItem.StartY , -gravityItem.StartY,
                        gravityItem.Duration, EaseType);
					
                    gravityItem.Field.SetDiceOffset(yOffset);
                }


                foreach (var removeAction in actionToRemove)
                {
                    items.Remove(removeAction);
                }
								
                yield return null;
                time += Time.deltaTime;
            }	
                                    
        }


        private void SetDurationAndLag(List<GravityItem> items, float cellHeight)
        {            
            
            Dictionary<int,int> minYByColumn = new Dictionary<int, int>();
            
            items.ForEach(item =>
            {
                if (!minYByColumn.ContainsKey(item.Position.x))
                {
                    minYByColumn.Add(item.Position.x, item.Position.y);
                }
                else
                {
                    minYByColumn[item.Position.x] = Mathf.Min(minYByColumn[item.Position.x], item.Position.y);
                }
            });
            
            items.ForEach(item =>
            {
                
                item.Duration = (item.StartY / cellHeight) * _timePerCellSpeed;
                int yLag = item.Position.y - minYByColumn[item.Position.x];
                item.Lag = yLag * _lagPerRow + item.Position.x * _lagPerColumn;
            });
        }
        
        
        
        

        /// <summary>
        /// pickup all gems which must be applied by gravity force
        /// </summary>
        /// <returns></returns>
        private List<GravityItem> GatherAllFloatingGems(FieldController[,] _fields)
        {
            List<GravityItem> result = new List<GravityItem>();
            for (int rowId = 0; rowId < _fields.GetLength(1); rowId++)
            {
                for (int colId = 0; colId < _fields.GetLength(0); colId++)
                {
                    if (!_fields[colId, rowId].IsDiceGrounded)
                    {
                        result.Add(new GravityItem()
                        {
                            Position    = new Vector2Int(colId, rowId),
                            StartY =  _fields[colId, rowId].Dice.transform.localPosition.y,
                            Field = _fields[colId, rowId]
                        });
                    }
                }                
            }
            return result;
        }
        

        private class GravityItem
        {
            public FieldController Field;
            public Vector2Int Position;
            public float StartY;
            public float Duration;
            public float Lag;
        }
    }
}