var $table = $('#dataTable');

$(function () {
    initTable();
});

//初始化表格
function initTable() {
    $table.bootstrapTable({
        url: '/Base_SysManage/DatabaseLink/GetDataList',
        idField: 'Id',
        method: 'post',
        contentType: 'application/x-www-form-urlencoded',
        clickToSelect: false,
        pagination: false,
        //sidePagination: "server",
        //queryParamsType: '',
        //pageNumber: 1,
        //pageSize: 30,
        //pageList: [10, 30, 60, 100],
        columns: [
            { title: 'ck', field: 'ck', checkbox: true, width: '3%' },
            { title: '连接名', field: 'LinkName', width: '10%' },
            { title: '连接字符串', field: 'ConnectionStr', width: '50%' },
            { title: '数据库类型', field: 'DbType', width: '10%' },
            {
                title: '操作', field: '_', width: '27%', formatter: function (value, row) {
                    var builder = new BtnBuilder();
                    builder.AddBtn({ icon: 'glyphicon-edit', function: 'openForm', param: [row['Id']] });
                    builder.AddBtn({ icon: 'glyphicon-trash', function: 'deleteData', param: [row['Id']] });

                    return builder.build();
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

function refreshTable() {
    var $searchForm = $('#search_form').on('submit', function () {
        $dt.DataTable().searchEx({}).draw();
        return false;
    });

    var $dt = $('#dataTable').DataTable({
        columns: [
            { title: 'ck', cellType: 'checkbox', width: '3%' },
            { title: '连接名', data: 'LinkName', width: '8%' },
            { title: '连接字符串', data: 'ConnectionStr', width: '50%' },
            { title: '数据库类型', data: 'DbType', width: '12%' },
            {
                title: '操作',  width: '27%', formatter: function (value, row) {
                    var builder = new BtnBuilder();
                    builder.AddBtn({ icon: 'glyphicon-edit', function: 'openForm', param: [row['Id']] });
                    builder.AddBtn({ icon: 'glyphicon-trash', function: 'deleteData', param: [row['Id']] });

                    return builder.build();
                }
            }
        ],
        "ajax": {//类似jquery的ajax参数，基本都可以用。
            type: "post",//后台指定了方式，默认get，外加datatable默认构造的参数很长，有可能超过get的最大长度。
            url: "/Base_SysManage/DatabaseLink/GetDataList",
            dataSrc: "data",//默认data，也可以写其他的，格式化table的时候取里面的数据
            data: function (d) {//d 是原始的发送给服务器的数据，默认很长。
                var param = {};//因为服务端排序，可以新建一个参数对象
                param.start = d.start;//开始的序号
                param.length = d.length;//要取的数据的
                var formData = $(
                    "#search_form")
                    .serializeArray();//把form里面的数据序列化成数组
                formData
                    .forEach(function (e) {
                        param[e.name] = e.value;
                    });
                return param;//自定义需要传递的参数。
            },
        },
        //"ajax": $.fn.dataTable.pagerAjax({url: "/listData"}),
        "destroy": true,
        lengthChange: false,
        serverSide: true,//分页，取数据等等的都放到服务端去
        searching: false,
        processing: true,//载入数据的时候是否显示“载入中”
        bDestroy: true,
        pageLength: 20,//首次加载的数据条数
        ordering: false,//排序操作在服务端进行，所以可以关了。
        language: {
            processing: "载入中",//处理页面数据的时候的显示
            paginate: {//分页的样式文本内容。
                previous: "上一页",
                next: "下一页",
                first: "第一页",
                last: "最后一页"
            },
            zeroRecords: "没有内容",//table tbody内容为空时，tbody的内容。
            //下面三者构成了总体的左下角的内容。
            info: "第 _PAGE_/_PAGES_页 共 _TOTAL_条记录",//左下角的信息显示，大写的词为关键字。
            infoEmpty: "第 _PAGE_/_PAGES_页 共 _TOTAL_条记录",//筛选为空时左下角的显示。
            infoFiltered: ""//筛选之后的左下角筛选提示(另一个是分页信息显示，在上面的info中已经设置，所以可以不显示)，
        }
    }).on('click', 'a[row-index]', function () {
    });
}

//打开表单   
function openForm(id, title) {
    dialogOpen({
        id: 'form',
        title: title,
        btn: ['确定', '取消'],
        url: '/Base_SysManage/DatabaseLink/Form?id={0}'.format(id || ''),
        yes: function (window, body) {
            window.submitForm();
        }
    });
}

//删除数据
function deleteData(id) {
    dialogComfirm('确认删除吗？', function () {
        var ids = [];

        if (typeof (id) == 'string') {//单条数据
            ids.push(id);
        } else {//多条数据
            var rows = $table.bootstrapTable('getSelections');
            if (rows.length == 0) {
                dialogError('请选择需要删除的数据！');
                return;
            } else {
                $.each(rows, function (index, value) {
                    ids.push(value['Id']);
                })
            }
        }

        loading();
        $.postJSON('/Base_SysManage/DatabaseLink/DeleteData', { ids: JSON.stringify(ids) }, function (resJson) {
            loading(false);

            if (resJson.Success) {
                $table.bootstrapTable('refresh');
                dialogSuccess('删除成功!');
            }
            else {
                dialogError(resJson.Msg);
            }
        });
    });
}