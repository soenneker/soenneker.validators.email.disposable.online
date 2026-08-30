[![](https://img.shields.io/nuget/v/Soenneker.Validators.Email.Disposable.Online.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Validators.Email.Disposable.Online/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.validators.email.disposable.online/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.validators.email.disposable.online/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Validators.Email.Disposable.Online.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Validators.Email.Disposable.Online/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.validators.email.disposable.online/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.validators.email.disposable.online/actions/workflows/codeql.yml)

# Soenneker.Validators.Email.Disposable.Online

Downloads a disposable-domain list once and checks email domains against it without sending email addresses to the source.

## Install

```bash
dotnet add package Soenneker.Validators.Email.Disposable.Online
```

## Registration

```csharp
using Soenneker.Validators.Email.Disposable.Online.Registrars;
using Microsoft.Extensions.DependencyInjection;

services.AddEmailDisposableOnlineValidatorAsSingleton();
```

Only singleton registration is provided. The validator owns one lazily initialized domain set and a named client entry in the shared HTTP client cache.

## Configure the list source

```json
{
  "Validators": {
    "Email": {
      "Disposable": {
        "Uri": "https://example.test/disposable-domains.json"
      }
    }
  }
}
```

The endpoint must return a JSON array of domain strings. When the setting is absent, the validator uses the `disposable/disposable-email-domains` `domains.json` file on GitHub. The URI is application configuration and should not be populated directly from request input.

## Warm up and validate

```csharp
using Soenneker.Validators.Email.Disposable.Online.Abstract;

await validator.WarmUp(cancellationToken);

bool? accepted = await validator.Validate(
    "person@example.com",
    cancellationToken);
```

Warm-up is optional; the first validation downloads the same data if needed. The download is cached for the lifetime of the validator and is not periodically refreshed.

Results have three states:

- `false`: the extracted domain is in the downloaded list;
- `true`: the domain is not listed, or the input has no extractable domain;
- `null`: the endpoint returned an empty domain list.

Domain matching is ordinal and case-insensitive. It is an exact lookup: listed parent domains do not automatically match subdomains. The utility extracts text after the last `@`; it does not validate email syntax, trim input, normalize internationalized domains, or verify deliverability. Use a syntax validator before this one when malformed input must be rejected.

## Network and failure behavior

The list request is retried three times after the initial attempt. HTTP failures, non-success responses after retries, invalid JSON, cancellation, and unexpected response shapes propagate rather than returning `null`. A `true` result means only that the domain was absent from this downloaded snapshot.

The configured list service receives only a GET for its domain data; email addresses are not sent to it. The validator no longer includes the checked address in its empty-list warning.

Disposal removes the validator's named client entry from the shared client cache and disposes the lazy domain holder. Let dependency injection dispose the singleton at application shutdown.
