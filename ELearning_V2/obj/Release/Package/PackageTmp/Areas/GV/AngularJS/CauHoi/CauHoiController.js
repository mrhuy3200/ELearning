myapp.controller('CauHoiController', function ($scope, CauHoiService) {
    $scope.MaMon = function (id) {
        $scope.id = id;
        LoadCauHoi(id);

    }
    function LoadCauHoi(id) {
        var CauHoiRecord = CauHoiService.getCauHoi(id);
        CauHoiRecord.then(function (d) {
            $scope.CauHois = d.data;

        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    function Refresh() {
    }


    //save cauhoi data 
    $scope.save = function () {
        var CauHoi = {
            MaCauHoi: $scope.MaCauHoi,
            NoiDung: $scope.NoiDung,
            Diem: $scope.Diem,
            DoKho: $scope.DoKho,
            CauA: $scope.CauA,
            CauB: $scope.CauB,
            CauC: $scope.CauC,
            CauD: $scope.CauD,
            DapAn: $scope.DapAn,
            MaMonHoc: $scope.id
        };
        var saverecords = CauHoiService.save(CauHoi);
        saverecords.then(function (d) {
            if (d.data.success === true) {
                LoadCauHoi($scope.id);
                alert("Thêm thành công");
                $scope.resetSave();
                Refresh();
            }
            else { alert("Lỗi."); }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình thêm.");
        });
    }

    //reset controls after save operation
    $scope.resetSave = function () {
        $scope.MaCauHoi = '';
        $scope.NoiDung = '';
        $scope.Diem = '';
        $scope.DoKho = '';
        $scope.CauA = '';
        $scope.CauB = '';
        $scope.CauC = '';
        $scope.CauD = '';
        $scope.DapAn = '';
    }

    // lấy chi tiết 
    $scope.Details = function (cauhoi) {
        $scope.DetailsMaCauHoi = cauhoi.MaCauHoi;
        $scope.DetailsNoiDung = cauhoi.NoiDung;
        $scope.DetailsDiem = cauhoi.Diem;
        $scope.DetailsDoKho = cauhoi.DoKho;
        $scope.DetailsCauA = cauhoi.CauA;
        $scope.DetailsCauB = cauhoi.CauB;
        $scope.DetailsCauC = cauhoi.CauC;
        $scope.DetailsCauD = cauhoi.CauD;
        $scope.DetailsDapAn = cauhoi.DapAn;
        $scope.DetailsTrangThai = cauhoi.TrangThai;
    }

    $scope.unlockTextBox = function () {
        $scope.textboxStatus = !$scope.textboxStatus;
    }

    $scope.close = function () {
        $scope.textboxStatus = true;
    }

    $scope.updateTT = function () {
        $scope.DetailsTrangThai = !$scope.DetailsTrangThai;
    }

    $scope.update = function () {
        var CauHoi = {
            MaCauHoi: $scope.DetailsMaCauHoi,
            NoiDung: $scope.DetailsNoiDung,
            Diem: $scope.DetailsDiem,
            DoKho: $scope.DetailsDoKho,
            CauA: $scope.DetailsCauA,
            CauB: $scope.DetailsCauB,
            CauC: $scope.DetailsCauC,
            CauD: $scope.DetailsCauD,
            DapAn: $scope.DetailsDapAn,
            TrangThai: $scope.DetailsTrangThai
        }
        var updaterecords = CauHoiService.update(CauHoi);
        updaterecords.then(function (d) {
            if (d.data.success === true) {
                LoadCauHoi($scope.id);
                alert("Cập nhật thành công");
                $scope.resetUpdate();
            }
            else {
                alert("Câu hỏi chưa được cập nhật.");
            }
        },
        function () {
            alert("Xảy ra lỗi trong quá trình cập nhật");
        });
    }

    $scope.resetUpdate = function () {
        $scope.DetailsMaCauHoi = '';
        $scope.DetailsNoiDung = '';
        $scope.DetailsDiem = '';
        $scope.DetailsDoKho = '';
        $scope.DetailsCauA = '';
        $scope.DetailsCauB = '';
        $scope.DetailsCauC = '';
        $scope.DetailsCauD = '';
        $scope.DetailsDapAn = '';
        $scope.textboxStatus = true;
    }


    $scope.getForPrivate = function (cauhoi) {
        $scope.DetailsMaCauHoi = cauhoi.MaCauHoi;
    }


    $scope.private = function () {
        var CauHoi = {
            MaCauHoi: $scope.DetailsMaCauHoi,
        }

        var re = CauHoiService.private(CauHoi);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadCauHoi($scope.id);
                Refresh();
                $scope.resetUpdate();
            }
            else {
                alert("Error");
            }
        });
    }

    $scope.getForPublish = function (cauhoi) {
        $scope.DetailsMaCauHoi = cauhoi.MaCauHoi;
    }


    $scope.publish = function () {
        var CauHoi = {
            MaCauHoi: $scope.DetailsMaCauHoi,
        }

        var pub = CauHoiService.publish(CauHoi);
        pub.then(function (d) {
            if (d.data.success === true) {
                LoadCauHoi($scope.id);
                Refresh();
                $scope.resetUpdate();

            }
            else {
                alert("Error");
            }
        });
    }

    //get data for delete confirmation
    $scope.getForDelete = function (ch) {
        $scope.DetailsMaCauHoi = ch.MaCauHoi;
    }


    //delete câu hỏi record
    $scope.delete = function (DetailsMaCauHoi) {
        var deleterecord = CauHoiService.delete($scope.DetailsMaCauHoi);
        deleterecord.then(function (d) {
            if (d.data.success === true) {
                LoadCauHoi($scope.id);
                alert("Xóa thành công");
            }
            else {
                alert("Câu hỏi chưa được xóa.");
            }
        });
    }
})

myapp.directive('latex', function() {
    return {
      restrict: 'AE',
      link: function(scope, element) {
        var newDom = element.clone();
        element.replaceWith(newDom);
        var pre = "\\(",
          post = "\\)";
        if (element[0].tagName === 'DIV') {
          pre = "\\[";
          post = "\\]";
        }
        scope.$watch(function() {
          return element.html();
        }, function() {
          console.log(element);
          newDom.html(pre + element.html() + post);
          MathJax.Hub.Typeset(newDom[0]);
        });
      }
    }
  });