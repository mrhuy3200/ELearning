myapp.service('DetailsService', function ($http, $filter) {
    //Lấy ds sinh viên theo mã lớp
    this.getSV_TheoLopID = function (id) {
        return $http.get('/GV/Lop/GetSV/' + id);
    }

    this.removeSV_TheoLop = function (id, malop) {
        var SV = { ID: id, MaLop: malop };
        var request = $http({
            method: 'post',
            url: '/GV/Lop/RemoveSV',
            data: SV
        });
        return request;
    }

    this.details = function (id) {
        return $http.get('/GV/Lop/Details/' + id);
    }

    this.GetBaiGiang_TheoLop = function (id) {
        return $http.get('/GV/BaiGiang/GetBaiGiang/' + id);
    }

    this.GetAllBaiGiang = function (id) {
        return $http.get('/GV/BaiGiang/GetAllBaiGiang/' + id);
    }

    this.AddNewBG = function (model) {
        var request = $http({
            method: 'post',
            url: '/GV/BaiGiang/AddBaiGiangToLop',
            data: model
        });
        return request;
    }


    this.removeBG = function (model) {
        var request = $http({
            method: 'post',
            url: '/GV/BaiGiang/RemoveBaiGiang',
            data: model
        });
        return request;
    }

    this.AddNewSV = function (id, maLop) {
        var HS_LOP = { ID: id, MaLop: maLop };
        var request = $http({
            method: 'post',
            url: '/GV/Lop/AddNew',
            data: HS_LOP
        });
        return request;
    }

    this.GetComment = function (id) {
        return $http.get('/GV/Comment/getAllComment/' + id);
    }

    //this.GetCommentByID = function (id) {
    //    return $http.get('/GV/Comment/getCommentByID/' + id);
    //}


    this.removeComment = function (id) {
        return $http.post('/GV/Comment/removeComment/' + id)
    }

    this.removeReply = function (id) {
        return $http.post('/GV/Comment/removeReply/' + id)
    }

    this.addnewComment = function (comment) {
        var request = $http({
            method: 'post',
            url: '/GV/Comment/InsertCmt',
            data: comment
        });
        return request;
    }

    this.addnewReply = function (reply) {
        var request = $http({
            method: 'post',
            url: '/GV/Comment/InsertRep',
            data: reply
        });
        return request;
    }

    this.GetThongBao = function (id) {
        return $http.get('/GV/ThongBao/GetThongBao_TheoLopID/' + id);
    }

    this.AddNewTB = function (thongbao) {
        var request = $http({
            method: 'post',
            url: '/GV/ThongBao/Insert',
            data: thongbao
        });
        return request;
    }

    this.DeleteTB = function (thongbao) {
        var request = $http({
            method: 'post',
            url: '/GV/ThongBao/Delete',
            data: thongbao
        });
        return request;
    }



})