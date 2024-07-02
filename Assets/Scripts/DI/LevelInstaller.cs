using System;
using GeneralInput;
using UI;
using UnityEngine;
using Views;
using Zenject;

namespace DI
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private HudScoreCount _hudScoreCount;

        [SerializeField] private MiniGameSettings _miniGameSettings;
        public override void InstallBindings()
        {
            Container.Bind(typeof(IInputSystem)).To<UnityInput>().AsSingle();
            Container.Bind(typeof(IInitializable),typeof(IDisposable), typeof(AnimalMiniGameController)).To<AnimalMiniGameController>().AsSingle().WithArguments(_miniGameSettings).NonLazy();
            Container.Bind<HudScoreCount>().FromInstance(_hudScoreCount).AsSingle();
            Container.Bind<ViewPool>().AsSingle().WithArguments(Container);
        }
    }
}
