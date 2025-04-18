using Clients;
using IdentityModel;
using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ConsoleEphemeralMtlsClient
{
    class Program
    {
        private static X509Certificate2 _clientCertificate;
        
        static async Task Main(string[] args)
        {
            _clientCertificate = CreateClientCertificate("client");
            
            var response = await RequestTokenAsync();
            response.Show();

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
        }
        
        static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient(GetHandler(_clientCertificate));

            var disco = await client.GetDiscoveryDocumentAsync(Constants.AuthorityMtls);
            if (disco.IsError) throw new Exception(disco.Error);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.MtlsEndpointAliases.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "resource1.scope1"
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task CallServiceAsync(string token)
        {
            var client = new HttpClient(GetHandler(_clientCertificate))
            {
                BaseAddress = new Uri(Constants.SampleApiMtls)
            };

            client.SetBearerToken(token);
            var response = await client.GetStringAsync("identity");

            "\n\nService claims:".ConsoleGreen();
            Console.WriteLine(response.PrettyPrintJson());
        }
        
        static X509Certificate2 CreateClientCertificate(string name)
        {
            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={name}");

            using (var rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256,RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature , false));

                request.CertificateExtensions.Add(
                    new X509EnhancedKeyUsageExtension(
                        new OidCollection { new Oid("1.3.6.1.5.5.7.3.2") }, false));

                return request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
            }
        }
        
        static SocketsHttpHandler GetHandler(X509Certificate2 certificate)
        {
            var handler = new SocketsHttpHandler
            {
                SslOptions =
                {
                    ClientCertificates = new X509CertificateCollection {certificate}
                }
            };

            return handler;
        }
    }
}