using System.Collections.Generic;
using Character.Base;
using GeneralInput;
using UnityEngine;

namespace Character
{
    public class PlayerCharacter : MoveToPointCharacter
    {
        [SerializeField] private int _maxAmmountOfFollowers;
        [SerializeField] private TriggerEnterFiltered _startFollowingTrigger;

        private HashSet<IFollower> _followers = new HashSet<IFollower>();
        private Camera _mainCamera;
        private IInputSystem _inputSystem;

        public void Initialize(IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
        }
        
        private void OnEnable()
        {
            _mainCamera = Camera.main;
            _startFollowingTrigger.Entered += OnEnteredFollowZone;
        }
        private void OnDisable()
        {
            _startFollowingTrigger.Entered -= OnEnteredFollowZone;
        }

        
        public void StartFollowTarget(Transform target){}
        
        private void OnEnteredFollowZone(GameObject obj)
        {
            if(_followers.Count >= _maxAmmountOfFollowers 
               || obj.TryGetComponent(out IFollower follower) == false // could be a problem in huge amount of objects
               || _followers.Contains(follower))
                return;
            
            _followers.Add(follower);
            follower.StartFollowTarget(transform);
        }
        
        public void RemoveFollower(IFollower follower)
        {
            _followers.Remove(follower);
            follower.StopFollowTarget();
        }

        private void Update()
        {
            if (_inputSystem.GetMouseButton(0))
            {
                var ray = _mainCamera.ScreenPointToRay(_inputSystem.GetMousePosition()); 
                var hit = Physics2D.Raycast(ray.origin, ray.direction);
                SetTargetPoint(hit.point);
            }
        }
    }
}