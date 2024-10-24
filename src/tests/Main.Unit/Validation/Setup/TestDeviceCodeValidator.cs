using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace IdentityServer.UnitTests.Validation.Setup
{
    public class TestDeviceCodeValidator : IDeviceCodeValidator
    {
        private readonly bool _shouldError;

        public TestDeviceCodeValidator(bool shouldError = false)
        {
            this._shouldError = shouldError;
        }

        public Task ValidateAsync(DeviceCodeValidationContext context)
        {
            if (_shouldError) context.Result = new TokenRequestValidationResult(context.Request, "error");
            else context.Result = new TokenRequestValidationResult(context.Request);

            return Task.CompletedTask;
        }
    }
}