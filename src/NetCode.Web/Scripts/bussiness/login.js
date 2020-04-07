$(function () {
    setPwd();

    //$('input').iCheck({
    //    checkboxClass: 'icheckbox_square-blue',
    //    radioClass: 'iradio_square-blue',
    //    increaseArea: '20%' // optional
    //});

    //表单校验
    $('#form').validator().on('submit', function (e) {
        //校验成功
        if (!e.isDefaultPrevented()) {
            e.preventDefault();

            var values = $('#form').getValues();
            //保存账号密码
            var checked = $('#savePwd')[0].checked;
            savePwd(values.userName, values.password, checked);
            loading();
            $.postJSON(rootUrl + 'Home/SubmitLogin', values, function (resJson) {
                loading(false);

                if (resJson.Success) {
                    window.location.href = rootUrl;
                }
                else {
                    dialogError(resJson.Msg);
                }
            });
        }
    })
});

//设置保存的账号密码
function setPwd() {
    var userName = $.cookie('userName');
    var password = $.cookie('password');
    if (userName && password) {
        $('#userName').val(userName);
        $('#savePwd').prop('checked', 'checked');
    } else {
        $('#savePwd').removeAttr('checked');
    }

    //$('#password').bootstrapPwdBox({
    //    value: password
    //});
}

//保存账号密码
function savePwd(userName, password, isSave) {
    if (isSave) {
        $.cookie('userName', userName, {
            expires: new Date('9999/1/1'),
            path: '/'
        });
        $.cookie('password', password, {
            expires: new Date('9999/1/1'),
            path: '/'
        });
    } else {
        $.removeCookie('userName', { path: '/' });
        $.removeCookie('password', { path: '/' });
    }
}