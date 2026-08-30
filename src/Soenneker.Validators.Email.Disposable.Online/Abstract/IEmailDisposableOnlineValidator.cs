using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Validators.Validator.Abstract;

namespace Soenneker.Validators.Email.Disposable.Online.Abstract;

/// <summary>
/// A validation module checking for disposable email addresses via online sources
/// </summary>
public interface IEmailDisposableOnlineValidator : IValidator, IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Downloads and caches the disposable-domain list before the first validation.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when warmup is complete.</returns>
    ValueTask WarmUp(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks the email's extracted domain against the downloaded disposable-domain list.
    /// </summary>
    /// <param name="email">Email address to validate or query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><see langword="false"/> for a listed domain, <see langword="true"/> for an unlisted or unextractable domain, or <see langword="null"/> when the downloaded list is empty.</returns>
    ValueTask<bool?> Validate(string? email, CancellationToken cancellationToken = default);
}
