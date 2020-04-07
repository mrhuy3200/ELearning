myapp.service('DeThiService', function ($http) {

    this.getDeThi = function (id) {
        return $http.get('/GV/DeThi/GetAllDeThi/' + id);
    }

    //thêm
    this.save = function (DeThi) {
        var request = $http({
            method: 'post',
            url: '/GV/DeThi/Insert',
            data: DeThi
        });
        return request;
    }

    //update
    this.update = function (dt) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/DeThi/Update',
            data: dt
        });
        return updaterequest;
    }

    this.delete = function (dt) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/DeThi/Delete',
            data: dt
        });
        return updaterequest;
    }

    this.publish = function (dt) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/DeThi/Publish',
            data: dt
        });
        return updaterequest;
    }

    this.private = function (dt) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/DeThi/Private',
            data: dt
        });
        return updaterequest;
    }

    this.getCauHoi = function (id) {
        return $http.get('/GV/CauHoi/GetCauHoi/' + id);
    }

})
