var LessionDetailApp = angular.module("LessionDetailApp", ['angularUtils.directives.dirPagination']);

LessionDetailApp.directive('ngFiles', ['$parse', function ($parse) {
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

LessionDetailApp.controller('LessionDetailController', function ($scope, $sce, LessionDetailService) {
    $scope.ID = function (id, CourseID) {
        $scope.currentPage = 1;
        $scope.pageSize = 10;
        $scope.LessionID = id;
        $scope.CourseID = CourseID;
        console.log(id);
        console.log(CourseID);

        LoadLession(id);
        LoadComment(id, CourseID);
        if (CourseID != '') {
            LoadListLession(CourseID);
        }
    };

    function LoadComment(ID, CourseID) {
        var CommentDTO = {
            LessionID: ID,
            CourseID: CourseID
        };
        LessionDetailService.LoadComment(CommentDTO).then(function (d) {
            var temp = d.data;
            for (var i = 0; i < temp.length; i++) {
                temp[i]["CreateDate"] = new Date(parseInt(temp[i]["CreateDate"].substr(6)));
                for (var j = 0; j < temp[i]["Replies"].length; j++) {
                    temp[i]["Replies"][j]["CreateDate"] = new Date(parseInt(temp[i]["Replies"][j]["CreateDate"].substr(6)));
                }
            }
            $scope.Comments = temp;
            temp = null;
            console.log($scope.Comments);
        }, function () {
            alert('Không tìm thấy dữ liệu cmt !!!');
        });
    }
    function LoadLession(ID) {
        LessionDetailService.LoadLession(ID).then(function (d) {
            $scope.Lession = d.data;
            $scope.Lession["CreateDate"] = new Date(parseInt($scope.Lession["CreateDate"].substr(6)));
            $scope.Lession["Content"] = $sce.trustAsHtml($scope.Lession["Content"]);
            $scope.Lession.URL = $sce.trustAsResourceUrl($scope.Lession.URL);

            console.log(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu lession!!!');
        });
    }
    function LoadListLession(CourseID) {
        LessionDetailService.LoadListLession(CourseID).then(function (d) {
            $scope.Lessions = d.data;
            for (var i = 0; i < $scope.Lessions.length; i++) {
                $scope.Lessions[i]["CreateDate"] = new Date(parseInt($scope.Lessions[i]["CreateDate"].substr(6)));
            }
            console.log(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu listLession !!!');
        });
    }
});




LessionDetailApp.factory('LessionDetailService', function ($http) {
    var fac = {};


    fac.LoadLession = function (ID) {
        return $http.get('/Lop/GetLessionByID/' + ID);
    };
    fac.LoadListLession = function (CourseID) {
        return $http.get('/Lop/GetLessionByClassID/' + CourseID);
    };
    fac.LoadComment = function (CommentDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/GetCommentByID',
            data: JSON.stringify(CommentDTO)
        });
    };
    return fac;
});

