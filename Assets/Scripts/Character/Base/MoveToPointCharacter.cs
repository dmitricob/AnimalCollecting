using Unity.Collections;
using UnityEngine;

namespace Character.Base
{
    // Describe partial case when character can move to the point
    public abstract class MoveToPointCharacter : Character
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        [SerializeField] private float _stopMoveDelta = float.Epsilon;

        
        private Vector2 _targetPoint;
        protected Vector2 MovePosition;

        private float _speedDeltaTime;
        private float _speedDeltaTimSq;
        private float _stopMoveDeltaDeltaTime;


        protected virtual void Awake()
        {
            _speedDeltaTime = _speed * Time.fixedDeltaTime;
            _speedDeltaTimSq = _speedDeltaTime * _speedDeltaTime;
            _stopMoveDeltaDeltaTime = _stopMoveDelta * _stopMoveDelta;

        }

        public void SetTargetPoint(Vector3 targetPoint)
        {
            _targetPoint = targetPoint;
        }
        
        protected virtual void UpdateDirection()
        {
            var direction = _targetPoint - _rigidbody2D.position;
            var distance = direction.sqrMagnitude;
            
            if(distance < _stopMoveDeltaDeltaTime)
            {
                Arrived();
            }else 
            if(distance >= _speedDeltaTimSq)
            {
                direction = direction.normalized * _speedDeltaTime;
            }
            
            MovePosition = _rigidbody2D.position + direction;
        }
        
        protected override void Move()
        {
            _rigidbody2D.MovePosition(MovePosition);
        }

        protected override void FixedUpdate()
        {
            UpdateDirection();

            base.FixedUpdate();
        }

        protected virtual void Arrived()
        {
            StopMove();
        }
        
        public virtual void StopMove()
        {
            MovePosition = _targetPoint = _rigidbody2D.position;
        }
    }
}