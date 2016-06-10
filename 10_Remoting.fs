namespace DigitalWB.WebSharper

open WebSharper
open FsEx
open DigitalWB
open System


module EmdPath = 
    let (+&+) a b = IO.Path.Combine(a,b)

    let svgs =
        [|
            "TopViewA.svg"
            "ReflectedCeilingA.svg"
            "ElevationA.svg "
            "ReflectedCeilingB.svg"
            "TopViewB.svg"
            "ElevationB.svg "
        |]

    let baseSvgPaths (root:string) = svgs |> Array.map (fun f -> root +&+ "static\\baseSvg" +&+ f)
    let latestSvgPaths (root:string) = svgs |> Array.map (fun f -> root +&+ "static\\latestSvg" +&+ f)
    let jsonbackup (root:string) = root +&+ "static\\latestSablono.json" 

module Server =
    open EmdPath

    let root = Web.Hosting.HostingEnvironment.MapPath("~")

    //let logPath = IO.Directory.CreateDirectory(IO.Path.Combine(root,"GosLog"))
    //[<Rpc>]
    //let log tx =
    //    async {
    //        let fp = logPath.FullName +&+ ("Log_" + DateTime.nowStrLong + ".txt")
    //        IO.File.WriteAllText(fp,tx)
    //        return fp} 

    let updateEMD () =   
        
        let baseSvgs = baseSvgPaths root 
        let latestSvgs = latestSvgPaths root  
        let id = "dab2c084-c2f6-4b0d-e43f-79de6b2cf528" 
        
        let time = DateTime.UtcNow
        let json =
            try Sablono.API.getJSON "Goswinr" "Louvre16" id ignore
            with _-> IO.File.ReadAllText <| jsonbackup root
        
        let panels = EMD.Panels.parseJson time json
        Array.zip baseSvgs latestSvgs
        |> Array.iter (fun (b,l) -> EMD.SVG.saveLatestSvg id time panels b l) // paralell ?
        
        IO.File.WriteAllText( jsonbackup root,json)
        
        // save xls
        let xlsPath = root +&+ "static\\EmdPanelStatus.xlsx"
        let xlsPathDet = root +&+ "static\\EmdPanelStatusDetailed.xlsx"
        let headNames = [|"Name";"Zone";"Area sqm"|]
        let stepNames =  (Seq.head panels.Values).allStepNames 
        let heads = panels.Values |> Seq.map (fun p -> let z,a = EMD.PriorityZoneAndAreas.values.GetValue p.Name in [|p.Name ; z; sprintf "%.2f" a|])
        let steps = panels.Values |> Seq.map (fun p -> p.allSteps)
        let items = Seq.zip heads steps |> Array.ofSeq
        Sablono.Excel.save xlsPathDet true headNames stepNames items
        Sablono.Excel.save xlsPath false headNames stepNames items

    [<Rpc>]
    let updateEMDAsync () =
        async { updateEMD () } |> Async.Start

    //[<Rpc>]
    //let svgString path =
    //    async { return IO.File.ReadAllText path}

