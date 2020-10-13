module Protected

open Giraffe.GiraffeViewEngine

let protectedResourceTemplate (userIdentifier: string) (secret: string): XmlNode list =
    [
        section [_class "section"] [
            h1 [_class "title"] [rawText ("Here's your protected resource, " + userIdentifier + ":")]
            div [] [
                rawText secret
            ]
        ]
    ]

let noProtectedResourceTemplate (userIdentifier: string): XmlNode list =
    [
        section [_class "section"] [
            h1 [_class "title"] [rawText ("Hey, " + userIdentifier + "!")]
            div [] [
                rawText "You don't have any protected resources."
            ]
        ]
    ]
    
let protectedResourceView (userIdentifier: string) (secret: string): XmlNode =
    App.layout (protectedResourceTemplate userIdentifier secret)

let noProtectedResourceView (userIdentifier: string): XmlNode =
    App.layout (noProtectedResourceTemplate userIdentifier)