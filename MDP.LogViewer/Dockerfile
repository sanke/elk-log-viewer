FROM node:latest as client
WORKDIR /app
COPY /mdp-logviewer-client .
RUN npm install
RUN npm run build  --prod

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MDP.LogViewer.Service/MDP.LogViewer.Service.csproj", "MDP.LogViewer.Service/"]
RUN dotnet restore "MDP.LogViewer.Service/MDP.LogViewer.Service.csproj"
COPY . .
WORKDIR "/src/MDP.LogViewer.Service"
RUN dotnet build "MDP.LogViewer.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MDP.LogViewer.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist/mdp-logviewer-client/browser ./wwwroot
ENTRYPOINT ["dotnet", "MDP.LogViewer.Service.dll"]
