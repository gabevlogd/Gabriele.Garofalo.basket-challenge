using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BasketChallenge.Gameplay
{
    public class MatchEnd : MatchState
    {
        private float _endDuration;
        private bool _matchEnded;
        
        protected override void OnEnter()
        {
            base.OnEnter();
            _endDuration = MatchManager.matchData.endDuration;
            
            // Trigger match end events immediately, but delay the actual end of the match to allow for any score from in
            // air balls to be registered and processed before the match officially ends
            _matchEnded = false;
            MatchManager.MatchTimeExpired();
            MatchManager.StartCoroutine(MatchEndCallDelay());
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            if (!_matchEnded) return;
            
            if (_endDuration > 0)
            {
                _endDuration -= Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        private IEnumerator MatchEndCallDelay()
        {
            yield return new WaitForSeconds(3f);
            _matchEnded = true;
            MatchManager.EndMatch();
        }
    }
}