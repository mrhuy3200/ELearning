myapp.controller('DetailsController', function ($scope, $window, DetailsService) {
    $scope.MaLop = function (id) {
        $scope.id = id;
        $scope.ShowForm = false;
        LoadListSV(id);
        LoadDetails(id);
        LoadBaiGiang(id);
        LoadComment(id);
        LoadThongBao(id);
    };

    function LoadListSV(id) {
        var lstSV = DetailsService.getSV_TheoLopID(id);
        lstSV.then(function (d) {
            $scope.SVs = d.data;

        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }
    function LoadDetails(id) {
        var lstSV = DetailsService.details(id);
        lstSV.then(function (d) {
            $scope.Details = d.data;

        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }
    function LoadBaiGiang(id) {
        var rec = DetailsService.GetBaiGiang_TheoLop(id);
        rec.then(function (d) {
            $scope.BGs = d.data;

        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }

    function LoadComment(id) {
        var rec = DetailsService.GetComment(id);
        rec.then(function (d) {
            $scope.Coms = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }
    function LoadThongBao(id) {
        var rec = DetailsService.GetThongBao(id);
        rec.then(function (d) {
            $scope.TBs = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });

    }
    function ResetTB() {
        $scope.NoiDungTB = '';
    }
    function resetID() {
        $scope.ID = '';
    }
    $scope.removeSV = function (SvID, LopID) {
        var re = DetailsService.removeSV_TheoLop(SvID, LopID);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadListSV(LopID);
            }
            else {
                alert("Failed");
            }
        })
    }

    $scope.AddBaiGiang = function (mabaigiang, malop) {
        var model = {
            MaBaiGiang: mabaigiang,
            MaLop: malop
        }
        var rec = DetailsService.AddNewBG(model);
        rec.then(function (d) {
            if (d.data.success === true) {
                LoadBaiGiang($scope.id);
                $scope.GetAllBaiGiang($scope.id);
            }
            else {
                alert("Failed");
            }
        })
    }

    $scope.GetAllBaiGiang = function (id) {
        var rec = DetailsService.GetAllBaiGiang(id);
        rec.then(function (d) {
            $scope.BaiGiangs = d.data;

        },
        function () {
            alert("Xảy ra lỗi trong khi tải bài giảng...");
        });
    }


    $scope.removeBG = function (mabaigiang, malop) {
        var model = {
            MaBaiGiang: mabaigiang,
            MaLop: malop
        }

        var re = DetailsService.removeBG(model);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadBaiGiang($scope.id);
            }
            else {
                alert("Failed");
            }
        })
    }

    $scope.AddNewHS = function () {
        var re = DetailsService.AddNewSV($scope.HSID, $scope.id);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadListSV($scope.id);
                resetID();
            }
            else {
                alert("Học sinh không tồn tại");
            }
        })
    }



    $scope.resetTB = function () {
        $scope.NoiDungTB = '';
    }

    $scope.removeCmt = function (ID) {
        var re = DetailsService.removeComment(ID);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadComment($scope.id);
            }
            else {
                alert("Failed");
            }
        })

    }

    $scope.removeRep = function (ID) {
        var re = DetailsService.removeReply(ID);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadComment($scope.id);
            }
            else {
                alert("Failed");
            }
        })

    }

    $scope.AddNewCmt = function () {
        var comment = {
            NoiDung: $scope.NewComment,
            ClassID: $scope.id
        }
        var re = DetailsService.addnewComment(comment);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadComment($scope.id);
                $scope.NewComment = "";
            }
            else {
                alert("Failed");
            }
        })
    }

    $scope.repComment = function (id) {
        $scope.ShowForm = true;
        $scope.CommentID = id;
    }

    $scope.repReply = function (id) {
        $scope.ShowForm = true;
        $scope.CommentID = id;
    }

    $scope.AddNewRep = function () {
        var reply = {
            NoiDung: $scope.NewRep,
            ParentCommentID: $scope.CommentID
        }
        var re = DetailsService.addnewReply(reply);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadComment($scope.id);
                $scope.NewRep = "";
                $scope.ShowForm = false;
            }
            else {
                alert("Failed");
            }
        })

    }

    $scope.cancelRep = function () {
        $scope.ShowForm = false;
        $scope.CommentID = "";
        $scope.NewRep = "";

    }

    $scope.AddTB = function () {
        var ThongBao = {
            NoiDung: $scope.NoiDungTB,
            LopID: $scope.id
        }
        var re = DetailsService.AddNewTB(ThongBao);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadThongBao($scope.id);
                ResetTB();
            }
            else {
                alert("Failed");
            }
        })

    }

    $scope.RemoveTB = function (id) {
        var ThongBao = {
            ID: id
        }
        var re = DetailsService.DeleteTB(ThongBao);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadThongBao($scope.id);
            }
            else {
                alert("Failed");
            }
        })

    }



})

myapp.directive('showFocus', function ($timeout) {
    return function (scope, element, attrs) {
        scope.$watch(attrs.showFocus,
          function (newValue) {
              $timeout(function () {
                  newValue && element[0].focus();
              });
          }, true);
    };
});
