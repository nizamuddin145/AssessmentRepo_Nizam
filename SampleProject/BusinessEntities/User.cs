using System.Collections.Generic;
using System.Linq;
using Common.Extensions;

namespace BusinessEntities
{
    public class User : IdObject
    {
        private readonly List<string> _tags = new List<string>();
        private int _age;
        private string _email;
        private decimal? _monthlySalary;
        private string _name;
        private UserTypes _type = UserTypes.Employee;

        /// <summary>
        /// User email address.
        /// </summary>
        public string Email
        {
            get => _email;
            private set => _email = value;
        }

        /// <summary>
        /// User name.
        /// </summary>
        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        /// <summary>
        /// USer type.
        /// </summary>
        public UserTypes Type
        {
            get => _type;
            private set => _type = value;
        }

        /// <summary>
        /// Monthly salary.
        /// </summary>
        public decimal? MonthlySalary
        {
            get => _monthlySalary;
            private set => _monthlySalary = value;
        }

        /// <summary>
        /// User age.
        /// </summary>
        public int Age
        {
            get => _age;
            private set => _age = value;
        }

        /// <summary>
        /// USer tags.
        /// </summary>
        public IEnumerable<string> Tags
        {
            get => _tags;
            private set => _tags.Initialize(value);
        }

        /// <summary>
        /// Set the User name.
        /// </summary>
        /// <param name="name">USer name.</param>
        public void SetName(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Set the user email address.
        /// </summary>
        /// <param name="email">User email address.</param>
        public void SetEmail(string email)
        {
            _email = email;
        }

        /// <summary>
        /// Set the user type.
        /// </summary>
        /// <param name="type">User type.</param>
        public void SetType(UserTypes type)
        {
            _type = type;
        }

        /// <summary>
        /// Set the user age.
        /// </summary>
        /// <param name="age">Age of the user.</param>
        public void SetAge(int age)
        {
            _age = age;
        }

        /// <summary>
        /// Set the monthly salary.
        /// </summary>
        /// <param name="annualSalary">Annual salary.</param>
        public void SetMonthlySalary(decimal? annualSalary)
        {
            _monthlySalary = annualSalary/12;
        }

        /// <summary>
        /// Set the tags of user.
        /// </summary>
        /// <param name="tags">USer tags</param>
        public void SetTags(IEnumerable<string> tags)
        {
            _tags.Initialize(tags);
        }
    }
}