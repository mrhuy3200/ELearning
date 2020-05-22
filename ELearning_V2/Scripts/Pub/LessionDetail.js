var LessionDetailApp = angular.module("LessionDetailApp", ['angularUtils.directives.dirPagination']);

LessionDetailApp.directive('ngFiles', ['$parse', function ($parse) {
    function fn_link(scope, element, attrs) {
        var onChange = $parse(attrs.ngFiles);
        element.on('change', function (event) {
            onChange(scope, { $files: event.target.files });
        });
    };

    return {
        link: fn_link
    }
}])

LessionDetailApp.controller('LessionDetailController', function ($scope, $window, $http, $sce, LessionDetailService) {
    $scope.ID = function (id, CourseID, UserID) {
        $scope.currentPage = 1;
        $scope.pageSize = 10;
        $scope.LessionID = id;
        $scope.CourseID = CourseID;
        $scope.UserID = UserID;
        $scope.UserRole = 0;
        //role = 1: Owner
        //role = 2: Member
        //role = 3: Guest
        console.log("UserID"+UserID);
        console.log(id);
        console.log(CourseID);

        LoadLession(id);
        if (CourseID != '') {
            CheckRole();

        }
        LoadComment(id, CourseID);
        if (CourseID != '') {
            LoadListLession(CourseID);
        }
    };

    $scope.Rep = function (Index) {
        var RepBtnID = "#RepBtn" + Index;
        var RepCollapse = "#Rep" + Index;
        var RepArea = "#repArea" + Index;
        var text = $(RepBtnID).text();
        var lstCollapse = $('.collapse');
        for (var i = 0; i < lstCollapse.length; i++) {
            if (i != Index) {
                $("#RepBtn" + i).text('reply');
                $("#Rep" + i).collapse('hide');
                $("#repArea" + i).val("");
            }
        }
        if (text == "reply") {
            $(RepBtnID).text('cancel');
            $("#SendRepBtn" + Index).css("display", "inline-block");
            $("#EditComBtn" + Index).css("display", "none");

        }
        if (text == "cancel") {
            $(RepBtnID).text('reply');
            $("#Rep" + Index).collapse('hide');
            $("#repArea" + Index).val("");
            console.log($(RepArea).val())
        }
    }
    $scope.SendComment = function () {
        console.log($('#CommentArea').val());
        if ($('#CommentArea').val() != null && $('#CommentArea').val() != '') {
            var CommentDTO = {
                NoiDung: $('#CommentArea').val(),
                CourseID: $scope.CourseID,
                LessionID: $scope.LessionID
            };
            console.log(CommentDTO);
            LessionDetailService.SendComment(CommentDTO).then(function (r) {
                if (r.data != null) {
                    LoadComment($scope.LessionID, $scope.CourseID);
                    $('#CommentArea').val('');
                }
            });
        }
        else {
            console.log("null")
        }
    }
    $scope.SendRep = function (Index, CmtID) {
        var RepBtnID = "#RepBtn" + Index;
        var RepCollapse = "#Rep" + Index;
        var RepArea = "#repArea" + Index;
        if ($(RepArea).val() != null && $(RepArea).val() != "") {
            var ReplyDTO = {
                NoiDung: $(RepArea).val(),
                CommentID: CmtID
            };
            LessionDetailService.SendRep(ReplyDTO).then(function (r) {
                if (r.data != null) {
                    LoadComment($scope.LessionID, $scope.CourseID);
                    $(RepArea).val("")
                    $(RepBtnID).text('reply');
                    $(RepCollapse).collapse('hide')
                }
            });
        }
    }
    $scope.EditCom = function (Index, CmtID) {
        if ($("#repArea" + Index).val() != null && $("#repArea" + Index).val() != "") {
            var CommentDTO = {
                NoiDung: $("#repArea" + Index).val(),
                ID: CmtID
            };
            LessionDetailService.EditComment(CommentDTO).then(function (r) {
                if (r.data != null) {
                    LoadComment($scope.LessionID, $scope.CourseID);
                    $("#repArea" + Index).val("");
                    $("#RepBtn" + Index).text('reply');
                    $("#Rep" + Index).collapse('hide');
                }
            });
        }
    }
    $scope.EditRep = function (CmtIndex, RepIndex, RepID) {
        if ($("#RrepArea" + CmtIndex + RepIndex).val() != null && $("#RrepArea" + CmtIndex + RepIndex).val() != "") {
            var ReplyDTO = {
                NoiDung: $("#RrepArea" + CmtIndex + RepIndex).val(),
                ID: RepID
            };
            console.log(ReplyDTO);
            LessionDetailService.EditReply(ReplyDTO).then(function (r) {
                if (r.data != null) {
                    LoadComment($scope.LessionID, $scope.CourseID);
                    $("#RrepArea" + CmtIndex + RepIndex).val("");
                    $("#RRepBtn" + CmtIndex + RepIndex).text('reply');
                    $("#RRep" + CmtIndex + RepIndex).collapse('hide');
                }
            });
        }

    }
    $scope.RemoveComment = function (Comment) {
        LessionDetailService.RemoveComment(Comment).then(function (r) {
            if (r.data != null) {
                LoadComment($scope.LessionID, $scope.CourseID);
            }
        });

    }
    $scope.RemoveReply = function (Reply) {
        LessionDetailService.RemoveReply(Reply).then(function (r) {
            if (r.data != null) {
                LoadComment($scope.LessionID, $scope.CourseID);
            }
        });
    }
    $scope.SendRepRep = function (CmtIndex, RepIndex, CmtID) {
        console.log($("#RrepArea" + CmtIndex + RepIndex).val());
        if ($("#RrepArea" + CmtIndex + RepIndex).val() != null && $("#RrepArea" + CmtIndex + RepIndex).val() != "") {
            $("#RepBtnID" + CmtIndex + RepIndex).text('reply');
            $("#Rrep" + CmtIndex + RepIndex).collapse('hide')
            var ReplyDTO = {
                NoiDung: $("#RrepArea" + CmtIndex + RepIndex).val(),
                CommentID: CmtID
            };
            LessionDetailService.SendRep(ReplyDTO).then(function (r) {
                if (r.data != null) {
                    LoadComment($scope.LessionID, $scope.CourseID);
                    $("#RrepArea" + CmtIndex + RepIndex).val("");
                }
            });
        }
    }
    $scope.RepRep = function (CmtIndex, RepIndex) {
        if ($("#RRepBtn" + CmtIndex + RepIndex).text() == "reply") {
            console.log($("#RRepBtn" + CmtIndex + RepIndex).text());
            $("#RrepArea" + CmtIndex + RepIndex).val('"' + $scope.Comments[CmtIndex].Replies[RepIndex].Fullname + '"' + ' ');
            $("#RRepBtn" + CmtIndex + RepIndex).text("cancel")
        }
        else {
            console.log("#RrepArea" + CmtIndex + RepIndex);
            $("#RrepArea" + CmtIndex + RepIndex).val("");
            $("#RRepBtn" + CmtIndex + RepIndex).text("reply")
        }
    }
    $scope.EditComment = function (Index) {
        $("#Rep" + Index).collapse('show');
        $("#RepBtn" + Index).text("cancel");
        $("#repArea" + Index).val($scope.Comments[Index].NoiDung);
        $("#EditComBtn" + Index).css("display", "inline-block");
        $("#SendRepBtn" + Index).css("display", "none");
    }
    $scope.EditReply = function (CmtIndex, RepIndex) {
        $("#RRep" + CmtIndex + RepIndex).collapse('show');
        $("#RRepBtn" + CmtIndex + RepIndex).text("cancel");
        $("#RrepArea" + CmtIndex + RepIndex).val($scope.Comments[CmtIndex].Replies[RepIndex].NoiDung);
        $("#EditRepBtn" + CmtIndex + RepIndex).css("display", "inline-block");
        $("#SendRepRepBtn" + CmtIndex + RepIndex).css("display", "none");
    }
    $scope.ViewLession = function (Lession) {
        if ($scope.UserID == 0) {
            if (Lession.Course_LessionStatus == 1) {
                $window.location.href = "/Lop/LessionDetail/" + Lession.ID + "?CourseID=" + $scope.CourseID;
            }
            else {
                alert("Bài giảng chỉ dành cho thành viên trong lớp")
            }
        }
        else {
            if (Lession.Course_LessionStatus == 1) {
                $window.location.href = "/Lop/LessionDetail/" + Lession.ID + "?CourseID=" + $scope.CourseID;
            }
            else {
                if (Lession.Course_LessionStatus == 2) {
                    if ($scope.UserRole != 3 ) {
                        $window.location.href = "/Lop/LessionDetail/" + Lession.ID + "?CourseID=" + $scope.CourseID;
                    }
                    alert("Bài giảng chỉ dành cho thành viên trong lớp")
                }
                else {
                    if ($scope.UserRole == 1) {
                        $window.location.href = "/Lop/LessionDetail/" + Lession.ID + "?CourseID=" + $scope.CourseID;
                    }
                    else {
                        alert("Bài giảng bị khóa")

                    }
                }
            }
        }
        
    }
    function LoadComment(ID, CourseID) {
        var CommentDTO = {
            LessionID: ID,
            CourseID: CourseID
        };
        LessionDetailService.LoadComment(CommentDTO).then(function (d) {
            var temp = d.data;
            for (var i = 0; i < temp.length; i++) {
                temp[i]["CreateDate"] = new Date(parseInt(temp[i]["CreateDate"].substr(6)));
                for (var j = 0; j < temp[i]["Replies"].length; j++) {
                    temp[i]["Replies"][j]["CreateDate"] = new Date(parseInt(temp[i]["Replies"][j]["CreateDate"].substr(6)));
                }
            }
            $scope.Comments = temp;
            temp = null;
            console.log($scope.Comments);
        }, function () {
            alert('Không tìm thấy dữ liệu cmt !!!');
        });
    }
    function LoadLession(ID) {
        LessionDetailService.LoadLession(ID, $scope.CourseID).then(function (d) {
            $scope.Lession = d.data;
            $scope.Lession["CreateDate"] = new Date(parseInt($scope.Lession["CreateDate"].substr(6)));
            $scope.Lession["Content"] = $sce.trustAsHtml($scope.Lession["Content"]);
            $scope.Lession.URL = $sce.trustAsResourceUrl($scope.Lession.URL);

            console.log($scope.Lession);
            setTimeout(function () {
                MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
            }, 200);
        }, function () {
            alert('Không tìm thấy dữ liệu lession!!!');
        });
    }
    function LoadListLession(CourseID) {
        LessionDetailService.LoadListLession(CourseID).then(function (d) {
            $scope.Lessions = d.data;
            for (var i = 0; i < $scope.Lessions.length; i++) {
                $scope.Lessions[i]["CreateDate"] = new Date(parseInt($scope.Lessions[i]["CreateDate"].substr(6)));
            }
            console.log(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu listLession !!!');
        });
    }
    function CheckRole() {
        var CourseDetailDTO = {
            UserID: $scope.UserID,
            CourseID: $scope.CourseID
        }
        $http({
            method: 'POST',
            url: '/Lop/CheckUserRole',
            data: JSON.stringify(CourseDetailDTO)
        }).then(function (r) {
            $scope.UserRole = r.data;
            console.log("ROLE: " + r.data);
        })
    }
});




LessionDetailApp.factory('LessionDetailService', function ($http) {
    var fac = {};


    fac.LoadLession = function (ID, CourseID) {
        return $http.get('/Lop/GetLessionByID/' + ID + '?CourseID=' + CourseID);
    };
    fac.LoadListLession = function (CourseID) {
        return $http.get('/Lop/GetLessionByClassID/' + CourseID);
    };
    fac.LoadComment = function (CommentDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/GetCommentByID',
            data: JSON.stringify(CommentDTO)
        });
    };
    fac.SendRep = function (RepDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/CreateReply',
            data: JSON.stringify(RepDTO)
        });
    };
    fac.SendComment = function (CommentDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/CreateComment',
            data: JSON.stringify(CommentDTO)
        });
    };
    fac.RemoveComment = function (CommentDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/RemoveComment',
            data: JSON.stringify(CommentDTO)
        });
    };
    fac.EditComment = function (CommentDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/EditComment',
            data: JSON.stringify(CommentDTO)
        });
    };
    fac.EditReply = function (ReplyDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/EditReply',
            data: JSON.stringify(ReplyDTO)
        });
    };
    fac.RemoveReply = function (ReplyDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/RemoveReply',
            data: JSON.stringify(ReplyDTO)
        });
    };
    return fac;
});

