myapp.controller('DetailsController', function ($scope, DetailsService) {
    $scope.MaMon = function (id) {
        $scope.id = id;
        LoadChuong(id);
    }

    $scope.MaDeThi = function (id) {
        $scope.DeThiID = id
        LoadCauHoiDeThi($scope.DeThiID);
        LoadCauHoi(id);

    }

    $scope.SoCauHoi = function (socauhoi) {
        $scope.CauHoiMax = socauhoi;

    }
    function LoadCauHoi(id) {
        var chdt = {
            MaMonHoc: $scope.id,
            DeThiID: $scope.DeThiID
        }
        var CauHoiRecord = DetailsService.getCauHoi(chdt);
        CauHoiRecord.then(function (d) {
            $scope.CauHois = d.data;

        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    function LoadCauHoiDeThi(id) {
        var CauHoiRecord = DetailsService.getCauHoiDeThi(id);
        CauHoiRecord.then(function (d) {
            $scope.CauHoiDeThis = d.data;
            $scope.randomMax = $scope.CauHoiMax - $scope.CauHoiDeThis.length;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }


    function LoadChuong(id) {
        var CauHoiRecord = DetailsService.getChuong(id);
        CauHoiRecord.then(function (d) {
            $scope.Chuongs = d.data;
            $scope.ChuongID = null;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    $scope.random = function () {
        var CauHoiDeThi = {
            ChuongID: $scope.ChuongRandom,
            DoKho: $scope.DoKhoRandom,
            SoCauHoi: $scope.SoLuongRandom,
            DeThiID: $scope.DeThiID
        }
        var re = DetailsService.RandomCauHoiDeThi(CauHoiDeThi);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadCauHoiDeThi($scope.DeThiID);
                LoadCauHoi($scope.DeThiID);
                $scope.ChuongRandom = null;
                $scope.DoKhoRandom = null;
                $scope.SoLuongRandom = null;
                alert("Tạo thành công");

            }
            else {
                if (d.data.success == -1) {
                    alert("Số câu hỏi không đủ");
                }
                else {
                    alert("Error");
                }

            }
        });

    }

    $scope.remove = function (MaCauHoi) {
        var CauHoiDeThi = {
            CauHoiID: MaCauHoi,
            DeThiID: $scope.DeThiID
        }
        var re = DetailsService.RemoveCauHoiDeThi(CauHoiDeThi);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadCauHoiDeThi($scope.DeThiID);
                LoadCauHoi($scope.DeThiID);
            }
            else {
                alert("Error");
            }
        });

    }

    $scope.add = function (MaCauHoi) {
        var CauHoiDeThi = {
            CauHoiID: MaCauHoi,
            DeThiID: $scope.DeThiID
        }
        var re = DetailsService.AddCauHoiDeThi(CauHoiDeThi);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadCauHoiDeThi($scope.DeThiID);
                LoadCauHoi($scope.DeThiID);

            }
            else {
                alert("Error");
            }
        });

    }


})

myapp.directive('latex', function () {
    return {
        restrict: 'AE',
        link: function (scope, element) {
            var newDom = element.clone();
            element.replaceWith(newDom);
            var pre = "\\(",
              post = "\\)";
            if (element[0].tagName === 'DIV') {
                pre = "\\[";
                post = "\\]";
            }
            scope.$watch(function () {
                return element.html();
            }, function () {
                console.log(element);
                newDom.html(pre + element.html() + post);
                MathJax.Hub.Typeset(newDom[0]);
            });
        }
    }
});