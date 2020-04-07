$(function () {
    initEvent();
});

//事件绑定
function initEvent() {
    //表单校验
    $('#form').validator().on('submit', function (e) {
        //校验成功
        if (!e.isDefaultPrevented()) {
            e.preventDefault();

            var values = $('#form').getValues();
            var areaName = values['areaName'];
            var buildType = [];
            $('input:checked').each(function (index, item) {
                buildType.push($(item).attr('value'));
            });

            loading();
            $.postJSON('/Base_SysManage/RapidDevelopment/BuildCode', {
                linkId: linkId,
                areaName: areaName,
                tables: tables,
                buildType: JSON.stringify(buildType)
            }, function (resJson) {
                loading(false);

                if (resJson.Success) {
                    parent.dialogSuccess('生成成功!');
                    parent.dialogClose('form');
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


function setTables(_tables) {
    tables = _tables;
}