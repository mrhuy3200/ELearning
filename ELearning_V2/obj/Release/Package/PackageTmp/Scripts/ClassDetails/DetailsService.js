myapp.service('ClassDetailsService', function ($http, $filter) {
    //Lấy ds sinh viên theo mã lớp

    this.GetThongBaoTheoLop = function (id) {
        return $http.get('/Lop/GetThongBao_TheoLopID/' + id);
    }


    this.details = function (id) {
        return $http.get('/Lop/Details/' + id);
    }

    this.GetBaiGiang_TheoLop = function (id) {
        return $http.get('/BaiGiang/GetBaiGiang_TheoMaLop/' + id);
    }




    this.GetComment = function (id) {
        return $http.get('/Comment/getAllComment/' + id);
    }

    this.addnewComment = function (comment) {
        var request = $http({
            method: 'post',
            url: '/Comment/InsertCmt',
            data: comment
        });
        return request;
    }

    this.addnewReply = function (reply) {
        var request = $http({
            method: 'post',
            url: '/Comment/InsertRep',
            data: reply
        });
        return request;
    }

})