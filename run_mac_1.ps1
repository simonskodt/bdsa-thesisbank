mkdir ./.local
echo '*' > ./.local/.gitignore
$pw = New-Guid
$connectionString = "Server=db;Database=ThesisBank;User Id=sa;Password=$pw"
echo "Server=db;Database=ThesisBank;User Id=sa;Password=$pw" > ./.local/connection_string.txt
Set-Clipboard -Value "Server=localhost;Database=ThesisBank;User Id=sa;Password=$pw" 
echo $pw > ./.local/db_password.txt
cd ./Server
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ThesisBank" "$connectionString"
cd ../Entities
dotnet ef migrations add InitialMigration -s ../Server/
cd ..
