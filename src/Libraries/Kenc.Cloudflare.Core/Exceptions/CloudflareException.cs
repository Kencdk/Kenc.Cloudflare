namespace Kenc.Cloudflare.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Exception wrapping errors from the Cloudflare API.
    /// </summary>
    public class CloudflareException : Exception
    {
        /// <summary>
        /// Gets a list of <see cref="CloudflareApiError"/> returned from the API.
        /// </summary>
        public IList<CloudflareApiError> Errors { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudflareException"/> class.
        /// </summary>
        /// <param name="errors">List of errors returned by the Cloudflare API.</param>
        public CloudflareException(IList<CloudflareApiError> errors) : base()
        {
            Errors = errors;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (CloudflareApiError error in Errors)
            {
                stringBuilder.AppendLine($"{error.Code}: {error.Message}");
            }

            return stringBuilder.ToString();
        }
    }
}
