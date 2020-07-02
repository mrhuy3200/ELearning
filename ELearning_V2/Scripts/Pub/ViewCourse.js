var ViewCourseApp = angular.module("ViewCourseApp", ['angularUtils.directives.dirPagination']);

ViewCourseApp.controller('ViewCourseController', function ($scope, $sce, $http, $window, ViewCourseService) {
    runWaiting();
    var formdata = new FormData();
    $scope.Init = function (CourseID, UserID) {
        console.log("UserID: " + UserID);
        $scope.CourseID = CourseID;
        $scope.UserID = UserID;
        //Init scope
        $scope.LcurrentPage = 1;
        $scope.LpageSize = 5;
        $scope.McurrentPage = 1;
        $scope.MpageSize = 5;
        $scope.LTAcurrentPage = 1;
        $scope.LTApageSize = 5;
        $scope.TcurrentPage = 1;
        $scope.TPageSize = 5;
        $scope.Username = '';
        $scope.Total = 0;
        $scope.TestIDEdit = 0;
        $scope.CommentRating = 0;
        //////////////////////////

        //var load1 = function () { LoadClass(CourseID); }
        //var load2 = function () { LoadMember(CourseID); }
        //var load3 = function () { LoadLession(CourseID); }
        //var load4 = function () { LoadComment(CourseID); }
        //var load5 = function () { LoadTest(CourseID); }
        //$(load1(), load2(), load3(), load4()).done(() => {
        //    console.log("DONE");
        //});
        CheckRole();

        LoadClass(CourseID, LoadComment);

        CheckJoinStatus();
        //downWaiting();
    };
    function initRatinBar() {
        console.log("Init rating")
        $(function () {
            $('#CommentRate').barrating('show', {
                theme: 'fontawesome-stars',
                onSelect: function (value, text, event) {
                    if (typeof (event) !== 'undefined') {
                        // rating was selected by a user
                        console.log(event.target);
                        console.log(value);
                        $scope.CommentRating = value;
                    } else {
                        // rating was selected programmatically
                        // by calling `set` method
                    }
                }
            });
        });
    }
    function runWaiting() {
        console.log("Open");
        angular.element("#main-wait").css("display", "block");
        angular.element("body").css("overflow", "hidden");
    }
    function downWaiting() {
        console.log("Close");

        angular.element("#main-wait").css("display", "none");
        angular.element("body").css("overflow", "auto");
    }
    function CheckJoinStatus() {
        console.log("CheckJoinStatus");

        if ($scope.UserID == 0) {
            $scope.JoinStatus = -1;//Chưa đăng nhập
        }
        else {
            var CourseDetailDTO = {
                CourseID: $scope.CourseID,
                UserID: $scope.UserID
            }
            $http({
                method: 'POST',
                url: '/Course/CheckJoinStatus',
                data: JSON.stringify(CourseDetailDTO)
            }).then(function (r) {
                if (r.data == true) {
                    console.log(r.data);
                    $scope.JoinStatus = 1;//Đã vào lớp
                    console.log("Join" + $scope.JoinStatus);

                }
                else {
                    $scope.JoinStatus = 0;//Chưa vào lớp
                }
            })
        }

    }
    function LoadClass(ID, callback) {
        console.log("Load Class");
        ViewCourseService.LoadClass(ID).then(function (d) {
            $scope.Lop = d.data;
            $scope.Lop.FDescription = $sce.trustAsHtml($scope.Lop.Description)
            console.log(d.data);
            if ($scope.Lop.NumOfPeo == $scope.Lop.Capacity) {
                //FULL
                $('#JoinBtn').css("display", "none");
            }
            callback(ID, LoadLession);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });

    }
    function LoadComment(ID, callback) {
        console.log("Load cmt");

        $scope.Five = 0;
        $scope.Four = 0;
        $scope.Three = 0;
        $scope.Two = 0;
        $scope.One = 0;

        var CommentDTO = {
            CourseID: ID
        };
        ViewCourseService.LoadComment(CommentDTO).then(function (d) {
            $scope.CommentStatus = 1;
            var temp = d.data;
            if (temp.length != 0) {
                for (var i = 0; i < temp.length; i++) {
                    if (temp[i]["Rate"] == 1) {
                        $scope.One++;
                    }
                    if (temp[i]["Rate"] == 2) {
                        $scope.Two++;
                    }
                    if (temp[i]["Rate"] == 3) {
                        $scope.Three++;
                    }
                    if (temp[i]["Rate"] == 4) {
                        $scope.Four++;
                    }
                    if (temp[i]["Rate"] == 5) {
                        $scope.Five++;
                    }
                    temp[i]["CreateDate"] = new Date(parseInt(temp[i]["CreateDate"].substr(6)));
                    if (temp[i].CreateBy == $scope.UserID) {
                        $scope.CommentStatus = 0;
                        console.log("đã cmt" + temp[i].CreateBy);
                    }
                }
                $scope.Total = ((1 * $scope.One) + (2 * $scope.Two) + (3 * $scope.Three) + (4 * $scope.Four) + (5 * $scope.Five)) / temp.length;
                $scope.FTotal = Math.floor($scope.Total);
                var bar1 = document.getElementById('bar1');
                bar1.style.width = ($scope.One / temp.length) * 100 + "%";
                var bar2 = document.getElementById('bar2');
                bar2.style.width = ($scope.Two / temp.length) * 100 + "%";
                var bar3 = document.getElementById('bar3');
                bar3.style.width = ($scope.Three / temp.length) * 100 + "%";
                var bar4 = document.getElementById('bar4');
                bar4.style.width = ($scope.Four / temp.length) * 100 + "%";
                var bar5 = document.getElementById('bar5');
                bar5.style.width = ($scope.Five / temp.length) * 100 + "%";
            }
            $scope.Comments = temp;
            temp = null;
            console.log($scope.Comments);
            initRatinBar();
            callback(ID, LoadMember);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });

    }

    function LoadLession(ID, callback) {
        console.log("LoadLession");

        ViewCourseService.LoadLession(ID).then(function (d) {
            $scope.Lessions = d.data;
            console.log(d.data);
            callback(ID, LoadTest);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });

    }
    function LoadMember(ID, callback) {
        console.log("LoadMember");

        ViewCourseService.LoadMember(ID).then(function (d) {
            $scope.Members = d.data;
            console.log(d.data);
            callback(ID, downWaiting);
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
        });

    }
    function LoadTest(ID, callback) {
        console.log("LoadTest");

        ViewCourseService.LoadTest(ID).then(function (d) {

            $scope.Tests = d.data;
            for (var i = 0; i < d.data.length; i++) {
                $scope.Tests[i].CreateDate = new Date(parseInt($scope.Tests[i].CreateDate.substr(6)));
            }
            console.log(d.data);
            callback();
        }, function () {
            alert('Không tìm thấy dữ liệu !!!');
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
            LoadNotifi($scope.CourseID);

        })
    }
    function LoadNotifi(CourseID) {
        if ($scope.UserRole != 3) {
            ViewCourseService.LoadNotifi(CourseID).then(function (d) {
                for (var i = 0; i < d.data.length; i++) {
                    d.data[i].CreateDate = new Date(parseInt((d.data[i].CreateDate).substr(6)));
                    d.data[i].Content = $sce.trustAsHtml(d.data[i].Content);

                }
                $scope.Notifis = d.data;
                setTimeout(function () {
                    for (var i = 0; i < d.data.length; i++) {
                        for (var j = 0; j < d.data[i].Files.length; j++) {
                            document.getElementById("notifile" + d.data[i].ID + "_" + j).href = "../../Content/Files/Notification/" + d.data[i].ID + "/" + d.data[i].Files[j];
                        }
                    }
                }, 500);

                console.log("Notifi" + JSON.stringify(d.data));
            }, function () {
                alert('Failed !!!');
            });
        }


    }

    function FindLession(LessionID) {
        for (var i = 0; i < $scope.Lessions.length; i++) {
            if ($scope.Lessions[i].ID == LessionID) {
                return i;
            }
        }
    }
    function LoadTestResult(TestID) {
        var TestResultDTO = {
            TestID: TestID,
            CourseID: $scope.CourseID
        }
        $http({
            method: 'POST',
            url: '/Course/LoadTestResult',
            data: JSON.stringify(TestResultDTO)
        }).then(function (r) {
            $scope.TestResult = r.data;
            $scope.TestResult.TestDate = new Date(parseInt(($scope.TestResult.TestDate).substr(6)));
            console.log("TestResult: " + r.data);
        })
    }
    $scope.ShowNotiContent = function (index) {
        if ($('#Noti' + index).is(":visible")) {
            $('#Noti' + index).collapse("toggle");
            $('#NotiIcon' + index).attr("class", "fas fa-caret-right");
            //$('#NotiIcon' + index).removeClass('fas fa-caret-down')
            //$('#NotiIcon1' + index).addClass('fas fa-caret-right')
        }
        else {
            $('#Noti' + index).collapse("toggle");
            $('#NotiIcon' + index).attr("class", "fas fa-caret-down");

            //$('#NotiIcon' + index).removeClass('fas fa-caret-right')
            //$('#NotiIcon' + index).addClass('fas fa-caret-down')
        }
    }

    $scope.Info = function (User) {
        $scope.User = User;
    }
    $scope.ViewLession = function (LessionID) {
        if ($scope.UserRole == 3) {
            if ($scope.Lessions[FindLession(LessionID)].Course_LessionStatus == 1) {
                $window.location.href = '/Lop/LessionDetail/' + LessionID + '?CourseID=' + $scope.CourseID;
            }
            if ($scope.Lessions[FindLession(LessionID)].Course_LessionStatus == 0) {
                alert("Bài giảng bị khóa")
            }
            if ($scope.Lessions[FindLession(LessionID)].Course_LessionStatus == 2) {
                alert("Bài giảng chỉ dành cho thành viên lớp học")
            }
        }
        else {
            if ($scope.Lessions[FindLession(LessionID)].Course_LessionStatus == 0) {
                alert("Bài giảng bị khóa")
            }
            else {
                $window.location.href = '/Lop/LessionDetail/' + LessionID + '?CourseID=' + $scope.CourseID;
            }
        }
    }
    $scope.ViewTest = function (Test) {
        $scope.TestInfo = Test;
        LoadTestResult(Test.ID);
        $('#TestInfo').modal('show');

    }
    $scope.Join = function () {
        if ($scope.JoinStatus == -1) {
            $("#JoinAlert").css("display", "block");
        }
        else {
            $http({
                method: 'GET',
                url: '/Course/GetUserInfo'
            }).then(function (r) {
                $scope.User = r.data;
                console.log($scope.User);
            })
            $('#JoinForm').modal('show');
        }
    }
    $scope.Regist = function () {
        var CourseDetailDTO = {
            UserID: $scope.User.ID,
            CourseID: $scope.CourseID
        };
        console.log(CourseDetailDTO)
        $http({
            method: 'POST',
            url: '/Course/AddMember',
            data: JSON.stringify(CourseDetailDTO)
        }).then(function (r) {
            if (r.data == 1) {
                alert("Đăng ký thành công");
                $window.location.reload(true);
            }
            if (r.data == 3) {
                alert("Số dư không đủ")
            }
            if (r.data == -1) {
                alert("Đã đăng ký trước đó")
            }
            if (r.data == 0) {
                alert("Xảy ra lỗi")
            }
            if (r.data == 2) {
                alert("Lớp đã đủ học viên")
            }
        })
    }
    $scope.SendComment = function () {
        console.log($scope.JoinStatus)
        console.log($scope.CommentRating)
        console.log($scope.Comment)

        if ($scope.JoinStatus == 1) {
            if ($('#CommentArea').val() != null && $('#CommentArea').val() != "" && $scope.CommentRating != 0) {
                var CommentDTO = {
                    NoiDung: $('#CommentArea').val(),
                    CourseID: $scope.CourseID,
                    Rate: $scope.CommentRating
                };
                console.log(CommentDTO);
                ViewCourseService.SendComment(CommentDTO).then(function (r) {
                    if (r.data != null) {
                        LoadComment($scope.CourseID);
                        $scope.Comment = "";
                    }
                });
            }
        }
    }
    $scope.DoTest = function () {
        if ($scope.TestInfo.Status != 1) {
            alert("Bài test hiện đang bị khóa")
        }
        else {
            $window.location.href = '/Course/DoTest/?CourseID=' + $scope.CourseID + '&TestID=' + $scope.TestInfo.ID;

        }
    }

});




ViewCourseApp.factory('ViewCourseService', function ($http) {
    var fac = {};
    fac.LoadClass = function (ID) {
        return $http.get('/Course/GetClassByID/' + ID);
    };
    fac.LoadMember = function (ID) {
        return $http.get('/Course/GetMemberByClassID/' + ID);
    };
    fac.LoadLession = function (ID) {
        return $http.get('/Course/GetLessionByClassID/' + ID);
    };
    fac.LoadTest = function (ID) {
        return $http.get('/Course/GetListTestByCourseID/' + ID);
    };
    fac.LoadNotifi = function (ID) {
        return $http.get('/Lop/GetListNotification/' + ID);
    };
    fac.LoadComment = function (CommentDTO) {
        return $http({
            method: 'POST',
            url: '/Course/GetCommentByID',
            data: JSON.stringify(CommentDTO)
        });
    };
    fac.SendComment = function (CommentDTO) {
        return $http({
            method: 'POST',
            url: '/Lop/CreateComment',
            data: JSON.stringify(CommentDTO)
        });
    };
    return fac;
});

