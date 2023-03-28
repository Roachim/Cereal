namespace CerealAPI.DTO
{
    public class UserDTO
    {
        Dictionary<UserDTO, string> UserDiction = new Dictionary<UserDTO, string>();
        public string Name { get; set; }
        public string Password { get; set; }
        UserDTO() { }
        public UserDTO(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
