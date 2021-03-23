FROM mcr.microsoft.com/dotnet/core/sdk:3.1.302-alpine AS base
WORKDIR /app
EXPOSE 80

FROM base AS build
RUN apk add --update npm
WORKDIR /src
COPY ["ActivityCalculator/ActivityCalculator.csproj", "ActivityCalculator/"]
RUN dotnet restore "ActivityCalculator/ActivityCalculator.csproj"
COPY . .
WORKDIR "/src/ActivityCalculator"
RUN dotnet build "ActivityCalculator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ActivityCalculator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD dotnet ActivityCalculator.dll