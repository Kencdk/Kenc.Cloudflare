namespace Kenc.Cloudflare.Core.Clients.EntityClients
{
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    public interface IUserClient : IEntityClient
    {
        /// <summary>
        /// Gets the currently logged in/authenticated User
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="User"/> object.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<User> GetUserAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit part of your user details
        /// </summary>
        /// <param name="firstName">Firstname</param>
        /// <param name="lastName">Lastname</param>
        /// <param name="telephone">Telephone</param>
        /// <param name="country">Country</param>
        /// <param name="zipcode">Zipcode</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns><see cref="User"/> object.</returns>
        /// <exception cref="Exceptions.CloudflareException">Thrown when an error is returned from the Cloudflare API.</exception>
        Task<User> PatchUserAsync(string? firstName = null, string? lastName = null, string? telephone = null, string? country = null, string? zipcode = null, CancellationToken cancellationToken = default);
    }
}