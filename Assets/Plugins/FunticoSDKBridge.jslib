var FunticoSDKPlugin = {
    InitializeFunticoSDK: function() {
        if (typeof window.sdk === 'undefined') {
            window.sdk = new FunticoSDK({
                authClientId: 'mock_store_fe',
                env: 'staging'
            });
        }
    },
    
    GetUserInfo: function() {
        if (window.sdk) {
            window.sdk.getUserInfo()
                .then(function(userInfo) {
                    var userData = {
                        username: userInfo[0].username,
                        user_id: userInfo[0].user_id
                    };
                    SendMessage('AuthManager', 'OnUserInfoSuccess', JSON.stringify(userData));
                })
                .catch(function(error) {
                    SendMessage('AuthManager', 'OnUserInfoError', error.toString());
                });
        }
    },
    
    SignInWithFuntico: function(redirectUrl) {
        var url = Pointer_stringify(redirectUrl);
        if (window.sdk) {
            window.sdk.signInWithFuntico(url);
        }
    },
    
    SignOut: function(redirectUrl) {
        var url = Pointer_stringify(redirectUrl);
        if (window.sdk) {
            window.sdk.signOut(url);
        }
    },
    
    SaveScore: function(score) {
        if (window.sdk) {
            window.sdk.saveScore(score)
                .then(function(result) {
                    SendMessage('AuthManager', 'OnScoreSaved', result.toString());
                })
                .catch(function(error) {
                    console.error('Failed to save score:', error);
                });
        }
    }
};

mergeInto(LibraryManager.library, FunticoSDKPlugin);