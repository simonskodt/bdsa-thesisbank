$password = New-Guid
sudo docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=y' -e "MSSQL_SA_PASSWORD=$password" -p 1433:1433 --name sqledge -d mcr.microsoft.com/azure-sql-edge
$database = "ThesisBank"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"
cd ./Server/
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ThesisBank" "$connectionString"
Set-Clipboard -Value  $connectionString
cd ../Entities/
dotnet ef migrations add InitialMigration -s ../Server/  
dotnet ef database update -s ../Server/