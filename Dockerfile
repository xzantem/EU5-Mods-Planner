FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Eu5ModPlanner.csproj", "./"]
RUN dotnet restore "Eu5ModPlanner.csproj"

COPY . .
RUN dotnet publish "Eu5ModPlanner.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:5068
EXPOSE 5068

ENTRYPOINT ["dotnet", "Eu5ModPlanner.dll"]
