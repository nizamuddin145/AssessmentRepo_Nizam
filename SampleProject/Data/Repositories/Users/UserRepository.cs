using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Data.Indexes;
using Raven.Client.Documents.Session;

namespace Data.Repositories.Users
{
    [AutoRegister]
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IAsyncDocumentSession _documentSession;

        /// <summary>
        /// UserRepository class initilization.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public UserRepository(IAsyncDocumentSession documentSession) : base(documentSession)
        {
            _documentSession = documentSession;
        }

        /// <summary>
        /// Get the document based on the filters.
        /// </summary>
        /// <param name="userType">User type.</param>
        /// <param name="name">User name.</param>
        /// <param name="email">Email address.</param>
        /// <param name="tag">User tag.</param>
        /// <returns>A list of User that match the filters.</returns>
        public async Task<IEnumerable<User>> GetDocumentAsync(UserTypes? userType = null, string name = null, string email = null, string tag = null)
        {
            var query = _documentSession.Advanced.AsyncDocumentQuery<User, UsersListIndex>();

            var hasFirstParameter = false;
            if (userType != null)
            {
                query = query.WhereEquals("Type", (int)userType);
                hasFirstParameter = true;
            }

            if (name != null)
            {
                if (hasFirstParameter)
                {
                    query = query.AndAlso();
                }
                else
                {
                    hasFirstParameter = true;
                }
                query = query.WhereEquals("Name", name);
            }

            if (email != null)
            {
                if (hasFirstParameter)
                {
                    query = query.AndAlso();
                }
                query = query.WhereEquals("Email", email);
            }

            if (tag != null)
            {
                if (hasFirstParameter)
                {
                    query = query.AndAlso();
                }
                query = query.WhereIn("Tags", new[] { tag });
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// Deletes all users from the store.
        /// </summary>
        public async Task DeleteAllDcoumentAsync()
        {
            await base.DeleteAllAsync<UsersListIndex>();
        }
    }
}