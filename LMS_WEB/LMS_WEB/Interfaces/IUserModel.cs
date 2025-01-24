using LMS_WEB.Entities;

namespace LMS_WEB.Interfaces
{
    public interface IUserModel
    {
        public UserEnt? LogIn(UserEnt entity);

        public int RecoverPassword(UserEnt entity);

        public int ChangeTempKey(UserEnt entity);

        public UserEnt? GetUser(long q);

        public int UpdateProfile(UserEnt entity);

        public int ChangePassword(UserEnt entity);
    }
}
