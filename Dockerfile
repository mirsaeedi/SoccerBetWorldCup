FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 19475
EXPOSE 44380

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY SoccerBet.csproj ./
RUN dotnet restore /SoccerBet.csproj
COPY . .
WORKDIR /src/
RUN dotnet build SoccerBet.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SoccerBet.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SoccerBet.dll"]
