!(function ($) {
    'use strict';

    var JS_NS = 'Viking';
    var JS_PLUGIN_NAME = 'ToolboxMgr';
    var PLUGIN_NAME = 'toolbox_mgr';


    function myScope() {
        return '.' + JS_PLUGIN_NAME;
    }
    function scopedName(name) {
        return name + myScope();
    }

    var ToolboxMgr = function (element, options) {
        this.$element = $(element); //toolbox
        this.options = options;
        if (!this.options.buttonDomObj)
            $.error('buttonDomObj is not specified for plugin "' + PLUGIN_NAME + "' / " + this.id());
    };

    /* --- Prototype --- */
    ToolboxMgr.prototype = {

        /* --- Public --- */
        constructor: ToolboxMgr,

        pluginName: PLUGIN_NAME,

        id: function () {
            return this.$element.attr('id');
        },


        open: function () {
            var visible = this.$element.is(':visible');

            if (visible)
                return;

            this.$element.trigger('tb-opened');

            $('.list-pager-toolbox:visible').not(this.$element).each(function (i, item) {
                $(item).data(PLUGIN_NAME).close(false); //close another toolboxes
            });

            this._setPosition();

            this.$element.addClass('active');
            this._keepZIndex();
        },

        close: function (ok) {
            if (this.isOpened()) {
                this.$element.trigger('tb-closed', ok);

                this._restoreZIndex();
                this.$element.removeClass('active');
            }
        },

        isOpened: function () {
            return this.$element.is(':visible');
        },
        /* --- END Public --- */

        /* --- Lifecycle --- */
        _init: function () {

            this._bind();
        },

        destroy: function () {
            this._teardown();
        },

        _bind: function () {
            this._getOpen().on(scopedName('click'), $.proxy(this._clickOpen, this));

            var $prx = $.proxy(this._clickClose, this);
            this._getOK().bind(scopedName('click'), true, $prx);
            this._getCancel().bind(scopedName('click'), false, $prx);
            this.$element.bind(scopedName('keyup'), true, function (e) {
                if (e.which == 13) {
                    $prx(e);
                    return false;
                }
            });

            $(document).on(scopedName('click'), $.proxy(this._docClick, this)).on(scopedName('keyup'), $.proxy(this._docKeyPress, this));
        },

        _unbind: function () {
            this.$element.unbind(myScope());
            this._getOpen().off(myScope());
            this._getOK().unbind(myScope());
            this._getCancel().unbind(myScope());
            $(document).off(myScope());
        },

        _teardown: function () {
            $.removeData(this.$element.get(0), PLUGIN_NAME);
            this.$element.removeClass(PLUGIN_NAME);
            this._unbind();
            this.$element = null;
        },
        /* --- END Lifecycle --- */

        /* --- Events ---*/
        _clickOpen: function (e) {
            if (this.isOpened())
                this.close();
            else
                this.open();

            return false;
        },

        _clickClose: function (e) {
            if (this.isOpened()) {
                this.close(e.data);
                return false;
            }
        },

        _docClick: function (e) {
            if (this.isOpened() && $(e.target).closest('.list-pager-toolbox').length == 0)
                this.close(false);
        },

        _docKeyPress: function (e) {
            if (this.isOpened() && e.which == 27)
                this.close(false);
        },
        /* --- END Events ---*/

        /* --- Traversal ---*/
        _getOpen: function () {
            return $(this.options.buttonDomObj);
        },
        _getOK: function () {
            return this.$element.find(this.options.okSelector);
        },
        _getCancel: function () {
            return this.$element.find(this.options.cancelSelector);
        },
        /* --- END Traversal ---*/

        _setPosition: function () {
            if (this.options.autoAligning) {

                var $btnOpen = this._getOpen();                

                var left = $btnOpen.offset().left + this.options.xShift - $(document).scrollLeft();
                if (left < 0)
                    left = 0;                

                var top = $btnOpen.offset().top + $btnOpen.outerHeight() + this.options.yShift - $(document).scrollTop();

                if (top + this.$element.outerHeight() > $(window).height())
                    top = $(window).height() - this.$element.outerHeight();

                if (!this.options.useFixedPosition ) {
                    var offsPar = this.$element.parent().offset();
                    left -= offsPar.left - $(document).scrollLeft();
                    top -= offsPar.top - $(document).scrollTop();
                }
                
                this.$element.css({
                    left: left + 'px',
                    top: top + 'px'
                });
            }
        },

        /* --- IE7 ZIndex correction --- */
        _parentZIndex: undefined,
        _elementZIndex: undefined,

        _keepZIndex: function () {
            if (this._needsCorrectZIndex()) {
                this._elementZIndex = this.$element.css('z-index');
                this._parentZIndex = this.$element.parent().css('z-index');

                this.$element.css('z-index', 9998);
                this.$element.parent().css('z-index', 9999);
            }
        },

        _restoreZIndex: function () {

            if (this._needsCorrectZIndex()) {
                this.$element.css('z-index', this._elementZIndex);
                this.$element.parent().css('z-index', this._parentZIndex);
            }
        },

        _isIeLess8: null,
        _needsCorrectZIndex: function () {            
            return this._isIeLess8 === null ? (this._isIeLess8 = $.jqh.isIE(null, 7)) : this._isIeLess8;
        }
        /* --- END IE7 ZIndex correction --- */

    };
    /* --- END Prototype --- */

    /* --- JQuery plugin registration API --- */
    $.fn[PLUGIN_NAME] = function (options) {

        var args = $.makeArray(arguments),
            extraParams = args.slice(1);


        return this.each(function () {
            var instance = $.data(this, PLUGIN_NAME);

            if (instance) {
                if (typeof options == 'string')
                    instance[options].apply(instance, extraParams);
                else if (instance.update)
                    instance.update.apply(instance, args);
            } else {
                var opt = $.extend({}, $.fn[PLUGIN_NAME].defaults, typeof options == 'object' && options);

                instance = new ToolboxMgr(this, opt);
                instance._init();

                $.data(this, PLUGIN_NAME, instance);
                $(this).addClass(PLUGIN_NAME);

                if (typeof options == 'string')
                    instance[options].apply(instance, extraParams);
            }
            return true;
        });
    };

    $.fn[PLUGIN_NAME].Constructor = ToolboxMgr;

    $.fn[PLUGIN_NAME].defaults = {
        /* Opening button  */
        buttonDomObj: null,

        /* The selector of OK sumbitting element inside of toolbox. Is evaluated relative to $element */
        okSelector: null,

        /* The selector of Cancel sumbitting element inside of toolbox. Is evaluated relative to $element */
        cancelSelector: null,

        /* If true, then the toolbox is aligned relative to buttonDomObj each time it is shown up */
        autoAligning: true,

        /* Shift of the toolbox in pixels. Is used when autoAligning = true */
        xShift: 0,

        /* Shift of the toolbox in pixels. Is used when autoAligning = true */
        yShift: 0,

        /* Set to true if your $element has css 'position: fixed', otherwise it should have 'position: absolute' */
        useFixedPosition: false
    };
    /* --- END JQuery plugin registration API --- */


})(window.jQuery);

