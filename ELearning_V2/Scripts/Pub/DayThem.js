var DayThemApp = angular.module("DayThemApp", ['angularUtils.directives.dirPagination']);

DayThemApp.controller('DayThemController', function ($scope, $http, DayThemService) {
    LoadClass();
    $scope.type = 0;
    function LoadClass() {
        DayThemService.LoadClass().then(function (d) {
            $scope.Lops = d.data;
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    $scope.lop = {
        Name: '',
        Capacity: null,
        NumOfPeo: 0,
        Description: '',
        Image: '',
        Price: null,
        Schedule: '',
        Condition: '',
        Type: null
    }
    $scope.DangKy = function (type) {
        if (type == 1) {
            $scope.lop.Type = 1;
            $scope.lop.Capacity = 15;
            $scope.lop.Price = 0;
        }
        if (type == 2) {
            $scope.lop.Type = 2;
            $scope.lop.Capacity = 45;
        }
        if (type == 3) {
            $scope.lop.Type = 3;
            $scope.lop.Capacity = null;
        }
        $scope.type = type;
    }

    $scope.Save = function () {
        console.log('Save' + JSON.stringify($scope.lop));
    }
});



DayThemApp.factory('DayThemService', function ($http) {
    var fac = {};
    fac.LoadClass = function () {
        return $http.get('/Lop/GetClassByUserID');
    };
    return fac;
});
