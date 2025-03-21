using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class SummonManager : MonoBehaviour
{
    public Transform resultPanel; // Where the pulled towers will be displayed
    public GameObject ActiveBannerDisplay;
    private BannerData selectedBanner;
    private int summonCount;

    public void SetSelectedBanner(BannerData _banner)
    {
        selectedBanner = _banner;
        if (ActiveBannerDisplay != null && selectedBanner.bannerImage != null)
        {
            // Get the Image component directly from the Panel
            ActiveBannerDisplay.GetComponent<Image>().sprite = selectedBanner.bannerImage;
            ActiveBannerDisplay.SetActive(true);
        }
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
        if (LevelManager.main.money < 200)
        {
            return;
        }

        StartCoroutine(SummonAnimation());
    }

    private IEnumerator SummonAnimation()
    {
        // TODO: Play Light Tunnel Animation Here
        yield return new WaitForSeconds(2f); // Simulating animation delay

        PerformSummon();
        LevelManager.main.SpendCurrency(summonCount * 200);
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
        ActiveBannerDisplay.SetActive(false);

        // Clear previous results
        foreach (Transform child in resultPanel)
        {
            Destroy(child.gameObject);
        }

        // Display new results
        foreach (TowerData tower in pulledTowers)
        {
            GameObject textObj = new GameObject("SummonResultText", typeof(TextMeshProUGUI));
            textObj.transform.SetParent(resultPanel, false);

            TextMeshProUGUI textComponent = textObj.GetComponent<TextMeshProUGUI>();
            textComponent.text = tower.towerName;
            textComponent.fontSize = 14;
            textComponent.alignment = TextAlignmentOptions.Center;
        }

        StartCoroutine(WaitForInputThenClear());
    }

    private IEnumerator WaitForInputThenClear()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.anyKeyDown || Input.GetMouseButtonDown(0));

        foreach (Transform child in resultPanel)
        {
            Destroy(child.gameObject);
        }

        ActiveBannerDisplay.SetActive(true);
    }

}
