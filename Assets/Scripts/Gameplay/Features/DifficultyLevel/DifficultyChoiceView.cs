using UnityEngine;
using UnityEngine.UI;

namespace BasketChallenge.Gameplay
{
    public class DifficultyChoiceView : MonoBehaviour
    {
        [SerializeField] private Button buttonEasy; 
        [SerializeField] private Button buttonNormal;
        [SerializeField] private Button buttonHard;
        
        [SerializeField] private AIBehaviourData easyDifficulty;
        [SerializeField] private AIBehaviourData normalDifficulty;
        [SerializeField] private AIBehaviourData hardDifficulty;
        
        private void Awake()
        {
            if (buttonEasy == null || buttonNormal == null || buttonHard == null)
            {
                Debug.LogError("DifficultyChoiceView: One or more button references are missing. Please assign them in the inspector.");
                return;
            }
            
            if (easyDifficulty == null || normalDifficulty == null || hardDifficulty == null)
            {
                Debug.LogError("DifficultyChoiceView: One or more difficulty data references are missing. Please assign them in the inspector.");
                return;
            }
            
            buttonEasy.onClick.AddListener(() => OnDifficultySelected(easyDifficulty));
            buttonNormal.onClick.AddListener(() => OnDifficultySelected(normalDifficulty));
            buttonHard.onClick.AddListener(() => OnDifficultySelected(hardDifficulty));
        }

        private void OnDifficultySelected(AIBehaviourData selectedDifficulty)
        {
            DifficultyManager.SetDifficultyLevel(selectedDifficulty);
        }
    }
}