using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class BallMeshRotator : MonoBehaviour
    {
        private bool _rotationEnabled;
        
        private void Update()
        { 
            if (!_rotationEnabled) return;
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(0f, 0f, -400 * Time.deltaTime));
        }
        
        public void EnableRotation()
        {
            _rotationEnabled = true;
        }
        
        public void DisableRotation()
        {
            _rotationEnabled = false;
        }
    }
}
