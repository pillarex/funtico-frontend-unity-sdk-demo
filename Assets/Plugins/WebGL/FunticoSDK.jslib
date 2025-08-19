mergeInto(LibraryManager.library, {

  InitializeSDK: function(authClientIdPtr, envPtr) {
    const authClientId = UTF8ToString(authClientIdPtr);
    const env = UTF8ToString(envPtr);

    if (typeof FunticoSDK === 'undefined') {
      console.error("FunticoSDK is not loaded. Add the SDK <script> to your index.html.");
      return;
    }
    window.funticoSDKInstance = new FunticoSDK.FunticoSDK({
      authClientId: authClientId,
      env: env
    });
    console.log("Funtico SDK Initialized.");
  },

  SignIn: function(callbackUrlPtr, gameObjectNamePtr, promiseId) {
    if (!funticoSDKInstance) return;
    const callbackUrl = UTF8ToString(callbackUrlPtr);
    const gameObjectName = UTF8ToString(gameObjectNamePtr);
    funticoSDKInstance.signInWithFuntico(callbackUrl)
      .then(() => {
        unityInstance.SendMessage(gameObjectName, 'ResolvePromise', `${promiseId}:true`);
      })
      .catch(error => {
        unityInstance.SendMessage(gameObjectName, 'RejectPromise', `${promiseId}:${JSON.stringify(error)}`);
      });
  },

  GetUserInfo: function(gameObjectNamePtr, promiseId) {
    if (!funticoSDKInstance) return;
    const gameObjectName = UTF8ToString(gameObjectNamePtr);
    funticoSDKInstance.getUserInfo()
      .then(userInfo => {
        unityInstance.SendMessage(gameObjectName, 'ResolvePromise', `${promiseId}:${JSON.stringify(userInfo)}`);
      })
      .catch(error => {
        unityInstance.SendMessage(gameObjectName, 'RejectPromise', `${promiseId}:${JSON.stringify(error)}`);
      });
  },

  SaveScore: function(score, gameObjectNamePtr, promiseId) {
    if (!funticoSDKInstance) return;
    const gameObjectName = UTF8ToString(gameObjectNamePtr);
    funticoSDKInstance.saveScore(score)
      .then(response => {
        unityInstance.SendMessage(gameObjectName, 'ResolvePromise', `${promiseId}:${JSON.stringify(response)}`);
      })
      .catch(error => {
        unityInstance.SendMessage(gameObjectName, 'RejectPromise', `${promiseId}:${JSON.stringify(error)}`);
      });
  },

  SignOut: function(redirectUrlPtr) {
    if (!funticoSDKInstance) return;
    const redirectUrl = UTF8ToString(redirectUrlPtr);
    funticoSDKInstance.signOut(redirectUrl);
  }
});
