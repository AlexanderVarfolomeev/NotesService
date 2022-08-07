#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Systems/Notes.Api/Notes.Api.csproj", "Systems/Notes.Api/"]
COPY ["Services/Notes.TaskTypeService/Notes.TaskTypeService.csproj", "Services/Notes.TaskTypeService/"]
COPY ["Services/Notes.NotesService/Notes.NotesService.csproj", "Services/Notes.NotesService/"]
COPY ["Data/Notes.Context/Notes.Context.csproj", "Data/Notes.Context/"]
COPY ["Data/Notes.Entities/Notes.Entities.csproj", "Data/Notes.Entities/"]
COPY ["Shared/Notes.Common/Notes.Common.csproj", "Shared/Notes.Common/"]
COPY ["Services/Notes.Settings/Notes.Settings.csproj", "Services/Notes.Settings/"]
COPY ["Services/Notes.ColorService/Notes.ColorService.csproj", "Services/Notes.ColorService/"]
RUN dotnet restore "Systems/Notes.Api/Notes.Api.csproj"
COPY . .
WORKDIR "/src/Systems/Notes.Api"
RUN dotnet build "Notes.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notes.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notes.Api.dll"]