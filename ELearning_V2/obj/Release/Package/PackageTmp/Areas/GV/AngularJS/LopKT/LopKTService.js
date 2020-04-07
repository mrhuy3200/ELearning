myapp.service('LopKTService', function ($http) {
    this.getLopKT = function () {
        return $http.get('/GV/LopKiemTra/GetLopKT');
    }

    this.GetAllMonHoc = function () {
        return $http.get('/GV/Lop/GetListMonHoc');
    }

    this.GetDeThi = function (id) {
        return $http.get('/GV/LopKiemTra/GetDeThi/' + id);
    }

    this.GetHocVien = function (id) {
        return $http.get('/GV/LopKiemTra/GetHocVien/' + id);
    }

    this.save = function (lop) {
        var request = $http({
            method: 'post',
            url: '/GV/LopKiemTra/Create',
            data: lop
        });
        return request;
    }

    this.update = function (lop) {
        var request = $http({
            method: 'post',
            url: '/GV/LopKiemTra/Update',
            data: lop
        });
        return request;
    }


    this.delete = function (lop) {
        var request = $http({
            method: 'post',
            url: '/GV/LopKiemTra/Delete',
            data: lop
        });
        return request;
    }

    this.updateDeThi = function (lop) {
        var request = $http({
            method: 'post',
            url: '/GV/LopKiemTra/UpdateDeThi',
            data: lop
        });
        return request;
    }

    this.AddNewSV = function (HS_LOP) {
        var request = $http({
            method: 'post',
            url: '/GV/LopKiemTra/AddNew',
            data: HS_LOP
        });
        return request;
    }

    this.RemoveHV = function (HS_LOP) {
        var request = $http({
            method: 'post',
            url: '/GV/LopKiemTra/RemoveSV',
            data: HS_LOP
        });
        return request;
    }


})