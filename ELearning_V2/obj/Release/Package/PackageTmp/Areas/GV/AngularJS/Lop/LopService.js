myapp.service('LopService', function ($http) {
    //read lop
    this.getLop = function () {
        return $http.get('/GV/Lop/GetLop');
    }

    //update Student records
    this.update = function (lop) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/Lop/Update',
            data: lop
        });
        return updaterequest;
    }

    this.save = function (lop) {
        var request = $http({
            method: 'post',
            url: '/GV/Lop/Insert',
            data: lop
        });
        return request;
    }

    this.delete = function (MaLop) {
        return $http.post('/GV/Lop/Delete/' + MaLop);
    }

    this.publish = function (l) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/Lop/Publish',
            data: l
        });
        return updaterequest;
    }

    this.private = function (l) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/Lop/Private',
            data: l
        });
        return updaterequest;
    }

    this.GetAllMonHoc = function () {
        return $http.get('/GV/Lop/GetListMonHoc');
    }
})