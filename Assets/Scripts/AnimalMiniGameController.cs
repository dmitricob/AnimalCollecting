using System;
using System.Collections.Generic;
using Character;
using GeneralInput;
using UI;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class AnimalMiniGameController: IInitializable, IDisposable
{
    private PlayerCharacter _player;

    private Dictionary<GameObject, IFollower> _animalsFollowingCharacter =
        new Dictionary<GameObject, IFollower>();
    private HashSet<IFollower> _countedFollowers = new HashSet<IFollower>();
        
    // And also we need to implement some kind of factory pattern to create entities
    // and pool pattern to reuse entities
    private readonly IInputSystem _inputSystem;
    private readonly HudScoreCount _hudScoreCount;
    private readonly MiniGameSettings _miniGameSettings;

    public AnimalMiniGameController(
        IInputSystem inputSystem,
        HudScoreCount hudScoreCount, 
        MiniGameSettings miniGameSettings)
    {
        _inputSystem = inputSystem;
        _hudScoreCount = hudScoreCount;
        _miniGameSettings = miniGameSettings;
        _hudScoreCount.SetScore(0);

        _player = Object.Instantiate(miniGameSettings.playerCharacterPrefab);
        _player.Initialize(_inputSystem);
            
        for (int i = 0; i < miniGameSettings.animalCount; i++)
        {
            var animalFollowingCharacter = Object.Instantiate(
                miniGameSettings.animalFollowingCharacterPrefab,
                miniGameSettings.animalPatrolZone.GetRandomPoint(), 
                Quaternion.identity);
                
            animalFollowingCharacter.StartPatrolZone(miniGameSettings.animalPatrolZone);
            _animalsFollowingCharacter.Add(animalFollowingCharacter.gameObject,animalFollowingCharacter);
        }
    }

    public void Initialize()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _miniGameSettings.triggerEnterFiltered.Entered += OnAnimalEntered;
    }
        
    private void Unsubscribe()
    {
        _miniGameSettings.triggerEnterFiltered.Entered -= OnAnimalEntered;
    }
        
    private void OnAnimalEntered(GameObject obj)
    {
        if(_animalsFollowingCharacter.TryGetValue(obj, out var follower) == false || _countedFollowers.Contains(follower))
            return;
            
        _countedFollowers.Add(follower);
        _hudScoreCount.SetScore(_countedFollowers.Count);
    }
    public void Dispose()
    {
        Unsubscribe();
    }
}
    
[Serializable]
public struct MiniGameSettings
{
    public PatrolZone.PatrolZone animalPatrolZone;
    public TriggerEnterFiltered triggerEnterFiltered;
    public int animalCount;
    public PlayerCharacter playerCharacterPrefab;
    public AnimalFollowingCharacter animalFollowingCharacterPrefab;
        
}