using Character.Base;
using UnityEngine;

namespace Character
{
    //Now this character have to much logic and for further it is better to Implement state machine pattern
    public class AnimalFollowingCharacter : MoveToPointCharacter, IFollower
    {
        [SerializeField] private Transform _followTarget;
        [SerializeField] private float _startFollowingRange = 5f;
        [SerializeField] private float _keepDistance = 1f;
        [SerializeField] private PatrolZone.PatrolZone _patrolZone;
        public bool IsFollowing { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _startFollowingRange = Mathf.Pow(_startFollowingRange, 2);
            _keepDistance = Mathf.Pow(_keepDistance, 2);
        }

        public void StartPatrolZone(PatrolZone.PatrolZone patrolZone)
        {
            StopFollowTarget();
            _patrolZone = patrolZone;
            SetRandomTargetPoint();
        }

        public void StartFollowTarget(Transform target)
        {
            IsFollowing = true;
            _followTarget = target;
        }

        public void StopFollowTarget()
        {
            IsFollowing = false;
            _followTarget = null;
        }

        protected override void UpdateDirection()
        {
            while (true)
            {
                base.UpdateDirection();
                if (IsFollowing || _patrolZone.IsInZone(MovePosition)) // better use some pathfinding plugin to specify patrol zone but lack of time :C
                    return;

                // if animal outside patrol zone look for another if not follow target
                SetRandomTargetPoint();
            }
        }

        protected override void FixedUpdate()
        {
            if (IsFollowing)
            {
                if(Vector3.SqrMagnitude(_followTarget.position - transform.position) < _keepDistance)
                {
                    StopMove();
                }
                else
                {
                    SetTargetPoint(_followTarget.position);
                }
            }
            
            base.FixedUpdate();
        }
        
        protected override void Arrived()
        {
            if (!IsFollowing)
            {
                SetRandomTargetPoint();
            }
        }
        
        private void SetRandomTargetPoint()
        {
            SetTargetPoint(_patrolZone.GetRandomPoint());
        }
    }

    public interface IFollower
    {
        bool IsFollowing { get; }
        void StartFollowTarget(Transform target);
        void StopFollowTarget();
    }
}