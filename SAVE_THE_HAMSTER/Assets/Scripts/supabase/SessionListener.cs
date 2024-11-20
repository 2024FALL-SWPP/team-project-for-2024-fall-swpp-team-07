using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using TMPro;
using UnityEngine;

namespace com.example
{
    public class SessionListener : MonoBehaviour
    {
        // Public Unity References
        public SupabaseManager SupabaseManager = null!;

        public void UnityAuthListener(
            IGotrueClient<User, Session> sender,
            Constants.AuthState newState
        )
        {
            switch (newState)
            {
                case Constants.AuthState.SignedIn:
                    Debug.Log("Signed In");
                    break;
                case Constants.AuthState.SignedOut:
                    Debug.Log("Signed Out");
                    break;
                case Constants.AuthState.UserUpdated:
                    Debug.Log("Signed In");
                    break;
                case Constants.AuthState.PasswordRecovery:
                    Debug.Log("Password Recovery");
                    break;
                case Constants.AuthState.TokenRefreshed:
                    Debug.Log("Token Refreshed");
                    break;
                case Constants.AuthState.Shutdown:
                    Debug.Log("Shutdown");
                    break;
                default:
                    Debug.Log("Unknown Auth State Update");
                    break;
            }
        }
    }
}
