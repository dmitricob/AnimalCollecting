using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DI;
using GeneralInput;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Views;
using Zenject;
using Random = UnityEngine.Random;

public class AnimalMiniGameController: IInitializable, IDisposable
{
    private PlayerCharacter _player;

    private Dictionary<GameObject, IFollower> _animalsFollowingCharacter = new Dictionary<GameObject, IFollower>();
    private HashSet<IFollower> _countedFollowers = new HashSet<IFollower>();

    private readonly IInputSystem _inputSystem;
    private readonly HudScoreCount _hudScoreCount;
    private readonly MiniGameSettings _miniGameSettings;
    private readonly ViewPool _viewPool;
    private readonly CoroutineHolder _coroutineHolder;

    public AnimalMiniGameController(
        HudScoreCount hudScoreCount, 
        MiniGameSettings miniGameSettings, 
        ViewPool viewPool,
        CoroutineHolder coroutineHolder)
    {
        _hudScoreCount = hudScoreCount;
        _miniGameSettings = miniGameSettings;
        _viewPool = viewPool;
        _coroutineHolder = coroutineHolder;
        _hudScoreCount.SetScore(0);

        _player = _viewPool.Pop(miniGameSettings.playerCharacterPrefab.gameObject, Vector3.zero, Quaternion.identity, null).GetComponent<PlayerCharacter>();
            
        for (int i = 0; i < miniGameSettings.startAnimalCount; i++)
        {
            SpawnAnimal();
        }
        _coroutineHolder.StartCoroutine(StartSpawner());
    }
    
    private void SpawnAnimal()
    {
        var animalFollowingCharacter = _viewPool.Pop(
            _miniGameSettings.animalFollowingCharacterPrefab.gameObject,
            _miniGameSettings.animalPatrolZone.GetRandomPoint(), 
            Quaternion.identity,
            null).GetComponent<AnimalFollowingCharacter>();
                
        animalFollowingCharacter.StartPatrolZone(_miniGameSettings.animalPatrolZone);
        _animalsFollowingCharacter.Add(animalFollowingCharacter.gameObject,animalFollowingCharacter);
    }

    private IEnumerator StartSpawner()
    {
        for (int i = 0; i < Random.Range(_miniGameSettings.minAdditionAnimalCount,_miniGameSettings.maxAdditionAnimalCount); i++)
        {
            yield return new WaitForSeconds(Random.Range(_miniGameSettings.minSpawnTime, _miniGameSettings.maxSpawnTime));
            SpawnAnimal(); 
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
    public int startAnimalCount;
    public float minSpawnTime;
    public float maxSpawnTime;
    public int minAdditionAnimalCount;
    public int maxAdditionAnimalCount;
    public PlayerCharacter playerCharacterPrefab;
    public AnimalFollowingCharacter animalFollowingCharacterPrefab;
        
}