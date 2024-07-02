using System.Collections.Generic;
using Character;
using DefaultNamespace.UI;
using GeneralInput;
using UnityEngine;

namespace DefaultNamespace
{
    public class AnimalMinigameController : MonoBehaviour
    {
        [SerializeField] private int _animalCount = 3;
        [SerializeField] private PlayerCharacter _playerCharacterPrefab;
        [SerializeField] private AnimalFollowingCharacter _animalFollowingCharacterPrefab;
        
        [SerializeField] private PlayerCharacter _player;
        [SerializeField] private AnimalFollowingCharacter _animalFollowingCharacter;
        
        [SerializeField] private TriggerEnterFiltered _triggerEnterFiltered;
        [SerializeField] private HudScoreCount _hudScoreCount;
        [SerializeField] private PatrolZone.PatrolZone _animalPatrolZone;
        
        private UnityInput _unityInput;

        private HashSet<GameObject> _countedFollowers = new HashSet<GameObject>();
        
        // By increasing ammount of dependencies need to implement DI pattern to inject them
        // And also we need to implement some kind of factory pattern to create entities
        // and pool pattern to reuse entities
        public void Start()
        {
            _unityInput = new UnityInput();
            _player = Instantiate(_playerCharacterPrefab);
            _player.Initialize(_unityInput);
            for (int i = 0; i < _animalCount; i++)
            {
                _animalFollowingCharacter = Instantiate(_animalFollowingCharacterPrefab, _animalPatrolZone.GetRandomPoint(), Quaternion.identity);
                _animalFollowingCharacter.StartPatrolZone(_animalPatrolZone);
            }
            
            _hudScoreCount.SetScore(0);
        }

        private void OnEnable()
        {
            _triggerEnterFiltered.Entered += OnAnimalEntered;
        }

        private void OnAnimalEntered(GameObject obj)
        {
            if(_countedFollowers.Contains(obj))
                return;
            
            _countedFollowers.Add(obj);
            _hudScoreCount.SetScore(_countedFollowers.Count);
        }

        private void OnDisable()
        {
            _triggerEnterFiltered.Entered -= OnAnimalEntered;
        }
    }
}