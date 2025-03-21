using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "NewBanner", menuName = "Gacha/Banner")]
public class BannerData : ScriptableObject
{
    public string bannerName;
    public Sprite bannerImage;
    public TowerData[] allTowers; // Store all towers, filtering by rarity at runtime

    public TowerData GetRandomTower()
    {
        float roll = Random.Range(0f, 100f);

        // Filter towers by rarity
        List<TowerData> commonTowers = allTowers.Where(t => t.rarity == "C").ToList();
        List<TowerData> rareTowers = allTowers.Where(t => t.rarity == "R").ToList();
        List<TowerData> ultraTowers = allTowers.Where(t => t.rarity == "U").ToList();
        List<TowerData> superTowers = allTowers.Where(t => t.rarity == "S").ToList();

        if (roll <= 1f && superTowers.Count > 0)
            return superTowers[Random.Range(0, superTowers.Count)];  // S-rank (1%)
        else if (roll <= 10f && ultraTowers.Count > 0)
            return ultraTowers[Random.Range(0, ultraTowers.Count)];  // U-rank (9%)
        else if (roll <= 40f && rareTowers.Count > 0)
            return rareTowers[Random.Range(0, rareTowers.Count)];  // R-rank (30%)
        else
            return commonTowers[Random.Range(0, commonTowers.Count)];  // C-rank (60%)
    }
}

public enum Rarity
{
    C,
    R,
    U,
    S
}