<img width="128" src="https://github.com/chequer-io/primarsql/blob/main/Logo.png?raw=true">

# PrimarSql [![Nuget](https://img.shields.io/nuget/v/PrimarSql)](https://www.nuget.org/packages/PrimarSql/)

## Overview

.NET Core SQL parser for DynamoDB

## Example

``` csharp
var sql = "SELECT * FROM table";
var statement = PrimarSqlParser.Parse(sql);
```
