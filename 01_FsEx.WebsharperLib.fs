namespace FsEx

open WebSharper
open WebSharper.UI.Next
open WebSharper.UI.Next.Html
open WebSharper.UI.Next.Client
open WebSharper.JavaScript
open WebSharper.JQuery


/// * extensions for WebSharper Html
[<JavaScript>]
module WebSharperEx =
    let (|>-) = ignore

    let isNull    x =     (x = null || x = JS.Undefined )
    let isNotNull x = not (x = null || x = JS.Undefined )

    let cls classAttr = attr.``class`` classAttr
    let typ typeAttr = attr.``type`` typeAttr
    let id idAttr = attr.id idAttr

    let divCls classAttr divs = divAttr [cls classAttr ] divs 
    let divId  idAttr    divs = divAttr [id     idAttr ] divs   

    let log tx (e:Dom.Node)  = 
        if isNull e then 
            Console.Log ( sprintf "%s: is null or undefined"  tx )
        else
            let mutable astr = "*Log: "+ tx + ": <" + e.NodeName + "> Attr: "
            let ats = e.Attributes
            if isNull ats then 
                astr <- astr + "-null or undefined -"
            else    
                for i=0 to ats.Length-1 do
                    let a = ats.[i]
                    if isNotNull a then 
                        astr <- astr + a.NodeName + "=\"" + a.NodeValue + "\"; "
            Console.Log astr


    module Svg =  
    
        /// *embeds an svg  file with an <object> tag
        let svgObj filepath classAttr idAttr onLoad = 
            Tags.objectAttr [
                attr.data filepath
                cls classAttr
                id  idAttr
                typ "image/svg+xml" 
                on.afterRender onLoad
                ]
                [ text "please update your browser. it seams that it does not supports svg files"]


        [<Inline "$el.contentDocument">]
        let private contentDocument (el: Dom.Element) = X<Dom.Document>
    
        [<Inline "$el.getSVGDocument()">]
        let private getSVGDocument (el: Dom.Element) = X<Dom.Document>
    
        /// * returns the svg object that is embede inside an <object> tag
        let getSvgDoc (objNode:Dom.Element) =
            //from:  dahlström.net/svg/html/get-embedded-svg-document-script.html
            let e = contentDocument objNode
            if isNull e then 
               getSVGDocument objNode
            else
                e

        let polygonCenter (e:Dom.Element)=
            // http://stackoverflow.com/questions/10298658/mouse-position-inside-autoscaled-svg            
            let mutable x = 0.
            let mutable y = 0.
            let mutable k = 0.
            for xy in e.GetAttribute("points").Split(' ') do
                k <- k + 1.
                let xys = xy.Split ','
                x <- x + float xys.[0]
                y <- y + float xys.[1]
            x <- x/k
            y <- y/k
            x , y
                
        [<Inline "$el.getComputedTextLength()">]
        let getComputedTextLength (el: Dom.Node) = X<float>
        
        [<Inline "$el.createSVGPoint()">]
        let createSVGPoint (el: Dom.Node) = X<Dom.Element> // a svg point from a SVGElement
        
        /// *returns a [| x ; y |] of the svg coordinate space
        [<Direct """
            var pt = $svgNode.createSVGPoint();
            pt.x = $x;
            pt.y = $y;
            pt = pt.matrixTransform($svgNode.getScreenCTM().inverse());
            return [pt.x , pt.y]; """ >]
        let screenToSvg (svgNode:Dom.Node) (x:int) (y:int) = X<float[]>
            // http://stackoverflow.com/questions/20957627/how-do-you-transform-event-coordinates-to-svg-coordinates-despite-bogus-getbound 
            // http://stackoverflow.com/questions/22183727/how-do-you-convert-screen-coordinates-to-document-space-in-a-scaled-svg
            
            

