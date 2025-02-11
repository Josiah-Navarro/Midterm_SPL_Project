using System;
using UnityEngine;

[Serializable]
public class EnemyManager
{
    public string name;
    public int spawnCount;
    public GameObject prefab;

    public EnemyManager(string _name, int _spawnCount, GameObject _prefab)
    {
        name = _name;
        spawnCount = _spawnCount;
        prefab = _prefab;
    }
}
