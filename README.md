![LowkeyVault](https://raw.githubusercontent.com/nagyesta/lowkey-vault/main/.github/assets/LowkeyVault-logo-full.png)

[![GitHub license](https://img.shields.io/github/license/nagyesta/lowkey-vault-example-dotnet?color=informational)](https://raw.githubusercontent.com/nagyesta/lowkey-vault-example-dotnet/main/LICENSE)
[![.Net build](https://img.shields.io/github/workflow/status/nagyesta/lowkey-vault-example-dotnet/.Net?logo=github)](https://github.com/nagyesta/lowkey-vault-example-dotnet/actions/workflows/dotnet.yml)
[![Lowkey secure](https://img.shields.io/badge/lowkey-secure-0066CC)](https://github.com/nagyesta/lowkey-vault)

# Lowkey Vault - Example .Net

This is an example for [Lowkey Vault](https://github.com/nagyesta/lowkey-vault). It demonstrates a basic scenario where
a key is used for encrypt/decrypt operations and database connection specific credentials.

### Points of interest

* [Key "repository"](src/AzureKeyVaultKeyRepository.cs)
* [Secret "repository"](src/AzureKeyVaultSecretRepository.cs)
* [Empty credentials for connecting to Lowkey Vault](test/NoopCredentials.cs)
* [Tests](test/Tests.cs)

### Usage

1. Start Lowkey Vault by following the steps [here](https://github.com/nagyesta/lowkey-vault#quick-start-guide).
2. Run the tests

### Note

This is my very first .Net project after using it for 2-3 hours, please have mercy when
commenting on code quality!