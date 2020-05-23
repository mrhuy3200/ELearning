var HomeApp = angular.module("HomeApp", []);

HomeApp.controller('HomeController', function ($scope, $http, $sce, $window) {
    LoadFreeCourse();
    LoadTopTeacher();
    LoadPublishLession();

    function LoadFreeCourse() {
        $http({
            method: 'GET',
            url: '/Home/GetFreeCourse'
        }).then(function (r) {
            $scope.Courses = r.data;
            console.log("Courses");
            console.log($scope.Courses);

        })

    }
    function LoadTopTeacher() {
        $http({
            method: 'GET',
            url: '/Home/GetTopTeacher'
        }).then(function (r) {
            $scope.TopTeachers = r.data;
            console.log("TopTeachers");

            console.log($scope.TopTeachers)

        })
    }
    function LoadPublishLession() {
        $http({
            method: 'GET',
            url: '/Home/GetPublishLession'
        }).then(function (r) {
            $scope.PubLessions = r.data;
            for (var i = 0; i < $scope.PubLessions.length; i++) {
                $scope.PubLessions[i].CreateDate = new Date(parseInt($scope.PubLessions[i].CreateDate.substr(6)));
                $scope.PubLessions[i].Content = $sce.trustAsHtml($scope.PubLessions[i].Content)
            }
            console.log("PubLessions");

            console.log($scope.PubLessions)

        })
    }
});
