#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY . /source
WORKDIR /source
#EXPOSE 5077

RUN dotnet restore

RUN dotnet publish --configuration Release --output /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5077/tcp
ENV ASPNETCORE_URLS http://*:5077

ENTRYPOINT ["dotnet", "ThesisBank.dll"]