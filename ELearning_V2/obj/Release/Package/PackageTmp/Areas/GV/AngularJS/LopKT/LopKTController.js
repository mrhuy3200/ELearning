myapp.controller('LopKTController', function ($scope, LopKTService) {

    loadLop();
    function loadLop() {
        var lop = LopKTService.getLopKT();
        lop.then(function (d) {
            $scope.Lops = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    function loadMonHoc() {
        var lop = LopKTService.GetAllMonHoc();
        lop.then(function (d) {
            $scope.MonHocs = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    function loadDeThi(id) {
        var lop = LopKTService.GetDeThi(id);
        lop.then(function (d) {
            $scope.DeThis = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load đề thi...");
        });
    }

    function loadHocVien(id) {
        var lop = LopKTService.GetHocVien(id);
        lop.then(function (d) {
            $scope.HVs = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load đề thi...");
        });
    }


    function resetSave() {
        $scope.Name = "";
        $scope.NgayKT = "";
        $scope.MonHocID = "";
    }

    function resetUpdate() {

        $scope.UpdateMaLop = "";
        $scope.UpdateName = "";
        $scope.UpdateNgayKT = "";
        $scope.UpdateMonHocID = "";
    }

    function resetDelete() {
        $scope.MaLop = "";
    }

    $scope.add = function () {
        loadMonHoc();
    }

    $scope.reset = function () {
        resetSave();
    }

    $scope.resetdel = function () {
        resetDelete();
    }


    $scope.save = function () {
        var Lop = {
            Name: $scope.Name,
            NgayKT: $scope.NgayKT,
            MonHocID: $scope.MonHocID,
            ThoiGianThi: $scope.ThoiGianThi
        };
        var saverecords = LopKTService.save(Lop);
        saverecords.then(function (d) {
            if (d.data.success === true) {
                loadLop();
                alert("Thêm thành công");
                resetSave();
            }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình thêm.");
        });
    }

    $scope.getForDelete = function (l) {
        $scope.MaLop = l.ID;
    }

    $scope.delete = function () {
        var Lop = {
            ID: $scope.MaLop
        };
        var saverecords = LopKTService.delete(Lop);
        saverecords.then(function (d) {
            if (d.data.success === true) {
                loadLop();
                alert("Đã xóa");
                resetDelete();
            }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình xóa.");
        });
    }

    $scope.getForUpdate = function (l) {
        loadMonHoc();
        $scope.UpdateMaLop = l.ID;
        $scope.UpdateName = l.Name;
        $scope.UpdateThoiGianThi = l.ThoiGianThi;
    }

    $scope.update = function () {
        var Lop = {
            ID: $scope.UpdateMaLop,
            Name: $scope.UpdateName,
            NgayKT: $scope.UpdateNgayKT,
            MonHocID: $scope.UpdateMonHocID,
            ThoiGianThi: $scope.UpdateThoiGianThi

        };
        var saverecords = LopKTService.update(Lop);
        saverecords.then(function (d) {
            if (d.data.success === true) {
                loadLop();
                alert("Đã cập nhật");
                resetUpdate();
            }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình cập nhật.");
        });
    }


    $scope.ClassDetails = function (lop) {
        loadDeThi(lop.MonHocID);
        loadHocVien(lop.ID);
        $scope.MaLop = lop.ID;
        $scope.TenDeThiHienTai = lop.TenDeThi;
    }

    $scope.UpdateDeThi = function (madethi, malop) {
        var Lop = {
            ID: malop,
            MaDeThi: madethi
        };
        var saverecords = LopKTService.updateDeThi(Lop);
        saverecords.then(function (d) {
            if (d.data.success != false) {
                loadLop();
                alert("Updated");
                $scope.TenDeThiHienTai = d.data.success
            }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình xóa.");
        });
    }

    $scope.AddnewHS = function (id, maLop) {
        var HS_LOP = {
            ID: id,
            MaLop: maLop
        }
        var saverecords = LopKTService.AddNewSV(HS_LOP);
        saverecords.then(function (d) {
            if (d.data.success != false) {
                loadHocVien(maLop);
                alert("Đã thêm");
                $scope.MaHS = "";
            }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình thêm.");
        });

    }

    $scope.removeSV = function (id, maLop) {
        var HS_LOP = {
            ID: id,
            MaLop: maLop
        }
        var saverecords = LopKTService.RemoveHV(HS_LOP);
        saverecords.then(function (d) {
            if (d.data.success != false) {
                loadHocVien(maLop);
                alert("Đã xóa");
            }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình xóa.");
        });

    }


})