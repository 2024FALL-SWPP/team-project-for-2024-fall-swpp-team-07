using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.example.Models; // 사용자 정의 모델 사용
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        public GameObject Panel = null!;

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

        // initialize variables (when login scene is re-loaded from other scenes)
        private void Start()
        {
            var loginUIObjects = GameObject.FindGameObjectsWithTag("LoginUI");
            foreach (var obj in loginUIObjects)
            {
                switch (obj.name)
                {
                    case "Email InputField":
                        EmailInput = obj.GetComponent<TMP_InputField>();
                        break;
                    case "Password InputField":
                        PasswordInput = obj.GetComponent<TMP_InputField>();
                        break;
                    case "Nickname InputField":
                        NicknameInput = obj.GetComponent<TMP_InputField>();
                        break;
                    case "Sign In Button":
                        SignInButton = obj;
                        break;
                    case "Sign Up Button":
                        SignUpButton = obj;
                        break;
                    case "Game Play Button":
                        GamePlayButton = obj;
                        break;
                    case "Error Label":
                        ErrorText = obj.GetComponent<TMP_Text>();
                        break;
                    case "Status Label":
                        StatusText = obj.GetComponent<TMP_Text>();
                        break;
                    case "Panel":
                        Panel = obj;
                        break;
                }
            }

            SupabaseManager = SupabaseManager.Instance;
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

            // Tab 키를 누르면 다음 입력 필드로 이동
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (EmailInput.isFocused)
                {
                    PasswordInput.Select();
                }
                else if (PasswordInput.isFocused)
                {
                    NicknameInput.Select();
                }
                else if (NicknameInput.isFocused)
                {
                    EmailInput.Select();
                }
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
                Panel.GetComponent<VerticalLayoutGroup>().spacing = 250;
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
                // 회원가입 후 로그인 수행
                session = await SupabaseManager
                    .Supabase()!
                    .Auth.SignIn(EmailInput.text, PasswordInput.text);

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
                // users 테이블에 사용자 프로필 추가
                await SupabaseManager
                    .Supabase()!
                    .From<UserProfile>()
                    .Insert(
                        new UserProfile
                        {
                            user_id = session.User.Id,
                            nickname = nickname,
                            almonds = 0,
                            stage1_clear = false,
                            stage2_clear = false,
                            stage3_clear = false,
                        }
                    );

                // store 테이블에 사용자 상점 정보 추가
                await SupabaseManager
                    .Supabase()!
                    .From<UserStore>()
                    .Insert(
                        new UserStore
                        {
                            user_id = session.User.Id,
                            hamster_skin_1 = false,
                            hamster_skin_2 = false,
                            cannon_skin_1 = false,
                            cannon_skin_2 = false,
                            ball_tail_effect_1 = false,
                            ball_tail_effect_2 = false,
                        }
                    );

                // stage_status 테이블에 사용자 스테이지 정보 추가
                var stageStatuses = Enumerable
                    .Range(1, 3)
                    .Select(i => new UserStageStatus
                    {
                        user_id = session.User.Id,
                        stage_id = i,
                        almond_status = new bool[3] { false, false, false },
                    })
                    .ToList();

                await SupabaseManager.Supabase()!.From<UserStageStatus>().Insert(stageStatuses);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create user profile: {e.Message}");
            }
        }
    }
}
