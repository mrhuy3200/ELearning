myapp.service('AccountService', function ($http, $filter) {
    this.GetAccount = function () {
        return $http.get('/Admin/Account/GetAllAccount/');
    }

    this.save = function (tk) {
        var request = $http({
            method: 'post',
            url: '/Admin/Account/Insert',
            data: tk
        });
        return request;
    }

    this.lock = function (tk) {
        var request = $http({
            method: 'post',
            url: '/Admin/Account/Lock',
            data: tk
        });
        return request;
    }

    this.unlock = function (tk) {
        var request = $http({
            method: 'post',
            url: '/Admin/Account/UnLock',
            data: tk
        });
        return request;
    }

})