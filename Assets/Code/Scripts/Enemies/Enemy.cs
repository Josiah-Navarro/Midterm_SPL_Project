using System;
using UnityEngine;

[Serializable]
public class Enemy
{
    public string name;
    public int spawnCount;
    public GameObject prefab;

    public Enemy(string _name, int _spawnCount, GameObject _prefab)
    {
        name = _name;
        spawnCount = _spawnCount;
        prefab = _prefab;
    }
}
