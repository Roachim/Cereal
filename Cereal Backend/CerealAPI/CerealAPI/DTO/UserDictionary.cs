using Microsoft.Extensions.Logging;

namespace CerealAPI.DTO
{
    public class UserDictionary
    {
        // Lazy<T>
        private static readonly Lazy<UserDictionary> _lazyLogger
            = new Lazy<UserDictionary>(() => new UserDictionary());

        // private static UserDictionary? _instance;

        /// <summary>
        /// Instance
        /// </summary>
        public static UserDictionary Instance
        {
            get
            {
                return _lazyLogger.Value;
                //if (_instance == null)
                //{
                //    _instance = new Logger();
                //}
                //return _instance;
            }
        }

        protected UserDictionary()
        {
        }

        /// <summary>
        /// SingletonOperation
        /// </summary> 
        public void AddUser(UserDTO user, string password)
        {
            
        }
    }
}
