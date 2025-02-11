using System.Collections;
using UnityEngine;

public class IceTower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private Transform firePoint; 
    [SerializeField] private GameObject freezeWavePrefab; 

    [Header("Attributes")]
    [SerializeField] private float attackCooldown = 2f; 
    [SerializeField] private float freezeDuration = 2f;
    [SerializeField] private float waveRadius = 5f;

    private float lastAttackTime;

    void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            FireFreezeWave();
            lastAttackTime = Time.time;
        }
    }

    private void FireFreezeWave()
    {
        GameObject wave = Instantiate(freezeWavePrefab, firePoint.position, Quaternion.identity);
        FreezeWave freezeWaveScript = wave.GetComponent<FreezeWave>();

        if (freezeWaveScript != null)
        {
            freezeWaveScript.SetProperties(waveRadius, freezeDuration, enemyLayerMask);
        }
    }
}
