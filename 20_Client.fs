namespace DigitalWB.WebSharper

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.JavaScript
open WebSharper.JQuery
open FsEx.WebSharperEx

[<JavaScript>]
module Client =



    let svgs =   [|
        "TopViewA"          , "56.3%"
        "ReflectedCeilingA" , "54.0%"
        "ElevationA"        , "25.0%"
        "ReflectedCeilingB" , "52.3%"
        "TopViewB"          , "50.2%"
        "ElevationB"        , "25.0%"|]
    let svgPaths = svgs |> Array.map (fun (f,sc) ->  sc,f,"/static/latestSvg/" + f + ".svg")


    let showPanelInfo (jsvg:JQuery) =        
        let txHeight = 40.0
        let margin = 10.
        let boxHeight = txHeight + margin //* 1.3
        let offset =  15. // in rell to text height
        let svgNode = jsvg.Find("svg").Get(0) // jsvg might actually be a div     

        let tip = 
            jsvg.Find("#tooltipTx")
                .Attr("font-size", txHeight |> string)
                .Attr("font-family","Arial Black,Gadget,sans-serif")
                .Attr("text-anchor","middle")
                .Attr("fill",       "black")
        let tipBg = 
            jsvg.Find("#tooltipBg")
                .Attr("stroke",         "black")
                .Attr("fill",           "white")
                .Attr("stroke-width",   "4")
                .Attr("opacity",        "0.60")
                .Attr("rx",             margin |> string)
                .Attr("ry",             margin |> string)
                .Attr("height",         boxHeight |> string)
        let border = 
            jsvg.Find("#highlightPolygon")
                .Attr("opacity","0.70")

        jsvg.Find(".hover")
            .Children()
            .Mouseenter( fun el ev -> 
                let pts = el.GetAttribute("points")
                border.Attr("points", pts).Attr("visibility", "visible" ).Ignore 
                )
            .Mousemove( fun el ev ->
                //let x,y = Svg.polygonCenter el
                let xy = Svg.screenToSvg svgNode ev.ClientX ev.ClientY
                let x,y = xy.[0], xy.[1]
                tip.Attr( "x", x          |> string )
                    .Attr("y", y - offset - margin |> string )
                    .Attr("visibility","visible" )
                    .Text(el.GetAttribute "id")
                    .Ignore
                
                let boxLen = 2.0 * margin + (tip.Get(0) |> Svg.getComputedTextLength)                

                tipBg.Attr("width",     boxLen              |> string)
                    .Attr("x",          x - boxLen * 0.5    |> string)
                    .Attr("y",          y - boxHeight - offset |> string)
                    .Attr("visibility", "visible" )
                    .Ignore 
                )
            .Mouseout( fun el ev ->                
                tip.Attr("visibility","hidden" ).Ignore
                tipBg.Attr("visibility","hidden").Ignore
                border.Attr("visibility","hidden").Ignore
                )                
            .Ignore

        
    let Main () =
        svgPaths |> Array.iter (fun (sc, n, p) ->    
            let xr = JQuery.Get(p)            
            xr.Done (fun _ -> JQuery.Of("#"+n + "cont").Html(xr.ResponseText) |> showPanelInfo ) |> ignore
            )
        
        Server.updateEMDAsync ()    
        Elt.Empty

       
              
    (*  
    http://www.petercollingridge.co.uk/interactive-svg-components/tooltip    	    
    http://stackoverflow.com/questions/14068031/embedding-external-svg-in-html-for-javascript-manipulation
    //divCls "bigsvgs" (svgPaths |> Array.map (fun (n,path) -> Svg.svgObj path "bigsvg" n showPanelInfo :> Doc))  
    
    *)
