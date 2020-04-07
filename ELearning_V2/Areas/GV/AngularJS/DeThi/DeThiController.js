
myapp.controller('DeThiController', function ($scope, DeThiService, $filter) {
    $scope.MaMon = function (id) {
        $scope.id = id;
        LoadDeThi(id);
    }

    function LoadCauHoi(id) {
        var CauHoiRecord = DeThiService.getCauHoi(id);
        CauHoiRecord.then(function (d) {
            $scope.CauHois = d.data;

        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }


    function LoadDeThi(id) {
        var re = DeThiService.getDeThi(id);
        re.then(function (d) {
            $scope.DeThis = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    $scope.getForPrivate = function (dethi) {
        $scope.MaDeThi = dethi.MaDeThi;
    }


    $scope.private = function () {
        var DeThi = {
            MaDeThi: $scope.MaDeThi
        }

        var re = DeThiService.private(DeThi);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadDeThi($scope.id);
                $scope.MaDeThi = "";
            }
            else {
                alert("Error");
            }
        });
    }

    $scope.getForPublish = function (dethi) {
        $scope.MaDeThi = dethi.MaDeThi;
    }

    $scope.publish = function () {
        var DeThi = {
            MaDeThi: $scope.MaDeThi,
        }

        var pub = DeThiService.publish(DeThi);
        pub.then(function (d) {
            if (d.data.success === true) {
                LoadDeThi($scope.id);
                $scope.MaDeThi = "";

            }
            else {
                alert("Error");
            }
        });
    }

    $scope.save = function () {
        var DeThi = {
            TenDeThi: $scope.TenDeThi,
            SoCauHoi: $scope.SoCauHoi,
            NgayThi: $scope.NgayThi,
            MaMonHoc: $scope.id
        };
        var saverecords = DeThiService.save(DeThi);
        saverecords.then(function (d) {
            if (d.data.success === true) {
                LoadDeThi($scope.id);
                alert("Thêm thành công");
                $scope.resetSave();
            }
            else { alert("Lỗi."); }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình thêm.");
        });
    }

    $scope.resetSave = function () {
        $scope.TenDeThi = '';
        $scope.SoCauHoi = '';
        $scope.NgayThi = '';
    }

    $scope.getForDelete = function (DeThi) {
        $scope.MaDeThi = DeThi.MaDeThi;
    }

    $scope.delete = function () {
        var DeThi = {
            MaDeThi: $scope.MaDeThi
        }
        var deleterecord = DeThiService.delete(DeThi);
        deleterecord.then(function (d) {
            if (d.data.success === true) {
                LoadDeThi($scope.id);
                alert("Xóa thành công");
                $scope.MaDeThi = "";
            }
            else {
                alert("Xảy ra lỗi trong quá trình xóa.");
            }
        });
    }

    $scope.UpdateDeThi = function (DeThi) {
        $scope.UpdateMaDeThi = DeThi.MaDeThi;
        $scope.UpdateTenDeThi = DeThi.TenDeThi;
        $scope.UpdateSoCauHoi = DeThi.SoCauHoi;
        $scope.UpdateNgayThi = "";

    }

    $scope.update = function () {
        var DeThi = {
            MaDeThi: $scope.UpdateMaDeThi,
            TenDeThi: $scope.UpdateTenDeThi,
            SoCauHoi: $scope.UpdateSoCauHoi,
            NgayThi: $scope.UpdateNgayThi
        };
        var saverecords = DeThiService.update(DeThi);
        saverecords.then(function (d) {
            if (d.data.success === true) {
                LoadDeThi($scope.id);
                alert("Đã cập nhật");
                $scope.resetUpdate();
            }
            else { alert("Lỗi."); }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình thêm.");
        });
    }

    $scope.resetUpdate = function () {
        $scope.UpdateTenDeThi = "";
        $scope.UpdateSoCauHoi = "";
        $scope.UpdateNgayThi = "";
        $scope.UpdateMaDeThi = "";
    }

    $scope.Update = function (DeThi) {

    }

    $scope.Details = function (DeThi) {
        LoadCauHoi(DeThi.MaMonHoc);

    }

})

myapp.filter('ctime', function () {

    return function (jsonDate) {

        var date = new Date(parseInt(jsonDate.substr(6)));
        return date;
    };

});

myapp.directive('mathJaxBind', function () {
    var refresh = function (element) {
        MathJax.Hub.Queue(["Typeset", MathJax.Hub, element]);
    };
    return {
        link: function (scope, element, attrs) {
            scope.$watch(attrs.mathJaxBind, function (newValue, oldValue) {
                element.text(newValue);
                refresh(element[0]);
            });
        }
    };
});



