rmr ./Migrations

rmr ../Voting.WebApp/app.db

dotnet ef migrations add Init  --startup-project ..\Voting.WebApp\

dotnet ef database update  --startup-project ..\Voting.WebApp\