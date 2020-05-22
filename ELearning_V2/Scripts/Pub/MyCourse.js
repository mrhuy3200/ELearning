var MyCourseApp = angular.module("MyCourseApp", ['angularUtils.directives.dirPagination']);

MyCourseApp.controller('MyCourseController', function ($scope, $http, $window) {
    $scope.CpageSize = 4;
    $scope.CcurrentPage = 1;
    LoadCourse();
    $scope.View = function (CourseID) {
        $window.location.href = "/Lop/ViewCourse/" + CourseID;
    }
    $scope.ExitCourse = function (CourseID) {
        if (confirm("Xác nhận ròi khỏi lớp học??")) {
            $http({
                method: 'GET',
                url: '/Course/ExitCourse/' + CourseID
            }).then(function (r) {
                if (r.data == 1) {
                    alert("Đã ra khỏi lớp");
                    LoadCourse();
                }
                if (r.data == 0) {
                    alert("Bạn không thuộc lớp này")
                }
                if (r.data == -1) {
                    alert("Không tìm thấy lớp")
                }
            })
        }

    }
    function LoadCourse() {
        $http({
            method: 'GET',
            url: '/Course/GetMyCourse'
        }).then(function (r) {
            $scope.Courses = r.data;
        })
    }
});
