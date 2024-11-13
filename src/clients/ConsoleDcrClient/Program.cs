using Clients;
using IdentityModel.Client;
using System.Text.Json;



Console.Title = "Dynamic Client Registration - Client Credentials Flow";
await RegisterClient();
Console.ReadLine();
var response = await RequestTokenAsync();
response.Show();

Console.ReadLine();
await CallServiceAsync(response.AccessToken);
Console.ReadLine();

static async Task RegisterClient()
{
    var client = new HttpClient();

    var request = new DynamicClientRegistrationRequest
    {
        Address = Constants.Authority + "/connect/dcr",
        Document = new DynamicClientRegistrationDocument
        {

            GrantTypes = { "client_credentials" },
            Scope = "resource1.scope1 resource2.scope1 IdentityServerApi"
        }
    };
    request.Document.Extensions.Add("client_id", JsonDocument.Parse($"\"client\"").RootElement);
    request.Document.Extensions.Add("client_secret", JsonDocument.Parse($"\"secret\"").RootElement);


    var serialized = JsonSerializer.Serialize(request.Document);

    var deserialized = JsonSerializer.Deserialize<DynamicClientRegistrationDocument>(serialized);


    var response = await client.RegisterClientAsync(request);

    if (response.IsError)
    {
        Console.WriteLine(response.Error);
        return;
    }
    Console.WriteLine(JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true }));
}

static async Task<TokenResponse> RequestTokenAsync()
{
    var client = new HttpClient();

    var disco = await client.GetDiscoveryDocumentAsync(Constants.Authority);
    if (disco.IsError) throw new Exception(disco.Error);

    var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = disco.TokenEndpoint,

        ClientId = "client",
        ClientSecret = "secret",
    });

    if (response.IsError) throw new Exception(response.Error);
    return response;
}

static async Task CallServiceAsync(string token)
{
    var baseAddress = Constants.SampleApi;

    var client = new HttpClient
    {
        BaseAddress = new Uri(baseAddress)
    };

    client.SetBearerToken(token);
    var response = await client.GetStringAsync("identity");

    "\n\nService claims:".ConsoleGreen();
    Console.WriteLine(response.PrettyPrintJson());
}