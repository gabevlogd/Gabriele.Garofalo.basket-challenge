using System;
using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class EndGameTransformsHandler : Singleton<EndGameTransformsHandler>
    {
        public Transform CameraTrs => cameraTrs;
        public Transform PlayerTrs => playerTrs;
        public Transform OpponentTrs => opponentTrs;
        
        [SerializeField] private Transform cameraTrs;
        [SerializeField] private Transform playerTrs;
        [SerializeField] private Transform opponentTrs;

        protected override void Awake()
        {
            base.Awake();
            if (cameraTrs == null || playerTrs == null || opponentTrs == null)
            {
                throw new Exception("Camera, Player or Opponent transforms are not assigned in the inspector.");
            }
        }
    }
}
