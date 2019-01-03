using System.Collections;
using Match3.Utils;
using UnityEngine;

namespace Scene
{
    public class DestroyDiceController : MonoBehaviour
    {
        [SerializeField] private EaseConfig _scaleEaseConfig;

        [SerializeField] private float _targetDiceScale;

        [SerializeField] private ParticleSystem _particles;

        [SerializeField] private int _particleNumberPerDestroy = 10;
        
        private DicePool _dicePool;

        
        public void Initialize(DicePool dicePool)
        {
            _dicePool = dicePool;
        }
        
        
        public int DestroyAction { get; private set; }
        public IEnumerator DestroyDice(FieldController field)
        {
            DestroyAction++;
            
            
            //_particles.transform.position = field.transform.position;
            ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams()
            {
                applyShapeToPosition = true,
                position = field.transform.position              
            };            
            _particles.Emit(emitParams, _particleNumberPerDestroy);
            //_particles.Play();
            
            
            float time = 0.0f;
            do
            {
                float relativeValue = Equations.ChangeFloat(time, 0.0f, 1.0f, _scaleEaseConfig.Duration,
                    _scaleEaseConfig.EaseType);
                float scale = 1.0f + relativeValue * (_targetDiceScale - 1.0f); 
                
                field.Dice.transform.localScale = new Vector3(scale, scale, scale);
                field.Dice.Alpha = 1 - relativeValue;
                time += Time.deltaTime;
                yield return null;
            } while (time < _scaleEaseConfig.Duration);
            
            _dicePool.Release(field.Dice);
            field.Dice.transform.localScale = Vector3.one;
            field.Dice.Alpha = 1.0f;
            field.ClearDice();

            DestroyAction--;
        }
    }
}