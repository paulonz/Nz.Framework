#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Src/Api/Auth/Nz.Api.Auth/Nz.Api.Auth.csproj", "Src/Api/Auth/Nz.Api.Auth/"]
COPY ["Src/Core/DatabaseContext/Nz.Core.DatabaseContext.Impl.Auth/Nz.Core.DatabaseContext.Impl.Auth.csproj", "Src/Core/DatabaseContext/Nz.Core.DatabaseContext.Impl.Auth/"]
COPY ["Src/Core/Model/Nz.Core.Model/Nz.Core.Model.csproj", "Src/Core/Model/Nz.Core.Model/"]
COPY ["Src/Common/Nz.Common.Helpers/Nz.Common.Helpers.csproj", "Src/Common/Nz.Common.Helpers/"]
COPY ["Src/Core/DatabaseContext/Nz.Core.DatabaseContext/Nz.Core.DatabaseContext.csproj", "Src/Core/DatabaseContext/Nz.Core.DatabaseContext/"]
COPY ["Src/Core/Model/Nz.Core.Model.Impl.Auth/Nz.Core.Model.Impl.Auth.csproj", "Src/Core/Model/Nz.Core.Model.Impl.Auth/"]
COPY ["Src/Core/Service/Nz.Core.Service.Impl.Auth/Nz.Core.Service.Impl.Auth.csproj", "Src/Core/Service/Nz.Core.Service.Impl.Auth/"]
COPY ["Src/Core/Business/Nz.Core.Business/Nz.Core.Business.csproj", "Src/Core/Business/Nz.Core.Business/"]
COPY ["Src/Libs/Encryption/Nz.Libs.Encryption/Nz.Libs.Encryption.csproj", "Src/Libs/Encryption/Nz.Libs.Encryption/"]
COPY ["Src/Core/Service/Nz.Core.Service/Nz.Core.Service.csproj", "Src/Core/Service/Nz.Core.Service/"]
COPY ["Src/Common/Nz.Common.GeneralSettings/Nz.Common.GeneralSettings.csproj", "Src/Common/Nz.Common.GeneralSettings/"]
COPY ["Src/Libs/Jwt/Nz.Libs.Jwt.Settings/Nz.Libs.Jwt.Settings.csproj", "Src/Libs/Jwt/Nz.Libs.Jwt.Settings/"]
COPY ["Src/Core/Service/Nz.Core.Service.Impl.Default/Nz.Core.Service.Impl.Default.csproj", "Src/Core/Service/Nz.Core.Service.Impl.Default/"]
COPY ["Src/Core/Business/Nz.Core.Business.Impl.Auth/Nz.Core.Business.Impl.Auth.csproj", "Src/Core/Business/Nz.Core.Business.Impl.Auth/"]
COPY ["Src/Core/UnitOfWork/Nz.Core.UnitOfWork/Nz.Core.UnitOfWork.csproj", "Src/Core/UnitOfWork/Nz.Core.UnitOfWork/"]
COPY ["Src/Libs/EmailSender/Nz.Libs.EmailSender/Nz.Libs.EmailSender.csproj", "Src/Libs/EmailSender/Nz.Libs.EmailSender/"]
COPY ["Src/Libs/MessageTemplate/Nz.Libs.MessageTemplate/Nz.Libs.MessageTemplate.csproj", "Src/Libs/MessageTemplate/Nz.Libs.MessageTemplate/"]
COPY ["Src/Core/Business/Nz.Core.Business.Impl.Default/Nz.Core.Business.Impl.Default.csproj", "Src/Core/Business/Nz.Core.Business.Impl.Default/"]
COPY ["Src/Api/Auth/Nz.Api.ViewModel.Auth/Nz.Api.ViewModel.Auth.csproj", "Src/Api/Auth/Nz.Api.ViewModel.Auth/"]
COPY ["Src/Api/Nz.Api.ViewModel/Nz.Api.ViewModel.csproj", "Src/Api/Nz.Api.ViewModel/"]
COPY ["Src/Api/Nz.Api/Nz.Api.csproj", "Src/Api/Nz.Api/"]
COPY ["Src/Core/Model/Nz.Core.Model.Impl.Jwt/Nz.Core.Model.Impl.Jwt.csproj", "Src/Core/Model/Nz.Core.Model.Impl.Jwt/"]
COPY ["Src/Libs/RestPagination/Nz.Libs.RestPagination/Nz.Libs.RestPagination.csproj", "Src/Libs/RestPagination/Nz.Libs.RestPagination/"]
COPY ["Src/Libs/Jwt/Nz.Libs.Jwt.Settings.Impl.Default/Nz.Libs.Jwt.Settings.Impl.Default.csproj", "Src/Libs/Jwt/Nz.Libs.Jwt.Settings.Impl.Default/"]
COPY ["Src/Common/Nz.Common.Helpers.Impl.Default/Nz.Common.Helpers.Impl.Default.csproj", "Src/Common/Nz.Common.Helpers.Impl.Default/"]
COPY ["Src/Common/Nz.Common.GeneralSettings.Impl.Default/Nz.Common.GeneralSettings.Impl.Default.csproj", "Src/Common/Nz.Common.GeneralSettings.Impl.Default/"]
COPY ["Src/Libs/Encryption/Nz.Libs.Encryption.Impl.HashAlgorithm/Nz.Libs.Encryption.Impl.HashAlgorithm.csproj", "Src/Libs/Encryption/Nz.Libs.Encryption.Impl.HashAlgorithm/"]
COPY ["Src/Core/UnitOfWork/Nz.Core.UnitOfWork.Impl.Postgresql/Nz.Core.UnitOfWork.Impl.Postgresql.csproj", "Src/Core/UnitOfWork/Nz.Core.UnitOfWork.Impl.Postgresql/"]
COPY ["Src/Libs/MessageTemplate/Nz.Libs.MessageTemplate.Impl.MessageResource/Nz.Libs.MessageTemplate.Impl.MessageResource.csproj", "Src/Libs/MessageTemplate/Nz.Libs.MessageTemplate.Impl.MessageResource/"]
COPY ["Src/Libs/EmailSender/Nz.Libs.EmailSender.Impl.Smtp/Nz.Libs.EmailSender.Impl.Smtp.csproj", "Src/Libs/EmailSender/Nz.Libs.EmailSender.Impl.Smtp/"]
RUN dotnet restore "Src/Api/Auth/Nz.Api.Auth/Nz.Api.Auth.csproj"
COPY . .
WORKDIR "/src/Src/Api/Auth/Nz.Api.Auth"
RUN dotnet build "Nz.Api.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nz.Api.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nz.Api.Auth.dll"]