
myapp.controller('LopController', function ($scope, $filter, LopService, Upload, $timeout) {
    loadMonHoc();
    loadLop();

    function loadMonHoc() {
        var lop = LopService.GetAllMonHoc();
        lop.then(function (d) {
            $scope.MonHocs = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    function loadLop() {
        var lop = LopService.getLop();
        lop.then(function (d) {
            $scope.Lops = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    $scope.save = function () {
        var Lop = {
            MaLop: $scope.MaLop,
            TenLop: $scope.TenLop,
            MoTa: $scope.MoTa,
            StartDate: $scope.date,
            Image: $scope.SelectedFiles[0].name,
            //Image: $scope.Image[0].name,
            MaMonHoc: $scope.MaMonHoc
        };
        var saverecords = LopService.save(Lop);
        saverecords.then(function (d) {
            if (d.data.success === true) {
                loadLop();
                alert("Thêm thành công");
                $scope.resetSave();
            }
            else { alert("Vượt quá số lượng lớp học có thể tạo."); }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình thêm.");
        });
    }

    $scope.resetSave = function () {
        $scope.MaLop = '';
        $scope.TenLop = '';
        $scope.MoTa = '';
        $scope.date = '';
        $scope.Image = null;
        $scope.MaMonHoc = '';
    }

    $scope.getForUpdate = function (Lop) {
        $scope.UpdateMaLop = Lop.MaLop;
        $scope.UpdateTenLop = Lop.TenLop;
        $scope.UpdateMoTa = Lop.MoTa;
        $scope.UpdateImage = Lop.Image;
        $scope.UpdateMaMonHoc = Lop.MaMonHoc;
    }

    $scope.update = function () {
        var Lop = {
            MaLop: $scope.UpdateMaLop,
            TenLop: $scope.UpdateTenLop,
            MoTa: $scope.UpdateMoTa,
            StartDate: $scope.NgayBatDau,
            Image: $scope.SelectedFiles[0].name,
            MaMonHoc: $scope.UpdateMaMonHoc
        }
        var re = LopService.update(Lop);
        re.then(function (d) {
            if (d.data.success === true) {
                loadLop();
                alert("Đã cập nhật");
                $scope.resetUpdate();
            }
            else { alert("Cập nhật thất bại"); }
        },
        function () {
            alert("Xảy ra lỗi trong quá cập nhật.");
        });
    }

    $scope.resetUpdate = function () {
        $scope.UpdateMaLop = '';
        $scope.UpdateTenLop = '';
        $scope.UpdateMoTa = '';
        $scope.Updatedate = '';
        $scope.UpdateImage = null;
        $scope.UpdateMaMonHoc = '';
    }


    $scope.getForDelete = function (Lop) {
        $scope.DeleteMaLop = Lop.MaLop;
    }

    $scope.delete = function (DeleteMaLop) {
        var re = LopService.delete($scope.DeleteMaLop);
        re.then(function (d) {
            if (d.data.success === true) {
                loadLop();
                alert("Xóa thành công");
                resetDelete();
            }
            else {
                alert("Lớp chưa được xóa.");
            }
        });
    }

    $scope.resetDelete = function () {
        $scope.DeleteMaLop = "";
    }

    $scope.publish = function (Lop) {
        var lop = {
            MaLop: Lop.MaLop
        }
        var re = LopService.publish(lop);
        re.then(function (d) {
            if (d.data.success === true) {
                loadLop();
            }
            else {
                alert("Error");
            }
        });

    }

    $scope.private = function (Lop) {
        var lop = {
            MaLop: Lop.MaLop
        }
        var re = LopService.private(lop);
        re.then(function (d) {
            if (d.data.success === true) {
                loadLop();
            }
            else {
                alert("Error");
            }
        });

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
}
)

myapp.directive("selectNgFiles", function () {
    return {
        require: "ngModel",
        link: function postLink(scope, elem, attrs, ngModel) {
            elem.on("change", function (e) {
                var files = elem[0].files;
                ngModel.$setViewValue(files);
            })
        }
    }
});