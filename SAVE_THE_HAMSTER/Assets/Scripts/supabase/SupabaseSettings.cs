using UnityEngine;

namespace com.example
{
    [CreateAssetMenu(fileName = "Supabase", menuName = "Supabase/Supabase Settings", order = 1)]
    public class SupabaseSettings : ScriptableObject
    {
        // supabase 연결을 위한 URL과 키
        public string SupabaseURL = null!;
        public string SupabaseAnonKey = null!;
    }
}
