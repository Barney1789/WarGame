using UnityEngine;
using TMPro;

public class WalletManager : MonoBehaviour
{
    public TextMeshProUGUI walletText; // Assign your TextMeshProUGUI in the inspector

    private int playerCoins;

    void Start()
    {
        // Initialize player coins from PlayerPrefs or set default value
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 1000);
        UpdateWalletDisplay();
    }

    void UpdateWalletDisplay()
    {
        // Update the wallet display text
        walletText.text = "Coins: " + playerCoins.ToString();
    }

    public void AddCoins(int amount)
    {
        // Add coins and update PlayerPrefs
        playerCoins += amount;
        PlayerPrefs.SetInt("PlayerCoins", playerCoins);
        UpdateWalletDisplay();
    }

    public void SpendCoins(int amount)
    {
        // Spend coins and update PlayerPrefs if the player has enough coins
        if (playerCoins >= amount)
        {
            playerCoins -= amount;
            PlayerPrefs.SetInt("PlayerCoins", playerCoins);
            UpdateWalletDisplay();
        }
        else
        {
            Debug.Log("Not enough coins.");
        }
    }
}
