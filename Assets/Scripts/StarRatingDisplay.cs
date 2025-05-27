using UnityEngine;
using UnityEngine.UI;

public class StarRatingDisplay : MonoBehaviour
{
    public Image[] starImages; // Assign Star1–Star5 in the Inspector
    public Sprite emptyStar;
    public Sprite halfStar;
    public Sprite fullStar;

    private float rating = 0f; // From 0.0 to 5.0

    public void IncreaseRating(float amount)
    {
        rating += amount;
        rating = Mathf.Clamp(rating, 0f, 5f);
        UpdateStars();
    }

    private void UpdateStars()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            if (rating >= i + 1)
                starImages[i].sprite = fullStar;
            else if (rating >= i + 0.5f)
                starImages[i].sprite = halfStar;
            else
                starImages[i].sprite = emptyStar;
        }
    }

    // Optional: reset for new day or level
    public void ResetRating()
    {
        rating = 0f;
        UpdateStars();
    }
}
