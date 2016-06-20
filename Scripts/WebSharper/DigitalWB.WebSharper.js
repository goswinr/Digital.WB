(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,Arrays,jQuery,DigitalWB,WebSharper1,Drawings,Remoting,AjaxRemotingProvider,UI,Next,Doc,FsEx,WebSharperEx,Svg,Strings,Number,List,AttrProxy,AttrModule,Unchecked,PrintfHelpers,console;
 Runtime.Define(Global,{
  DigitalWB:{
   WebSharper:{
    Client:{
     Main:function()
     {
      Arrays.iter(function(tupledArg)
      {
       var n,xr;
       n=tupledArg[1];
       xr=jQuery.get(tupledArg[2]);
       xr.done.apply(xr,[function()
       {
        return Drawings.showPanelInfo(jQuery("#"+n+"cont").html(xr.responseText));
       }]);
       return;
      },Drawings.svgPaths());
      AjaxRemotingProvider.Send("DigitalWB.WebSharper:0",[]);
      return Doc.get_Empty();
     }
    },
    Drawings:{
     showPanelInfo:function(jsvg)
     {
      var boxHeight,svgNode,tip,tipBg,border;
      boxHeight=40+10;
      svgNode=jsvg.find("svg").get(0);
      tip=jsvg.find("#tooltipTx").attr("font-size",Global.String(40)).attr("font-family","Arial Black,Gadget,sans-serif").attr("text-anchor","middle").attr("fill","black");
      tipBg=jsvg.find("#tooltipBg").attr("stroke","black").attr("fill","white").attr("stroke-width","4").attr("opacity","0.60").attr("rx",Global.String(10)).attr("ry",Global.String(10)).attr("height",Global.String(boxHeight));
      border=jsvg.find("#highlightPolygon").attr("opacity","0.70");
      return jsvg.find(".hover").children().mouseenter(function()
      {
       var pts;
       pts=this.getAttribute("points");
       return border.attr("points",pts).attr("visibility","visible");
      }).mousemove(function(ev)
      {
       var xy,patternInput,y,x,value,boxLen,el,value1,value2;
       xy=Svg.screenToSvg(svgNode,ev.clientX,ev.clientY);
       patternInput=[Arrays.get(xy,0),Arrays.get(xy,1)];
       y=patternInput[1];
       x=patternInput[0];
       value=y-15-10;
       tip.attr("x",Global.String(x)).attr("y",Global.String(value)).attr("visibility","visible").text(this.getAttribute("id"));
       el=tip.get(0);
       boxLen=2*10+el.getComputedTextLength();
       value1=x-boxLen*0.5;
       value2=y-boxHeight-15;
       return tipBg.attr("width",Global.String(boxLen)).attr("x",Global.String(value1)).attr("y",Global.String(value2)).attr("visibility","visible");
      }).mouseout(function()
      {
       tip.attr("visibility","hidden");
       tipBg.attr("visibility","hidden");
       return border.attr("visibility","hidden");
      });
     },
     svgPaths:Runtime.Field(function()
     {
      var mapping,array;
      mapping=function(tupledArg)
      {
       var f,sc;
       f=tupledArg[0];
       sc=tupledArg[1];
       return[sc,f,"/static/latestSvg/"+f+".svg"];
      };
      array=Drawings.svgs();
      return Arrays.map(mapping,array);
     }),
     svgs:Runtime.Field(function()
     {
      return[["TopViewA","56.3%"],["ReflectedCeilingA","54.0%"],["ElevationA","25.0%"],["ReflectedCeilingB","52.3%"],["TopViewB","50.2%"],["ElevationB","25.0%"]];
     })
    },
    SablonoFrame:{
     set:function()
     {
      return jQuery("#saboDiv");
     }
    }
   }
  },
  FsEx:{
   WebSharperEx:{
    Svg:{
     getSvgDoc:function(objNode)
     {
      var e;
      e=objNode.contentDocument;
      return WebSharperEx.isNull(e)?objNode.getSVGDocument():e;
     },
     polygonCenter:function(e)
     {
      var x,y,k,arr,idx,xy,xys;
      x=0;
      y=0;
      k=0;
      arr=Strings.SplitChars(e.getAttribute("points"),[32],1);
      for(idx=0;idx<=arr.length-1;idx++){
       xy=Arrays.get(arr,idx);
       k=k+1;
       xys=Strings.SplitChars(xy,[44],1);
       x=x+Number(Arrays.get(xys,0));
       y=y+Number(Arrays.get(xys,1));
      }
      x=x/k;
      y=y/k;
      return[x,y];
     },
     screenToSvg:function($svgNode,$x,$y)
     {
      var $0=this,$this=this;
      var pt=$svgNode.createSVGPoint();
      pt.x=$x;
      pt.y=$y;
      pt=pt.matrixTransform($svgNode.getScreenCTM().inverse());
      return[pt.x,pt.y];
     },
     svgObj:function(filepath,classAttr,idAttr,onLoad)
     {
      return Doc.Element("object",List.ofArray([AttrProxy.Create("data",filepath),WebSharperEx.cls(classAttr),WebSharperEx.id(idAttr),WebSharperEx.typ("image/svg+xml"),AttrModule.OnAfterRender(onLoad)]),List.ofArray([Doc.TextNode("please update your browser. it seams that it does not supports svg files")]));
     }
    },
    cls:function(classAttr)
    {
     return AttrProxy.Create("class",classAttr);
    },
    divCls:function(classAttr,divs)
    {
     return Doc.Element("div",List.ofArray([WebSharperEx.cls(classAttr)]),divs);
    },
    divId:function(idAttr,divs)
    {
     return Doc.Element("div",List.ofArray([WebSharperEx.id(idAttr)]),divs);
    },
    id:function(idAttr)
    {
     return AttrProxy.Create("id",idAttr);
    },
    isNotNull:function(x)
    {
     return!(Unchecked.Equals(x,undefined)?true:Unchecked.Equals(x,undefined));
    },
    isNull:function(x)
    {
     return Unchecked.Equals(x,undefined)?true:Unchecked.Equals(x,undefined);
    },
    log:function(tx,e)
    {
     var _,a,astr,ats,_1,i,a1,a2;
     if(WebSharperEx.isNull(e))
      {
       a=PrintfHelpers.toSafe(tx)+": is null or undefined";
       _=console?console.log(a):undefined;
      }
     else
      {
       astr="*Log: "+tx+": <"+e.nodeName+"> Attr: ";
       ats=e.attributes;
       if(WebSharperEx.isNull(ats))
        {
         _1=astr=astr+"-null or undefined -";
        }
       else
        {
         for(i=0;i<=ats.length-1;i++){
          a1=ats.item(i);
          WebSharperEx.isNotNull(a1)?astr=astr+a1.nodeName+"=\""+a1.nodeValue+"\"; ":null;
         }
        }
       a2=astr;
       _=console?console.log(a2):undefined;
      }
     return _;
    },
    op_BarGreaterMinus:function()
    {
    },
    typ:function(typeAttr)
    {
     return AttrProxy.Create("type",typeAttr);
    }
   }
  }
 });
 Runtime.OnInit(function()
 {
  Arrays=Runtime.Safe(Global.WebSharper.Arrays);
  jQuery=Runtime.Safe(Global.jQuery);
  DigitalWB=Runtime.Safe(Global.DigitalWB);
  WebSharper1=Runtime.Safe(DigitalWB.WebSharper);
  Drawings=Runtime.Safe(WebSharper1.Drawings);
  Remoting=Runtime.Safe(Global.WebSharper.Remoting);
  AjaxRemotingProvider=Runtime.Safe(Remoting.AjaxRemotingProvider);
  UI=Runtime.Safe(Global.WebSharper.UI);
  Next=Runtime.Safe(UI.Next);
  Doc=Runtime.Safe(Next.Doc);
  FsEx=Runtime.Safe(Global.FsEx);
  WebSharperEx=Runtime.Safe(FsEx.WebSharperEx);
  Svg=Runtime.Safe(WebSharperEx.Svg);
  Strings=Runtime.Safe(Global.WebSharper.Strings);
  Number=Runtime.Safe(Global.Number);
  List=Runtime.Safe(Global.WebSharper.List);
  AttrProxy=Runtime.Safe(Next.AttrProxy);
  AttrModule=Runtime.Safe(Next.AttrModule);
  Unchecked=Runtime.Safe(Global.WebSharper.Unchecked);
  PrintfHelpers=Runtime.Safe(Global.WebSharper.PrintfHelpers);
  return console=Runtime.Safe(Global.console);
 });
 Runtime.OnLoad(function()
 {
  Drawings.svgs();
  Drawings.svgPaths();
  return;
 });
}());
