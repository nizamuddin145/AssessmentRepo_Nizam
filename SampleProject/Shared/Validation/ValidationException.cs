using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Validation
{
    public class ValidationException : ArgumentException
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : base(string.Join("; ", errors))
        {
            Errors = errors;
        }
    }
}
