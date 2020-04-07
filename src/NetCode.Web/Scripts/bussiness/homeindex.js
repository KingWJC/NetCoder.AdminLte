(function ($) {
    $.fn.homeindex = function (options) {
        options = $.extend({}, $.fn.homeindex.defaults, options || {});
        var $menu_ul = $(this);
        var level = 0;
        if (options.data) {
            init($menu_ul, options.data, level);
        }
        else {
            if (!options.url) return;
            $.getJSON(options.url, options.param, function (data) {

                init($menu_ul, data, level);
            });
        }

        function init($menu_ul, data, level) {
            $.each(data, function (i, item) {
                //如果标签是isHeader
                var $header = $('<li class="header"></li>');
                if (item.IsHeader !== null && item.IsHeader === true) {
                    $header.append(item.text);
                    $menu_ul.append($header);
                    return;
                }

                //如果不是header
                var li = $('<li class="nav-item"></li>');

                //a标签
                var $a;
                if (level > 0) {
                    $a = $('<a style="padding-left:' + (level * 20) + 'px"></a>');
                } else {
                    $a = $('<a></a>');
                }

                //图标
                var $icon = $('<i></i>');
                $icon.addClass(item.Icon);

                //标题
                var $title = $('<p></p>');
                $title.text(item.MenuName);

                $a.append($icon);
                $a.addClass("nav-link");

                if (item.IsOpen === true) {
                    li.addClass("active");
                }
                if (item.Children && item.Children.length > 0) {
                    var pullIcon = $('<i class="right fas fa-angle-left"></i>');
                    $title.append(pullIcon);
                    $a.append($title);
                    li.append($a);
                    li.addClass("has-treeview");

                    var menus = $('<ul class="nav nav-treeview"></ul>');
                    init(menus, item.Children, level + 1);
                    li.append(menus);
                }
                else {
                    if (item.TargetType != null && item.TargetType === "blank") //代表打开新页面
                    {
                        $a.attr("href", item.url);
                        $a.attr("target", "_blank");
                    }
                    else if (item.TargetType != null && item.TargetType === "ajax") { //代表ajax方式打开页面
                        $a.attr("href", item.url);
                        $a.addClass("ajaxify");
                    }
                    else if (item.TargetType != null && item.TargetType === "iframe-tab") {
                        item.urlType = item.urlType ? item.urlType : 'relative';
                        var href = 'addTabs({id:\'' + item.id + '\',title: \'' + item.text + '\',close: true,url: \'' + item.url + '\',urlType: \'' + item.urlType + '\'});';
                        $a.attr('onclick', href);
                    }
                    else if (item.TargetType != null && item.TargetType === "iframe") { //代表单iframe页面
                        $a.attr("href", item.PageUrl);
                        $a.attr("target","iframe-main");
                        $("#iframe-main").addClass("tab_iframe");
                    } else {
                        $a.attr("href", item.PageUrl);
                        $a.addClass("iframeOpen");
                        $("#iframe-main").addClass("tab_iframe");
                    }
                    $a.append($title);
                    li.append($a);
                }
                $menu_ul.append(li);
            });
        }

        //另外绑定菜单被点击事件,做其它动作
        $menu_ul.on("click", "li.treeview a", function () {
            var $a = $(this);

            if ($a.next().size() == 0) {//如果size>0,就认为它是可以展开的
                if ($(window).width() < $.AdminLTE.options.screenSizes.sm) {//小屏幕
                    //触发左边菜单栏按钮点击事件,关闭菜单栏
                    $($.AdminLTE.options.sidebarToggleSelector).click();
                }
            }
        });
    };

    $.fn.homeindex.defaults = {
        url: null,
        param: null,
        data: null,
        isHeader: false
    };
})(jQuery);


$(function () {
    $('.nav').homeindex({ data: menus });
});
