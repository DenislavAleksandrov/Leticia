include('/Scripts/ProjectScripts/html5.js');
//----jquery-plagins----
include('/Scripts/ProjectScripts/jquery-1.8.3.min.js');
include('/Scripts/ProjectScripts/jquery.ba-resize.min.js');
include('/Scripts/ProjectScripts/jquery.easing.1.3.js');
include('/Scripts/ProjectScripts/jquery.color.js');
//----transform----
include('/Scripts/ProjectScripts/jquery.transform.js');
//----SlideShow----
include('/Scripts/ProjectScripts/tms-0.4.1.js');
//----request_url----
include('/Scripts/ProjectScripts/request_url.js');
//----contact form----
include('/Scripts/ProjectScripts/cform.js');
//----Lightbox--
include('/Scripts/ProjectScripts/jquery.prettyPhoto.js');
//----jplayer-sound--
//----include('../Scripts/ProjectScripts/jquery.jplayer.min.js');
//----AjaxSwitcher----
include('/Scripts/ProjectScripts/ajax.js.switch.js');
//----All-Scripts----
include('/Scripts/ProjectScripts/script.js');
//----Include-Function----
function include(url){ 
  document.write('<script type="text/javascript" src="'+ url +'" ></script>'); 
}