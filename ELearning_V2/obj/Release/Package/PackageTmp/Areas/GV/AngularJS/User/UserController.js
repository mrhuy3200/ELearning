﻿myapp.controller('UserController', function ($scope, userService, Upload, $timeout) {
    loadUser();
    loadMonHoc();

    function loadUser() {
        var user = userService.getUser();
        user.then(function (d) {
            $scope.u = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    function loadMonHoc() {
        var lop = userService.GetAllMonHoc();
        lop.then(function (d) {
            $scope.MonHocs = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }


    //get single record by ID
    $scope.getForUpdate = function (gv) {
        $scope.Email = gv.Email;
        $scope.SDT = gv.SDT;

    }

    $scope.ChangePass = function (gv) {
        $scope.textboxPass = !$scope.textboxPass;
    }

    $scope.CancelChangePass = function (gv) {
        $scope.textboxPass = false;
        resetChange();
    }


    $scope.save = function () {
        if ($scope.SelectedFiles != null) {
            var GV = {
                Email: $scope.Email,
                SDT: $scope.SDT,
                Image: $scope.SelectedFiles[0].name,
                MaMonHoc: $scope.MaMonHoc
            };
        }
        else {
            var GV = {
                Email: $scope.Email,
                SDT: $scope.SDT,
                MaMonHoc: $scope.MaMonHoc
            };

        }

        var saverecords = userService.update(GV);
        saverecords.then(function (d) {
            if (d.data.success === true) {
                loadUser();
                alert("Cập nhật thành công");
            }
            else { alert("Cập nhật thất bại."); }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình thêm.");
        });
    }


    //update Employee data
    $scope.update = function () {
        var GV = {
            ID: $scope.u.ID,
            HoVaTen: $scope.u.HoVaTen,
            Email: $scope.u.Email
        };
        var updaterecords = userService.update(GV);
        updaterecords.then(function (d) {
            if (d.data.success === true) {
                loadUser();
                alert("Cập nhật thành công");
                $scope.resetUpdate();
                $scope.textboxStatus = !$scope.textboxStatus;

            }
            else {
                alert("Sinh viên chưa được cập nhật.");
            }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình cập nhật");
        });
    }

    //reset update
    $scope.resetSave = function () {
        $scope.UpdateID = '';
        $scope.UpdateHoVaTen = '';
        $scope.UpdateEmail = '';
    }

    $scope.UploadFiles = function (files) {
        $scope.SelectedFiles = files;
        if ($scope.SelectedFiles && $scope.SelectedFiles.length) {
            Upload.upload({
                url: '/GV/UpLoad/Upload/',
                data: {
                    files: $scope.SelectedFiles
                }
            }).then(function (response) {
                $timeout(function () {
                    $scope.Result = response.data;
                });
            }, function (response) {
                if (response.status > 0) {
                    var errorMsg = response.status + ': ' + response.data;
                    alert(errorMsg);
                }
            }, function (evt) {
                var element = angular.element(document.querySelector('#dvProgress'));
                $scope.Progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                element.html('<div style="width: ' + $scope.Progress + '%">' + $scope.Progress + '%</div>');
            });
        }
    }

    $scope.savePass = function () {
        var pass = {
            OldPass: $scope.OldPass,
            NewPass: $scope.NewPass
        };
        var saverecords = userService.changePass(pass);
        saverecords.then(function (d) {
            if (d.data.success === true) {
                loadUser();
                resetChange();

                alert("Đã thay đổi");
            }
            else { alert("Cập nhật thất bại."); }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình thêm.");
        });
    }

    function resetChange() {
        $scope.OldPass = '';
        $scope.NewPass = '';
        $scope.Confirm = '';
        $scope.textboxPass = true
    }

})
