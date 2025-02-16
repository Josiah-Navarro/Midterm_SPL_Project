using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BannerNavigation : MonoBehaviour
{
    [Header("Banner Settings")]
    public Image bannerDisplay;  // Reference to the UI Image
    public Sprite[] banners;     // Array to hold banner images
    public Button leftButton, rightButton;
    public float slideSpeed = 5f;  // Speed of animation

    private int currentIndex = 0;
    private RectTransform bannerRect;
    private Vector2 offscreenLeft, offscreenRight, centerPosition;
    private bool isSliding = false;

    void Start()
    {
        if (banners.Length > 0)
        {
            bannerRect = bannerDisplay.rectTransform;

            // Define positions
            centerPosition = bannerRect.anchoredPosition;
            offscreenLeft = centerPosition + new Vector2(-Screen.width, 0);
            offscreenRight = centerPosition + new Vector2(Screen.width, 0);

            UpdateBannerInstant(); // Set the first banner without animation
        }

        // Add click events
        leftButton.onClick.AddListener(() => SlideBanner(-1));
        rightButton.onClick.AddListener(() => SlideBanner(1));
    }

    public void SlideBanner(int direction)
    {
        if (isSliding) return;  // Prevent multiple slides at once

        int newIndex = (currentIndex + direction + banners.Length) % banners.Length; // Loop around
        Sprite newSprite = banners[newIndex];

        StartCoroutine(SlideTransition(newSprite, direction));
        currentIndex = newIndex;
    }

    IEnumerator SlideTransition(Sprite newSprite, int direction)
    {
        isSliding = true;
        Vector2 start = bannerRect.anchoredPosition;
        Vector2 target = (direction > 0) ? offscreenLeft : offscreenRight; 

        // Slide current banner out
        while (Vector2.Distance(bannerRect.anchoredPosition, target) > 1f)
        {
            bannerRect.anchoredPosition = Vector2.Lerp(bannerRect.anchoredPosition, target, slideSpeed * Time.deltaTime);
            yield return null;
        }

        // Change sprite and reset position to offscreen before sliding in
        bannerDisplay.sprite = newSprite;
        bannerRect.anchoredPosition = (direction > 0) ? offscreenRight : offscreenLeft;

        // Slide new banner in
        while (Vector2.Distance(bannerRect.anchoredPosition, centerPosition) > 1f)
        {
            bannerRect.anchoredPosition = Vector2.Lerp(bannerRect.anchoredPosition, centerPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        bannerRect.anchoredPosition = centerPosition; // Ensure final position is exact
        isSliding = false;
    }

    void UpdateBannerInstant()
    {
        bannerDisplay.sprite = banners[currentIndex];  
        bannerRect.anchoredPosition = centerPosition; // No animation for first display
    }
}
