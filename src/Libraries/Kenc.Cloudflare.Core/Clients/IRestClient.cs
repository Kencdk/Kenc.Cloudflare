namespace Kenc.Cloudflare.Core.Clients
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Kenc.Cloudflare.Core.Entities;

    /// <summary>
    /// Describes an interface for a restful client.
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Sends a GET request to the <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of expected data to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="TResult"/></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<TResult> GetAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : ICloudflareEntity;

        /// <summary>
        /// Sends a PATCH request to the <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="TMessage">Type of data to send.</typeparam>
        /// <typeparam name="TResult">Type of expected data to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="message">Object to send of type <typeparamref name="TMessage"/></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="TResult"/></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<TResult> PatchAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default) where TResult : ICloudflareEntity;

        /// <summary>
        /// Sends a POST request to the <paramref name="uri"/>.
        /// </summary>
        /// <typeparam name="TMessage">Type of data to send.</typeparam>
        /// <typeparam name="TResult">Type of expected data to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="message">Object to send of type <typeparamref name="TMessage"/></param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Data returned from the server as <typeparamref name="TResult"/></returns>
        /// <exception cref="Exceptions.CloudflareException"></exception>
        Task<TResult> PostAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default) where TResult : ICloudflareEntity;

        /// <summary>
        /// Sends a DELETE request to the <paramref name="uri"/>
        /// </summary>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An async Task.</returns>
        /// <exception cref="CloudflareException"></exception>
        Task<TResult> DeleteAsync<TResult>(Uri uri, CancellationToken cancellationToken = default) where TResult : ICloudflareEntity;

        /// <summary>
        /// Sends a PUT request to the <paramref name="uri"/>
        /// </summary>
        /// <typeparam name="TMessage">Type of data to send.</typeparam>
        /// <typeparam name="TResult">Type of message to receive.</typeparam>
        /// <param name="uri">Endpoint to target.</param>
        /// <param name="message">Payload to the request.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        Task<TResult> PutAsync<TMessage, TResult>(Uri uri, TMessage message, CancellationToken cancellationToken = default) where TResult : ICloudflareEntity;
    }
}
