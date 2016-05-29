var isSplash = -1;
function start(){
	
};
function startF(){	
	setTimeout(function () {
		
	}, 200);
};
function showSplash(){
	setTimeout(function () {		
		
	}, 400);	
};
function hideSplash(){ 
	
};
function hideSplashQ(){
	
};

/////////////////////// ready
$(document).ready(function () {
	MSIE8 = ($.browser.msie) && ($.browser.version == 8),
	$.fn.ajaxJSSwitch({
		classMenu:"#menu",
		classSubMenu:".submenu",
		topMargin: 199,//mandatory property for decktop
		bottomMargin: 80,//mandatory property for decktop
		topMarginMobileDevices: 199,//mandatory property for mobile devices
		bottomMarginMobileDevices: 80,//mandatory property for mobile devices
		delaySubMenuHide: 300,
		fullHeight: true,
		bodyMinHeight: 700,
		menuInit:function (classMenu, classSubMenu){
			//classMenu.find(">li").each(function(){
			//	$(">a", this).append("<div class='openPart'></div>");
			//})
		},
		buttonOver: function (item) {
            $('>.over1',item).stop().animate({'marginTop':'0'},300,'easeOutCubic');            
            $('>.txt2',item).stop().animate({'marginTop':'0', 'opacity':'1'},300,'easeInOutCubic');            
            $('>.txt1',item).stop().animate({'marginTop':'-100px', 'opacity':'0'},300,'easeOutCubic');
		},
		buttonOut:function (item){
            $('>.over1',item).stop().animate({'marginTop':'-172px'},300,'easeInOutCubic');
            $('>.txt2',item).stop().animate({'marginTop':'-172px', 'opacity':'0'},300,'easeOutCubic');
            $('>.txt1',item).stop().animate({'marginTop':'0', 'opacity':'1'},300,'easeOutCubic');
		},
		subMenuButtonOver:function (item){
		},
		subMenuButtonOut:function (item){
		},
		subMenuShow:function(subMenu){        	
        	subMenu.stop(true,true).animate({"height":"show"}, 500, "easeOutCubic");
		},
		subMenuHide:function(subMenu){
        	subMenu.stop(true,true).animate({"height":"hide"}, 700, "easeOutCubic");
		},
		pageInit:function (pages){
			//console.log('pageInit');
		},
		currPageAnimate:function (page){
			//console.log('currPageAnimate');
			var Delay=400; // default
			if(isSplash==-1){ // on reload				
				hideSplashQ();
				Delay=0;				
			}
			if(isSplash==0){ // on first time click				
				hideSplash();
				Delay=800;
			}
			isSplash = 2;
			
			// animation of current page
			page.css({'opacity':'0', "top":-$(window).height()}).stop(true).delay(Delay).animate({'opacity':'1',"top":'0'}, 1000, "easeOutCubic", function (){
					$(window).trigger('resize');
			});    	
		},
		prevPageAnimate:function (page){
			//console.log('prevPageAnimate');
			page.stop(true).animate({"display":"block",'opacity':'0', "top":$(window).height()}, 500, "easeInCubic");
		},
		backToSplash:function (){
			//console.log('backToSplash');
			if(isSplash==-1){
				isSplash = 0;
				startF();				
			}
			else{
				isSplash = 0;				
				showSplash();
			};
			$(window).trigger('resize');			      
		},
		pageLoadComplete:function (){
			//console.log('pageLoadComplete');            
    }
	});  /// ajaxJSSwitch end

	
});

