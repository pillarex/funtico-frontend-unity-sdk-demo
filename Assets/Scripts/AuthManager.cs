using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;
    
    public string username = "";
    public string userID = "";
    
    [DllImport("__Internal")]
    private static extern void InitializeFunticoSDK();
    
    [DllImport("__Internal")]
    private static extern void GetUserInfo();
    
    [DllImport("__Internal")]
    private static extern void SignInWithFuntico(string redirectUrl);
    
    [DllImport("__Internal")]
    private static extern void SignOut(string redirectUrl);
    
    [DllImport("__Internal")]
    private static extern void SaveScore(int score);
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        InitializeFunticoSDK();
        CheckAuthStatus();
        #endif
    }
    
    public void CheckAuthStatus()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        GetUserInfo();
        #endif
    }
    
    public void LoginWithFuntico()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        SignInWithFuntico(Application.absoluteURL);
        #else
        Debug.Log("Login with Funtico - WebGL only");
        #endif
    }
    
    public void SignOutFromFuntico()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        SignOut(Application.absoluteURL);
        #else
        Debug.Log("Sign out - WebGL only");
        #endif
    }
    
    public void SaveUserScore(int score)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        SaveScore(score);
        #else
        Debug.Log($"Save score: {score} - WebGL only");
        #endif
    }
    
    // Called from JavaScript when user info is successfully retrieved
    public void OnUserInfoSuccess(string userInfoJson)
    {
        var userInfo = JsonUtility.FromJson<UserInfo>(userInfoJson);
        username = userInfo.username;
        userID = userInfo.user_id.ToString();
        
        SceneManager.LoadScene("LoggedInScene");
    }
    
    // Called from JavaScript when user info retrieval fails
    public void OnUserInfoError(string error)
    {
        // Stay on login scene or load login scene
        if (SceneManager.GetActiveScene().name != "LoginScene")
        {
            SceneManager.LoadScene("LoginScene");
        }
    }
    
    // Called from JavaScript when score is saved successfully
    public void OnScoreSaved(string result)
    {
        Debug.Log("Score saved successfully!");
        // You could show a toast notification here
    }
    
    [System.Serializable]
    public class UserInfo
    {
        public string username;
        public int user_id;
    }
}