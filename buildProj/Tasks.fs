
module Build.Tasks

open BlackFox.Fake
open System.IO
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.Tools.Git
open Fake.IO.Globbing.Operators
open Fake.JavaScript

let yarn = 
    if Environment.isWindows then "yarn.cmd" else "yarn"
    |> ProcessUtils.tryFindFileOnPath
    |> function
       | Some yarn -> yarn
       | ex -> failwith ( sprintf "yarn not found (%A)\n" ex )

let gitName = "sample-react-todomvc"
let gitOwner = "elmish"
let gitHome = sprintf "https://github.com/%s" gitOwner

// Filesets
let projects  =
      !! "src/**.fsproj"

let createAndGetDefault () =

    let clean = BuildTask.create "Clean" [] {
        Shell.cleanDir "build"
    }

    let install = BuildTask.create "Install" [] {
        Yarn.install id

        projects
        |> Seq.iter (fun s -> 
            let dir = Path.GetDirectoryName s
            DotNet.restore id dir
        )
    }

    let build = BuildTask.create "Build" [] {
        Npm.run "build" (fun p ->
            { p with
                NpmFilePath = yarn
            })
    }

    let watch = BuildTask.create "Watch" [build] {
        Npm.run "start" (fun p ->
            { p with
                NpmFilePath = yarn
            })
    }

    let releaseSample = BuildTask.create "ReleaseSample" [build] {
        let tempDocsDir = "temp/gh-pages"
        Shell.cleanDir tempDocsDir
        
        Repository.cloneSingleBranch "" (gitHome + "/" + gitName + ".git") "gh-pages" tempDocsDir

        Shell.copyRecursive "build" tempDocsDir true |> Trace.logfn "%A"

        Staging.stageAll tempDocsDir
        Commit.exec tempDocsDir (sprintf "Update generated sample")
        Branches.push tempDocsDir
    }

    let publish = BuildTask.createEmpty "Publish" [build; releaseSample]

    BuildTask.createEmpty "All" [build; watch]

let listAvailable() = BuildTask.listAvailable()
