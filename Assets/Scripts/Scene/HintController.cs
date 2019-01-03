using UnityEngine;

namespace Match3.Scene
{
    public class HintController : MonoBehaviour
    {
        private GameManager _gameManager;

        private bool countTime = false;
        private float time = 0;
        
        void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.Events.OnStateChange.AddListener(state =>
            {
                countTime = state == GameState.WaitForInput;
                time = 0.0f;
            });
        }


        void Update()
        {
            if (countTime)
            {
                time += Time.deltaTime;
                
            }
        }
        
    }
}