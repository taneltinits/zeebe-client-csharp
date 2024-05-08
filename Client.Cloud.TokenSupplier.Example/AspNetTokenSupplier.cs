using System.Security.Authentication;
using Duende.AccessTokenManagement;
using Zeebe.Client.Api.Builder;

namespace Client.Cloud.TokenSupplier.Example;

public class AspNetTokenSupplier(IClientCredentialsTokenManagementService service) : IAccessTokenSupplier
{
    public async Task<string> GetAccessTokenForRequestAsync(string authUri = null,
        CancellationToken cancellationToken = new ())
    {
        var token = await service.GetAccessTokenAsync("zb-client", cancellationToken: cancellationToken);
        if (token.AccessToken is null)
        {
            throw new AuthenticationException("Failed to get access token");
        }

        return token.AccessToken;
    }
}