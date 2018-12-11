using System.Collections.Generic;
using PronetAutomation.Values;

namespace PronetAutomation.Mongo
{
    public enum LoginType
    {
        AnyUser,
        Admin,
        NothingOrdered,
        ProfileCompleted
    }
    public class LoginInfo
    {
        public LoginInfo(Dictionary<string, object> dictionaryFields, LoginType loginType) 
        {
            DictionaryFields = dictionaryFields;
            LoginType = loginType;
        }
        public Dictionary<string, object> DictionaryFields { get; set; }
        public LoginType LoginType { get; set; }
    }
}