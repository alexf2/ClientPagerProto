!(function ($) {
    'use strict';

    var JS_NS = 'Viking';
    var JS_PLUGIN_NAME = 'PaginalListOption';
    var PLUGIN_NAME = 'paginal_list_option';


    function myScope() {
        return '.' + JS_PLUGIN_NAME;
    }
    function scopedName(name) {
        return name + myScope();
    }

    var PaginalListOption = function (element, options) {
        this.$element = $(element);
        this.options = options;

        if (!this.options.searchBarSpace)
            this.options.searchBarSpace = this._getSearchBar().height();
        if (!this.options.pagerBarSpace)
            this.options.pagerBarSpace = this._getPagerBar().height() + (this._isIe7() ? 6 : 0);
    };

    /* --- Prototype --- */
    PaginalListOption.prototype = {

        /* --- Public --- */
        constructor: PaginalListOption,

        pluginName: PLUGIN_NAME,

        /* Returns root plugin's Html node ID */
        id: function () {
            return this.$element.attr('id');
        },

        /* Saves current configuration into related hidden input, including the page number and the selection */
        saveConfig: function () {
            this._hasConfig();
            this._getConfigHiddenElement().val($.jqh.JSON.stringify(this._config));
        },

        /* Returns current configuration wihout the data items. Is destined for using with asynchronous page requests */
        getConfiguration: function () {
            var res = {};

            for (var key in this._config)
                if (key != 'items' && this._config.hasOwnProperty(key))
                    res[key] = this._config[key];

            return res;
        },

        /* Returns overall number of records across all the pages */
        getTotalCount: function () {
            return this._config ? this._config.pagging.totalRecords : 0;
        },

        /* Returns current page items count */
        getCurrentCount: function () {
            return this._config && this._config.items ? this._config.items.length : 0;
        },

        /* Returns current page selected items count */
        getSelectedCount: function () {
            if (!this._config || !this._config.items)
                return 0;
            var res = 0;
            $.each(this._config.items, function (i, item) {
                if (item.selected)
                    ++res;
            });
            return res;
        },

        /* Returns an array of the selected items' ids */
        getSelectedIds: function () {
            this._hasConfig();
            var res = [];
            $.each(this._config.items, function (i, item) {
                if (item.selected)
                    res.push(item.value);
            });
            return res;
        },

        /* Returns selected items */
        getSelected: function () {
            this._hasConfig();
            var res = [];
            $.each(this._config.items, function (i, item) {
                if (item.selected)
                    res.push(item);
            });
            return res;
        },

        /* Finds an item's index by it's id */
        itemIndexById: function (id) {
            this._hasConfig();
            var tmp = (id || '').toLowerCase();

            for (var i = 0; i < this._config.items.length; ++i)
                if ((this._config.items[i].value || '').toLowerCase() == tmp)
                    return i;

            return -1;
        },

        /* Finds an item's index by it's description */
        itemIndexByDescr: function (id) {
            this._hasConfig();
            var tmp = (id || '').toLowerCase();

            for (var i = 0; i < this._config.items.length; ++i) {
                var d = (this._config.items[i].description || '').toLowerCase();
                if (d == tmp)
                    return i;

                var valIdx = d.lastIndexOf('(');
                if (valIdx > -1)
                    d = d.substring(0, valIdx);
                if (d == tmp)
                    return i;
            }

            return -1;
        },

        /* Selects items by one ID or by an array of ids. If no ids specified, then selects all. If searchInBoth = true, then searches in both: ids and descriptions */
        select: function (ids, searchInBoth, fireEvent) {
            var $items = this._getItems();
            var idx;
            var notfound = [];
            var changed = false;

            if (ids === undefined) {
                if (this.options.multiSelect) {
                    changed |= this._selectAll(true);
                    changed |= this._unselectExclusiveItems();
                } else {
                    changed |= this._selectAll(false);
                    changed |= this._selectOne(0, true, $items.eq(0));
                }
            } else if ($.isArray(ids)) {
                ids = this.options.multiSelect ? ids : ids.slice(0, 1);

                this._selectAll(false);
                var that = this;
                $.each(ids, function (i, itemId) {
                    idx = that.itemIndexById(itemId);
                    if (searchInBoth && idx == -1)
                        idx = that.itemIndexByDescr(itemId);

                    if (idx > -1)
                        changed |= that._selectOne(idx, true, that._uiItemByIndex($items, idx));
                    else
                        notfound.push(itemId);
                });
            } else {

                this._selectAll(false);
                idx = this.itemIndexById(ids);
                if (searchInBoth && idx == -1)
                    idx = this.itemIndexByDescr(ids);

                if (idx > -1)
                    changed |= this._selectOne(idx, true, this._uiItemByIndex($items, idx));
                else
                    notfound.push(ids);
            }

            if (fireEvent && changed)
                this._onSelectionChanged();

            return notfound;
        },

        /* Deselects items by one ID or by an array of ids. If no ids specified, then unselects all */
        unselect: function (ids, searchInBoth, fireEvent) {
            var $items = this._getItems();
            var idx;
            var changed = false;

            if (ids === undefined) {
                changed |= this._selectAll(false);
            } else if ($.isArray(ids)) {

                var that = this;
                $.each(ids, function (i, itemId) {
                    idx = that.itemIndexById(itemId);
                    if (searchInBoth && idx == -1)
                        idx = that.itemIndexByDescr(itemId);

                    if (idx > -1)
                        changed |= that._selectOne(idx, false, $items.eq(idx));
                });

            } else {
                idx = this.itemIndexById(ids);
                if (searchInBoth && idx == -1)
                    idx = this.itemIndexByDescr(ids);

                if (idx > -1)
                    changed |= this._selectOne(idx, false, $items.eq(idx));
            }

            if (fireEvent && changed)
                this._onSelectionChanged();
        },

        /* Scrolls to show current selection. If showTop is specified then shows the first selected items, otherwise determines the moving direction and shows respective part of the selection */
        ensureVisibleItems: function (showTop) {
            var i1 = this._lastSelIndex1;
            var i2 = this._lastSelIndex2;
            this._updateLastSelIndexes();

            if (this._lastSelIndex1 != null && this._lastSelIndex1 != -1 && (showTop || i1 > this._lastSelIndex1)) //moving up
                this._ensureVisibleUp(this._lastSelIndex1);
            else if (this._lastSelIndex2 != null && this._lastSelIndex2 != -1 && i2 < this._lastSelIndex2) //moving down
                this._ensureVisibleDown(this._lastSelIndex2);
        },

        /* Requests a page from the server */
        loadPage: function (pageNumber) {
            this._loadPage(pageNumber, true);
        },

        /* Clear the searching filter */
        clearFilter: function () {
            this._getSearchBar().find('.list-filter-input').val('');
            if (this._config)
                this._config.filtering.filterValue = '';
        },
        /* --- END Public --- */


        /* --- Lifecycle --- */
        _init: function () {

            this._loadConfig();
            this._updatePaggingControls();
            this._updateSearchControl();
            this._updateLayout(true);
            this._setupOption();
            this._renderData();
            this._bind();

            this._getToolbox().toolbox_mgr({
                buttonDomObj: this._getPagerBar().find('.list-pager-number')[0],
                okSelector: '.list-pager-toolbox-ok',
                cancelSelector: '.list-pager-toolbox-cancel',
                yShift: 2,
                xShift: 2
            });

            if (this.options.autoLoadPageAtInit && !this._hasDataItems(1))
                this._loadPage(this._config.pagging.currentPageNumber, true);
        },

        destroy: function () {
            this._teardown();
        },

        _setupOption: function () {
            if (this.options.multiSelect)
                this._getListContainer().attr('multiple', 'multiple');
            else
                this._getListContainer().removeAttr('multiple');
        },

        _bind: function () {

            //Ctrl + A
            var kdPrx = $.proxy(this._elemKeyUpDown, this);
            this.$element.on(scopedName('keyup'), kdPrx).on(scopedName('keydown'), kdPrx);

            //Html options
            this._getListContainer().
                on(scopedName('change'), $.proxy(this._selectionChange, this)).
                on(scopedName('keyup'), $.proxy(this._listKeyUp, this)).
                on(scopedName('keydown'), $.proxy(this._listKeyDown, this)).
                on(scopedName('mousedown'), $.proxy(this._listMouseDown, this));

            //to support autoscrolling when the mouse is outside of the list
            this._getRootContainer().
                on(scopedName('mouseenter'), $.proxy(this._containerMouseEnter, this)).
                on(scopedName('mouseleave'), $.proxy(this._containerMouseLeave, this));
            //on(scopedName('scroll'), $.proxy(this._rootScroll, this));

            //to support autoscrolling when the mouse is outside of the list
            $(document).on(scopedName('mouseup'), $.proxy(this._docMouseUp, this)).
                        on(scopedName('mousemove'), $.proxy(this._docMouseMove, this));


            //bug: https://bugs.webkit.org/show_bug.cgi?id=114384
            if ($.jqh.isChrome())
                this.$element.on(scopedName('scroll'), function (e) {
                    var $el = $(e.target);
                    $el.scrollLeft(0);
                    $el.scrollTop(0);
                });

            //pager
            var $pgBar = this._getPagerBar();
            $pgBar.find('.pager-pg-first').on(scopedName('click'), $.proxy(this._pageFirst, this));
            $pgBar.find('.pager-pg-prev').on(scopedName('click'), $.proxy(this._pagePrev, this));
            $pgBar.find('.pager-pg-last').on(scopedName('click'), $.proxy(this._pageLast, this));
            $pgBar.find('.pager-pg-next').on(scopedName('click'), $.proxy(this._pageNext, this));

            //list search
            this._getSearchBar().find('.list-filter-input').on(scopedName('keyup'), $.proxy(this._searchKeyUp, this));

            //pager dropdown toolbox
            var $tb = this._getToolbox();
            $tb.find('.list-pager-toolbox-page').on(scopedName('change') + ', ' + scopedName('input'), function (e) { $tb.find('.list-pager-toolbox-page-inp').val($(e.target).val()); });
            $tb.find('.list-pager-toolbox-size').on(scopedName('change') + ', ' + scopedName('input'), function (e) { $tb.find('.list-pager-toolbox-size-inp').text($(e.target).val()); });
            //fired by toolbox-mgr-plugin
            $tb.on(scopedName('tb-opened'), $.proxy(this._tbOpened, this)).on(scopedName('tb-closed'), $.proxy(this._tbClosed, this));
            $tb.find('.list-pager-toolbox-page-inp').on(scopedName('blur') + ', ' + scopedName('keydown') + ', ' + scopedName('input'), $.proxy(this._pgSizeBlur, this));
        },
        _unbind: function () {
            this._getListContainer().off(myScope());
            this._getPagerBar().find('.pager-pg-first, .pager-pg-prev, .pager-pg-last, .pager-pg-next').off(myScope());
            this._getToolbox().find('.list-pager-toolbox-page, .list-pager-toolbox-size, .list-pager-toolbox-page-inp').off(myScope());
            this._getToolbox().off(myScope());
            this._getSearchBar().find('.list-filter-input').off(myScope());
            $(document).off(myScope());
            this.$element.off(myScope());
        },

        _teardown: function () {
            clearTimeout(this._searchTimer);
            clearTimeout(this._loaderIconTimer);

            $.removeData(this.$element.get(0), PLUGIN_NAME);
            this.$element.removeClass(PLUGIN_NAME);
            this._getToolbox().toolbox_mgr('destroy');
            this._unbind();
            this.$element = null;
            this._config = null;
        },
        /* --- END Lifecycle --- */

        /* --- Plugin events --- */
        _onSelectionChanged: function () {
            this.ensureVisibleItems();
            this.$element.trigger('pl-changed');
        },
        /* --- END Plugin events --- */

        _searchTimer: null,
        /* --- Events ---*/

        _selectionChange: function (e) {
            var changed = false;
            changed |= this._unselectExclusiveItems();
            changed |= this._readSelectionFromUI();
            if (changed)
                this._onSelectionChanged();
        },

        _elemKeyUpDown: function (e) {
            var $trg = $(e.target);
            var changed;

            if (e.ctrlKey && e.which == 65 && !$trg.is(':text')) {
                if (this.options.multiSelect && e.type == 'keyup') {
                    changed = this._selectAllVisible(true);
                    changed |= this._unselectExclusiveItems();
                    if (changed)
                        this._onSelectionChanged();
                }
                return false;
            }
        },

        /* --- Events: keyboard ---*/
        _searchKeyUp: function (e) {
            var that = this;
            var $input = $(e.target);

            if (this._searchEnabled()) {

                clearTimeout(this._searchTimer); //messages debouncing: to wait for 'options.searchDelay' until user stops typing

                this._searchTimer = setTimeout(function () {

                    that._searchTimer = null;

                    if (that._config && !$input.is('[disabled]') && that._updateSearchValue(true).changed === true) {
                        if (that._pagingEnabled())
                            that._loadPage(1, false);
                        else
                            that._applyClientFilter(); //client filtering
                    }

                }, this.options.searchDelay);
            }

            return e.keyCode != 13;
        },

        _listKeyUp: function (e) {
            var $item = $(e.target);

            switch (e.which) {

                case 39: //right
                    break;

                case 37: //left            
                    break;

                default:
                    return true;
            }
            return false;
        },

        _listKeyDown: function (e) {

            switch (e.which) {

                case 39: //right
                    this._pageNext(e, true);
                    break;

                case 37: //left       
                    this._pagePrev(e, true);
                    break;

                default:
                    return true;
            }
            return false;
        },
        /* --- END Events: keyboard ---*/

        /* --- Events: mouse ---*/
        /* --- Mouse auto scrolling (hold down left button and drag outside the list) --- */
        _mouseHoldDown: false,
        _listMouseDown: function (e) {
            if (e.which != 1)
                return;

            this._mouseHoldDown = true;
        },

        _docMouseUp: function (e) {
            if (e.which != 1)
                return;
            this._mouseHoldDown = false;
            this._cancelAutoscroll();
        },

        _mouseXY: { x: 0, y: 0 },
        _lastMouseMoveArg: null,
        _isInside: false,
        _docMouseMove: function (e) {

            if (!this.$element.is(':visible'))
                return;

            var isInside = this._isInList(e.pageX, e.pageY);

            this._mouseXY.x = e.pageX;
            this._mouseXY.y = e.pageY;
            this._lastMouseMoveArg = e;

            if (isInside && !this._isInside) {
                this._isInside = true;
                this._containerMouseEnter(e);
            } else if (this._isInside && !isInside) {
                this._isInside = false;
                this._containerMouseLeave(e);
            }
        },

        _isInList: function (x, y) {
            var $el = this._getRootContainer();

            var offset = $el.offset();
            return offset.left <= x && offset.left + $el.outerWidth() > x &&
                   offset.top <= y && offset.top + $el.outerHeight() > y;
        },

        _containerMouseEnter: function (e) {
            this._cancelAutoscroll();
        },

        _mouseScrollTimer: null,
        _cancelAutoscroll: function () {
            if (this._mouseScrollTimer) {
                clearTimeout(this._mouseScrollTimer);
                this._mouseScrollTimer = null;
            }
        },

        /*_firstScroll: true,
        _rootScroll: function() {
            if (this._firstScroll) {
                this._firstScroll = false;
                this._getListContainer().parent().css({
                    'padding-top': '8px',
                    'padding-bottom': '10px'
                });
            }
        },*/

        //As soon as mouse with hold down left button leaves the list we start extending selection and focusing last selected item
        _containerMouseLeave: function (e) {
            if (!this._mouseHoldDown || !this.$element.is(':visible'))
                return;

            var that = this;
            var $items = this._getItems();
            var $item;
            var $cont = this._getRootContainer();

            function autoScroll() {
                that._mouseScrollTimer = null;

                if (!that._mouseHoldDown) //end of scrolling
                    return;

                var offs = $cont.offset();
                if (that._mouseXY.y <= offs.top) {
                    $item = $items.filter(':selected').first();
                    var $itemPrev = $item.prevAll().eq(0);
                    if ($itemPrev.length > 0) {
                        $itemPrev.prop('selected', true).focus();
                        if (!$.jqh.isChrome())
                            that.ensureVisibleItems();
                    }
                }
                else if (that._mouseXY.y >= offs.top + $cont.outerHeight()) {
                    $item = $items.filter(':selected').last();
                    var $itemNext = $item.nextAll().eq(0);
                    if ($itemNext.length > 0) {
                        $itemNext.prop('selected', true).focus();
                        if (!$.jqh.isChrome())
                            that.ensureVisibleItems();
                    }
                }
                that._mouseScrollTimer = setTimeout(autoScroll, 200);
            }

            this._cancelAutoscroll();
            this._mouseScrollTimer = setTimeout(autoScroll, 100);
        },

        /* --- END Events: mouse ---*/

        /* --- Events: pager ---*/
        _pageFirst: function (e, setFocus) {
            e.preventDefault();

            if (this._getPageActionEnabled(0))
                this._loadPage(1, true, setFocus);
        },
        _pagePrev: function (e, setFocus) {
            e.preventDefault();

            if (this._getPageActionEnabled(1))
                this._loadPage(this._config.pagging.currentPageNumber - 1, true, setFocus);
        },
        _pageLast: function (e, setFocus) {
            e.preventDefault();

            if (this._getPageActionEnabled(3))
                this._loadPage(this._config.pagging.totalPages, true, setFocus);
        },
        _pageNext: function (e, setFocus) {
            e.preventDefault();

            if (this._getPageActionEnabled(2))
                this._loadPage(this._config.pagging.currentPageNumber + 1, true, setFocus);
        },

        _tbOpened: function (e) {
            this._hasConfig();
            this._exchangePaggingParams(true);

            return false;
        },

        _tbClosed: function (e, okClicked) {
            if (okClicked) {
                this._hasConfig();

                this._getListContainer().focus();

                if (this._exchangePaggingParams(false))
                    this._loadPage(this._config.pagging.currentPageNumber, true, true);
            }

            return false;
        },

        _pgSizeBlur: function (e) {
            if (e.type == 'blur' || e.type == 'input' || e.which == 13) {
                var inputVal = parseInt($(e.target).val());
                if ($.isNumeric(parseInt(inputVal)) && inputVal > 0)
                    this._getToolbox().find('.list-pager-toolbox-page').val(inputVal);
            }
        },
        /* --- END Events: pager ---*/
        /* --- END Events ---*/

        _exchangePaggingParams: function (loadIntoControls) {
            var $tb = this._getToolbox();
            if (loadIntoControls) {
                $tb.find('.list-pager-toolbox-page-inp').attr({ min: Math.min(1, this._config.pagging.totalPages), max: this._config.pagging.totalPages }).val(this._config.pagging.currentPageNumber);
                $tb.find('.list-pager-toolbox-page').val(this._config.pagging.currentPageNumber).attr({ min: Math.min(1, this._config.pagging.totalPages), max: this._config.pagging.totalPages });

                $tb.find('.list-pager-toolbox-size').attr({ min: 10, max: 500, step: 10 }).val(this._config.pagging.pageSize);
                $tb.find('.list-pager-toolbox-size-inp').text(this._config.pagging.pageSize);

                var $tbPgSizeWrap = $tb.find('.list-pager-toolbox-size-wrapper');
                if (this.options.enableChangingPageSize)
                    $tbPgSizeWrap.show();
                else
                    $tbPgSizeWrap.hide();

                return true;
            } else {
                var pn = this._config.pagging.currentPageNumber, ps = this._config.pagging.pageSize;
                this._config.pagging.currentPageNumber = parseInt($tb.find('.list-pager-toolbox-page').val()) || 1;
                this._config.pagging.pageSize = parseInt($tb.find('.list-pager-toolbox-size').val()) || 10;

                return pn != this._config.pagging.currentPageNumber || ps != this._config.pagging.pageSize;
            }
        },

        /* --- Traversal ---*/
        _getConfigHiddenElement: function () {
            return this.options.configSelectorLocal ?
                this.$element.find(this.options.configFieldSelector) :
                $(this.options.configFieldSelector);
        },
        _getSearchBar: function () {
            return this.$element.find('.list-filter-container');
        },
        _getPagerBar: function () {
            return this.$element.find('.list-pager-container');
        },
        _getListContainer: function () {
            return this.$element.find('.list-page-items-select');
        },
        _getRootContainer: function () {
            return this.$element.find('.list-page-container-opt');
        },
        _getItems: function (extraSelector) {
            return this.$element.find('.list-page-items-select>option' + (extraSelector ? extraSelector : ''));
        },
        _getToolbox: function () {
            return this.$element.parent().find('.viking-toolbox');
        },
        _getIndex: function ($item) {
            return parseInt($item.attr('data-index'));
        },
        /* --- END Traversal ---*/

        /* --- Data operations ---*/
        _hasConfig: function () {
            if (!this._config)
                $.error('Configuration for plugin "' + PLUGIN_NAME + "' / " + this.id() + " hasn't been loaded");
        },

        _hasDataItems: function (minCount) {
            return this._config && this._config.items && $.isArray(this._config.items) && this._config.items.length >= minCount;
        },

        _loadConfig: function () {
            //this._config = $.parseJSON(this.$element.find(this.options.configFieldSelector).val());
            this._config = $.parseJSON(this._getConfigHiddenElement().val());
        },

        /* If the paging is disabled, then filtering is executed at client side */
        _applyClientFilter: function () {
            var that = this;
            var $root = this._getListContainer();

            var flt = this._updateSearchValue(false).actual ? this._config.filtering.filterValue : '';

            if (flt === undefined || flt === null)
                flt = '';
            if (flt != '') {
                if (this._config.filtering.FilteringMode == 2)
                    flt = '^' + flt;
                else if (this._config.filtering.FilteringMode == 3)
                    flt = flt + '$';
                flt = new RegExp(flt, this._config.filtering.filterCaseSensitive ? '' : 'i');
            }

            var changed = this._selectAllVisible(false);
            $root.empty();

            function showItem(index, item, showFlag) {
                if (showFlag) {
                    delete that._config.items[index].notVisible;
                    that._addItem(item.value, item.description, item.selected, index, $root);
                } else {
                    that._config.items[index].notVisible = true;
                }
            };

            $.each(this._config.items, function (i, item) {

                if (flt === '')
                    showItem(i, item, true);
                else {
                    var test = that._config.filtering.filterByValue ? item.value : item.description;
                    showItem(i, item, test.match(flt));
                }
            });

            if (changed)
                this._onSelectionChanged();
        },

        _renderData: function (setFocus) {
            var that = this;
            this._hasConfig();
            var $container = this._getRootContainer();
            var $root = this._getListContainer();

            function populateData() {
                $root.empty();
                that._lastSelIndex1 = that._lastSelIndex2 = null;

                if ($.isArray(that._config.items)) {

                    that._getListContainer().attr('size', Math.max(5, that._config.items.length + 1));

                    $.each(that._config.items, function (i, it) {
                        that._addItem(it.value, it.description, it.selected, i, $root);
                    });

                    if (that._config.items.length > 0) //to prevent showing a strip of v-scrollbar
                        that._getListContainer().parent().css('width', '');
                    else
                        that._getListContainer().parent().css('width', '400px');
                }
                else
                    that._getListContainer().attr('size', '5');
            }

            function adjustFocusAndVisibility() {
                that.ensureVisibleItems(true);
            }

            if (this.options.useAnimation) {

                $container.clearQueue();
                $container.stop();
                $container.fadeTo('normal', 0.2, function () {

                    populateData();

                    $container.fadeTo('slow', 1, function () {
                        adjustFocusAndVisibility();
                    });
                });
            } else {
                populateData();
                adjustFocusAndVisibility();
            }
        },

        _addItem: function (val, text, isSelected, index, $parent) {
            var tmp = $('<option />').
                attr('value', val).
                prop('selected', isSelected).
                attr('data-index', index).
                text(text);

            if ($.inArray(val, this.options.exclusiveItemIds) != -1)
                tmp.attr('data-exclusive', 1);

            tmp.appendTo($parent);
        },

        _loadPage: function (pageNumber, showLoader, setFocus) {
            var that = this;
            var shl = showLoader && this.options.enableLoaderIcon;
            this._hasConfig();

            function start(xhr) {
                that._disableAllControls(true, shl); if (shl == true) that._showLoader(true);
            }
            function complete(dataXHR, textStatus, jqXHR) {
                //setTimeout(function() {
                that._showLoader(false);
                //}, 1000); //debug timeout
            }
            function success(result, textStatus, jqXHR) {
                var data = $.parseJSON(result);

                that._config.pagging.currentPageNumber = data.pageNumber;
                that._config.pagging.totalPages = data.totalPages;
                that._config.pagging.totalRecords = data.totalRecordsCount;
                that._config.items = data.data;
                that._config.tag = data.tag;

                that._renderData(setFocus);

                that._disableAllControls(false);
                that._updatePaggingControls();
                that._updateLayout();
            }
            function failure(jqXHR, textStatus, errorThrown) {
                that._disableAllControls(false);
                that._updatePaggingControls();
                if (!jqXHR)
                    window.console && console.log(textStatus);
                else
                    window.console && console.log('Request failed - ' + textStatus + ': \'' + errorThrown + '\'' + (jqXHR.responseText ? ': ' + jqXHR.responseText : ''));
            }

            this._config.pagging.requestedPageNumber = pageNumber;

            if (this.options.pageLoader) {
                this._updateSearchValue(false); //stores filter value into the config
                this.options.pageLoader(start, complete, success, failure);
            } else {

                var xhrRequest = {
                    url: this._buildPageUrl(pageNumber),
                    dataType: "json",
                    beforeSend: start,
                    type: 'GET'
                };

                if ($.isFunction(this.options.prepareXhrPageRequest))
                    this.options.prepareXhrPageRequest(xhrRequest);

                $.ajax(xhrRequest).
                    always(complete).
                    done(success).
                    fail(failure);
            }
        },

        _loaderIconTimer: null,
        _showLoader: function (show) {
            var that = this;

            if (show) {
                clearTimeout(this._loaderIconTimer);

                this._loaderIconTimer = setTimeout(function () {
                    that._loaderIconTimer = null;

                    if (that.$element.has('.list-ajax-loader').length == 0)
                        $('<div class="list-ajax-loader" />').prependTo(that.$element);

                }, this.options.loaderIconDelay);
            } else {
                clearTimeout(this._loaderIconTimer);
                that._loaderIconTimer = null;

                this.$element.find('.list-ajax-loader').remove();
            }
        },

        _buildPageUrl: function (pageNumber) {
            if (!this._config.dataServiceUrl || typeof this._config.dataServiceUrl != 'string' || this._config.dataServiceUrl.length == 0)
                $.error('Plugin ' + PLUGIN_NAME + 'can not recognize dataServiceUrl specified in config data');

            var res = this._config.dataServiceUrl.replace(/\{page\}/i, pageNumber.toString());

            res += '?size=' + this._config.pagging.pageSize;

            if (this._sortingEnabled())
                res += '&sortByValue=' + this._config.sorting.sortByValue + '&sortOrder=' + this._config.sorting.sortingOrder;

            if (this._searchEnabled() && this._updateSearchValue(false).actual === true)
                res += '&fltByValue=' + this._config.filtering.filterByValue + '&filteringMode=' + this._config.filtering.filteringMode +
                    '&filterCaseSensitive=' + this._config.filtering.filterCaseSensitive +
                    '&fltVal=' + encodeURIComponent(this._config.filtering.filterValue);

            return res;
        },

        _updateSearchValue: function (evalOnly) {
            this._hasConfig();

            var val = this._getSearchBar().find('.list-filter-input').val().replace(/^\s+|\s+$/g, '');

            var result = {
                changed: ((this._config.filtering.filterValue || '') != val),
                actual: (val.length >= parseInt(this._config.filtering.minFilterSize))
            };
            if (!evalOnly)
                this._config.filtering.filterValue = val;

            return result;
        },

        /* --- Selection ---*/
        _uiItemByIndex: function ($items, idx) {
            return $items.filter('[data-index=' + idx + ']');
        },

        /* index - in this._config.items array; select - flag true/false;  $item - option jQuery element */
        _selectOne: function (index, select, $item) {
            if (this._config) {
                var it = this._config.items[index];
                var flag = !!select;

                if (it.selected != flag || $item.prop('selected') != flag) {
                    it.selected = flag;
                    $item.prop('selected', flag);
                    return true;
                }
            }
            return false;
        },

        _selectAll: function (select, exceptIndex) {
            return this._selectAllInternal(select, exceptIndex);
        },

        _selectAllVisible: function (select, exceptIndex) {
            return this._selectAllInternal(select, exceptIndex, true);
        },

        /* exceptIndex - in this._config.items array */
        _selectAllInternal: function (select, exceptIndex, onlyVisible) {
            if (this._config) {
                var count = 0;
                var flag = !!select;
                var $items = this._getItems();

                $.each(this._config.items, function (i, it) {
                    if (it.selected != flag && i != exceptIndex) {

                        if (it.selected != flag && (!onlyVisible || !it.notVisible)) {
                            it.selected = flag;
                            ++count;
                        }
                    }
                });

                $items.prop('selected', flag);

                return count > 0;
            }
            return false;
        },

        _unselectExclusiveItems: function () {
            var changed = false;
            var that = this;
            var $items = this._getItems(':selected');
            var $items2 = $items.filter('[data-exclusive="1"]');

            if ($items2.length > 0 && $items.length > 1) {
                changed = true;

                $.each($items, function (i, it) {
                    var $it = $(it);
                    that._selectOne(that._getIndex($it), false, $it);
                });

                var $itemExc = $items2.eq(0);
                this._selectOne(this._getIndex($itemExc), true, $itemExc);
            }

            return changed;
        },

        _readSelectionFromUI: function () {
            var count = 0;

            if (this._config) {
                var that = this;
                var $items = this._getItems();

                $.each($items, function (i, item) {
                    var $item = $(item);
                    var dataItem = that._config.items[that._getIndex($item)];

                    if (dataItem.selected != $item.prop('selected'))
                        ++count;

                    dataItem.selected = $item.prop('selected');
                });
            }

            return count > 0;
        },

        _lastSelIndex1: null,
        _lastSelIndex2: null,
        _updateLastSelIndexes: function () {
            var $items = this._getItems(':selected');
            this._lastSelIndex1 = $items.first().index();
            this._lastSelIndex2 = $items.last().index();
        },

        _ensureVisibleUp: function (index) {
            this._ensureVisibleDirected(index, false);
        },
        _ensureVisibleDown: function (index) {
            this._ensureVisibleDirected(index, true);
        },

        _isIe: null,
        _needsCalculateItems: function () {
            return this._isIe === null ? (this._isIe = $.jqh.isIE()) : this._isIe;
        },

        __isIe7: null,
        _isIe7: function () {
            return this.__isIe7 === null ? (this.__isIe7 = $.jqh.isIE(null, 7)) : this.__isIe7;
        },

        _scrollbarSize: null,
        _getScrollbarSize: function () {
            return this._scrollbarSize === null ? (this._scrollbarSize = $.jqh.getScrollBarSize()) : this._scrollbarSize;
        },

        _ensureVisibleDirected: function (index, down) {
            //window.console&& console.log((down ? 'Ensure Down: ' : 'Ensure Up: ') +index);
            if ($.isNumeric(index) && index != -1) {
                var $cont = this._getRootContainer();
                var h = $cont.innerHeight();
                var $item = this._getItems(':eq(' + index + ')');
                if ($item.length == 0)
                    return;

                var itemTop;
                var itemHeight;
                var scroll = $cont.scrollTop();
                var extra = 0;

                if (this._needsCalculateItems()) { //we manually calculate <option> position for IE
                    var $listCont = this._getListContainer();

                    if (this._getItems().length)
                        itemHeight = $listCont.innerHeight() / this._getItems().length;
                    else
                        itemHeight = $item.outerHeight() - 1;
                    itemTop = itemHeight * index;
                    if (this._isIe7())
                        extra = itemHeight;
                } else {
                    itemTop = $item.position().top;
                    itemHeight = $item.outerHeight();
                }

                if ($cont[0].scrollWidth > $cont.innerWidth()) //has H-scrollbar
                    extra += this._getScrollbarSize().height;

                var yAbsT = itemTop - scroll;
                var yAbsB = itemTop + itemHeight + extra;

                if (index == 0)
                    $cont.scrollTop(0);
                else if (down && $item.is(':last-child'))
                    $cont.scrollTop(this._getRootContainer().prop('scrollHeight') - this._getRootContainer().innerHeight());
                else if (yAbsT < 0)
                    $cont.scrollTop(scroll + yAbsT);
                else if (yAbsB - scroll > h)
                    $cont.scrollTop(down ? (yAbsB - h) : yAbsT);
            }
        },

        /* --- END Selection ---*/

        /* --- END Data operations ---*/

        /* --- Behaviour --- */
        _pagingEnabled: function () {
            this._hasConfig();
            return this._config.pagging.pageSize > 0;
        },

        _searchEnabled: function () {
            this._hasConfig();
            return this._config.filtering.filteringMode > 0;
        },

        _sortingEnabled: function () {
            this._hasConfig();
            return this._config.sorting.sortingOrder > 0;
        },
        /* --- END Behaviour --- */

        _disableAllControls: function (disable, disableSearch) {
            var $pager = this._getPagerBar().find('.pager-pg-first, .pager-pg-prev, .pager-pg-last, .pager-pg-next');
            var $search = this._getSearchBar().find('.list-filter-input');
            var $numCtl = this._getPagerBar().find('.list-pager-number');

            if (disable) {
                $pager.addClass('list-img-disabled').attr("disabled", "disabled");
                if (disableSearch)
                    $search.attr("disabled", "disabled");
                $numCtl.removeClass('list-pager-number-enabled');
            } else {
                $pager.removeClass('list-img-disabled').removeAttr("disabled");
                $search.removeAttr("disabled");
                $numCtl.addClass('list-pager-number-enabled');
            }
        },

        _updateSearchControl: function () {
            this._getSearchBar().find('.list-filter-input').val(this._searchEnabled() ? this._config.filtering.filterValue : '');
        },

        _updatePaggingControls: function () {
            this._hasConfig();
            var $bar = this._getPagerBar();

            var $p = $bar.find('.pager-pg-first, .pager-pg-prev');
            var $n = $bar.find('.pager-pg-last, .pager-pg-next');
            var $numCtl = $bar.find('.list-pager-number');
            var $num = $bar.find('.pager-pg-numbers');

            if (this._getPageActionEnabled(1))
                $p.removeClass('list-img-disabled').removeAttr("disabled");
            else
                $p.addClass('list-img-disabled').attr("disabled", "disabled");

            if (this._getPageActionEnabled(2))
                $n.removeClass('list-img-disabled').removeAttr("disabled");
            else
                $n.addClass('list-img-disabled').attr("disabled", "disabled");

            if (!this._pagingEnabled())
                $num.text('All'), $numCtl.removeClass('list-pager-number-enabled');
            else {
                var pagerTitle = this.options.pagerTemplate.replace(/\{page\}/i, this._config.pagging.currentPageNumber).
                    replace(/\{pages\}/i, this._config.pagging.totalPages).
                    replace(/\{page-size\}/i, this._config.pagging.pageSize).
                    replace(/\{records\}/i, this._config.pagging.totalRecords);

                $num.text(pagerTitle);
                $numCtl.attr('title', pagerTitle);

                $numCtl.addClass('list-pager-number-enabled');
            }
        },

        _getPageActionEnabled: function (action) {

            if (this._pagingEnabled()) {

                switch (action) {
                    case 0:
                    case 1: //first, prev
                        return this._config.pagging.currentPageNumber > 1;

                    case 2:
                    case 3: //next, last
                        return this._config.pagging.totalPages > 1 && this._config.pagging.currentPageNumber < this._config.pagging.totalPages;
                }
            }
            return false;
        },

        _pagerBarBorderWidth: null,
        _updateLayout: function (init) {
            this._hasConfig();

            var hasPagingBar = this._pagingEnabled() && (init || !this.options.autohidePagebar || this._config.pagging.totalRecords > this._config.pagging.pageSize);

            var paddingBottom = (hasPagingBar ? this.options.pagerBarSpace : 0) + (this._searchEnabled() ? this.options.searchBarSpace : 0);

            if (!this._pagerBarBorderWidth)
                this._pagerBarBorderWidth = this._getRootContainer().css('borderBottomWidth');


            if (hasPagingBar) {
                this._getPagerBar().show();
                this._getRootContainer().css('borderBottomWidth', this._pagerBarBorderWidth);
            } else {
                this._getPagerBar().hide();
                this._getRootContainer().css('borderBottomWidth', '0');
            }

            if (this._searchEnabled())
                this._getSearchBar().show();
            else
                this._getSearchBar().hide();

            this.$element.filter('.list-container').css('padding-bottom', paddingBottom + 'px');
        }
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

                instance = new PaginalListOption(this, opt);
                instance._init();

                $.data(this, PLUGIN_NAME, instance);
                $(this).addClass(PLUGIN_NAME);

                if (typeof options == 'string')
                    instance[options].apply(instance, extraParams);
            }
            return true;
        });
    };

    $.fn[PLUGIN_NAME].Constructor = PaginalListOption;

    $.fn[PLUGIN_NAME].defaults = {

        modelUniqueId: null,

        /* The CSS selector of a hidden field with JSON serialized PagedListConfigModel inside. The selector is evaluated relative to $element */
        configFieldSelector: 'input:first-child[type="hidden"]',

        /* How to locate config data: if true - inside the $element, otherwise - globally */
        configSelectorLocal: true,

        /* Height of the search bar in pixels, including the border and margins */
        searchBarSpace: 25,

        /* Height of the pager in pixels, including the border and margins */
        pagerBarSpace: 28, /*29*/

        /* Specified the selection behaviour */
        multiSelect: false,

        /* If true, then if no data passed in PagedListConfigModel the specified page will be requested from server */
        autoLoadPageAtInit: true,

        /* External function to setup AJAX page request before requesting a page */
        prepareXhrPageRequest: null,

        /*Items, which can't be selected with other items*/
        exclusiveItemIds: [],

        /* Enables/disables showing 'loading' icon and shading the list when a page is being requested */
        enableLoaderIcon: true,

        /* Delay between starting a data request and showing the progress icon and shading */
        loaderIconDelay: 400,

        /* Delay before requesting new data when a search expression is being typed */
        searchDelay: 400,

        /* An external data loader. Facilitates a complete replacing of page asynchronous requesting */
        pageLoader: null, //(start, complete, success, failure)

        /* Pager's button face text template */
        pagerTemplate: '{page} of {pages} ({page-size})',

        /* Shows/hides page-size slider in the toolbox */
        enableChangingPageSize: true,

        /* If true and pagging.totalRecords less or equal to the page size, then the paging bar is hidden */
        autohidePagebar: false,

        /* Determines whether to show transition at loading data  */
        useAnimation: true
    };
    /* --- END JQuery plugin registration API --- */


})(window.jQuery);

