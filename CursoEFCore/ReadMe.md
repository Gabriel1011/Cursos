# Banco de dados sql server
docker run -v sqlserver_ef_core:/var/opt/mssql --name sqlserver_ef_core -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=efcore@2021' -e 'MSSQL_PID=Express' -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

# Criando primeira migração
dotnet ef migrations add PrimeiraMigracao

# Criando migração em scripts
dotnet ef migrations script -p CursoEFCore.csproj -o PrimeiraMigracao.SQL

# Criando script Idempotente para migração
dotnet ef migrations script -p CursoEFCore.csproj -o Idempotente.SQL -i

# Executando a migração do banco de dados
dotnet ef database update -v

# Migração no código indicada apenas para desenvolvimento
using var db = new Data.ApplicationContext();
db.Database.Migration();

# Nova Migração
dotnet ef migrations add AdicionarEmail

# Revertento a migração para um versão específica
dotnet ef database update PrimeiraMigracao

# Removendo uma a última migração criada
dotnet ef migrations remove

# Rollback de todas as migrações
dotnet ef database update 0