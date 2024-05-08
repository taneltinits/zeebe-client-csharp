// See https://aka.ms/new-console-template for more information

using Client.Cloud.TokenSupplier.Example;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using Zeebe.Client;
using Zeebe.Client.Api.Builder;


const string zeebeUrl = "https://localhost:26501";
const string authUrl = "http://localhost:18080/auth/realms/camunda-platform/protocol/openid-connect/token";
const string clientId = "zeebe";
const string clientSecret = "zecret";

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddClientCredentialsTokenManagement().AddClient("zb-client", client =>
{
    client.TokenEndpoint = authUrl;
    client.ClientId = clientId;
    client.ClientSecret = clientSecret;
});
builder.Services.AddSingleton<IAccessTokenSupplier, AspNetTokenSupplier>();

Console.WriteLine("Hello, World!");
var client = ZeebeClient.Builder()
    .UseLoggerFactory(new NLogLoggerFactory())
    .UseGatewayAddress(zeebeUrl)
    .UseTransportEncryption()
    .UseAccessTokenSupplier(builder.Services.BuildServiceProvider().GetService<IAccessTokenSupplier>())
    .Build();

var topology = await client.TopologyRequest()
    .Send();

Console.WriteLine(topology);