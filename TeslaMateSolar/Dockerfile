ARG arch=bullseye-slim

FROM mcr.microsoft.com/dotnet/runtime:7.0-${arch} AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /src
COPY ["TeslaMateSolar/TeslaMateSolar.csproj", "TeslaMateSolar/"]
RUN dotnet restore "TeslaMateSolar/TeslaMateSolar.csproj"
COPY . .
WORKDIR "/src/TeslaMateSolar"
RUN dotnet build "TeslaMateSolar.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeslaMateSolar.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeslaMateSolar.dll"]