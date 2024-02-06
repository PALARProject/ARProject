using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class GoogleSignInManager : MonoBehaviour {
    private FirebaseAuth auth;

    private void Start() {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if(task.Exception != null) {
                Debug.LogError($"Firebase initialization failed: {task.Exception}");
                return;
            }

            auth = FirebaseAuth.DefaultInstance;
            SignInWithGoogle();
        });
    }

    public void SignInWithGoogle() {
        if(auth == null) {
            Debug.LogError("Firebase authentication is not initialized.");
            return;
        }

        Firebase.Auth.FirebaseUser user = null;

        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(null, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if(task.IsCanceled || task.IsFaulted) {
                Debug.LogError($"Failed to sign in with Google credential: {task.Exception}");
                return;
            }

            user = task.Result;
            Debug.Log("Google user signed in successfully!");
        });
    }
}