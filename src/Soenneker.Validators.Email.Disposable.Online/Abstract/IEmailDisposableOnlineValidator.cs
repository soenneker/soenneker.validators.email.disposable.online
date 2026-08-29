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
    /// Not necessary to call on construction of this, but makes the first validation faster
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when warmup is complete.</returns>
    ValueTask WarmUp(CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates the request Basic credentials against the configured username and password hash.
    /// </summary>
    /// <param name="email">Email address to validate or query.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>Null if the online validation list cannot be reached</returns>
    ValueTask<bool?> Validate(string email, CancellationToken cancellationToken = default);
}
