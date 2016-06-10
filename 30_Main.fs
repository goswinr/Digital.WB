namespace DigitalWB.WebSharper

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Next
open WebSharper.UI.Next.Server
open FsEx.WebSharperEx

type EndPoint =
    | [<EndPoint "/">] Home
    //| [<EndPoint "/about">] About

module Templating =
    open WebSharper.UI.Next.Html

    type MainTemplate = Templating.Template<"Main.html">

    // Compute a menubar where the menu item for the given endpoint is active
    let MenuBar (ctx: Context<EndPoint>) endpoint : Doc list =
        let ( => ) txt act =
             liAttr [if endpoint = act then yield attr.``class`` "active"] [
                aAttr [attr.href (ctx.Link act)] [text txt]
             ]
        [
            li ["EMD cladding" => EndPoint.Home]
            //li ["About" => EndPoint.About]
        ]

   
    let Main ctx action title body =
        Content.Page(
            MainTemplate.Doc(
                title = title,
                menubar = MenuBar ctx action,
                body = body
            )
        )

module Site =
    open WebSharper.UI.Next.Html

    let HomePage ctx =
        //let a = EMD.latestSvgPaths ctx.RootFolder |> Seq.head 
        // let a = EmdPath.latestSvgPaths "" |> Seq.head 
        Templating.Main ctx EndPoint.Home "digital.Waagner-Biro.com" [
            divId "bigsvgs" [
                for (n,sc) in Client.svgs do 
                    yield divAttr [attr.id    <| n + "cont"
                                   attr.style <| "position: relative; height: 0; width: 100%; padding: 0; padding-bottom: "+sc+";"] 
                                  [text <| "loading " + n + "..."] :> Doc
                            ]            
            div [client <@ Client.Main() @>]    
        ]

    //let AboutPage ctx =
    //    Templating.Main ctx EndPoint.About "About" [
    //         h1 [text "About"]
    //        p [text "This is a template WebSharper client-server application."]
    //    ]

    [<Website>]
    let Main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home -> HomePage ctx
            //| EndPoint.About -> AboutPage ctx
        )
