using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [Header("UI References")]
    public Button loginButton;
    
    void Start()
    {
        if (loginButton != null)
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
        }
    }
    
    void OnLoginButtonClicked()
    {
        if (AuthManager.Instance != null)
        {
            AuthManager.Instance.LoginWithFuntico();
        }
    }
    
    void OnDestroy()
    {
        if (loginButton != null)
        {
            loginButton.onClick.RemoveListener(OnLoginButtonClicked);
        }
    }
}