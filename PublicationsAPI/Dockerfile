# Use the full Debian image as the base
FROM debian:12 AS base

# Install .NET dependencies
RUN apt-get update && \
    apt-get install -y wget apt-transport-https && \
    wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-8.0 aspnetcore-runtime-8.0

# Create a non-root user to run the application
WORKDIR /app

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM base AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src
COPY ["PublicationsAPI.csproj", "."]
RUN dotnet restore "./PublicationsAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./PublicationsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "./PublicationsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final

# Install AWS CLI
RUN apt-get update && \
    apt-get install -y unzip && \
    apt-get install -y curl && \
    curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip" && \
    unzip awscliv2.zip && \
    ./aws/install

WORKDIR /app
COPY --from=publish /app/publish .

# Copy the AWS configuration script
COPY configure-aws.sh /app/configure-aws.sh
RUN chmod +x /app/configure-aws.sh

# Run the AWS configuration script
RUN /app/configure-aws.sh

# Sets the correct URL and PORT for the application to start on
ENV ASPNETCORE_URLS=http://+:8080

# Configure the entrypoint
ENTRYPOINT ["dotnet", "PublicationsAPI.dll"]
