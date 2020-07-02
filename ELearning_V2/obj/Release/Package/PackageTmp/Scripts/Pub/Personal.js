var PersonalApp = angular.module("PersonalApp", []);
PersonalApp.directive('ngFiles', ['$parse', function ($parse) {
    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}])
PersonalApp.controller('PersonalController', function ($scope, $http, $window) {
    LoadUser();
    SetHash();
    var formdata = new FormData();
    var file;

    $scope.update = async function () {
        console.log($scope.User)
        if ($scope.User.Email != '' && $scope.User.Fullname != '' && $scope.User.Fullname != null && $scope.User.Email != null) {
            $http({
                method: 'GET',
                url: '/User/CheckEmail/?Email=' + $scope.User.Email
            }).then(function (r) {
                if (r.data == 1) {
                    $http({
                        method: 'POST',
                        url: '/User/EditUserInfo',
                        data: JSON.stringify($scope.User)
                    }).then(function (r) {
                        if (r.data == 1) {
                            alert("Đã thay đổi");
                            $('#EmailErr1').css('display', 'none');
                            $('#EmailErr2').css('display', 'none');
                            $('#FullnameErr').css('display', 'none');

                            LoadUser();
                            console.log($scope.User)
                        }
                    })
                }
                else {
                    $('#EmailErr2').css('display', 'block');
                }
            })
        }
        else {
            if ($scope.User.Fullname == '' || $scope.User.Fullname == null) {
                $('#FullnameErr').css('display', 'block');
            }
            else {
                $('#EmailErr1').css('display', 'block');

            }
        }

    }
    $scope.ChangePassword = function () {
        if ($scope.checkOld && $scope.NewPassword == $scope.ConfirmPassword) {
            $scope.User.Password = md5($scope.NewPassword);
            console.log($scope.User);
            $http({
                method: 'POST',
                url: '/User/ChangeUserPassword',
                data: JSON.stringify($scope.User)
            }).then(function (r) {
                if (r.data == 1) {
                    alert("Đã thay đổi");
                    LoadUser();
                    console.log($scope.User)
                }
            })
        }

    }

    $scope.getTheFiles = function ($files) {
        console.log($files);
        file = $files[0];
    };
    $scope.UploadImage = function () {
        var blob = file.slice(0, file.size, 'image/jpg');
        newFile = new File([blob], $scope.User.ID + '.jpg', { type: 'image/jpg' });
        console.log('filename ' + newFile);
        formdata.set(0, newFile);
        console.log(formdata.get(0));
        $http({
            method: 'POST',
            url: '/api/API/UploadUserImage',
            data: formdata,
            headers: {
                'Content-Type': undefined
            }
        }).then(function successCallback(response) {
            
            if (response.data == "OK") {
                $scope.User.Image = $scope.User.ID + ".jpg";
                $http({
                    method: 'POST',
                    url: '/User/EditUserInfo',
                    data: JSON.stringify($scope.User)
                }).then(function (r) {
                    if (r.data == 1) {
                        alert("Đã thay đổi");
                        LoadUser();
                        console.log($scope.User)
                    }
                })
            }
        });
    };
    $scope.Recharge = function (Money) {
        $scope.User.Balance += Money;
        $http({
            method: 'POST',
            url: '/User/Recharge',
            data: JSON.stringify($scope.User)
        }).then(function (r) {
            if (r.data == 1) {
                alert("Nạp thành công");
                LoadUser();
                console.log($scope.User)
            }
        })

    }
    function LoadUser() {
        $http({
            method: 'GET',
            url: '/User/GetUserInfo'
        }).then(function (r) {
            $scope.User = r.data;
            console.log($scope.User)
            var input = $("input[data-type='currency']")
            $("input[data-type='currency']").val($scope.User.Balance);
            formatCurrency($(input));

        })

    }
    function CheckEmail(email) {
        $http({
            method: 'GET',
            url: '/User/CheckEmail/?Email=' + email
        }).then(function (r) {
            return r.data;
        })
    }
    function SetHash() {
        $("#OldPassword").keyup(function () {
            $scope.HashPass = md5($("#OldPassword").val());
            console.log($scope.HashPass);
            if ($scope.HashPass == $scope.User.Password) {
                $scope.checkOld = true;
                console.log($scope.checkOld);

                $("#OldPasswordError").css("display", "none");

            }
            else {
                $scope.checkOld = false;
                console.log($scope.checkOld);

                $("#OldPasswordError").css("display", "block");
            }
        })

        document.getElementById('file').onchange = function (evt) {
            var tgt = evt.target || window.event.srcElement,
                files = tgt.files;

            // FileReader support
            if (FileReader && files && files.length) {
                var fr = new FileReader();
                fr.onload = function () {
                    document.getElementById("output").src = fr.result;
                }
                fr.readAsDataURL(files[0]);
            }

            // Not supported
            else {
                // fallback -- perhaps submit the input to an iframe and temporarily store
                // them on the server until the user's session ends.
            }
        }
        $("input[data-type='currency']").on({
            keyup: function () {
                formatCurrency($(this));
            },
            blur: function () {
                formatCurrency($(this), "blur");
            },
            change: function () {
                console.log("Change")
                formatCurrency($(this));
            }
        });
    }
    function formatNumber(n) {
        // format number 1000000 to 1,234,567
        return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
    }
    function formatCurrency(input, blur) {
        // appends $ to value, validates decimal side
        // and puts cursor back in right position.

        // get input value
        var input_val = input.val();

        // don't validate empty input
        if (input_val === "") { return; }

        // original length
        var original_len = input_val.length;

        // initial caret position 
        var caret_pos = input.prop("selectionStart");

        // check for decimal
        if (input_val.indexOf(".") >= 0) {

            // get position of first decimal
            // this prevents multiple decimals from
            // being entered
            var decimal_pos = input_val.indexOf(".");

            // split number by decimal point
            var left_side = input_val.substring(0, decimal_pos);
            var right_side = input_val.substring(decimal_pos);

            // add commas to left side of number
            left_side = formatNumber(left_side);

            // validate right side
            right_side = formatNumber(right_side);

            // On blur make sure 2 numbers after decimal
            if (blur === "blur") {
                right_side += "00";
            }

            // Limit decimal to only 2 digits
            right_side = right_side.substring(0, 2);

            // join number by .
            input_val = "$" + left_side + "." + right_side;

        } else {
            // no decimal entered
            // add commas to number
            // remove all non-digits
            input_val = formatNumber(input_val);
            input_val = "$" + input_val;

            // final formatting
            if (blur === "blur") {
                input_val += ".00";
            }
        }

        // send updated string to input
        input.val(input_val);

        // put caret back in the right position
        var updated_len = input_val.length;
        caret_pos = updated_len - original_len + caret_pos;
        input[0].setSelectionRange(caret_pos, caret_pos);
    }
    $scope.AddMoney = function () {
    }
});
