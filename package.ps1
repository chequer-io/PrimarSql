dotnet build PrimarSql -c Release --no-incremental
dotnet pack PrimarSql -c Release -p:Packaging=true -o build
