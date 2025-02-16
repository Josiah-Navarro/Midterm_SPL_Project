using UnityEngine;
using System.Collections.Generic;

public class AlanTuringTower : BaseTower
{   
    [Header("Attributes")]
    [SerializeField] public float waveInterval = 2f;
    [SerializeField] public float waveRadius = 5f;
    [SerializeField] private float waveTimer;
 

      protected override void Update()
    {
        waveTimer += Time.deltaTime;
        if (waveTimer >= waveInterval)
        {
            EmitDecipheringWave();
            waveTimer = 0f;
        }
    }


    void EmitDecipheringWave()
    {
        GameObject wave = Instantiate(towerData.bulletPrefab, firingPoint.position, Quaternion.identity);
        AlanTuringWave turingWaveScript = wave.GetComponent<AlanTuringWave>();
        turingWaveScript.SetProperties(waveRadius, enemyMask);
    }
}
