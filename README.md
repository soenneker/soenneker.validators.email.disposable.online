[![](https://img.shields.io/nuget/v/Soenneker.Validators.Email.Disposable.Online.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Validators.Email.Disposable.Online/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.validators.email.disposable.online/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.validators.email.disposable.online/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Validators.Email.Disposable.Online.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Validators.Email.Disposable.Online/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Validators.Email.Disposable.Online
### A validation module checking for disposable email addresses via online sources

This makes a request to grab a list of disposable email domains, and is good for scenarios where your NuGet packages may not get updated often. Otherwise, [Soenneker.Validators.Email.Disposable](https://github.com/soenneker/soenneker.validators.email.disposable) is recommended. The list is stored within the package; it's faster to load with no reliance on availability.

## Installation

```
dotnet add package Soenneker.Validators.Email.Disposable.Online
```
