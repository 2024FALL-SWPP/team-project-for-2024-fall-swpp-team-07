using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using TMPro;
using UnityEngine;

namespace com.example
{
    public class SignInWithEmailPassword : MonoBehaviour
    {
        // Public Unity References
        public TMP_InputField EmailInput = null!;
        public TMP_InputField PasswordInput = null!;
        public TMP_Text ErrorText = null!;
        public SupabaseManager SupabaseManager = null!;

        // Private implementation
        private bool _doSignIn;
        private bool _doSignUp;

        // Unity does not allow async UI events, so we set a flag and use Update() to do the async work
        public void SignIn()
        {
            _doSignIn = true;
        }

        public void SignUp()
        {
            _doSignUp = true;
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private async void Update()
        {
            if (_doSignIn)
            {
                _doSignIn = false;
                await PerformSignIn();
                _doSignIn = false;
            }

            if (_doSignUp)
            {
                _doSignUp = false;
                await PerformSignUp();
                _doSignUp = false;
            }
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private async Task PerformSignIn()
        {
            try
            {
                Session session = (
                    await SupabaseManager
                        .Supabase()!
                        .Auth.SignIn(EmailInput.text, PasswordInput.text)
                )!;
                ErrorText.text = $"Success! Signed In as {session.User?.Email}";
            }
            catch (GotrueException goTrueException)
            {
                ErrorText.text = $"{goTrueException.Reason} {goTrueException.Message}";
                Debug.LogException(goTrueException, gameObject);
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
            }
        }

        // This is where we do the async work and handle exceptions
        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private async Task PerformSignUp()
        {
            try
            {
                Session session = (
                    await SupabaseManager
                        .Supabase()!
                        .Auth.SignUp(EmailInput.text, PasswordInput.text)
                )!;
                await CreateUserProfile(session);
                ErrorText.text = $"Success! Signed Up as {session.User?.Email}";
            }
            catch (GotrueException goTrueException)
            {
                ErrorText.text = $"{goTrueException.Reason} {goTrueException.Message}";
                Debug.LogException(goTrueException, gameObject);
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
            }
        }

        private async Task CreateUserProfile(Session session)
        {
            try
            {
                Debug.Log(
                    $"Signed User Info - ID: {session.User?.Id}, Email: {session.User?.Email}"
                );

                var response = await SupabaseManager
                    .Supabase()!
                    .From<UserProfile>()
                    .Insert(
                        new UserProfile
                        {
                            user_id = session.User.Id,
                            nickname = "Guest" + UnityEngine.Random.Range(1000, 9999),
                            almonds = 0,
                            is_guest = false,
                        }
                    );

                Debug.Log($"User profile created for {session.User.Email}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create user profile: {e.Message}");
            }
        }
    }

    [Table("users")] // public schema의 users 테이블 지정
    public class UserProfile : BaseModel
    {
        [PrimaryKey("user_id", false)]
        [Column("user_id")]
        public string user_id { get; set; }

        [Column("nickname")]
        public string nickname { get; set; }

        [Column("almonds")]
        public int almonds { get; set; }

        [Column("is_guest")]
        public bool is_guest { get; set; }
    }
}
