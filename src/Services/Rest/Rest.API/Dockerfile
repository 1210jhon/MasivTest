#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Services/Rest/Rest.API/Rest.API.csproj", "src/Services/Rest/Rest.API/"]
COPY ["src/Crosscuting/Crosscuting.SeedWork/Crosscuting.SeedWork.csproj", "src/Crosscuting/Crosscuting.SeedWork/"]
COPY ["src/Crosscuting/Crosscuting.Common/Crosscuting.Common.csproj", "src/Crosscuting/Crosscuting.Common/"]
RUN dotnet restore "src/Services/Rest/Rest.API/Rest.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Rest/Rest.API"
RUN dotnet build "Rest.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rest.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rest.API.dll"]