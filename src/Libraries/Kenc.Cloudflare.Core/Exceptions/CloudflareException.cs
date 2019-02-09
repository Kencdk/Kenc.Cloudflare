namespace Kenc.Cloudflare.Core.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class CloudflareException : Exception
    {
        public IList<CloudflareAPIError> Errors { get; private set; }

        public CloudflareException(IList<CloudflareAPIError> errors) : base()
        {
            Errors = errors;
        }
    }
}
