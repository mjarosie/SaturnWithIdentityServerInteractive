### How to build the application

1. Make sure you have installed version of .Net SDK defined in `global.json`
2. Run `dotnet tool restore` to restore all necessary tools
3. Run `dotnet fake build -t Run` to start the application in watch mode (automatic recompilation and restart at file save)

### How to test the Client Credentials flow

The IdentityServer has 2 mock logins available:
- `alice` (password: `alice`)
- `bob` (password: `bob`)