var $table = $('#dataTable');

$(function () {
    bindEvent();
});

//初始化表格
function initTable() {
    $table.bootstrapTable({
        url: '/Base_SysManage/RapidDevelopment/GetDbTableList',
        idField: 'Id',
        pagination: false,
        method: 'post',
        clickToSelect: true,
        //sidePagination: "server",
        //pageNumber: 1,
        //pageSize: 30,
        //pageList: [10, 30, 60, 100],
        columns: [
            { title: 'ck', field: 'ck', checkbox: true, width: '3%' },
            { title: '表名', field: 'TableName', width: '20%' },
            { title: '描述', field: 'Description', width: '20%' },
            {
                title: '', field: '_', width: '100%', formatter: function () {
                    return '';
                }
            }
        ],
        queryParams: function (params) {
            var searchParams = $('#searchForm').getValues();
            $.extend(params, searchParams);

            return params;
        }
    });
}

//绑定事件
function bindEvent() {
    $('#linkId').selectpicker({
        url: '/Base_SysManage/RapidDevelopment/GetAllDbLink',
        textField: 'LinkName',
        valueField: 'Id',
        pleaseSelect: false,
        onSelect: function (value) {
            $table.bootstrapTable('refresh');
        },
        onLoadSuccess: function () {
            initTable();
        }
    });

    $('#buildCode').click(function () {
        var tables = [];

        var rows = $table.bootstrapTable('getSelections');
        if (rows.length == 0) {
            dialogError('请选择需要生成的表！');
            return;
        } else {
            $.each(rows, function (index, value) {
                tables.push(value['TableName']);
            })
        }

        var linkId = $('#linkId').val();
        dialogOpen({
            id: 'form',
            height: '50%',
            title: '生成选项',
            btn: ['生成', '取消'],
            url: '/Base_SysManage/RapidDevelopment/Form?linkId={0}'.format(linkId),
            yes: function (window, body) {
                window.setTables(JSON.stringify(tables));
                window.submitForm();
            }
        });
    });
}