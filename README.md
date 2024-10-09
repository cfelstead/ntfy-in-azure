# Running NTFY in Azure

## What is in this repository

- **src/AzureNtfyApp**
  - The application inside the container. This consists of the NTFY application (linux) and the .NET console application wrapping the NTFY binary.
- **vol**
  - The contents of the volume to be mounted to be mounted with the container. This allows easy access to the manipulate the NTFY configuration and the auth.db
- **base-container**
  - Contains a docker compose file that boots the base NTFY image. This is useful to allow you to initially config an auth.db

## How does the .NET application work

It is simple. We are using [CliWrap](https://github.com/Tyrrrz/CliWrap) to launch NTFY with the arguments we desire. In this case, it is simply the configuration file that is hosted in the mounted drive for easy access. Should the NTFY app fail to launch, the output will be captured and printed to the screen.

## Key points of configuration

The listening port has been changed to 8080. The default port (80) will be blocked on Azure, so this change is necessary.

We are pointing cache-file and auth-file to the mounted drive so that they can be easily accessed once the container is deployed.

The default-host and base-url will need to match the URL of your deployment in order for push notifications to work correctly.

## Construction / Deployment steps

### Pre-requisits

Make sure you have Docker installed and working on your machine.

### If you don't have an auth.db

Open a terminal and navigate to the 'base-container' folder. Run `docker compose up`.

When the container is running, you can access the terminal and run the `ntfy user` commands in order to configure the auth.db.

When completed, you can extract the auth.db for usage later once deployed. The auth.db is a SQL Lite database if you want to edit the database later.

### Azure Resources to create

- A resource group containing
 - A container registry
 - A storage account with File shares

### Deployment

Inside Visual Studio
- Build the .NET application
- Build the Docker Image

In the terminal
- Tag the docker image for your Azure Container Registry
	- `docker tag {imageName}:latest {containerRegistryName}/{imageName}:{someTag}`
- Login to Azure
	- `az login`
- Login to the Container Registry
	- `az acr login -n {containerRegistryName}`
- Push the image to the registry
	- `docker push {containerRegistryName}/{imageName}:{someTag}`

- Run the container instance creation command
	- `az container create --resource-group {resourceGroupName} --name {containerInstanceName} --image {containerRegistryName}/{imageName}:{someTag} --dns-name-label {dnsName} --ports 8080 --ip-address public --location {uksouth} --memory 1 --no-wait --registry-login-server {containerRegistryServer} --registry-username {containerRegistryUsername} --registry-password {containerRegistryPassword} --azure-file-volume-account-key {storageAcconntKey} --azure-file-volume-account-name {storageAccountName} --azure-file-volume-mount-path /app/ntfy-vol --azure-file-volume-share-name {storageAccountShareName}`

