using System.Threading.Tasks;
using FluentAssertions;
using Soenneker.Tests.FixturedUnit;
using Soenneker.Validators.Email.Disposable.Online.Abstract;
using Xunit;
using Xunit.Abstractions;

namespace Soenneker.Validators.Email.Disposable.Online.Tests;

[Collection("Collection")]
public class EmailDisposableOnlineValidatorTests : FixturedUnitTest
{
    private readonly IEmailDisposableOnlineValidator _validator;

    public EmailDisposableOnlineValidatorTests(Fixture fixture, ITestOutputHelper outputHelper) : base(fixture, outputHelper)
    {
        _validator = Resolve<IEmailDisposableOnlineValidator>();
    }

    [Fact]
    public async Task Validate_on_known_temporary_should_be_false()
    {
        bool? result = await _validator.Validate("blah@10minutemail.com");
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_on_known_google_should_be_true()
    {
        bool? result = await _validator.Validate("blah@gmail.com");
        result.Should().BeTrue();
    }
}