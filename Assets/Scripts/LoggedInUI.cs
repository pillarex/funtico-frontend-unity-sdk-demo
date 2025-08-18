using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoggedInUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI usernameLabel;
    public TextMeshProUGUI userIDLabel;
    public TMP_InputField scoreInput;
    public Button saveScoreButton;
    public Button signOutButton;
    
    void Start()
    {
        UpdateUserInfo();
        
        if (saveScoreButton != null)
        {
            saveScoreButton.onClick.AddListener(OnSaveScoreClicked);
        }
        
        if (signOutButton != null)
        {
            signOutButton.onClick.AddListener(OnSignOutClicked);
        }
    }
    
    void UpdateUserInfo()
    {
        if (AuthManager.Instance != null)
        {
            if (usernameLabel != null)
            {
                usernameLabel.text = "Username: " + AuthManager.Instance.username;
            }
            
            if (userIDLabel != null)
            {
                userIDLabel.text = "User ID: " + AuthManager.Instance.userID;
            }
        }
    }
    
    void OnSaveScoreClicked()
    {
        if (scoreInput != null && AuthManager.Instance != null)
        {
            if (int.TryParse(scoreInput.text, out int score))
            {
                AuthManager.Instance.SaveUserScore(score);
            }
            else
            {
                Debug.LogWarning("Invalid score input");
            }
        }
    }
    
    void OnSignOutClicked()
    {
        if (AuthManager.Instance != null)
        {
            AuthManager.Instance.SignOutFromFuntico();
        }
    }
    
    void OnDestroy()
    {
        if (saveScoreButton != null)
        {
            saveScoreButton.onClick.RemoveListener(OnSaveScoreClicked);
        }
        
        if (signOutButton != null)
        {
            signOutButton.onClick.RemoveListener(OnSignOutClicked);
        }
    }
}