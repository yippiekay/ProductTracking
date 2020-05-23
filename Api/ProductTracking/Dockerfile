#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ProductTracking/ProductTracking.csproj", "ProductTracking/"]
COPY ["ProductTracking.BLL/ProductTracking.BLL.csproj", "ProductTracking.BLL/"]
COPY ["ProductTracking.DAL/ProductTracking.DAL.csproj", "ProductTracking.DAL/"]
RUN dotnet restore "ProductTracking/ProductTracking.csproj"
COPY . .
WORKDIR "/src/ProductTracking"
RUN dotnet build "ProductTracking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductTracking.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductTracking.dll"]