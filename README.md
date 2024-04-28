![LowkeyVault](https://raw.githubusercontent.com/nagyesta/lowkey-vault/main/.github/assets/LowkeyVault-logo-full.png)

[![GitHub license](https://img.shields.io/github/license/nagyesta/lowkey-vault-example-dotnet?color=informational)](https://raw.githubusercontent.com/nagyesta/lowkey-vault-example-dotnet/main/LICENSE)
[![.Net build](https://img.shields.io/github/actions/workflow/status/nagyesta/lowkey-vault-example-dotnet/dotnet.yml?logo=github&branch=main)](https://github.com/nagyesta/lowkey-vault-example-dotnet/actions/workflows/dotnet.yml)
[![Lowkey secure](https://img.shields.io/badge/lowkey-secure-0066CC)](https://github.com/nagyesta/lowkey-vault)

# Lowkey Vault - Example .Net

This is an example for [Lowkey Vault](https://github.com/nagyesta/lowkey-vault). It demonstrates a basic scenario where
a key is used for encrypt/decrypt operations and database connection specific credentials as well as getting a PKCS12
store with a certificate and matching private key inside.

### Points of interest

> [!NOTE]
> In order to better understand what is needed in general to make similar examples work, please find a generic overview
[here](https://github.com/nagyesta/lowkey-vault/wiki/Example:-How-can-you-use-Lowkey-Vault-in-your-tests).

#### .NET Framework

* [Key "repository"](dotnet-framework/src/AzureKeyVaultKeyRepository.cs)
* [Secret "repository"](dotnet-framework/src/AzureKeyVaultSecretRepository.cs)
* [Certificate "repository"](dotnet-framework/src/AzureKeyVaultCertificateRepository.cs)
* [Empty credentials for connecting to Lowkey Vault](dotnet-framework/test/NoopCredentials.cs)
* [Tests](dotnet-framework/test/Tests.cs)

#### .NET Core

* [Key "repository"](dotnet-core/src/AzureKeyVaultKeyRepository.cs)
* [Secret "repository"](dotnet-core/src/AzureKeyVaultSecretRepository.cs)
* [Certificate "repository"](dotnet-core/src/AzureKeyVaultCertificateRepository.cs)
* [Empty credentials for connecting to Lowkey Vault](dotnet-core/test/NoopCredentials.cs) (Not needed when using Assumed Identity container)
* [Docker Compose](docker-compose.yml) to allow easy testing locally
* Tests
  * [Tests using the empty credentials](dotnet-core/test/Tests.cs)
  * [Tests using the DefaultAzureCredential](dotnet-core/test/ManagedIdentityTests.cs)

### Usage

1. Start Lowkey Vault by following the steps [here](https://github.com/nagyesta/lowkey-vault#quick-start-guide).
   1. Make sure it is accessible on `https://localhost:8443`
   2. If you want to use DefaultAzureCredential
      1. start [Assumed Identity](https://github.com/nagyesta/assumed-identity)
      2. in the [Managed Identity tests](dotnet-core/test/ManagedIdentityTests.cs), make sure to:
         1. Set ```IDENTITY_ENDPOINT``` environment variable to point to the `/metadata/identity/oauth2/token` path of Assumed Identity e.g., http://localhost:8080/metadata/identity/oauth2/token
         2. Set ```IDENTITY_HEADER``` environment variable to anything (just needs to exist) e.g., `header`
2. Run the tests

> [!TIP]
> Since v2.4.2, Lowkey Vault is providing the same token endpoint on the `8080` port by default. Therefore, you don't need to start another container.
