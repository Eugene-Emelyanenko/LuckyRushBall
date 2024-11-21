using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private void Start()
    {
        bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }
}
