module ProtectedHandlers

open Giraffe
open Saturn
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Authentication.OpenIdConnect
open FSharp.Control.Tasks.V2.ContextInsensitive
open System.Threading.Tasks

let protectedViewPipeline = pipeline {
    requires_authentication (Giraffe.Auth.challenge OpenIdConnectDefaults.AuthenticationScheme)
}

let protectedHandler : HttpHandler = fun next ctx ->
    task {
        let getUserName (): string =
            ctx.User.Claims 
            |> Seq.pick (fun claim -> if claim.Type = "given_name" then Some claim else None)
            |> (fun claim -> claim.Value)
            
        let getUserSecret (userName: string): Task<string option> =
            task {
                let! accessToken = ctx.GetTokenAsync("access_token")
                // Call an API with the accessToken...
                // ...

                // Mock:
                match userName with
                | "Alice" -> return Some "Red"
                | "Bob" ->  return Some "Blue"
                | _ ->  return None
            }

        let userNameId = getUserName()
        match! getUserSecret(userNameId) with
        | Some secret -> return! htmlView (Protected.protectedResourceView userNameId secret) next ctx
        | None -> return! htmlView (Protected.noProtectedResourceView userNameId) next ctx
    }
