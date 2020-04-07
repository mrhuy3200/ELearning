myapp.service('DetailsService', function ($http) {


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

    this.getCauHoi = function (chdt) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/CauHoi/GetCauHoi',
            data: chdt
        });
        return updaterequest;

    }

    this.getChuong = function (id) {
        return $http.get('/GV/Chuong/GetChuong/' + id);
    }

    this.getCauHoiDeThi = function (id) {
        return $http.get('/GV/CauHoi/GetCauHoi_DeThi/' + id);
    }

    this.RemoveCauHoiDeThi = function (chdt) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/CauHoi/Delete_CauHoi_DeThi',
            data: chdt
        });
        return updaterequest;

    }

    this.AddCauHoiDeThi = function (chdt) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/CauHoi/Add_CauHoi_DeThi',
            data: chdt
        });
        return updaterequest;

    }

    this.RandomCauHoiDeThi = function (chdt) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/CauHoi/RandomCauHoiDeThi',
            data: chdt
        });
        return updaterequest;

    }

})
