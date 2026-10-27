FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY . .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080
CMD ["dotnet", "run", "--project", "API_LCKR", "--no-launch-profile"]