﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Utils.HttpClientCache.Registrar;
using Soenneker.Utils.String.Registrars;
using Soenneker.Validators.Email.Disposable.Online.Abstract;

namespace Soenneker.Validators.Email.Disposable.Online.Registrars;

/// <summary>
/// A validation module checking for disposable email addresses
/// </summary>
public static class EmailDisposableOnlineValidatorRegistrar
{
    /// <summary>
    /// Adds <see cref="IEmailDisposableOnlineValidator"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddEmailDisposableOnlineValidatorAsSingleton(this IServiceCollection services)
    {
        services.AddHttpClientCacheAsSingleton().AddStringUtilAsSingleton().TryAddSingleton<IEmailDisposableOnlineValidator, EmailDisposableOnlineValidator>();

        return services;
    }
}