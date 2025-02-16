using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BannerHoverInfo : MonoBehaviour
{
    [Header("References")]
    public BannerData bannerData;  
    public GameObject infoPanel;   
    public TextMeshProUGUI bannerNameText;
    public Transform rewardGrid;

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = infoPanel.GetComponent<CanvasGroup>();
        HideInfoInstant();
    }

    public void ShowInfo()
    {
        StopAllCoroutines();
        bannerNameText.text = bannerData.bannerName;
        PopulateRewards();
        StartCoroutine(FadeIn());
    }

    public void HideInfo()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private void PopulateRewards()
    {
        Debug.Log($"Populating rewards for banner: {bannerData.bannerName}");

        if (bannerData.allTowers == null || bannerData.allTowers.Length == 0)
        {
            Debug.LogWarning("No towers found in bannerData!");
            return;
        }

        int index = 0;

        foreach (TowerData tower in bannerData.allTowers)
        {
            Debug.Log($"Processing tower: {tower.towerName} | Rarity: {tower.rarity}");

            Transform rewardSlot;

            // Reuse existing slots if available
            if (index < rewardGrid.childCount)
            {
                rewardSlot = rewardGrid.GetChild(index);
                rewardSlot.gameObject.SetActive(true);
            }
            else    
            {
                rewardSlot = new GameObject("RewardSlot", typeof(RectTransform)).transform;
                rewardSlot.SetParent(rewardGrid, false);
            }

            // Assign Tower Name Text
            TextMeshProUGUI towerText = rewardSlot.GetComponent<TextMeshProUGUI>();
            if (!towerText) towerText = rewardSlot.gameObject.AddComponent<TextMeshProUGUI>();

            towerText.text = tower.towerName;
            towerText.fontSize = 10;

            index++;
        }

        // Hide extra slots if the number of towers is less than existing UI elements
        for (int i = index; i < rewardGrid.childCount; i++)
        {
            rewardGrid.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log("Finished populating rewards.");
    }   

    private IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0;
        infoPanel.SetActive(true);
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * 4;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * 4;
            yield return null;
        }
        infoPanel.SetActive(false);
    }

    private void HideInfoInstant()
    {
        canvasGroup.alpha = 0;
        infoPanel.SetActive(false);
    }
}
