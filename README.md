# UPTS Backend

## Important

To run the project, install the .NET SDK version 8 **AND** SDK version 9 \
And change the global.json file of each project to the version you installed \
To check the installed version, run the following command in the terminal:
```
dotnet --list-sdks
```

You can also use the following version, which is the version I installed and already has global.json configured:
```
dotnet 8.0.415
dotnet 9.0.300
```

## Running the project

1. Clone the repo
2. To run in **Development mode**, start dev-compose.yml to start the required services (Production mode is not supported yet)
```
docker-compose -f dev-compose.yml up 
```
3. You may need to generate a dev HTTPS certificate with this command
```
dotnet dev-certs https
```
4. This backend requires 2 projects to run simultaneously: API and IdentityServer. After Docker Compose has started, start each project individually
```
cd Api # or cd IdentityServer for IdentityServer
dotnet restore
dotnet run
```
5. Start your frontend project; if the frontend has OIDC configured, it should automatically redirect you to the login page. Ref: [Sample Frontend with OIDC Configured](https://github.com/SupakornSJB/ait-front-ref-with-auth#)
6. Log in to access protected frontend routes and protected API. 
