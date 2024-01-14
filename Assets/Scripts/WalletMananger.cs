using UnityEngine;
using TMPro;

public class WalletManager : MonoBehaviour
{
    public static WalletManager Instance { get; private set; }
    public TextMeshProUGUI walletText; // Assign your TextMeshProUGUI in the inspector
    public int playerCoins;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this) {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // Initialize player coins from PlayerPrefs or set default value
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 1000);
        UpdateWalletDisplay();
    }

    void Update()
    {
        // Check if the 'M' key was pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Add 500 coins to the wallet
            AddCoins(500);
        }
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

    public bool SpendCoins(int amount)
    {
        // Spend coins and update PlayerPrefs if the player has enough coins
        if (playerCoins >= amount)
        {
            playerCoins -= amount;
            PlayerPrefs.SetInt("PlayerCoins", playerCoins);
            UpdateWalletDisplay();
            return true;
        }
        else
        {
            Debug.Log("Not enough coins.");
            return false;
        }
    }
}
