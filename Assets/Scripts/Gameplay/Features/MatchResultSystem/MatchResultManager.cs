using System;
using System.Collections.Generic;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public interface IMatchParticipant
    {
        public Guid GetParticipantId();
        public string GetParticipantName();
        public int GetParticipantScore();
    }
    
    public struct MatchResult
    {
        public List<IMatchParticipant> Winners;
        public List<IMatchParticipant> Losers;
        public bool IsDraw => Losers.Count == 0 && Winners.Count > 1;
    }
    
    public class MatchResultManager : MonoBehaviour
    {
        private List<IMatchParticipant> _participants = new List<IMatchParticipant>();
        
        public void RegisterParticipant(IMatchParticipant participant)
        {
            if (participant == null)
            {
                Debug.LogError("MatchResultManager: Cannot register a null participant.");
                return;
            }
            
            if (!_participants.Contains(participant))
            {
                _participants.Add(participant);
            }
        }

        public void UnregisterParticipant(IMatchParticipant participant)
        {
            if (participant == null)
            {
                Debug.LogError("MatchResultManager: Cannot unregister a null participant.");
                return;
            }
            
            if (_participants.Contains(participant))
            {
                _participants.Remove(participant);
            }
        }

        public MatchResult GetMatchResult()
        {
            MatchResult result = new MatchResult
            {
                Winners = new List<IMatchParticipant>(),
                Losers = new List<IMatchParticipant>()
            };

            int highestScore = int.MinValue;
            foreach (var participant in _participants)
            {
                int score = participant.GetParticipantScore();
                if (score > highestScore)
                {
                    highestScore = score;
                    result.Losers.AddRange(result.Winners);
                    result.Winners.Clear();
                    result.Winners.Add(participant);
                }
                else if (score == highestScore)
                {
                    result.Winners.Add(participant);
                }
                else
                {
                    result.Losers.Add(participant);
                }
            }

            return result;
        }
    }
}
