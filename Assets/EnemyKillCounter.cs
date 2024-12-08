using System.Collections;
using UnityEngine;
using TMPro;            // For TextMeshPro UI
using UnityEngine.UI;  // For GameObject and UI elements

public class EnemyKillCounter : MonoBehaviour
{
    [Header("Kill Count Settings")]
    public int killCount = 0;           // Current kill count
    public int requiredCount = 10;     // Number of kills required to activate the portal

    [Header("UI Elements")]
    [SerializeField] private TMP_Text killCountText; // Text to display the kill count
    [SerializeField] private GameObject nextText;    // UI element displayed when required kills are met
    [SerializeField] private GameObject portal;      // Portal that appears after required kills

    void Start()
    {
        if (nextText != null) 
            nextText.SetActive(false); // Ensure the nextText is hidden on start

        if (portal != null) 
            portal.SetActive(false);  // Ensure the portal is hidden on start

        UpdateKillCountUI(); // Initialize the UI to show starting kill count
    }

    // This method should be called when an enemy is killed
    public void OnEnemyKilled()
    {
        killCount++;
        UpdateKillCountUI();
        Debug.Log("Enemy Killed! Total Kills: " + killCount);

        // Check if the required kill count has been met
        if (killCount >= requiredCount)
        {
            ActivateNextLevelElements();
        }
    }

    // Update the UI text to display the new kill count
    private void UpdateKillCountUI()
    {
        if (killCountText != null)
        {
            killCountText.text = "Enemies Killed: " + killCount + " / " + requiredCount;
        }
    }

    // Activates the next level UI and portal when the required kill count is met
    private void ActivateNextLevelElements()
    {
        if (nextText != null) 
            nextText.SetActive(true);

        if (portal != null) 
            portal.SetActive(true);
    }
}
