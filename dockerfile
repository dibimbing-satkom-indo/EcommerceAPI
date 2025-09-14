# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy semua file project
COPY . .

# restore dependencies
RUN dotnet restore "./EcommerceAPI.csproj"

# publish output ke /app/publish
RUN dotnet publish "./EcommerceAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "EcommerceAPI.dll"]
