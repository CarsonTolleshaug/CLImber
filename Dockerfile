FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CLImber/CLImber.csproj", "CLImber/"]
RUN dotnet restore "CLImber/CLImber.csproj"
COPY . .
WORKDIR "/src/CLImber"
RUN dotnet build "CLImber.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CLImber.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CLImber.dll"]