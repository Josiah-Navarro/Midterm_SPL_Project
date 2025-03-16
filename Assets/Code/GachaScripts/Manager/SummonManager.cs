using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SummonManager : MonoBehaviour
{
    public Transform resultPanel; // Where the pulled towers will be displayed
    private BannerData selectedBanner;
    private int summonCount;

    public void SetSelectedBanner(BannerData _banner)
    {
        selectedBanner = _banner;
    }

    public void SetPullAmount(int _amount)
    {
        summonCount = _amount;
    }

    public void Summon()
    {
        if (selectedBanner == null || summonCount <= 0)
        {
            Debug.LogWarning("Summon failed: No banner selected or invalid pull amount!");
            return;
        }

        StartCoroutine(SummonAnimation());
    }

    private IEnumerator SummonAnimation()
    {
        // TODO: Play Light Tunnel Animation Here
        yield return new WaitForSeconds(2f); // Simulating animation delay

        PerformSummon();
    }

    private void PerformSummon()
    {
        List<TowerData> pulledTowers = new List<TowerData>();

        for (int i = 0; i < summonCount; i++)
        {
            TowerData pulledTower = selectedBanner.GetRandomTower();
            pulledTowers.Add(pulledTower);
        }
        
        TowerInventory.Instance.AddSummonedTowers(pulledTowers);
        DisplaySummonResults(pulledTowers);
    }

    private void DisplaySummonResults(List<TowerData> pulledTowers)
    {
        foreach (Transform child in resultPanel)
        {
            Destroy(child.gameObject); // Clear previous results
        }

        foreach (TowerData tower in pulledTowers)
        {
            GameObject textObj = new GameObject("SummonResultText", typeof(TextMeshProUGUI));
            textObj.transform.SetParent(resultPanel, false);

            TextMeshProUGUI textComponent = textObj.GetComponent<TextMeshProUGUI>();
            textComponent.text = tower.towerName;
            textComponent.fontSize = 14;
            textComponent.alignment = TextAlignmentOptions.Center;
        }
    }
}
