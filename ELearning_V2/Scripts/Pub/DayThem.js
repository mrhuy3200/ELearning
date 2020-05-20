var DayThemApp = angular.module("DayThemApp", ['angularUtils.directives.dirPagination']);


DayThemApp.directive('ngFiles', ['$parse', function ($parse) {
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

DayThemApp.controller('DayThemController', function ($scope, $http, $window, $sce, DayThemService) {
    LoadClass();
    LoadQuestion();
    LoadLession();
    $scope.QcurrentPage = 1;
    $scope.QpageSize = 5;
    $scope.CcurrentPage = 1;
    $scope.CpageSize = 3;
    $scope.LcurrentPage = 1;
    $scope.LpageSize = 5;
    var formdata = new FormData();
    var file;
    $scope.type = 0;
    function LoadClass() {
        DayThemService.LoadClass().then(function (d) {
            $scope.Lops = d.data;
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function LoadQuestion() {
        DayThemService.LoadQuestion().then(function (d) {
            $scope.Questions = d.data;
            for (var i = 0; i < $scope.Questions.length; i++) {
                $scope.Questions[i]["Content"] = $sce.trustAsHtml($scope.Questions[i]["Content"]);
            }
            setTimeout(function () {
                MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
            }, 200);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function LoadLession() {
        DayThemService.LoadLession().then(function (d) {
            $scope.Lessions = d.data;
            console.log(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    function FindQuestion(ID) {
        for (var i = 0; i < $scope.Questions.length; i++) {
            if ($scope.Questions[i].ID == ID) {
                return i;
            }
        }
    }
    function FindCourse(ID) {
        for (var i = 0; i < $scope.Lops.length; i++) {
            if ($scope.Lops[i].ID == ID) {
                return i;
            }
        }
    }
    function FindLession(ID) {
        for (var i = 0; i < $scope.Lessions.length; i++) {
            if ($scope.Lessions[i].ID == ID) {
                return i;
            }
        }
    }
    $scope.getTheFiles = function ($files) {
        console.log($files);
        angular.forEach($files, function (value, key) {
            console.log(key + ' ' + value.name);
            formdata.set(key, value);

        });
        file = $files[0];
        console.log(formdata);
    };
    $scope.lop = {
        Name: '',
        Capacity: null,
        Description: '',
        Image: '',
        Price: null,
        Schedule: '',
        Condition: '',
        Type: null
    }
    $scope.DangKy = function (type) {
        if (type == 1) {
            $scope.lop.Type = 1;
            $scope.lop.Capacity = 15;
            $scope.lop.Price = 0;
        }
        if (type == 2) {
            $scope.lop.Type = 2;
            $scope.lop.Capacity = 45;
        }
        if (type == 3) {
            $scope.lop.Type = 3;
            $scope.lop.Capacity = null;
        }
        $scope.type = type;
    }
    $scope.Save = function () {
        var ID;
        console.log('Save' + JSON.stringify($scope.lop));
        $http({
            method: 'POST',
            url: '/Lop/CreateClass',
            data: JSON.stringify($scope.lop)
        }).then(function successCallback(response) {
            ID = response.data;
            console.log(ID);
            //$scope.Clear();
            console.log('filename ' + file.name);
            var blob = file.slice(0, file.size, 'image/jpg');
            newFile = new File([blob], ID + '.jpg', { type: 'image/jpg' });
            console.log('filename ' + newFile);
            formdata.set(0, newFile);
            console.log(formdata.get(0));
            var request = {
                method: 'POST',
                url: '/api/API/UploadClassImage',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };
            $http(request)
                .then(function (d) {
                    alert("Đăng ký thành công");
                    LoadClass();
                })
        }, function errorCallback(response) {
            alert("Error : " + response.data.ExceptionMessage);
        });
    }
    $scope.Clear = function () {
        $scope.lop.Name = '';
        $scope.lop.Capacity = null;
        $scope.lop.Description = '';
        $scope.lop.Image = '';
        $scope.lop.Price = null;
        $scope.lop.Schedule = '';
        $scope.lop.Condition = '';
        $scope.lop.Type = null;
    }
    $scope.View = function (ID) {
        $window.location.href = '/Lop/CourseDetail/' + ID;
    }
    $scope.ViewQuestion = function (Question) {
        console.log(Question);
        var Ans = [];
        $scope.Question = Question;
        //$scope.Questions["Content"] = $sce.trustAsHtml($scope.Questions["Content"]);

        var charcode = 65;
        for (var i = 0; i < Question.Answers.length; i++) {
            var res = String.fromCharCode(charcode);
            Ans.push(res);
            console.log(Ans[i]);
            console.log($scope.Question["Answers"][i]);
            $scope.Question["Answers"][i]["FContent"] = $sce.trustAsHtml($scope.Question["Answers"][i]["Content"]);
            if ($scope.Question["AnswerID"] == $scope.Question["Answers"][i]["ID"]) {
                console.log("OK");
                $scope.Question["DapAn"] = res;
            }
            charcode++;
        }
        $scope.Ans = Ans;
        setTimeout(function () {
            MathJax.Hub.Queue(["Typeset", MathJax.Hub]);
        }, 200);
    }
    $scope.EditQuestion = function (QuestionID) {
        $http({
            method: 'GET',
            url: '/Lop/EditQuestion/' + QuestionID
        }).then(function successCallback(response) {
            if (response.data == -1) {
                alert("Không tìm thấy câu hỏi");
            }
            else {
                if (response.data == 0) {
                    alert("Không đủ quyền hạn");
                }
                else {
                    $window.location.href = '/Lop/EditQuestion/' + QuestionID;
                }
            }
        });
    }
    $scope.RemoveQuestion = function (QuestionDTO) {
        if (confirm("Xác nhận xóa, sau khi xóa không thể phục hổi!!!")) {
            console.log(QuestionDTO);
            $http({
                method: 'POST',
                url: '/Lop/RemoveQuestion',
                data: JSON.stringify(QuestionDTO)
            }).then(function successCallback(response) {
                if (response.data == 1) {
                    $scope.Questions.splice(FindQuestion(QuestionDTO.ID), 1);
                    alert("Đã xóa");
                }
                if (response.data == 0) {
                    alert("Không đủ quyền hạn");
                }
                if (response.data == -1) {
                    alert("Xóa thất bại");
                }
            });
        }
    }
    $scope.RemoveCourse = function (ID) {
        console.log(ID);
        if (confirm("Xác nhận xóa, hành động không thể phục hồi?", "thông báo")) {
            DayThemService.RemoveCourse(ID).then(function (d) {
                if (d.data == "OK") {
                    $scope.Lops.splice([FindCourse(ID)], 1);
                }
                //LoadClass();
                alert(d.data);
            }, function () {
                alert('Không tìm thấy dữ liệu !!!');
            });
        }
    }
    $scope.ChangeCourseStatus = function (ID, Status) {
        var CourseDTO = {
            ID: ID,
            Status: Status
        };
        DayThemService.ChangeCourseStatus(CourseDTO).then(function (d) {
            alert(d.data);
            if (d.data == "OK") {
                $scope.Lops[FindCourse(ID)].Status = Status;
            }
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    $scope.ChangeQuestionStatus = function (ID, Status) {
        var QuestionDTO = {
            ID: ID,
            Status: Status
        };
        DayThemService.ChangeQuestionStatus(QuestionDTO).then(function (d) {
            alert(d.data);
            if (d.data == "OK") {
                $scope.Questions[FindQuestion(ID)].Status = Status;
            }
            //LoadQuestion();
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }
    $scope.ViewLession = function (LessionID) {
        $window.location.href = '/Lop/LessionDetail/' + LessionID;

        //$http({
        //    method: 'GET',
        //    url: '/Lop/LessionDetail/' + LessionID
        //}).then(function successCallback(response) {

        //    if (response.data == 0) {
        //        alert("Không tìm thấy bài giảng");
        //    }
        //    else {
        //        if (response.data == -1) {
        //            alert("Không không đủ quyền hạn")
        //        }
        //        else {
        //        }
        //    }
        //});

    }
    $scope.CreateLession = function () {
        $window.location.href = '/Lop/CreateLession';
    }
    $scope.EditLession = function (ID) {
        $window.location.href = '/Lop/EditLession/' + ID;

        //$http({
        //    method: 'GET',
        //    url: '/Lop/EditLession/' + ID
        //}).then(function successCallback(response) {
        //    if (response.data == 0) {
        //        alert("Không tìm thấy bài giảng");
        //    }
        //    else {
        //        if (response.data == -1) {
        //            alert("Không không đủ quyền hạn")
        //        }
        //        else {
        //        }
        //    }
        //});
    }
    $scope.RemoveLession = function (ID) {
        var LessionlDTO = {
            ID: ID,
        };
        DayThemService.RemoveLession(LessionlDTO).then(function (d) {
            if (d.data == "OK") {
                
                $scope.Lessions.splice(FindLession(ID),1);
            }
            alert(d.data);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });
    }

});




DayThemApp.factory('DayThemService', function ($http) {
    var fac = {};
    fac.LoadClass = function () {
        return $http.get('/Lop/GetClassByUserID');
    };
    fac.LoadQuestion = function () {
        return $http.get('/Lop/GetListQuesionByUserID');
    };
    fac.LoadLession = function (ID) {
        return $http.get('/Lop/GetLessionByUserID');
    };
    fac.RemoveCourse = function (ID) {
        return $http.get('/Lop/RemoveCourse/' + ID);
    };
    fac.RemoveLession = function (LessionDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/RemoveLession',
            data: JSON.stringify(LessionDTO)
        });
    };
    fac.ChangeCourseStatus = function (CourseDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/ChangeCourseStatus',
            data: JSON.stringify(CourseDTO)
        });
    };
    fac.ChangeQuestionStatus = function (QuestionDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/ChangeQuestionStatus',
            data: JSON.stringify(QuestionDTO)
        });
    };
    return fac;
});

//MathJax.Hub.Queue(["Typeset", MathJax.Hub]);