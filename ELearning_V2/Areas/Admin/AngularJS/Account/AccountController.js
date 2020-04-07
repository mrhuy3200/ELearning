myapp.controller('AccountController', function ($scope, AccountService) {
    LoadAccount();

    function LoadAccount() {
        var lstSV = AccountService.GetAccount();
        lstSV.then(function (d) {
            $scope.Accounts = d.data;
            
        },
        function () {
            alert("Xảy ra lỗi trong khi load...");
        });

    }

    $scope.Create = function () {
        var tk = {
            Username: $scope.Username,
            Role: $scope.Role
        }
        var re = AccountService.save(tk);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadAccount();
                $scope.Username = "";
                $scope.Role = null;
            }
            else {
                alert("Failed");
            }
        })

    }

    $scope.Lock = function (id) {
        var tk = {
            ID:id
        }
        var re = AccountService.lock(tk);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadAccount();
            }
            else {
                alert("Failed");
            }
        })
    }

    $scope.UnLock = function (id) {
        var tk = {
            ID: id
        }
        var re = AccountService.unlock(tk);
        re.then(function (d) {
            if (d.data.success === true) {
                LoadAccount();
            }
            else {
                alert("Failed");
            }
        })
    }

})