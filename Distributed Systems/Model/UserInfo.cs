using DataCenter.Interface;

namespace Model
{
    public class UserInfo : IDbModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }
    }
}
