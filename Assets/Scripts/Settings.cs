using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [System.Serializable]
    public class GameSettings
    {
        public int labirintSize;
        [Range(0, 3)] public int labirintDifficulty;
    }

    [System.Serializable]
    public class WallSettings
    {
        public GameObject WallGameObject;
    }

    [System.Serializable]
    public class PlayerSettings
    {
        public GameObject PlayerGameObject;
        public float speed;
    }

    [System.Serializable]
    public class EnemySettings
    {
        public GameObject EnemyrGameObject;
        public float speed;
    }

    public static Settings Instance { get; private set; }

    public GameSettings gameSettings;
    public WallSettings wallSettings;
    public PlayerSettings playerSettings;
    public EnemySettings enemySettings;

    private void Awake()
    {
        Instance = this;
    }
}
