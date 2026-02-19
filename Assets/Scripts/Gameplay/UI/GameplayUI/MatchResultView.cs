using BasketChallenge.Core;
using TMPro;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class MatchResultView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI resultText;

        [SerializeField]
        private TextMeshProUGUI personalScore;
        
        [SerializeField]
        private TextMeshProUGUI opponentScore;
        
        private void Awake()
        {
            if (resultText == null)
            {
                Debug.LogError("MatchResultView: resultText reference is not set.");
            }
            if (personalScore == null)
            {
                Debug.LogError("MatchResultView: personalScore reference is not set.");
            }
            if (opponentScore == null)
            {
                Debug.LogError("MatchResultView: opponentScore reference is not set.");
            }
        }
        
        public void UpdateMatchResult()
        {
            if (!resultText || !personalScore  || !opponentScore) return;

            if (!CoreUtility.TryGetGameMode(out GameplayGameMode gameplayGameMode)) return;
            if (!CoreUtility.TryGetPlayerControlledObject(out PlayerCharacter player)) return;
            
            MatchResult matchResult = gameplayGameMode.MatchResultManager.GetMatchResult();
            
            if (ReferenceEquals(matchResult.Winners[0], player))
            {
                resultText.text = "You Win!";
                personalScore.text = matchResult.Winners[0].GetParticipantScore().ToString();
                if (matchResult.Losers.Count > 0)
                    opponentScore.text = matchResult.Losers[0].GetParticipantScore().ToString();
            }
            else
            {
                resultText.text = "You Lose!";
                personalScore.text = matchResult.Losers[0].GetParticipantScore().ToString();
                if (matchResult.Winners.Count > 0)
                    opponentScore.text = matchResult.Winners[0].GetParticipantScore().ToString();
            }
        }
    }
}
