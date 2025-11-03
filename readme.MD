# UPTS Backend

## Important

To run the project, install dotnet SDK version 8 **AND** SDK version 9 \
And change the global.json file to the version you installed \
To check the installed version, run the following command in the terminal:
```
dotnet --list-sdks
```

You can also use the following version which is the version I installed and already has global.json configured:
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
3. This backend requires 2 projects to run simuteneously: API and IdentityServer. After the project is running, start each project individually
```
cd Api # or cd IdentityServer for IdentityServer
dotnet restore
dotnet run
```
4. Start your frontend project, if the frontend has OIDC configured, it should automatically redirect you to the login page
5. Login to access protected frontend routes and protected API.