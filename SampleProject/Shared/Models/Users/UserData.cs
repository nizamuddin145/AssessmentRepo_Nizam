using BusinessEntities;

namespace Shared.Models.Users
{
    public class UserData : IdObjectData
    {
        /// <summary>
        /// User data constructor to initializes a new instance of a User object.
        /// </summary>
        /// <param name="user">User object.</param>
        public UserData(User user) : base(user)
        {
            Email = user.Email;
            Name = user.Name;
            Type = new EnumData(user.Type);
            MonthlySalary = user.MonthlySalary;
            Age = user.Age;
        }

        /// <summary>
        /// Name of the User.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email address of the User.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User type.
        /// </summary>
        public EnumData Type { get; set; }

        /// <summary>
        /// Monthly salary of the User.
        /// </summary>
        public decimal? MonthlySalary { get; set; }

        /// <summary>
        /// Age of the User.
        /// </summary>
        public int Age { get; set; }
    }
}