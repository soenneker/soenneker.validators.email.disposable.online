using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Extensions.Enumerable;
using Soenneker.Extensions.HttpClient;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.String.Abstract;
using Soenneker.Validators.Email.Disposable.Online.Abstract;

namespace Soenneker.Validators.Email.Disposable.Online;

///<inheritdoc cref="IEmailDisposableOnlineValidator"/>
public class EmailDisposableOnlineValidator : Validator.Validator, IEmailDisposableOnlineValidator
{
    private readonly AsyncSingleton<HashSet<string>?> _disposableDomains;
    private readonly IStringUtil _stringUtil;
    private readonly IHttpClientCache _httpClientCache;

    public EmailDisposableOnlineValidator(ILogger<Validator.Validator> logger, IHttpClientCache httpClientCache,
        IConfiguration config, IStringUtil stringUtil) : base(logger)
    {
        _stringUtil = stringUtil;
        _httpClientCache = httpClientCache;

        _disposableDomains = new AsyncSingleton<HashSet<string>?>(async () =>
        {
            string? disposableJsonUri = config["Validators:Email:Disposable:Uri"];

            if (disposableJsonUri.IsNullOrEmpty())
                disposableJsonUri = "https://raw.githubusercontent.com/disposable/disposable-email-domains/master/domains.json";

            Logger.LogDebug("Getting list of disposable email domains from uri ({uri})...", disposableJsonUri);

            HttpClient client = await httpClientCache.Get(nameof(EmailDisposableOnlineValidator));

            var domains = await client.SendWithRetryToType<HashSet<string>>(disposableJsonUri, 3, logger: Logger);

            Logger.LogDebug("Finished retrieving list of disposable domains, count {domains}", domains?.Count);

            return domains;
        });
    }

    public async ValueTask WarmUp()
    {
        await _disposableDomains.Get();
    }

    public async ValueTask<bool?> Validate(string email)
    {
        HashSet<string>? disposableDomains = await _disposableDomains.Get();

        if (disposableDomains.IsNullOrEmpty())
        {
            Logger.LogWarning("Disposable email domains are not populated, returning true for valid for email ({email})", email);
            return null;
        }

        string? domain = _stringUtil.GetDomainFromEmail(email);

        if (domain == null)
            return true;

        if (disposableDomains.Contains(domain))
            return false;

        return true;
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _httpClientCache.Remove(nameof(EmailDisposableOnlineValidator));

        await _disposableDomains.DisposeAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _httpClientCache.RemoveSync(nameof(EmailDisposableOnlineValidator));

        _disposableDomains.Dispose();
    }
}