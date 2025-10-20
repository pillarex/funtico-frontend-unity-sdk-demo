# Funtico Frontend Unity SDK Demo

## Importing package thru UPM
1. Start a new Unity project or use your existing one (make sure to create a backup first).
2. Open the Package Manager
Go to Window → Package Manager in the Unity menu.
3. Add the required packages
In the Package Manager window, click the “+” button → choose “Add package from Git URL…”, then add the following URLs one by one:
```html
https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
```
```html
https://github.com/pillarex/funtico-frontend-unity-sdk-demo.git?path=Assets/FunticoSDK
```
4. (Optional): Import the sample project
After installation, open the Funtico SDK package page in the Package Manager and click “Import Sample” to load the example scene and script

### Prerequisites
- Unity 2022.3+ LTS 
- UniTask package

### Export WebGL Build
1. Download and install [Unity Hub](https://unity.com/download) and Unity 2022.3+ LTS
2. Open Unity Hub
3. Click "Open" and select the project folder
4. Go to File → Build Settings
5. Select "WebGL" platform and click "Switch Platform"
6. Click "Build" and choose `exports/web` as the build folder
7. Serve the exported files:

#### Node.js http-server
```bash
npm install -g http-server
cd exports/web
http-server -p 3000
```

#### Python
```bash
cd exports/web
# Python 3
python -m http.server 3000

# Python 2
python -m SimpleHTTPServer 3000
```

#### npx serve
```bash
cd exports/web
npx serve -p 3000
```

#### PHP
```bash
cd exports/web
php -S localhost:3000
```

### Additional info
If you wish to use your own template you need modify your `index.html` file:

* Add the following in head section:
```html
<script src="https://funtico-frontend-js-sdk.pages.dev/funtico-sdk.min.js"> </script>
```

* Add the following:
```html
    var myGameInstance = null;
```
above given line:
```html
    var script = document.createElement("script");
```

* Add the following:
```html
    myGameInstance = unityInstance;
```
inside of 'then' of createUnityInstance call:
```html
        createUnityInstance(canvas, config, (progress) => {
          document.querySelector("#unity-progress-bar-full").style.width = 100 * progress + "%";
              }).then((unityInstance) => {
                myGameInstance = unityInstance;
```

Open your browser to `http://localhost:3000`

