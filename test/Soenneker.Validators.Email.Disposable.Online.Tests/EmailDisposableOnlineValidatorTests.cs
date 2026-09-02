using System.Threading;
using System.Threading.Tasks;
using AwesomeAssertions;
using Soenneker.Tests.HostedUnit;
using Soenneker.Validators.Email.Disposable.Online.Abstract;

namespace Soenneker.Validators.Email.Disposable.Online.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class EmailDisposableOnlineValidatorTests : HostedUnitTest
{
    private readonly IEmailDisposableOnlineValidator _validator;

    public EmailDisposableOnlineValidatorTests(Host host) : base(host)
    {
        _validator = Resolve<IEmailDisposableOnlineValidator>();
    }

    [Test]
    public async Task Validate_on_known_temporary_should_be_false(CancellationToken cancellationToken)
    {
        bool? result = await _validator.Validate("blah@10minutemail.com", cancellationToken);
        result.Should().BeFalse();
    }

    [Test]
    public async Task Validate_on_uppercase_known_temporary_should_be_false(CancellationToken cancellationToken)
    {
        bool? result = await _validator.Validate("BLAH@10MINUTEMAIL.COM", cancellationToken);
        result.Should().BeFalse();
    }

    [Test]
    public async Task Validate_on_known_google_should_be_true(CancellationToken cancellationToken)
    {
        bool? result = await _validator.Validate("blah@gmail.com", cancellationToken);
        result.Should().BeTrue();
    }
}
