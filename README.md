# Funtico Frontend Unity SDK Demo

This Unity project demonstrates OAuth2 + PKCE authentication with automatic token refresh using the Funtico SDK. It replicates the functionality of the Godot version in the `funtico-frontend-godot-sdk-demo-/` folder.

## Features

- **Login Screen**: OAuth2 authentication with Funtico
- **User Information Display**: Shows username and user ID after successful login
- **Score Saving**: Allows users to save scores through the Funtico API
- **Sign Out**: Logout functionality with proper token cleanup
- **WebGL Support**: Fully functional in web browsers with JavaScript bridge

## Prerequisites

- Unity 2022.3 LTS or newer
- WebGL build support module installed

## Project Structure

```
Assets/
├── Scenes/
│   ├── LoginScene.unity          # Initial login screen
│   └── LoggedInScene.unity       # User info and score management
├── Scripts/
│   ├── AuthManager.cs            # Global authentication manager
│   ├── LoginUI.cs               # Login screen UI controller
│   └── LoggedInUI.cs            # Logged-in screen UI controller
├── Plugins/
│   └── FunticoSDKBridge.jslib   # JavaScript bridge for WebGL
└── WebGLTemplates/
    └── FunticoSDK/
        └── index.html            # Custom HTML template with SDK
```

## Setup Instructions

### 1. Open the Project
1. Open Unity Hub
2. Click "Add" and select this project folder
3. Open the project with Unity 2022.3 LTS or newer

### 2. Configure Build Settings
1. Go to **File** → **Build Settings**
2. Select **WebGL** platform
3. Click **Switch Platform**
4. Add scenes to build:
   - `Assets/Scenes/LoginScene.unity` (Index: 0)
   - `Assets/Scenes/LoggedInScene.unity` (Index: 1)

### 3. Configure WebGL Settings
1. Go to **Edit** → **Project Settings** → **Player** → **WebGL**
2. Under **Publishing Settings**:
   - Set **WebGL Template** to `FunticoSDK`
3. Under **Other Settings**:
   - Set **Color Space** to `Linear` (recommended)
   - Ensure **Auto Graphics API** is checked

### 4. Set Up UI (Manual Step Required)

Since Unity scene files need to be configured in the editor, you'll need to manually set up the UI:

#### LoginScene Setup:
1. Open `LoginScene.unity`
2. Create UI elements:
   - Canvas with UI Scale Mode set to "Scale With Screen Size"
   - Background panel with light gray color
   - Title text: "Funtico SDK Authentication Demo"
   - Subtitle text: "OAuth2 + PKCE authentication with automatic token refresh"
   - Login button with text "Login with Funtico"
3. Add `AuthManager.cs` script to an empty GameObject named "AuthManager"
4. Add `LoginUI.cs` script to the Canvas
5. Connect the login button to the LoginUI script's `loginButton` field

#### LoggedInScene Setup:
1. Open `LoggedInScene.unity`
2. Create UI elements:
   - Canvas with UI Scale Mode set to "Scale With Screen Size"
   - Background panel with light gray color
   - Title text: "Funtico SDK Authentication Demo"
   - Username label: "Username: "
   - User ID label: "User ID: "
   - Score input field (TMP_InputField)
   - Save Score button
   - Sign Out button
3. Add `LoggedInUI.cs` script to the Canvas
4. Connect all UI elements to the LoggedInUI script fields

### 5. Build and Run
1. Go to **File** → **Build and Run**
2. Choose a build folder (e.g., `Build/`)
3. Unity will build and automatically open the project in your browser

## How to Use

1. **Login**: Click "Login with Funtico" to authenticate
2. **View User Info**: After successful login, see your username and user ID
3. **Save Score**: Enter a score and click "Save Score"
4. **Sign Out**: Click "Sign Out" to logout and return to login screen

## Technical Details

### JavaScript Bridge
The project uses Unity's WebGL JavaScript interop to communicate with the Funtico SDK:
- `FunticoSDKBridge.jslib` contains JavaScript functions callable from C#
- `AuthManager.cs` uses `[DllImport("__Internal")]` to call JavaScript functions
- Callbacks from JavaScript to Unity use `SendMessage()`

### Authentication Flow
1. **Initialize SDK**: Funtico SDK is loaded via the custom HTML template
2. **Check Auth Status**: On scene load, check if user is already authenticated
3. **Login**: Redirect to Funtico OAuth2 flow
4. **Token Management**: SDK handles token refresh automatically
5. **API Calls**: Authenticated API calls for user info and score saving

### Scene Management
- `LoginScene`: Entry point, handles authentication
- `LoggedInScene`: Post-authentication user interface
- Scene switching handled by `AuthManager.cs`

## Serving the Built Files

After building, serve the files using a local server:

### Node.js http-server
```bash
npm install -g http-server
cd Build
http-server -p 3000
```

### Python
```bash
cd Build
# Python 3
python -m http.server 3000

# Python 2
python -m SimpleHTTPServer 3000
```

### npx serve
```bash
cd Build
npx serve -p 3000
```

Open your browser to `http://localhost:3000`

## Configuration

The Funtico SDK is configured in `not_logged_in.gd` (Godot) and `AuthManager.cs` (Unity):
- **Auth Client ID**: `mock_store_fe`
- **Environment**: `staging`

## Troubleshooting

### Common Issues
1. **CORS Errors**: Ensure you're serving files from a local server, not opening HTML directly
2. **SDK Not Loading**: Check network connection and SDK URL in HTML template
3. **Authentication Fails**: Verify client ID and environment settings
4. **UI Not Responding**: Ensure UI elements are properly connected in the Unity editor

### Development Tips
- Use browser developer tools to debug JavaScript bridge calls
- Check Unity Console for C# errors and debug logs
- Test in multiple browsers for compatibility

## Comparison with Godot Version

This Unity version replicates all functionality from the Godot demo:
- ✅ OAuth2 + PKCE authentication
- ✅ User information display
- ✅ Score saving functionality
- ✅ Sign out capability
- ✅ WebGL deployment
- ✅ Funtico SDK integration

The main differences:
- **Language**: C# instead of GDScript
- **UI System**: Unity UI instead of Godot's Control nodes
- **JavaScript Bridge**: Unity's WebGL interop instead of JavaScriptBridge
- **Build System**: Unity's WebGL build instead of Godot's HTML5 export# funtico-frontend-unity-sdk-demo
