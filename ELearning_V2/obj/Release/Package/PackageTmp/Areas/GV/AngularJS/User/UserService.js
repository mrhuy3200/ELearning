myapp.service('userService', function ($http) {
    //read Student
    this.getUser = function () {
        return $http.get('/GV/User/GetUser');
    }

    //update Student records
    this.update = function (gv) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/User/Update',
            data: gv
        });
        return updaterequest;
    }

    this.GetAllMonHoc = function () {
        return $http.get('/GV/Lop/GetListMonHoc');
    }

    this.changePass = function (pass) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/User/ChangePassword',
            data: pass
        });
        return updaterequest;
    }
})