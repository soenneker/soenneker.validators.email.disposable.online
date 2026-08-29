[![](https://img.shields.io/nuget/v/Soenneker.Validators.Email.Disposable.Online.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Validators.Email.Disposable.Online/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.validators.email.disposable.online/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.validators.email.disposable.online/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Validators.Email.Disposable.Online.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Validators.Email.Disposable.Online/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.validators.email.disposable.online/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.validators.email.disposable.online/actions/workflows/codeql.yml)

# Soenneker.Validators.Email.Disposable.Online

A validation module checking for disposable email addresses via online sources.

## Install

```bash
dotnet add package Soenneker.Validators.Email.Disposable.Online
```

## Quick start

```csharp
using Soenneker.Validators.Email.Disposable.Online.Registrars;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
var result = services.AddEmailDisposableOnlineValidatorAsSingleton();
```

Adds `IEmailDisposableOnlineValidator` as a singleton service.

## What you get

- `IEmailDisposableOnlineValidator` — A validation module checking for disposable email addresses via online sources.
- `EmailDisposableOnlineValidatorRegistrar` — A validation module checking for disposable email addresses.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `IEmailDisposableOnlineValidator.WarmUp(cancellationToken)` | Not necessary to call on construction of this, but makes the first validation faster. | A task that completes when warmup is complete. |
| `IEmailDisposableOnlineValidator.Validate(email, cancellationToken)` | Validates the request Basic credentials against the configured username and password hash. | Null if the online validation list cannot be reached. |
| `EmailDisposableOnlineValidatorRegistrar.AddEmailDisposableOnlineValidatorAsSingleton(services)` | Adds `IEmailDisposableOnlineValidator` as a singleton service. | The same service collection, so additional registrations can be chained. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.
- Dispose instances you own when their scope ends so held resources can be released.
