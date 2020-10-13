module Index

open Giraffe.GiraffeViewEngine

let index =
    [
        section [_class "section"] [
            h1 [_class "title"] [rawText "Resources"]
            div [_class "tile"] [
                a [_class "title"; _href "/protected/" ] [rawText "See the protected resource!"]
            ]
        ]
    ]

let layout =
    App.layout index