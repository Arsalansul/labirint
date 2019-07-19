using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [System.Serializable]
    public class GameSettings
    {
        public int labirintSize;
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

    public static Settings Instance { get; private set; }

    public GameSettings gameSettings;
    public WallSettings wallSettings;
    public PlayerSettings playerSettings;

    private void Awake()
    {
        Instance = this;
    }
}
