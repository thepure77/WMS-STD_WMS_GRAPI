FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80


FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /src


COPY ./GRDataAccessBalance/GRDataAccessBalance.csproj ./GRDataAccessBalance/
COPY ./GRDataAccess/GRDataAccess.csproj ./GRDataAccess/
COPY ./GRBusiness/GRBusiness.csproj ./GRBusiness/
COPY ./GRAPI/GRAPI.csproj ./GRAPI/
RUN dotnet restore ./GRAPI/


COPY . .
WORKDIR /src/GRAPI
RUN dotnet build GRAPI.csproj


FROM build-env AS publish
RUN dotnet publish . -o /publish -c Release


FROM base AS final
ENV ConnectionStrings:DefaultConnection="Server=10.0.1.11\SQLEXPRESS,1433;Database=WMSDB_PO;Trusted_Connection=False;Integrated Security=False;user id=sa;password=K@sc0db12345;"
WORKDIR /app
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "GRAPI.dll"]