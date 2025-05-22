# ---- build stage ----
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# copy csproj(s) and restore
COPY *.sln .
COPY RivalTranslator.Server/*.csproj ./RivalTranslator.Server/
COPY RivalTranslator.Shared/*.csproj ./RivalTranslator.Shared/
COPY RivalTranslator.Client/*.csproj ./RivalTranslator.Client/
RUN dotnet restore

# copy everything else and publish
COPY . .
RUN dotnet publish RivalTranslator.Server/RivalTranslator.Server.csproj \
    -c Release -o out

# ---- runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "RivalTranslator.Server.dll"]
