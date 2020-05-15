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

LessionDetailApp.controller('LessionDetailController', function ($scope, $http, LessionDetailService) {
    $scope.ID = function (id, CourseID) {
        $scope.currentPage = 1;
        $scope.pageSize = 10;
        $scope.LessionID = id;
        $scope.CourseID = CourseID
        LoadLession(id);
        LoadComment(id);
        LoadListLession(CourseID);
    };

    function LoadComment(ID) {
        var CommentDTO = {
            LessionID: ID
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
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function LoadLession(ID) {
        LessionDetailService.LoadLession(ID).then(function (d) {
            $scope.Lession = d.data;
            $scope.Lession["CreateDate"] = new Date(parseInt($scope.Lession["CreateDate"].substr(6)));
            console.log(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
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
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
});




LessionDetailApp.factory('LessionDetailService', function ($http) {
    var fac = {};
    fac.LoadClass = function (ID) {
        return $http.get('/Lop/GetClassByID/' + ID);
    };
    fac.LoadMember = function (ID) {
        return $http.get('/Lop/GetMemberByClassID/' + ID);
    };
    fac.LoadLession = function (ID) {
        return $http.get('/Lop/GetLessionByID/' + ID);
    };
    fac.LoadListLession = function (CourseID) {
        return $http.get('/Lop/GetLessionByClassID/' + CourseID);
    };
    fac.AddMember = function (LessionDetailDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/AddMember',
            data: JSON.stringify(LessionDetailDTO)
        });
    };
    fac.ChangeLessionStatus = function (LessionDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/ChangeStatus',
            data: JSON.stringify(LessionDTO)
        });
    };
    fac.RemoveLession = function (LessionDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/RemoveLession',
            data: JSON.stringify(LessionDTO)
        });
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

