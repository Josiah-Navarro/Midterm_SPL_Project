using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    private List<BaseSupport> supportTowers = new List<BaseSupport>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterSupportTower(BaseSupport tower)
    {
        supportTowers.Add(tower);
    }

    public void UnregisterSupportTower(BaseSupport tower)
    {
        supportTowers.Remove(tower);
    }

    public List<BaseSupport> GetSupportTowers()
    {
        return new List<BaseSupport>(supportTowers);
    }
}
