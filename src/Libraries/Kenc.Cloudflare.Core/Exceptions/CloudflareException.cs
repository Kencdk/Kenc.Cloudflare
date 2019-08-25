namespace Kenc.Cloudflare.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CloudflareException : Exception
    {
        public IList<CloudflareAPIError> Errors { get; private set; }

        public CloudflareException(IList<CloudflareAPIError> errors) : base()
        {
            Errors = errors;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var error in Errors)
            {
                stringBuilder.AppendLine($"{error.Code}: {error.Message}");
            }

            return stringBuilder.ToString();
        }
    }
}
