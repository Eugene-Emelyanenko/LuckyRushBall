using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private Image levelImage;
    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unclockedSprite;
    [SerializeField] private TextMeshProUGUI levelNumberText;
    [SerializeField] private GameObject stars;
    public Button button;

    public void SetUp(LevelData data)
    {
        stars.SetActive(false);

        if (data.isUnlocked)
        {
            levelNumberText.gameObject.SetActive(true);
            button.interactable = true;
            levelImage.sprite = unclockedSprite;
            levelNumberText.text = data.levelNumber.ToString();
            if(data.isCompleted)
            {
                stars.SetActive(true);
            }
        }
        else
        {
            levelNumberText.gameObject.SetActive(false);
            levelImage.sprite = lockedSprite;
            button.interactable = false;
        }
    }
}
