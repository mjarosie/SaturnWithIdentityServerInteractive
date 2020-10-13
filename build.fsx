#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open System.Threading

let appPath = "./src/App/" |> Path.getFullName
let projectPath = Path.combine appPath "App.fsproj"

let identityServerPath = "./src/IdentityServer/" |> Path.getFullName
let identityServerProjectPath = Path.combine identityServerPath "IdentityServer.csproj"


Target.create "Clean" ignore

Target.create "Restore" (fun _ ->
    DotNet.restore id projectPath
    DotNet.restore id identityServerProjectPath
)

Target.create "Build" (fun _ ->
    DotNet.build id projectPath
    DotNet.build id identityServerProjectPath
)


Target.create "Run" (fun _ ->
  let identityServer = async {
    DotNet.exec (fun p -> { p with WorkingDirectory = identityServerPath } ) "watch" "run" |> ignore
  }
  let server = async {
    DotNet.exec (fun p -> { p with WorkingDirectory = appPath } ) "watch" "run" |> ignore
  }
  let browser = async {
    Thread.Sleep 5000
    Process.start (fun i -> { i with FileName = "https://localhost:8085" }) |> ignore
  }

  [ identityServer; server; browser]
  |> Async.Parallel
  |> Async.RunSynchronously
  |> ignore
)

"Clean"
  ==> "Restore"
  ==> "Build"

"Clean"
  ==> "Restore"
  ==> "Run"

Target.runOrDefault "Build"