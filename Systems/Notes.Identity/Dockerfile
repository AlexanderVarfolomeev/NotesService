#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Systems/Notes.Identity/Notes.Identity.csproj", "Systems/Notes.Identity/"]
COPY ["Data/Notes.Context/Notes.Context.csproj", "Data/Notes.Context/"]
COPY ["Data/Notes.Entities/Notes.Entities.csproj", "Data/Notes.Entities/"]
COPY ["Shared/Notes.Common/Notes.Common.csproj", "Shared/Notes.Common/"]
COPY ["Services/Notes.Settings/Notes.Settings.csproj", "Services/Notes.Settings/"]
RUN dotnet restore "Systems/Notes.Identity/Notes.Identity.csproj"
COPY . .
WORKDIR "/src/Systems/Notes.Identity"
RUN dotnet build "Notes.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notes.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notes.Identity.dll"]