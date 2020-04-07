$(function () {
    initEvent();
});

//事件绑定
function initEvent() {
    //数据库类型
    $('#DbType').selectpicker({
        value: theEntity.DbType
    });

    //表单校验
    $('#form').validator().on('submit', function (e) {
        //校验成功
        if (!e.isDefaultPrevented()) {
            e.preventDefault();

            var values = $('#form').getValues();

            $.extend(theEntity, values);
            loading();
            $.postJSON('/Base_SysManage/DatabaseLink/SaveData', theEntity, function (resJson) {
                loading(false);

                if (resJson.Success) {
                    parent.$('#dataTable').bootstrapTable('refresh');
                    parent.dialogSuccess();
                    dialogClose();
                }
                else {
                    dialogError(resJson.Msg);
                }
            });
        }
    })
}

//提交表单
function submitForm() {
    $('#submit').trigger('click');
}