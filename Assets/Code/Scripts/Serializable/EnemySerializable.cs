using System;
using UnityEngine;

[Serializable]
public class EnemySerializable
{
    public string name;
    public int spawnCount;
    public GameObject prefab;

    public EnemySerializable(string _name, int _spawnCount, GameObject _prefab)
    {
        name = _name;
        spawnCount = _spawnCount;
        prefab = _prefab;
    }
}
