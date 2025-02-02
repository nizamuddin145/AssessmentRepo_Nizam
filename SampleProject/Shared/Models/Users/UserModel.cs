using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessEntities;

namespace Shared.Models.Users
{
    public class UserModel
    {
        /// <summary>
        /// User name, a required field.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// User email address, a required field.
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// User type. 
        /// </summary>
        public UserTypes Type { get; set; }

        /// <summary>
        /// User age, a required field and must be between 0 and max value.
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int Age { get; set; }

        /// <summary>
        /// User annual salary, a required field and must be between 0 and max value.
        /// </summary>
        [Required]
        [Range(0, double.MaxValue)]
        public decimal? AnnualSalary { get; set; }

        /// <summary>
        /// Tags collection of a User, a required field.
        /// </summary>
        [Required]
        public IEnumerable<string> Tags { get; set; }
    }
}