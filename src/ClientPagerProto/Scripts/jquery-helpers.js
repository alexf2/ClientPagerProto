!(function ($) {    

    function checkVersion(ver, min, max) {
        
        if (ver === 0)
            return false;
        if (min && ver < min)
            return false;
        if (max && ver > max)
            return false;

        return true;
    }

    $.jqh = {        

        isIE: function (min, max) {
            return checkVersion($.browser.msie ? parseInt($.browser.version):0, min, max);
        },

        isChrome: function (min, max) {
            return checkVersion($.browser.chrome ? parseInt($.browser.version) : 0, min, max);
        },

        isFireFox: function (min, max) {
            return checkVersion($.browser.mozilla ? parseInt($.browser.version) : 0, min, max);
        },

        isQuirksMode: function() {
            return document.compatMode !== 'CSS1Compat';
        },

        getScrollBarSize: function () {
            var $outer = $('<div>').css({ width: '100px', height: '100px', overflow: 'scroll', position: 'absolute', top: '-999px' }).appendTo('body'),
                tmp = $('<div>').css({ width: '100%', height: '100%' }).appendTo($outer),
                w = tmp.outerWidth(),
                h = tmp.outerHeight();

            $outer.remove();

            return { width: 100 - w, height: 100 - h };
        },

        //supporting JSON.stringify for IE5
        JSON: {
            stringify: (window['JSON'] && window['JSON'].stringify ? window['JSON'].stringify : function (obj) { throw "JSON.stringify is not found"; })
        }
    };

    //fix of $(window).height() for quirks mode
    $.fn.oldHeightFunction = $.fn.height;

    $.fn.height = function () {

        if (this.is($(window)) && $.jqh.isQuirksMode()) {

            if (typeof window.innerWidth != 'undefined') 

                return window.innerHeight;            

                // IE6
            else if (typeof document.documentElement != 'undefined' && typeof document.documentElement.clientHeight != 'undefined' && document.documentElement.clientHeight != 0)
                return document.documentElement.clientHeight;
                //Older IE
            else
                return document.getElementsByTagName('body')[ 0 ].clientHeight;
            
        } else 
            return this.oldHeightFunction();
    };

})(window.jQuery);

