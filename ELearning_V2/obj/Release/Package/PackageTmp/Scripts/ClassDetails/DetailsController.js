myapp.controller('ClassDetailsController', function ($scope, $window, ClassDetailsService) {
    $scope.MaLop = function (id) {
        $scope.id = id;
        $scope.ShowForm = false;
        LoadDetails(id);
        LoadBaiGiang(id);
        LoadComment(id);
        LoadThongBao(id);
    };

    function LoadDetails(id) {
        var lstSV = ClassDetailsService.details(id);
        lstSV.then(function (d) {
            $scope.Details = d.data;

        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }
    function LoadBaiGiang(id) {
        var rec = ClassDetailsService.GetBaiGiang_TheoLop(id);
        rec.then(function (d) {
            $scope.BaiGiangs = d.data;

        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }
    function LoadComment(id) {
        var rec = ClassDetailsService.GetComment(id);
        rec.then(function (d) {
            $scope.Coms = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });
    }
    function LoadThongBao(id) {
        var rec = ClassDetailsService.GetThongBaoTheoLop(id);
        rec.then(function (d) {
            $scope.TBs = d.data;
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });

    }

    $scope.AddNewCmt = function () {
        var comment = {
            NoiDung: $scope.NewComment,
            ClassID: $scope.id
        }
        var re = ClassDetailsService.addnewComment(comment);
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
        var re = ClassDetailsService.addnewReply(reply);
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

    $scope.cancelRep = function (){
        $scope.ShowForm = false;
        $scope.CommentID ="";
        $scope.NewRep = "";

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
