using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
#pragma warning disable CS1998

namespace test
{
    /// <summary>
    /// Allows us to bypass authentication when using Lowkey Vault.
    /// <br/>
    /// <b>WARNING: Will not work with real Azure services.</b>
    /// </summary>
    public class NoopCredentials: TokenCredential
    {
        public override async ValueTask<AccessToken> GetTokenAsync(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return GetToken(requestContext, cancellationToken);
        }

        public override AccessToken GetToken(TokenRequestContext requestContext, CancellationToken cancellationToken)
        {
            return new AccessToken("noop", DateTimeOffset.MaxValue);
        }
    }
}