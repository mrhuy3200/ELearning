var PersonalApp = angular.module("PersonalApp", []);

PersonalApp.controller('PersonalController', function ($scope, $http, $window) {
    LoadUser();
    SetHash();
    $scope.update = function () {
        console.log($scope.User)
        $http({
            method: 'POST',
            url: '/User/UpdateInfo',
            data: JSON.stringify($scope.User)
        }).then(function (r) {
            if (r.data == 1) {
                LoadUser();
                console.log($scope.User)
            }
        })
    }
    $scope.ChangePassword = function () {
        if ($scope.checkOld && $scope.NewPassword == $scope.ConfirmPassword) {
            console.log("OK")
        }

    }
    function LoadUser() {
        $http({
            method: 'GET',
            url: '/User/GetUserInfo'
        }).then(function (r) {
            $scope.User = r.data;
            console.log($scope.User)

        })

    }
    function SetHash() {
        $("#OldPassword").keyup(function () {
            $scope.HashPass = md5($("#OldPassword").val());
            console.log($scope.HashPass);
            if ($scope.HashPass == $scope.User.Password) {
                $scope.checkOld = true;
                console.log($scope.checkOld);

                $("#OldPasswordError").css("display", "none");

            }
            else {
                $scope.checkOld = false;
                console.log($scope.checkOld);

                $("#OldPasswordError").css("display", "block");
            }
        })
    }
});
