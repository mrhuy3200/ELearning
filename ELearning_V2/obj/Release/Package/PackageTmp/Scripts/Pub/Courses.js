var CoursesApp = angular.module("CoursesApp", ['angularUtils.directives.dirPagination']);

CoursesApp.controller('CoursesController', function ($scope, $http, $window, ) {
    $scope.CPageSize = 6;
    $scope.CcurrentPage = 1;
    $scope.MaMonHoc = '';
    $scope.TenMonHoc = '';
    $scope.Index = -1;
    $scope.InitUserID = function (UserID) {
        $scope.UserID = UserID;
    }
    runWaiting();
    LoadCourse();
    LoadMonHoc();
    function LoadMonHoc() {
        $http({
            method: 'GET',
            url: '/Lop/GetListMonHoc'
        }).then(function successCallback(response) {
            console.log(response.data);
            $scope.MonHocs = response.data;
        });
    }
    function LoadCourse() {
        $http({
            method: 'GET',
            url: '/Lop/GetAllCourse'
        }).then(function successCallback(response) {
            console.log(response.data);
            $scope.Courses = response.data;
            downWaiting();
        });

    }
    function runWaiting() {
        console.log("Open");
        angular.element("#main-wait").css("display", "block");
        angular.element("body").css("overflow", "hidden");
    }
    function downWaiting() {
        console.log("Close");

        angular.element("#main-wait").css("display", "none");
        angular.element("body").css("overflow", "auto");
    }
    $scope.LocMonHoc = function (index, MaMonHoc, TenMonHoc) {
        console.log($scope.MaMonHoc)
        console.log($scope.Index)

        if ($scope.MaMonHoc == '') {
            $('#btnMonHoc' + index).css("backgroundColor", "#4dbf1c");
            $scope.MaMonHoc = MaMonHoc;
            $scope.TenMonHoc = TenMonHoc;
            $scope.Index = index;
        }
        else {
            if ($scope.MaMonHoc == MaMonHoc) {
                $('#btnMonHoc' + index).css("backgroundColor", "#ffffff");
                $scope.MaMonHoc = '';
                $scope.TenMonHoc = '';
                $scope.Index = -1;
            }
            else {
                $('#btnMonHoc' + $scope.Index).css("backgroundColor", "#ffffff");
                $('#btnMonHoc' + index).css("backgroundColor", "#4dbf1c");
                $scope.MaMonHoc = MaMonHoc;
                $scope.TenMonHoc = TenMonHoc;
                $scope.Index = index;
            }
        }
        //for (var i = 0; i < $scope.Courses.length; i++) {
        //    var MonHoc = $scope.Courses[i].MaMonHoc;
        //    if ($scope.Index == -1) {
        //        $scope.Courses[i]["Hide"] = false;
        //    }
        //    else {
        //        $scope.Courses[i]["Hide"] = true;
        //        if (MonHoc == $scope.MaMonHoc) {
        //            $scope.Courses[i]["Hide"] = false;
        //        }
        //    }
        //}
    }
    $scope.ViewCourse = function (Course) {
        if (Course.UserID == $scope.UserID) {
            $window.location.href = "/Lop/CourseDetail/" + Course.ID;
        }
        else {
            $window.location.href = "/Lop/ViewCourse/" + Course.ID;
        }
    }
});