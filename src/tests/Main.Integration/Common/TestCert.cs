// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer.IntegrationTests.Common
{
    internal static class TestCert
    {
        private static X509Certificate2 LoadCertificateFromFile(string fileName, string password)
        {
            X509Certificate2 cert = null;

#if NET9_0_OR_GREATER
            // In Directly using the new API in NET 9.0+
             cert = X509CertificateLoader.LoadPkcs12FromFile(fileName, password);
#else
            // In Reflection is used in NET 8.0 and below versions
            var loaderType = Type.GetType("System.Security.Cryptography.X509Certificates.X509CertificateLoader, System.Security.Cryptography");
            if (loaderType != null)
            {
                var method = loaderType.GetMethod("LoadFromFile", new[] { typeof(string), typeof(string) });
                if (method != null)
                {
                    cert = (X509Certificate2) method.Invoke(null, new object[] { fileName, password });
                    if (cert != null)
                        return cert;
                }
            }

            // If the reflection fails, revert back to the old method
#pragma warning disable SYSLIB0057 // Type or member is obsolete
            cert = new X509Certificate2(fileName, password);
#pragma warning restore SYSLIB0057 // Type or member is obsolete
#endif

            return cert;
        }

        public static X509Certificate2 Load()
        {
            var cert = Path.Combine(System.AppContext.BaseDirectory, "identityserver_testing.pfx");
            return LoadCertificateFromFile(cert, "password");
        }
    }
}