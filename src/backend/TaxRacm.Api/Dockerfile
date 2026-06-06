FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TaxRacm.Api/TaxRacm.Api.csproj", "TaxRacm.Api/"]
COPY ["TaxRacm.SharedKernel/TaxRacm.SharedKernel.csproj", "TaxRacm.SharedKernel/"]
COPY ["TaxRacm.Clients/TaxRacm.Clients.csproj", "TaxRacm.Clients/"]
COPY ["TaxRacm.Risks/TaxRacm.Risks.csproj", "TaxRacm.Risks/"]
COPY ["TaxRacm.Controls/TaxRacm.Controls.csproj", "TaxRacm.Controls/"]
COPY ["TaxRacm.Intelligence/TaxRacm.Intelligence.csproj", "TaxRacm.Intelligence/"]
RUN dotnet restore "TaxRacm.Api/TaxRacm.Api.csproj"
COPY . .
WORKDIR "/src/TaxRacm.Api"
RUN dotnet build "TaxRacm.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaxRacm.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaxRacm.Api.dll"]
