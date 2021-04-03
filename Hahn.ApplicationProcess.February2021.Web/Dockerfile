#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_14.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_14.x | bash \
    && apt-get install nodejs -yq
WORKDIR /src
COPY ["Hahn.ApplicationProcess.February2021.Web/Hahn.ApplicationProcess.February2021.Web.csproj", "Hahn.ApplicationProcess.February2021.Web/"]
RUN dotnet restore "Hahn.ApplicationProcess.February2021.Web/Hahn.ApplicationProcess.February2021.Web.csproj"
COPY . .
WORKDIR "/src/Hahn.ApplicationProcess.February2021.Web"
RUN rm -rf wwwroot/dist
RUN rm wwwroot/index.html
RUN dotnet build "Hahn.ApplicationProcess.February2021.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hahn.ApplicationProcess.February2021.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hahn.Web.dll"]