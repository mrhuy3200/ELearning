myapp.service('CauHoiService', function ($http) {
    //read câu hỏi môn toán 10
    this.getCauHoi = function (id) {
        return $http.get('/GV/CauHoi/GetAllQuestion/' + id);
    }

    //thêm câu hỏi
    this.save = function (CauHoi) {
        var request = $http({
            method: 'post',
            url: '/GV/CauHoi/Insert',
            data: CauHoi
        });
        return request;
    }

    //update câu hỏi
    this.update = function (gv) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/CauHoi/Update',
            data: gv
        });
        return updaterequest;
    }

    this.delete = function (MaCauHoi) {
        return $http.post('/GV/CauHoi/Delete/' + MaCauHoi);
    }

    this.publish = function (gv) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/CauHoi/Publish',
            data: gv
        });
        return updaterequest;
    }

    this.private = function (gv) {
        var updaterequest = $http({
            method: 'post',
            url: '/GV/CauHoi/Private',
            data: gv
        });
        return updaterequest;
    }



})