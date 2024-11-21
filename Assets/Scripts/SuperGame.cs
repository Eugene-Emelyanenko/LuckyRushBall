using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuperGame : MonoBehaviour
{
    [SerializeField] private GameObject superGamePanel;

    [SerializeField] private Transform gravityButtonContainer;
    [SerializeField] private Transform speedButtonContainer;
    [SerializeField] private Transform ballButtonContainer;

    [SerializeField] private Color selectedColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color deselectedColor = new Color(0f, 0f, 0f, 0f);

    private int selectedGravityIndex;
    private int selectedSpeedIndex;
    private int selectedBallIndex;

    private void Start()
    {
        EnableMenu(false);
    }

    public void EnableMenu(bool isOpen)
    {
        superGamePanel.SetActive(isOpen);
        LoadSettings();
        UpdateButtonColors();
    }

    private void SetGravity(int index)
    {
        selectedGravityIndex = index;
        PlayerPrefs.SetInt("SelectedGravity", selectedGravityIndex);
        PlayerPrefs.Save();
        UpdateButtonColors();
    }

    private void SetSpeed(int index)
    {
        selectedSpeedIndex = index;
        PlayerPrefs.SetInt("SelectedSpeed", selectedSpeedIndex);
        PlayerPrefs.Save();
        UpdateButtonColors();
    }

    private void SetBall(int index)
    {
        selectedBallIndex = index;
        PlayerPrefs.SetInt("SelectedBall", selectedBallIndex);
        PlayerPrefs.Save();
        UpdateButtonColors();
    }

    private void LoadSettings()
    {
        selectedGravityIndex = PlayerPrefs.GetInt("SelectedGravity", 1);
        selectedSpeedIndex = PlayerPrefs.GetInt("SelectedSpeed", 1);
        selectedBallIndex = PlayerPrefs.GetInt("SelectedBall", 1);
    }

    private void UpdateButtonColors()
    {
        UpdateButtonContainerColors(gravityButtonContainer, selectedGravityIndex);
        UpdateButtonContainerColors(speedButtonContainer, selectedSpeedIndex);
        UpdateButtonContainerColors(ballButtonContainer, selectedBallIndex);
    }

    private void UpdateButtonContainerColors(Transform container, int selectedIndex)
    {
        for (int i = 0; i < container.childCount; i++)
        {
            Image buttonImage = container.GetChild(i).GetComponent<Image>();
            if (i == selectedIndex - 1)
            {
                buttonImage.color = selectedColor;
            }
            else
            {
                buttonImage.color = deselectedColor;
            }
        }
    }

    public void OnGravityButtonClicked(int index)
    {
        SetGravity(index);
    }

    public void OnSpeedButtonClicked(int index)
    {
        SetSpeed(index);
    }

    public void OnBallButtonClicked(int index)
    {
        SetBall(index);
    }

    public void StartSuperGame()
    {
        PlayerPrefs.SetInt("SelectedLevel", 0);
        SceneManager.LoadScene("Game");
    }
}
