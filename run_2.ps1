# Use this script to generate a valid dev certificate and start the ProjectBank via docker-compose.

# Ensure certificate is present
Write-Host "CREATING DEV CERTIFICATE FOR OS $Env:OS"
dotnet dev-certs https --clean

if ($IsWindows) {
    Write-Host "WINDOWS DETECTED"
    dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p localhost
 } elseif ($IsMacOS -or $IsLinux) {
    Write-Host "MAC OR LINUX DETECTED"
    dotnet dev-certs https -ep $env:HOME/.aspnet/https/aspnetapp.pfx -p localhost
}

dotnet dev-certs https --trust
Write-Host "DONE."

# Run docker-compose
Write-Host
Write-Host "STARTING PROJECT"
docker-compose up
Write-Host "DONE."