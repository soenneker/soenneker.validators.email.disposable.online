using System;
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
    ValueTask WarmUp();

    /// <returns>Null if the online validation list cannot be reached</returns>
    ValueTask<bool?> Validate(string email);
}