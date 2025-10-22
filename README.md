
# Funtico Frontend Unity SDK Demo

### Prerequisites

* Unity 2022.3+ LTS
* UniTask package

## Importing package thru UPM

#### 1. Start a new Unity project or use your existing one (make sure to create a backup first).
#### 2. Open the Package Manager
Go to Window → Package Manager in the Unity menu.
#### 3. Add the required packages
In the Package Manager window, click the “+” button → choose “Add package from Git URL…”, then add the following URLs one by one:

```HTML
https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
```

```HTML
https://github.com/pillarex/funtico-frontend-unity-sdk-demo.git?path=Assets/FunticoSDK
```

#### 4. (Optional): Import the sample project
After installation, open the Funtico SDK package page in the Package Manager and click “Import Sample” to load the example scene and script


## 🚀 How to Use the Funtico SDK

This guide will walk you through integrating the Funtico SDK into your Unity WebGL game. We'll cover everything from initial setup to signing in users and saving scores, all with practical code examples.

### Getting Started: 

#### 1. Scene Setup

Before you can call any SDK functions, you need to add the FunticoManager to your scene. This is a crucial step, as this component handles all the communication with the Funtico backend.
Create a Manager Object: In your first scene (like a loading or main menu scene), create a new empty GameObject. A good name for it is FunticoManager.
Add the Script: Attach the FunticoManager.cs script to the GameObject you just created.
Make it Persistent: The FunticoManager is a singleton that needs to persist across scene loads. The script handles this for you with DontDestroyOnLoad(gameObject), so you're all set!
That's it for the scene setup. Now you can access the SDK from anywhere in your code using FunticoManager.Instance.

#### 2. Setting up the WebGL Template

#### Option A: Use Our Pre-configured Template (Recommended)
The easiest way is to use the template that comes with the Funtico SDK package, which is already set up for you.

##### 1. To get it:
* use UPM packages sample - import WebGL Template sample from our package
* move FunticoSDK dir from Assets/Samples/Gameloop Funtico UnitySDK/x.x.x/WebGL Template/ to WebGLTemplates
* if it is not appearing in Presentation section you might to need restart Unity

##### 2. How to set That template:
* In the Unity Editor, go to Edit > Project Settings > Player.
* Select the WebGL tab.
* Open the Resolution and Presentation section.
* From the WebGL Template dropdown menu, select the Funtico template.
* Now, when you build your project, Unity will use this template automatically.

#### Option B: Modify Your Own Custom Template

If you are using your own WebGL template, you'll need to make a few manual edits to your index.html file. These changes are necessary to ensure the Funtico JavaScript SDK (which runs in the browser) can find and communicate with your Unity game instance.
Here are the three required steps:

##### 1. Add the Funtico SDK Script
In the <head> section of your index.html, add the following line to load the Funtico JavaScript library:
```HTML
<script src="https://funtico-frontend-js-sdk.pages.dev/funtico-sdk.min.js"></script>
```

##### 2. Create a Global Instance Variable
The Funtico SDK needs a global variable to find your game. In your index.html, locate this line:
```HTML
var script = document.createElement("script");
```
Just before it, add the following line to declare the variable:
```HTML
var myGameInstance = null;
```

##### 3. Assign the Unity Instance
Finally, you need to assign the created Unity game instance to the variable from the previous step. Find the createUnityInstance function call in your file. Inside its .then() block, add myGameInstance = unityInstance;.
It will look like this:
```HTML
createUnityInstance(canvas, config, (progress) => {
  //... progress bar logic
}).then((unityInstance) => {
  myGameInstance = unityInstance; // <-- Add this line
  //... other logic
});
```

### 🎮 SDK Usage

All the core functions of the SDK are asynchronous and use UniTask. Think of UniTask as a modern, high-performance replacement for Coroutines that lets you use the clean async/await syntax.1 This makes your code easier to read and manage, especially when dealing with web requests.1
Here’s how to use the main features.

#### 1. Initialization

First things first, you need to initialize the SDK. This should be done as early as possible when your game starts.

