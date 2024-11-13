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
        public TMP_InputField NicknameInput = null!;
        public TMP_Text ErrorText = null!;
        public TMP_Text StatusText = null!;
        public GameObject SignInButton = null!;
        public GameObject SignUpButton = null!;
        public GameObject GamePlayButton = null!;
        public SupabaseManager SupabaseManager = null!;

        // Private implementation
        private bool _doSignIn;
        private bool _doSignUp;
        private bool _doSignOut;

        // Unity does not allow async UI events, so we set a flag and use Update() to do the async work
        public void SignIn()
        {
            _doSignIn = true;
        }

        public void SignUp()
        {
            _doSignUp = true;
        }

        public void SignOut()
        {
            _doSignOut = true;
        }

        public void PlayGame()
        {
            // move to WaitingGame scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("WaitingGame");
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

            if (_doSignOut)
            {
                _doSignOut = false;
                await SupabaseManager.Supabase()!.Auth.SignOut();
                _doSignOut = false;
            }
        }

        // UI 상태 변경
        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private async Task UpdateUIState(Session session = null)
        {
            bool isLoggedIn = session != null;

            // 비로그인 시에만 활성화
            EmailInput.gameObject.SetActive(!isLoggedIn);
            PasswordInput.gameObject.SetActive(!isLoggedIn);
            NicknameInput.gameObject.SetActive(!isLoggedIn);
            SignInButton.SetActive(!isLoggedIn);
            SignUpButton.SetActive(!isLoggedIn);
            ErrorText.gameObject.SetActive(!isLoggedIn);

            // 로그인 시에만 활성화
            GamePlayButton.SetActive(isLoggedIn);

            // 항상 활성화
            StatusText.gameObject.SetActive(true);

            if (isLoggedIn)
            {
                var userProfile = await GetUserProfile(session);
                StatusText.text = $"Hello {userProfile.nickname}! Let's save the hamster!";
            }
            else
            {
                StatusText.text = "Please Login to save your hamster ...";
            }
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private async Task<UserProfile> GetUserProfile(Session session)
        {
            try
            {
                // session.User.Id 를 가지고 사용자 닉네임을 가져옴
                var userProfile = await SupabaseManager
                    .Supabase()!
                    .From<UserProfile>()
                    .Select("*")
                    .Where(x => x.user_id == session.User.Id)
                    .Single();

                return userProfile;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to fetch user profile: {e.Message}");
                throw;
            }
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private async Task PerformSignIn()
        {
            try
            {
                StatusText.text = "Loading...";

                Session session = (
                    await SupabaseManager
                        .Supabase()!
                        .Auth.SignIn(EmailInput.text, PasswordInput.text)
                )!;

                await UpdateUIState(session);
            }
            catch (GotrueException goTrueException)
            {
                ErrorText.text = $"{goTrueException.Reason} {goTrueException.Message}";
                StatusText.text = "Please Login to save your hamster ...";
                Debug.LogException(goTrueException, gameObject);
            }
            catch (Exception e)
            {
                StatusText.text = "Please Login to save your hamster ...";
                Debug.LogException(e, gameObject);
            }
        }

        // This is where we do the async work and handle exceptions
        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private async Task PerformSignUp()
        {
            try
            {
                StatusText.text = "Loading...";

                Session session = (
                    await SupabaseManager
                        .Supabase()!
                        .Auth.SignUp(EmailInput.text, PasswordInput.text)
                )!;
                await CreateUserProfile(session, NicknameInput.text);

                await UpdateUIState(session);
            }
            catch (GotrueException goTrueException)
            {
                ErrorText.text = $"{goTrueException.Reason} {goTrueException.Message}";
                StatusText.text = "Please Login to save your hamster ...";
                Debug.LogException(goTrueException, gameObject);
            }
            catch (Exception e)
            {
                StatusText.text = "Please Login to save your hamster ...";
                Debug.LogException(e, gameObject);
            }
        }

        private async Task CreateUserProfile(Session session, string nickname)
        {
            try
            {
                var response = await SupabaseManager
                    .Supabase()!
                    .From<UserProfile>()
                    .Insert(
                        new UserProfile
                        {
                            user_id = session.User.Id,
                            nickname = nickname,
                            almonds = 0,
                            is_guest = false,
                        }
                    );
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
