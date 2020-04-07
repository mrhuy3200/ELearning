myapp.service('userService', function ($http) {
    //read Student
    this.getUser = function () {
        return $http.get('/User/GetUser');
    }

    //update Student records
    this.update = function (hv) {
        var updaterequest = $http({
            method: 'post',
            url: '/User/Update',
            data: hv
        });
        return updaterequest;
    }

    this.changePass = function (pass) {
        var updaterequest = $http({
            method: 'post',
            url: '/User/ChangePassword',
            data: pass
        });
        return updaterequest;
    }
})