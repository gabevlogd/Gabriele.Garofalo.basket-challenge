
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BasketChallenge.Gameplay
{
    public class MatchEnd : MatchState
    {
        private float _endDuration;
        
        protected override void OnEnter()
        {
            base.OnEnter();
            _endDuration = MatchManager.matchData.endDuration;
            MatchManager.EndMatch();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (_endDuration > 0)
            {
                _endDuration -= Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}