#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app
ENV CONNECTION_STRING="mongodb://localhost:27017"
ENV DB_NAME="Weather_AI"
ENV COLLECTION_NAME="Weather_AI_Data"
ENV API_URL="https://weatherapi-com.p.rapidapi.com/current.json"
ENV LOCATION_QUERY=""
ENV API_KEY=""
ENV API_HOST=""

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["asagiv.weatherai.service/asagiv.weatherai.service.csproj", "asagiv.weatherai.service/"]
RUN dotnet restore "asagiv.weatherai.service/asagiv.weatherai.service.csproj"
COPY . .
WORKDIR "/src/asagiv.weatherai.service"
RUN dotnet build "asagiv.weatherai.service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "asagiv.weatherai.service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "asagiv.weatherai.service.dll"]