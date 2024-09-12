# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine AS base
USER app
WORKDIR /app


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Worker/Worker.csproj", "Worker/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure.Brocker.Kafka.Consumer.Service/Infrastructure.Brocker.Kafka.Consumer.Service.csproj", "Infrastructure.Brocker.Kafka.Consumer.Service/"]
COPY ["Infrastructure.Brocker.Kafka.Producer/Infrastructure.Brocker.Kafka.Producer.csproj", "Infrastructure.Brocker.Kafka.Producer/"]
COPY ["Infrastructure.Logging.InfluexDB/Persistense.Logging.InfluxDB.csproj", "Infrastructure.Logging.InfluexDB/"]
COPY ["Infrastructure.Reader/Infrastructure.Brocker.Kafka.Consumer.csproj", "Infrastructure.Reader/"]
COPY ["Infrastructure.Sender/Infrastructure.Sender.csproj", "Infrastructure.Sender/"]
COPY ["Notificator.Persistense/Persistense.Notifications.EFCore.csproj", "Notificator.Persistense/"]
COPY ["Persistense.Notifications.Cache/Persistense.Notifications.Cache.csproj", "Persistense.Notifications.Cache/"]
RUN dotnet restore "./Worker/Worker.csproj"
COPY . .
WORKDIR "/src/Worker"
RUN dotnet build "./Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Worker.dll"]