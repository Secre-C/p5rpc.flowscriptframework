# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/FlowscriptFrameworkTest/*" -Force -Recurse
dotnet publish "./FlowscriptFrameworkTest.csproj" -c Release -o "$env:RELOADEDIIMODS/FlowscriptFrameworkTest" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location