using System;
using System.IO;
using System.Threading.Tasks;
using static Bullseye.Targets;
using static SimpleExec.Command;

namespace build
{
    internal static class Program
    {
        private const string solution = "src/IdentityServer4Plus.sln";
        private const string solutionCodeQL = "src/IdentityServer4Plus.CodeQL.sln";
        private const string packOutput = "./artifacts";
        private const string PACKOUTPUTCOPY = "../../nuget";
        private const string envVarMissing = " environment variable is missing. Aborting.";

        private static class Targets
        {
            public const string RestoreTools = "restore-tools";
            public const string CleanBuildOutput = "clean-build-output";
            public const string CleanPackOutput = "clean-pack-output";
            public const string Build = "build";
            public const string CodeQL = "codeql";
            public const string Test = "test";
            public const string Pack = "pack";
            public const string SignBinary = "sign-binary";
            public const string SignPackage = "sign-package";
            public const string CopyPackOutput = "copy-pack-output";
        }

        static async Task Main(string[] args)
        {
            Target(Targets.RestoreTools, () =>
            {
                Run("dotnet", "tool restore");
            });

            Target(Targets.CleanBuildOutput, () =>
            {
                Run("dotnet", $"clean {solution} -c Release -v m --nologo");
            });

            Target(Targets.Build, DependsOn(Targets.CleanBuildOutput), () =>
		    {
			    Run("dotnet", $"build {solution} -c Release --nologo");
		    });

            Target(Targets.CodeQL, () =>
            {
                Run("dotnet", $"build {solutionCodeQL} -c Release --nologo");
            });

            Target(Targets.Test, DependsOn(Targets.Build), () =>
            {
                Run("dotnet", $"test {solution} -c Release --no-build --nologo");
            });

            Target(Targets.CleanPackOutput, () =>
            {
                if (Directory.Exists(packOutput))
                {
                    Directory.Delete(packOutput, true);
                }
            });

            Target(Targets.Pack, DependsOn(Targets.Build, Targets.CleanPackOutput), () =>
            {
                var directory = Directory.CreateDirectory(packOutput).FullName;

                Run("dotnet", $"pack \"src/main/Storage/IdentityServer4Plus.Storage.csproj\" -c Release -o \"{directory}\" --no-build --nologo");
                Run("dotnet", $"pack \"src/main/IdentityServer4Plus/IdentityServer4Plus.csproj\" -c Release -o \"{directory}\" --no-build --nologo");

                Run("dotnet", $"pack \"src/main/EntityFramework.Storage/IdentityServer4Plus.EntityFramework.Storage.csproj\" -c Release -o \"{directory}\" --no-build --nologo");
                Run("dotnet", $"pack \"src/main/EntityFramework/IdentityServer4Plus.EntityFramework.csproj\" -c Release -o \"{directory}\" --no-build --nologo");

                Run("dotnet", $"pack \"src/main/AspNetIdentity/IdentityServer4Plus.AspNetIdentity.csproj\" -c Release -o \"{directory}\" --no-build --nologo");
            });


            Target(Targets.SignPackage, DependsOn(Targets.Pack, Targets.RestoreTools), () =>
            {
                // SignNuGet();
            });

            Target("default", DependsOn(Targets.Test, Targets.Pack));
            Target("sign", DependsOn(Targets.Test, Targets.SignPackage));

            await RunTargetsAndExitAsync(args, ex => ex is SimpleExec.ExitCodeException || ex.Message.EndsWith(envVarMissing));
        }

        private static void SignNuGet()
        {
            var signClientSecret = Environment.GetEnvironmentVariable("SignClientSecret");
			var clientId = Environment.GetEnvironmentVariable("ClientId");
			var tenantId = Environment.GetEnvironmentVariable("TenantId");

            if (string.IsNullOrWhiteSpace(signClientSecret))
            {
                throw new Exception($"SignClientSecret{envVarMissing}");
            }

            foreach (var file in Directory.GetFiles(packOutput, "*.nupkg", SearchOption.AllDirectories))
            {
                Console.WriteLine($"  Signing {file}");

                Run("dotnet",
                        "NuGetKeyVaultSignTool " +
                        $"sign {file} " +
                        "--file-digest sha256 " +
                        "--timestamp-rfc3161 http://timestamp.digicert.com " +
                        "--azure-key-vault-url https://duendecodesigning.vault.azure.net/ " +
                        $"--azure-key-vault-client-id {clientId} " +
                        $"--azure-key-vault-tenant-id {tenantId} " +
                        $"--azure-key-vault-client-secret {signClientSecret} " +
                        "--azure-key-vault-certificate CodeSigning"
                        , noEcho: true);
            }
        }
    }
}
