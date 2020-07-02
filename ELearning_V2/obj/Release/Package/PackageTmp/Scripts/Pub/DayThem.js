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
    InitCss();
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
    function formatNumber(n) {
        // format number 1000000 to 1,234,567
        return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",")
    }
    function formatCurrency(input, blur) {
        // appends $ to value, validates decimal side
        // and puts cursor back in right position.

        // get input value
        var input_val = input.val();

        // don't validate empty input
        if (input_val === "") { return; }

        // original length
        var original_len = input_val.length;

        // initial caret position 
        var caret_pos = input.prop("selectionStart");

        // check for decimal
        if (input_val.indexOf(".") >= 0) {

            // get position of first decimal
            // this prevents multiple decimals from
            // being entered
            var decimal_pos = input_val.indexOf(".");

            // split number by decimal point
            var left_side = input_val.substring(0, decimal_pos);
            var right_side = input_val.substring(decimal_pos);

            // add commas to left side of number
            left_side = formatNumber(left_side);

            // validate right side
            right_side = formatNumber(right_side);

            // On blur make sure 2 numbers after decimal
            //if (blur === "blur") {
            //    right_side += "00";
            //}

            // Limit decimal to only 2 digits
            right_side = right_side.substring(0, 2);

            // join number by .
            input_val = "$" + left_side + "." + right_side;

        } else {
            // no decimal entered
            // add commas to number
            // remove all non-digits
            input_val = formatNumber(input_val);
            input_val = "$" + input_val;

            // final formatting
            //if (blur === "blur") {
            //    input_val += ".00";
            //}
        }

        // send updated string to input
        input.val(input_val);

        // put caret back in the right position
        var updated_len = input_val.length;
        caret_pos = updated_len - original_len + caret_pos;
        input[0].setSelectionRange(caret_pos, caret_pos);
    }
    function InitCss() {
        var x = document.getElementsByClassName("cke_textarea_inline");
        x[0].style.border = "1px solid #ced4da";
        x[0].style.position = "relative";
        x[0].style.height = "auto";
        x[0].style.borderRadius = ".25rem";
        x[0].style.backgroundColor = "snow";
        x[0].style.minHeight = "44px";
        x[0].style.padding = "2px 10px";

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
    function initCourse() {
        $scope.lop = {
            Name: '',
            Capacity: null,
            Description: '',
            Image: '',
            Price: '',
            Schedule: '',
            Condition: '',
            Type: null
        }

    }
    $scope.DangKy = function (type) {
        initCourse();
        DayThemService.LoadMonHoc().then(function (r) {
            $scope.MonHocs = r.data;
        })
        if (type == 1) {
            $scope.lop.Type = 1;
            $scope.lop.Capacity = 15;
            $('#PriceInput').val(0)
            $scope.lop.Pay = 0;
        }
        if (type == 2) {
            $("input[name='Price']").on({
                keyup: function () {
                    formatCurrency($(this));
                },
                blur: function () {
                    formatCurrency($(this), "blur");
                },
                change: function () {
                    console.log("Change")
                    formatCurrency($(this));
                }
            });
            var input = $("input[name='Price']")
            formatCurrency($(input));
            $scope.lop.Type = 2;
            $scope.lop.Capacity = 45;
            $scope.lop.Pay = 200000;

        }
        if (type == 3) {
            $("input[name='Price']").on({
                keyup: function () {
                    formatCurrency($(this));
                },
                blur: function () {
                    formatCurrency($(this), "blur");
                },
                change: function () {
                    console.log("Change")
                    formatCurrency($(this));
                }
            });
            var input = $("input[name='Price']")
            formatCurrency($(input));
            $scope.lop.Type = 3;
            $scope.lop.Capacity = null;
            $scope.lop.Pay = 500000;
        }
        $scope.type = type;
    }
    $scope.Save = function () {
        var val1,val2,val3,val4;
        if ($('#file').get(0).files.length === 0) {
            val1 = false;
            $('#CourseImageError').css("display", "block");
        }
        else {
            val1 = true
            $('#CourseImageError').css("display", "none");

        }
        if ($scope.lop.Name == null || $scope.lop.Name == '') {
            val2 = false;
            $('#CourseNameError').css("display", "block");
        }
        else {
            val2 = true;
            $('#CourseNameError').css("display", "none");

        }
        if ($('#PriceInput').val() == null || $('#PriceInput').val() == '') {
            val3 = false;
            $('#CoursePriceError').css("display", "block");
        }
        else {
            val3 = true;
            $('#CoursePriceError').css("display", "none");

        }
        if ($scope.lop.MaMonHoc == null || $scope.lop.MaMonHoc == '') {
            val4 = false;
            $('#CourseMonHocError').css("display", "block");
        }
        else {
            val4 = true;
            $('#CourseMonHocError').css("display", "none");

        }
        if (val1 && val2 && val3 && val4) {
            $scope.lop.Price = parseInt($('#PriceInput').val().replace(/\D/g, ""))
            $scope.lop.Description = CKEDITOR.instances.DContent.getData();
            console.log('Save' + JSON.stringify($scope.lop));
            $http({
                method: 'GET',
                url: '/Course/GetUserInfo'
            }).then(function (r) {
                $scope.User = r.data;
                console.log($scope.User);
                console.log($scope.lop)
            })

            $('#PayConfirm').modal('show');
  
        }

        //$http({
        //    method: 'POST',
        //    url: '/Lop/CreateClass',
        //    data: JSON.stringify($scope.lop)
        //}).then(function successCallback(response) {
        //    ID = response.data;
        //    console.log(ID);
        //    //$scope.Clear();
        //    console.log('filename ' + file.name);
        //    var blob = file.slice(0, file.size, 'image/jpg');
        //    newFile = new File([blob], ID + '.jpg', { type: 'image/jpg' });
        //    console.log('filename ' + newFile);
        //    formdata.set(0, newFile);
        //    console.log(formdata.get(0));
        //    var request = {
        //        method: 'POST',
        //        url: '/api/API/UploadClassImage',
        //        data: formdata,
        //        headers: {
        //            'Content-Type': undefined
        //        }
        //    };
        //    $http(request)
        //        .then(function (d) {
        //            alert("Đăng ký thành công");
        //            LoadClass();
        //        })
        //}, function errorCallback(response) {
        //    alert("Error : " + response.data.ExceptionMessage);
        //});
    }
    $scope.Pay = function () {
        if ($scope.lop.Pay <= $scope.User.Balance) {
            console.log("OK");
            $http({
                method: 'POST',
                url: '/Lop/CreateClass',
                data: JSON.stringify($scope.lop)
            }).then(function successCallback(response) {
                if (response.data != -1) {
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
                            $('.modal').modal('hide');

                        })
                }
                else {
                    alert("Số dư không đủ để thanh toán");
                }
            }, function errorCallback(response) {
                alert("Error : " + response.data.ExceptionMessage);
            });

        }
        else {
            alert("Số dư không đủ để thanh toán");
        }

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

                $scope.Lessions.splice(FindLession(ID), 1);
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
    fac.LoadMonHoc = function () {
        return $http.get('/Lop/GetListMonHoc');
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