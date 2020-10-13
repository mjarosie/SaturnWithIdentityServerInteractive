module Server

open Saturn
open Microsoft.AspNetCore.Authentication

let app = application {

    use_open_id_auth_with_config (fun (options: OpenIdConnect.OpenIdConnectOptions) ->
        options.Authority <- "https://localhost:5001";

        options.ClientId <- "interactive";
        options.ClientSecret <- "secret";
        options.ResponseType <- "code";

        options.SaveTokens <- true;
        
        // Say our Client wants to call the API which requires `scope2` scope.
        options.Scope.Add("scope2")

        // Following options are so that our app can have access to the user's profile data, such as name, website etc.
        // See: https://identityserver4.readthedocs.io/en/latest/quickstarts/2_interactive_aspnetcore.html#getting-claims-from-the-userinfo-endpoint
        options.Scope.Add("profile")
        options.GetClaimsFromUserInfoEndpoint <- true;
    )

    use_router Router.appRouter
    url "https://0.0.0.0:8085/"
}

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code