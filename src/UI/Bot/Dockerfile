FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UI/Bot/Bot.csproj", "src/UI/Bot/"]
COPY ["src/Infrastructure/Discord/Discord.csproj", "src/Infrastructure/Discord/"]
COPY ["src/Infrastructure/Persistence/Persistence.csproj", "src/Infrastructure/Persistence/"]
COPY ["src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
RUN dotnet restore "src/UI/Bot/Bot.csproj"
COPY . .
WORKDIR "/src/src/UI/Bot"
RUN dotnet build "Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiscoGroupie.UI.Bot.dll"]
