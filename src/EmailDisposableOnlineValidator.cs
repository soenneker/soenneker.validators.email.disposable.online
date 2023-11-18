using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Extensions.Enumerable;
using Soenneker.Extensions.HttpResponseMessage;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Utils.HttpClientCache.Abstract;
using Soenneker.Utils.String.Abstract;
using Soenneker.Validators.Email.Disposable.Online.Abstract;

namespace Soenneker.Validators.Email.Disposable.Online;

///<inheritdoc cref="IEmailDisposableOnlineValidator"/>
public class EmailDisposableOnlineValidator : Validator.Validator, IEmailDisposableOnlineValidator
{
    private readonly AsyncSingleton<HashSet<string>?> _disposableDomains;

    private int _attempts;

    private readonly IStringUtil _stringUtil;

    public EmailDisposableOnlineValidator(ILogger<Validator.Validator> logger, IHttpClientCache httpClientCache,
        IConfiguration config, IStringUtil stringUtil) : base(logger)
    {
        _stringUtil = stringUtil;
        _disposableDomains = new AsyncSingleton<HashSet<string>?>(async () =>
        {
            // TODO: Use Polly

            if (_attempts > 1)
                return null;

            _attempts++;

            HttpClient httpClient = await httpClientCache.Get(nameof(EmailDisposableOnlineValidator));

            string? disposableJsonUri = config["Validators:Email:Disposable:Uri"];

            if (disposableJsonUri.IsNullOrEmpty())
                disposableJsonUri = "https://raw.githubusercontent.com/disposable/disposable-email-domains/master/domains.json";

            Logger.LogInformation("Getting list of disposable email domains from uri ({}) ...", disposableJsonUri);

            HttpResponseMessage message;

            try
            {
                message = await httpClient.GetAsync(disposableJsonUri);
                message.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to get the list of disposable email domains from uri ({uri})", disposableJsonUri);
                return null;
            }

            var domains = await message.To<HashSet<string>>();
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

        if (disposableDomains == null)
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

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        return _disposableDomains.DisposeAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        _disposableDomains.Dispose();
    }
}