```C#
using FunticoSDK.Runtime.Scripts;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameInitializer : MonoBehaviour
{
    private string authClientId = "YOUR_CLIENT_ID";
    private string env = "production"; // or "development"

    void Start()
    {
        // Initialize the SDK
        FunticoManager.Instance.Init(authClientId, env);
        
        // You can then proceed with other logic, like trying to sign the user in
        CheckUserLoginStatus().Forget();
    }

    private async UniTaskVoid CheckUserLoginStatus()
    {
        //... see next steps
    }
}
```

The Init function sets up the connection to the Funtico backend. You'll get your authClientId from the Funtico developer portal.

#### 2. Signing In a User

To prompt the user to sign in, you'll call SignInAsync. Since this is an asynchronous operation, you'll need to await it.

```C#


public async UniTask SignInUser()
{
    try
    {
        Debug.Log("Attempting to sign in...");
        await FunticoManager.Instance.SignInAsync();
        Debug.Log("Sign-in successful!");
        // Now you can load the main game scene or get user info
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Sign-in failed: {ex.Message}");
    }
}
```

#### 💡 Why async UniTask and try/catch?
Web operations can sometimes fail (no internet, server issues, etc.). Using async/await with a try/catch block is the standard and cleanest way to handle these situations gracefully.

#### 3. Getting User Information

After a user has signed in, you can retrieve their profile information. The GetUserInfoAsync method returns a FunticoUser object, or null if no user is logged in.

```C#
using FunticoSDK.Runtime.Scripts;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI; // For Text elements

public class UserProfile : MonoBehaviour
{
    private Text userNameText;
    private Text userIDText;

    async void Start()
    {
        try
        {
            FunticoManager.FunticoUser user = await FunticoManager.Instance.GetUserInfoAsync();
            if (user!= null)
            {
                Debug.Log($"Welcome back, {user.UserName}!");
                userNameText.text = user.UserName;
                userIDText.text = user.UserId;
            }
            else
            {
                Debug.Log("No user is logged in.");
                // Maybe show a "Sign In" button here
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log($"Could not get user info: {ex.Message}");
        }
    }
}
```

The FunticoUser object has the following properties:
string UserName
string UserId

#### 4. Saving a Score

Saving a player's score is another simple async call.

```C#
public async UniTask SavePlayerScore(int score)
{
    try
    {
        Debug.Log($"Saving score: {score}");
        string response = await FunticoManager.Instance.SaveScoreAsync(score);
        Debug.Log($"Server response: {response}");
        FunticoManager.ShowAlert("Score saved successfully!");
    }
    catch (System.Exception ex)
    {
        Debug.LogError($"Failed to save score: {ex.Message}");
        FunticoManager.ShowAlert("Error saving score.");
    }
}
```

#### 5. Signing Out

To sign the user out, simply call DoSignOut. This is a synchronous call, so no await is needed.

```C#
public void LogOut()
{
    Debug.Log("Signing out...");
    FunticoManager.Instance.DoSignOut();
    // Return to the main menu or login screen
}
```

### 💡 Editor Mocking

When you run your game in the Unity Editor, it's not in a browser, so it can't use the JavaScript library. To make development easier, the SDK provides mock (fake) data for most functions.
For example, calling GetUserInfoAsync in the editor will instantly return a fake user:
{UserName = "Editor", UserId = "123"}.
This allows you to build and test your UI and game logic without needing to create a full WebGL build every time. You'll see Debug.LogWarning messages in the console to remind you that you're using mock data.

### 📦 Building and Running Your WebGL Game


#### 1. Building the Project

To create a WebGL build of your game:
Go to File → Build Settings.
Select WebGL and click "Switch Platform" if it's not already active.
Click "Build". Unity will ask you to choose a folder to save the build files.

#### 2. Running a Local Server

For security reasons, browsers don't allow WebGL builds to run directly from the local file system (file://...). You need to serve the files from a local web server.
Here are a few simple ways to start a local server. Open your terminal or command prompt, navigate into your build folder, and run one of the following commands:

Node.js
```Bash
npx serve -p 300
```

Python 3
```Bash
python -m http.server 3000
```

PHP
```Bash
php -S localhost:3000
```

Then, open your browser to http://localhost:3000.
