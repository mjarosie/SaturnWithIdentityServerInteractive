module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Authentication.OpenIdConnect
open FSharp.Control.Tasks.V2.ContextInsensitive

let protectedView = router {
    pipe_through ProtectedHandlers.protectedViewPipeline
    
    get "/" ProtectedHandlers.protectedHandler
}

let signOutAndRedirect (authScheme : string) (redirectUrl: string) : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let props = AuthenticationProperties()
            props.RedirectUri <- redirectUrl
            do! ctx.SignOutAsync (authScheme, props)
            return! next ctx
        }

let defaultView = router {
    get "/" (htmlView Index.layout)
    get "/index.html" (redirectTo false "/")
    get "/default.html" (redirectTo false "/")

    get "/logout" (Giraffe.Auth.signOut CookieAuthenticationDefaults.AuthenticationScheme
                    >=> signOutAndRedirect OpenIdConnectDefaults.AuthenticationScheme "index.html") // After logout - the Identity Provider will redirect the user to this page.
}

let browser = pipeline {
    plug acceptHtml
    plug putSecureBrowserHeaders
}

let browserRouter = router {
    pipe_through browser

    forward "" defaultView
    forward "/protected" protectedView
}

let appRouter = router {
    forward "" browserRouter
}