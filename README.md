![LowkeyVault](https://raw.githubusercontent.com/nagyesta/lowkey-vault/main/.github/assets/LowkeyVault-logo-full.png)

[![GitHub license](https://img.shields.io/github/license/nagyesta/lowkey-vault-example-dotnet?color=informational)](https://raw.githubusercontent.com/nagyesta/lowkey-vault-example-dotnet/main/LICENSE)
[![.Net build](https://img.shields.io/github/actions/workflow/status/nagyesta/lowkey-vault-example-dotnet/dotnet.yml?logo=github&branch=main)](https://github.com/nagyesta/lowkey-vault-example-dotnet/actions/workflows/dotnet.yml)
[![Lowkey secure](https://img.shields.io/badge/lowkey-secure-0066CC)](https://github.com/nagyesta/lowkey-vault)

# Lowkey Vault - Example .Net

This is an example for [Lowkey Vault](https://github.com/nagyesta/lowkey-vault). It demonstrates a basic scenario where
a key is used for encrypt/decrypt operations and database connection specific credentials as well as getting a PKCS12
store with a certificate and matching private key inside.

### Points of interest

Note: In order to better understand what is needed in general to make similar examples work, please find a generic overview
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
* [Empty credentials for connecting to Lowkey Vault](dotnet-core/test/NoopCredentials.cs)
* [Tests](dotnet-core/test/Tests.cs)

### Usage

1. Start Lowkey Vault by following the steps [here](https://github.com/nagyesta/lowkey-vault#quick-start-guide).
   1. Make sure it is accessible on `https://localhost:8443`
2. Run the tests

### Note

I am not a professional .Net developer. Please do not judge me by the code quality. I am open to any suggestions and
improvements.
