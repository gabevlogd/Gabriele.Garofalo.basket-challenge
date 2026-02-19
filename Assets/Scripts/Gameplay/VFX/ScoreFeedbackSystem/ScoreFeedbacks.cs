using System.Collections.Generic;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "ScoreFeedbacks", menuName = "ScriptableObjects/ScoreFeedbacks", order = 1)]
    public class ScoreFeedbacks : ScriptableObject
    {
        public List<ScoreFeedback> scoreFeedbackList;
    }
}
