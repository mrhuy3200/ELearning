var CreateTestApp = angular.module("CreateTestApp", []);

CreateLessionApp.controller('CreateTestController', function ($scope, $http, $window, ) {
    $scope.InitCourseID = function (CourseID) {
        $scope.CourseID = CourseID;
        LoadQuestion();
    }
    function LoadQuestion() {
        $http({
            method: 'GET',
            url: '/Lop/GetListQuesionByUserID',
            data: JSON.stringify($scope.Lop)
        }).then(function successCallback(response) {
            console.log(response.data);
            $scope.Questions = response.data;
        });
        
    }
});