using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;

public class FunticoManager : MonoBehaviour
{
    public static FunticoManager Instance { get; private set; }

    [DllImport("__Internal")] private static extern void InitializeSDK(string authClientId, string env);
    [DllImport("__Internal")] private static extern void SignIn(string callbackUrl, string gameObjectName, int promiseId);
    [DllImport("__Internal")] private static extern void GetUserInfo(string gameObjectName, int promiseId);
    [DllImport("__Internal")] private static extern void SaveScore(int score, string gameObjectName, int promiseId);
    [DllImport("__Internal")] private static extern void SignOut(string redirectUrl);

    private int _nextPromiseId = 0;
    private readonly Dictionary<int, object> _pendingPromises = new Dictionary<int, object>();

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

    public void Init(string authClientId, string env)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        InitializeSDK(authClientId, env);
#endif
    }

    public UniTask SignInAsync(string callbackUrl)
    {
#if UNITY_EDITOR
        Debug.LogWarning("Funtico SDK >> Mock SignInAsync call in Editor.");
        return UniTask.CompletedTask;
#else
        var utcs = new UniTaskCompletionSource<bool>();
        int promiseId = _nextPromiseId++;
        _pendingPromises[promiseId] = utcs;
        SignIn(callbackUrl, gameObject.name, promiseId);
        return utcs.Task;
#endif
    }

    public UniTask<string> GetUserInfoAsync()
    {
#if UNITY_EDITOR
        Debug.LogWarning("Funtico SDK >> Mock GetUserInfoAsync call in Editor.");
        return UniTask.FromResult("{\"name\":\"EditorUser\"}");
#else
        var utcs = new UniTaskCompletionSource<string>();
        int promiseId = _nextPromiseId++;
        _pendingPromises[promiseId] = utcs;
        GetUserInfo(gameObject.name, promiseId);
        return utcs.Task;
#endif
    }

    public UniTask<string> SaveScoreAsync(int score)
    {
#if UNITY_EDITOR
        Debug.LogWarning($"Funtico SDK >> Mock SaveScoreAsync({score}) call in Editor.");
        return UniTask.FromResult("{\"status\":\"success_editor\"}");
#else
        var utcs = new UniTaskCompletionSource<string>();
        int promiseId = _nextPromiseId++;
        _pendingPromises[promiseId] = utcs;
        SaveScore(score, gameObject.name, promiseId);
        return utcs.Task;
#endif
    }

    public void DoSignOut(string redirectUrl)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        SignOut(redirectUrl);
#endif
    }

    public void ResolvePromise(string result)
    {
        var parts = result.Split(new[] { ':' }, 2);
        if (parts.Length < 2 || !int.TryParse(parts[0], out int promiseId)) return;
        if (!_pendingPromises.TryGetValue(promiseId, out object utcsObj)) return;

        string payload = parts[1];
        if (utcsObj is UniTaskCompletionSource<string> stringUtcs) {
            stringUtcs.TrySetResult(payload);
        } else if (utcsObj is UniTaskCompletionSource<bool> boolUtcs) {
            boolUtcs.TrySetResult(true);
        }
        _pendingPromises.Remove(promiseId);
    }

    public void RejectPromise(string error)
    {
        var parts = error.Split(new[] { ':' }, 2);
        if (parts.Length < 2 || !int.TryParse(parts[0], out int promiseId)) return;
        if (!_pendingPromises.TryGetValue(promiseId, out object utcsObj)) return;

        string payload = parts[1];
        var exception = new Exception(payload);
        if (utcsObj is UniTaskCompletionSource<string> stringUtcs) {
            stringUtcs.TrySetException(exception);
        } else if (utcsObj is UniTaskCompletionSource<bool> boolUtcs) {
            boolUtcs.TrySetException(exception);
        }
        _pendingPromises.Remove(promiseId);
    }
}
4. MyGameLogic.cs (Example Usage)
This script shows how to call the awaitable methods from the FunticoManager.
C#
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class MyGameLogic : MonoBehaviour
{
    private async UniTaskVoid Start()
    {
        FunticoManager.Instance.Init("your-auth-client-id", "staging");
        await AuthenticateAndPlay();
    }

    private async UniTask AuthenticateAndPlay()
    {
        try
        {
            await FunticoManager.Instance.SignInAsync("https://your-game.com/callback");
            string userInfo = await FunticoManager.Instance.GetUserInfoAsync();
            Debug.Log("Welcome, user: " + userInfo);

            await FunticoManager.Instance.SaveScoreAsync(5000);
            Debug.Log("High score saved!");
        }
        catch (Exception e)
        {
            Debug.LogError("Funtico process failed: " + e.Message);
        }
    }
}
