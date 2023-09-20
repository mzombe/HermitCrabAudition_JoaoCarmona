using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SceneTestInstaller", menuName = "Installers/SceneTestInstaller")]
public class SceneTestInstaller : ScriptableObjectInstaller<SceneTestInstaller>
{
    public PlayerSettings Player;
    public GameSettings Game;
    [Serializable]
    public class PlayerSettings{
        public Player.Settings P_Settings;
    }
    [Serializable]
    public class GameSettings{
        public GameManager.Settings G_Settings;
    }

    public override void InstallBindings()
    {        
        Container.BindInstance<Player.Settings>(Player.P_Settings).AsSingle();
        Container.BindInstance<GameManager.Settings>(Game.G_Settings).AsSingle();
    }
}