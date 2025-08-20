using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Exception = System.Exception;

public class FunticoSDKExample : MonoBehaviour
{
    [SerializeField] private string authClientId;
    [SerializeField] private string env;
    [SerializeField] private string secondScene;
    [SerializeField] private Text userNameText;
    [SerializeField] private Text userIDText;
    [SerializeField] private InputField scoreInput;
    [CanBeNull] private static FunticoManager.FunticoUser userName = null;
    private void Start()
    {
        if (userName == null)
        {
            FunticoManager.Instance.Init(authClientId, env);
            ChangeSceneIfSingedIn().Forget();
            return;
        }

        userNameText.text = userName.UserName;
        userIDText.text = userName.UserId;
    }

    #region  SIGN_IN
    public void SingIn()
    {
        SignInAsync().Forget();
    }

    public async UniTask SignInAsync()
    {
        try
        {
            Debug.LogError("SignIn called.");
            await FunticoManager.Instance.SignInAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Couldnt signIn: {ex.Message}");
        }
    }


    private async UniTask ChangeSceneIfSingedIn()
    {
        try
        {
            Debug.LogError("GetUserInfoAsync called.");
            userName = await FunticoManager.Instance.GetUserInfoAsync();
            SceneManager.LoadScene(secondScene);
        }
        catch (Exception)
        {
            Debug.Log("User is not logged in");
        }

    }

    #endregion

    #region SEND_SCORE

    public void SendScore()
    {
        Debug.LogError("SaveScore called.");
        if (int.TryParse(scoreInput.text, out int score))
        {
            SendScoreAsync(score).Forget();
        }
    }
    private async UniTask SendScoreAsync(int score)
    {
        await FunticoManager.Instance.SaveScoreAsync(score);
        FunticoManager.ShowAlert("Score saved successfully!");
    }
    #endregion

    #region SIGN_OUT

    public void SignOut()
    {
        Debug.LogError("SignOut called.");
        FunticoManager.Instance.DoSignOut();
    }

    #endregion
}
