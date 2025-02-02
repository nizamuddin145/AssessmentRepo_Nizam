using BusinessEntities;
using Common;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Operations;
using System.Threading.Tasks;

namespace Data.Repositories
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class Repository<T> : IRepository<T> where T : IdObject
    {
        private readonly IAsyncDocumentSession _documentSession;

        /// <summary>
        /// Repository class initialization.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public Repository(IAsyncDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        /// <summary>
        /// Saves the document to the store.
        /// </summary>
        /// <param name="document">Document to be saved.</param>
        public async Task SaveAsync(T document)
        {
            await _documentSession.StoreAsync(document);
        }

        /// <summary>
        /// Delete the document from the store by Id.
        /// </summary>
        /// <param name="document">A document for deletion.</param>
        public async Task DeleteAsync(T document)
        {
            _documentSession.Delete(document);
        }

        /// <summary>
        /// Get the document from store by Id.
        /// </summary>
        /// <param name = "id" > The Id of the document</param>
        /// <returns>The document object from the store.</returns>
        public async Task<T> GetAsync(string id)
        {
            return await _documentSession.LoadAsync<T>(id);
        }

        /// <summary>
        /// Deletes all the documents from the data store using the index.
        /// </summary>
        /// <typeparam name="TIndex">The index for deletion.</typeparam>
        protected async Task DeleteAllAsync<TIndex>() where TIndex : AbstractIndexCreationTask<T>
        {
            var indexName = typeof(TIndex).Name;

            var deleteByQueryOperation = new DeleteByQueryOperation($"from index '{indexName}'");

            await _documentSession.Advanced.DocumentStore.Operations.SendAsync(deleteByQueryOperation);
        }
    }
}