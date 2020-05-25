var CoursesApp = angular.module("CoursesApp", ['angularUtils.directives.dirPagination']);

CoursesApp.controller('CoursesController', function ($scope, $http, $window, ) {
    $scope.CPageSize = 6;
    $scope.CcurrentPage = 1;
    LoadCourse();
    function LoadCourse() {
        $http({
            method: 'GET',
            url: '/Lop/GetAllCourse'
        }).then(function successCallback(response) {
            console.log(response.data);
            $scope.Courses = response.data;
        });

    }
});