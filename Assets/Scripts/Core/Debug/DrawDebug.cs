using UnityEngine;

namespace BasketChallenge.Core
{
    public class DrawDebug
    {
        protected Color color;
        private float _duration = 0f;
        public bool IsExpired => _duration <= 0f;

        public void OnDrawGizmos()
        {
            if (IsExpired) return;
            Draw();
            UpdateDuration();
        }

        protected DrawDebug(Color color, float duration)
        {
            this.color = color;
            _duration = duration;
        }

        protected virtual void Draw(){}

        private void UpdateDuration()
        {
            if (_duration > 0f)
            {
                _duration -= Time.deltaTime;
                if (_duration <= 0f)
                {
                    _duration = 0f;
                }
            }
        }
    }

    public class DrawDebugSphere : DrawDebug
    {
        private readonly Vector3 _position;
        private readonly float _radius;

        public DrawDebugSphere(Vector3 position, float radius, Color color, float duration) : base(color, duration)
        {
            _position = position;
            _radius = radius;
        }

        protected override void Draw()
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(_position, _radius);
        }
    }
}