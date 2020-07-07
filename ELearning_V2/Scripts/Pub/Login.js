$('#ResetPassBtn').on('click', function () {
    const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var email = $('#Email').val();
    if (email) {
        if (re.test(email)) {
            $.ajax({
                url: '/Login/ForgotPassword/?Email=' + email,
                type: "GET",
                success: function (result) {
                    var str
                    if (result == 1) {
                        str = '<div style="background-color:#28a74524;margin:5px 10px 0px 10px;border-radius: 5px;padding: 10px;">Một link hướng dẫn lấy lại mật khẩu đã được gửi đến email ' + email + '</div>';
                        $('#Result').html(str);
                    }
                    else {
                        str = '<div style="background-color:#bb343429;margin:5px 10px 0px 10px;border-radius: 5px;padding: 10px;">Email này chưa đăng ký tài khoản tại ELearning.net, vui lòng kiểm tra lại!</div>';
                        $('#Result').html(str);
                    }
                }
            });
        }
        else {
            str = '<div style="background-color:#bb343429;margin:5px 10px 0px 10px;border-radius: 5px;padding: 10px;">Vui lòng nhập địa chỉ Email đúng!</div>';
            $('#Result').html(str);
        }
    }
    else {
        str = '<div style="background-color:#bb343429;margin:5px 10px 0px 10px;border-radius: 5px;padding: 10px;">Vui lòng nhập email!</div>';
        $('#Result').html(str);
    }
})
