$ErrorActionPreference = "Stop"
$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path

function Run-Step {
    param(
        [string] $Name,
        [scriptblock] $Action
    )

    try {
        & $Action

        if ($LASTEXITCODE -ne 0) {
            throw "Step '$Name' exited with code $LASTEXITCODE"
        }
    }
    catch {
        [Console]::Error.WriteLine("Step '$Name' FAILED")
        exit -1
    }
}

Run-Step "Build" {
    Set-Location (Join-Path $scriptRoot "..\..\")
    dotnet build
}

Run-Step "InstallLibs" {
    Set-Location (Join-Path $scriptRoot "..\..\")
    abp install-libs
}

Run-Step "DbMigrator" {
    Set-Location (Join-Path $scriptRoot "../../LFG")
    dotnet run --migrate-database
    dotnet run --migrate-database
}

Run-Step "DevCert" {
    Set-Location (Join-Path $scriptRoot "../../LFG")
    dotnet dev-certs https -v -ep openiddict.pfx -p de8b5206-aa93-4ca8-9b3f-0442d0984ead
}

exit 0
