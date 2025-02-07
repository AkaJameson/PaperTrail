FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# 设置工作目录
WORKDIR /app

COPY ./ ./API

EXPOSE 44920
EXPOSE 44921

WORKDIR /app/API

ENTRYPOINT ["dotnet", "PaperTrail.Api.dll"]