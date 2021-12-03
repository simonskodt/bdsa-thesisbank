$password = New-Guid
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
$database = "ThesisBank"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"
cd .\Server\
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ThesisBank" "$connectionString"
Set-Clipboard -Value  $connectionString
cd ..\Entities\
dotnet ef migrations add InitialMigration -s ..\Server\  
dotnet ef database update -s ..\Server\