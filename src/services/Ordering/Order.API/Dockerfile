#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/Ordering/Order.API/Order.API.csproj", "src/services/Ordering/Order.API/"]
COPY ["src/services/Ordering/Ordering.Application/Ordering.Application.csproj", "src/services/Ordering/Ordering.Application/"]
COPY ["src/services/Ordering/Ordering.Domain/Ordering.Domain.csproj", "src/services/Ordering/Ordering.Domain/"]
COPY ["src/services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "src/services/Ordering/Ordering.Infrastructure/"]
RUN dotnet restore "src/services/Ordering/Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/src/services/Ordering/Order.API"
RUN dotnet build "Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"]